using AwesomeAssertions;
using BLL.DTO;
using BLL.Mappers;
using BLL.Services;
using DAL.Contracts;
using Moq;
using DalAppUser = global::DAL.DTO.Identity.AppUser;
using DalComment = global::DAL.DTO.Comment;

namespace Tests.Unit.BLL.Services;

public class CommentService_Tests
{
    private readonly Mock<IAppUOW> _uow = new();
    private readonly Mock<ICommentRepository> _repository = new();
    private readonly Mock<IUserProjectRepository> _userProjectRepository = new();
    private readonly CommentBLLMapper _mapper = new();
    private readonly CommentService _sut;

    public CommentService_Tests()
    {
        _uow.SetupGet(u => u.CommentRepository).Returns(_repository.Object);
        _uow.SetupGet(u => u.UserProjectRepository).Returns(_userProjectRepository.Object);

        _sut = new CommentService(_uow.Object, _mapper);
    }

    [Fact]
    public async Task AllAsyncByProjectId_WhenUserInProject_ReturnsMappedComments()
    {
        var projectId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var dalComments = new List<DalComment>
        {
            new()
            {
                Id = Guid.NewGuid(),
                ProjectId = projectId,
                UserId = userId,
                Content = "First comment",
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                ProjectId = projectId,
                UserId = userId,
                Content = "Second comment",
                CreatedAt = DateTime.UtcNow
            }
        };

        _userProjectRepository
            .Setup(r => r.UserInProject(projectId, userId))
            .ReturnsAsync(true);

        _repository
            .Setup(r => r.AllAsyncByProjectId(projectId, userId))
            .ReturnsAsync(dalComments);

        var result = (await _sut.AllAsyncByProjectId(projectId, userId, false)).ToList();

        result.Should().HaveCount(2);
        result.Should().Contain(x => x.Content == "First comment");
        result.Should().Contain(x => x.Content == "Second comment");

        _repository.Verify(r => r.AllAsyncByProjectId(projectId, userId), Times.Once);
    }

    [Fact]
    public async Task AllAsyncByProjectId_WhenAdminAndNotInProject_ReturnsMappedComments()
    {
        var projectId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _userProjectRepository
            .Setup(r => r.UserInProject(projectId, userId))
            .ReturnsAsync(false);

        _repository
            .Setup(r => r.AllAsyncByProjectId(projectId, userId))
            .ReturnsAsync(new List<DalComment>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    ProjectId = projectId,
                    UserId = userId,
                    Content = "Admin visible comment"
                }
            });

        var result = (await _sut.AllAsyncByProjectId(projectId, userId, true)).ToList();

        result.Should().HaveCount(1);
        result[0].Content.Should().Be("Admin visible comment");
    }

    [Fact]
    public async Task AllAsyncByProjectId_WhenUserNotInProjectAndNotAdmin_ThrowsUnauthorizedAccessException()
    {
        var projectId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _userProjectRepository
            .Setup(r => r.UserInProject(projectId, userId))
            .ReturnsAsync(false);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _sut.AllAsyncByProjectId(projectId, userId, false));

        _repository.Verify(r => r.AllAsyncByProjectId(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task AddAsync_WhenUserInProject_AddsMappedComment()
    {
        var commentId = Guid.NewGuid();
        var projectId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var comment = new Comment
        {
            Id = commentId,
            ProjectId = projectId,
            UserId = userId,
            Content = "New comment"
        };

        _userProjectRepository
            .Setup(r => r.UserInProject(projectId, userId))
            .ReturnsAsync(true);

        await _sut.AddAsync(comment, userId, false);

        _repository.Verify(
            r => r.Add(
                It.Is<DalComment>(x =>
                    x.Id == commentId &&
                    x.ProjectId == projectId &&
                    x.UserId == userId &&
                    x.Content == "New comment"),
                userId),
            Times.Once);
    }

    [Fact]
    public async Task AddAsync_WhenAdminAndNotInProject_AddsMappedComment()
    {
        var projectId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var comment = new Comment
        {
            Id = Guid.NewGuid(),
            ProjectId = projectId,
            UserId = userId,
            Content = "Admin comment"
        };

        _userProjectRepository
            .Setup(r => r.UserInProject(projectId, userId))
            .ReturnsAsync(false);

        await _sut.AddAsync(comment, userId, true);

        _repository.Verify(r => r.Add(It.IsAny<DalComment>(), userId), Times.Once);
    }

    [Fact]
    public async Task AddAsync_WhenUserNotInProjectAndNotAdmin_ThrowsUnauthorizedAccessException()
    {
        var projectId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var comment = new Comment
        {
            Id = Guid.NewGuid(),
            ProjectId = projectId,
            UserId = userId,
            Content = "Forbidden comment"
        };

        _userProjectRepository
            .Setup(r => r.UserInProject(projectId, userId))
            .ReturnsAsync(false);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _sut.AddAsync(comment, userId, false));

        _repository.Verify(r => r.Add(It.IsAny<DalComment>(), It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task FindAsync_ReturnsMappedCommentWithUser()
    {
        var commentId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var projectId = Guid.NewGuid();

        _repository.Setup(r => r.FindAsync(commentId))
            .ReturnsAsync(new DalComment
            {
                Id = commentId,
                UserId = userId,
                ProjectId = projectId,
                Content = "Comment with user",
                CreatedAt = DateTime.UtcNow,
                User = new DalAppUser
                {
                    Id = userId,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john@example.com"
                }
            });

        var result = await _sut.FindAsync(commentId);

        result.Should().NotBeNull();
        result!.Id.Should().Be(commentId);
        result.Content.Should().Be("Comment with user");
        result.User.Should().NotBeNull();
        result.User!.Email.Should().Be("john@example.com");
    }

    [Fact]
    public async Task FindAsync_WhenRepositoryReturnsNull_ReturnsNull()
    {
        var commentId = Guid.NewGuid();

        _repository.Setup(r => r.FindAsync(commentId))
            .ReturnsAsync((DalComment?)null);

        var result = await _sut.FindAsync(commentId);

        result.Should().BeNull();
    }

    [Fact]
    public void Remove_WhenCommentHasReplies_SoftDeletesComment()
    {
        var commentId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var dalComment = new DalComment
        {
            Id = commentId,
            UserId = userId,
            ProjectId = Guid.NewGuid(),
            Content = "Original comment"
        };

        _repository.Setup(r => r.Find(commentId, userId))
            .Returns(dalComment);

        _repository.Setup(r => r.CommentHasReplies(commentId))
            .ReturnsAsync(true);

        _sut.Remove(commentId, userId);

        dalComment.Content.Should().Be("Deleted comment.");

        _repository.Verify(r => r.Update(It.Is<DalComment>(x =>
            x.Id == commentId &&
            x.Content == "Deleted comment.")), Times.Once);

        _repository.Verify(r => r.Remove(It.IsAny<DalComment>(), It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public void Remove_WhenCommentHasNoReplies_RemovesComment()
    {
        var commentId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var dalComment = new DalComment
        {
            Id = commentId,
            UserId = userId,
            ProjectId = Guid.NewGuid(),
            Content = "Comment"
        };

        _repository.Setup(r => r.Find(commentId, userId))
            .Returns(dalComment);

        _repository.Setup(r => r.CommentHasReplies(commentId))
            .ReturnsAsync(false);

        _sut.Remove(commentId, userId);

        _repository.Verify(r => r.Remove(dalComment, userId), Times.Once);
        _repository.Verify(r => r.Update(It.IsAny<DalComment>()), Times.Never);
    }

    [Fact]
    public void Remove_WhenCommentNotFound_DoesNothing()
    {
        var commentId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _repository.Setup(r => r.Find(commentId, userId))
            .Returns((DalComment?)null);

        _sut.Remove(commentId, userId);

        _repository.Verify(r => r.CommentHasReplies(It.IsAny<Guid>()), Times.Never);
        _repository.Verify(r => r.Remove(It.IsAny<DalComment>(), It.IsAny<Guid>()), Times.Never);
        _repository.Verify(r => r.Update(It.IsAny<DalComment>()), Times.Never);
    }

    [Fact]
    public async Task RemoveAsync_WhenCommentHasReplies_SoftDeletesComment()
    {
        var commentId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var dalComment = new DalComment
        {
            Id = commentId,
            UserId = userId,
            ProjectId = Guid.NewGuid(),
            Content = "Original comment"
        };

        _repository.Setup(r => r.FindAsync(commentId, userId))
            .ReturnsAsync(dalComment);

        _repository.Setup(r => r.CommentHasReplies(commentId))
            .ReturnsAsync(true);

        _repository.Setup(r => r.UpdateAsync(It.IsAny<DalComment>()))
            .ReturnsAsync((DalComment x, Guid _) => x);

        await _sut.RemoveAsync(commentId, userId);

        dalComment.Content.Should().Be("Deleted comment.");

        _repository.Verify(r => r.UpdateAsync(It.Is<DalComment>(x =>
            x.Id == commentId &&
            x.Content == "Deleted comment.")), Times.Once);

        _repository.Verify(r => r.RemoveAsync(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task RemoveAsync_WhenCommentHasNoReplies_RemovesComment()
    {
        var commentId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var dalComment = new DalComment
        {
            Id = commentId,
            UserId = userId,
            ProjectId = Guid.NewGuid(),
            Content = "Comment"
        };

        _repository.Setup(r => r.FindAsync(commentId, userId))
            .ReturnsAsync(dalComment);

        _repository.Setup(r => r.CommentHasReplies(commentId))
            .ReturnsAsync(false);

        await _sut.RemoveAsync(commentId, userId);

        _repository.Verify(r => r.RemoveAsync(commentId, userId), Times.Once);
        _repository.Verify(r => r.UpdateAsync(It.IsAny<DalComment>()), Times.Never);
    }

    [Fact]
    public async Task RemoveAsync_WhenCommentNotFound_DoesNothing()
    {
        var commentId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _repository.Setup(r => r.FindAsync(commentId, userId))
            .ReturnsAsync((DalComment?)null);

        await _sut.RemoveAsync(commentId, userId);

        _repository.Verify(r => r.CommentHasReplies(It.IsAny<Guid>()), Times.Never);
        _repository.Verify(r => r.RemoveAsync(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
        _repository.Verify(r => r.UpdateAsync(It.IsAny<DalComment>()), Times.Never);
    }

    [Fact]
    public void Remove_WithEntity_DelegatesToRemoveById()
    {
        var commentId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var bllComment = new Comment
        {
            Id = commentId,
            UserId = userId,
            ProjectId = Guid.NewGuid(),
            Content = "Comment"
        };

        var dalComment = new DalComment
        {
            Id = commentId,
            UserId = userId,
            ProjectId = bllComment.ProjectId,
            Content = bllComment.Content
        };

        _repository.Setup(r => r.Find(commentId, userId))
            .Returns(dalComment);

        _repository.Setup(r => r.CommentHasReplies(commentId))
            .ReturnsAsync(false);

        _sut.Remove(bllComment, userId);

        _repository.Verify(r => r.Remove(dalComment, userId), Times.Once);
    }
}