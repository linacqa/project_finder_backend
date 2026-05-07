using AwesomeAssertions;
using BLL.DTO;
using BLL.Mappers;
using Comment = global::BLL.DTO.Comment;
using DalComment = global::DAL.DTO.Comment;
using DalAppUser = global::DAL.DTO.Identity.AppUser;
using DalProject = global::DAL.DTO.Project;

namespace Tests.Unit.BLL.Mappers;

public class CommentBLLMapper_Tests
{
    private readonly CommentBLLMapper _sut = new();

    [Fact]
    public void Map_DalToBll_AllFieldsCopied()
    {
        var userId = Guid.NewGuid();
        var projectId = Guid.NewGuid();
        var replyToCommentId = Guid.NewGuid();
        
        var dal = new DalComment
        {
            Id = Guid.NewGuid(),
            Content = "content",
            UserId = userId,
            User = new DalAppUser()
            {
                Id = userId,
                FirstName = "first",
                LastName = "last",
                Email = "user@example.com"
            },
            ProjectId = projectId,
            Project = new DalProject()
            {
                Id = projectId,
                TitleInEstonian = "et",
                TitleInEnglish = "en",
            },
            ReplyToCommentId = replyToCommentId,
            ReplyToComment = new DalComment()
            {
                Id = replyToCommentId,
                Content = "content2",
                UserId = userId,
                ProjectId = projectId,
            },
            CreatedAt = DateTime.UtcNow
        };

        var bll = _sut.Map(dal)!;

        bll.Id.Should().Be(dal.Id);
        bll.Content.Should().Be("content");
        bll.UserId.Should().Be(userId);
        bll.ReplyToCommentId.Should().Be(replyToCommentId);
        bll.ProjectId.Should().Be(projectId);
        bll.User.Id.Should().Be(userId);
        bll.User.FirstName.Should().Be("first");
        bll.User.LastName.Should().Be("last");
        bll.User.Email.Should().Be("user@example.com");
        bll.CreatedAt.Should().Be(dal.CreatedAt);
    }

    [Fact]
    public void Map_BllToDal_AllFieldsCopied()
    {
        var bll = new Comment
        {
            Id = Guid.NewGuid(),
            Content = "content",
            UserId = Guid.NewGuid(),
            ProjectId = Guid.NewGuid(),
            ReplyToCommentId = Guid.NewGuid(),
        };

        var dal = _sut.Map(bll)!;

        dal.Id.Should().Be(bll.Id);
        dal.Content.Should().Be("content");
        dal.UserId.Should().Be(bll.UserId);
        dal.ProjectId.Should().Be(bll.ProjectId);
        dal.ReplyToCommentId.Should().Be(bll.ReplyToCommentId);
    }

    [Fact]
    public void Map_NullDal_ReturnsNull()
    {
        _sut.Map((DalComment?)null).Should().BeNull();
    }

    [Fact]
    public void Map_NullBll_ReturnsNull()
    {
        _sut.Map((Comment?)null).Should().BeNull();
    }
}