using AwesomeAssertions;
using DAL.EF.Mappers;
using DalComment = global::DAL.DTO.Comment;
using DomainComment = global::Domain.Comment;
using DomainAppUser = global::Domain.Identity.AppUser;
using DomainProject = global::Domain.Project;

namespace Tests.Unit.DAL.Mappers;

public class CommentDalMapperTests
{
    private readonly CommentUOWMapper _sut = new();

    [Fact]
    public void Map_DomainToDal_AllFieldsCopied()
    {
        var userId = Guid.NewGuid();
        var projectId = Guid.NewGuid();
        var replyToCommentId = Guid.NewGuid();
        
        var domain = new DomainComment
        {
            Id = Guid.NewGuid(),
            Content = "content",
            UserId = userId,
            User = new DomainAppUser()
            {
                Id = userId,
                FirstName = "first",
                LastName = "last",
                Email = "user@example.com"
            },
            ProjectId = projectId,
            Project = new DomainProject()
            {
                Id = projectId,
                TitleInEstonian = "et",
                TitleInEnglish = "en",
            },
            ReplyToCommentId = replyToCommentId,
            ReplyToComment = new DomainComment()
            {
                Id = replyToCommentId,
                Content = "content2",
                UserId = userId,
                ProjectId = projectId,
            },
            CreatedAt = DateTime.UtcNow
        };

        var dal = _sut.Map(domain)!;

        dal.Id.Should().Be(dal.Id);
        dal.Content.Should().Be("content");
        dal.UserId.Should().Be(userId);
        dal.ReplyToCommentId.Should().Be(replyToCommentId);
        dal.ProjectId.Should().Be(projectId);
        dal.User.Id.Should().Be(userId);
        dal.User.FirstName.Should().Be("first");
        dal.User.LastName.Should().Be("last");
        dal.User.Email.Should().Be("user@example.com");
        dal.CreatedAt.Should().Be(dal.CreatedAt);
    }

    [Fact]
    public void Map_DalToDomain_AllFieldsCopied()
    {
        var dal = new DalComment
        {
            Id = Guid.NewGuid(),
            Content = "content",
            UserId = Guid.NewGuid(),
            ProjectId = Guid.NewGuid(),
            ReplyToCommentId = Guid.NewGuid(),
        };

        var domain = _sut.Map(dal)!;

        domain.Id.Should().Be(dal.Id);
        domain.Content.Should().Be("content");
        domain.UserId.Should().Be(dal.UserId);
        domain.ProjectId.Should().Be(dal.ProjectId);
        domain.ReplyToCommentId.Should().Be(dal.ReplyToCommentId);
    }

    [Fact]
    public void Map_NullDomain_ReturnsNull()
    {
        _sut.Map((DomainComment?)null).Should().BeNull();
    }

    [Fact]
    public void Map_NullDal_ReturnsNull()
    {
        _sut.Map((DalComment?)null).Should().BeNull();
    }
}