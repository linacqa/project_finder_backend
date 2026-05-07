using AwesomeAssertions;
using BLL.DTO;
using BLL.DTO.Identity;
using BLL.Mappers;
using DalApplication = global::DAL.DTO.Application;
using DalGroup = global::DAL.DTO.Group;
using DalUser = global::DAL.DTO.Identity.AppUser;
using DalProject = global::DAL.DTO.Project;
using DalUserGroup = global::DAL.DTO.UserGroup;

namespace Tests.Unit.BLL.Mappers;

public class ApplicationBLLMapper_Tests
{
    private readonly ApplicationBLLMapper _sut = new();

    [Fact]
    public void Map_DalToBll_AllFieldsCopied()
    {
        var groupId = Guid.NewGuid();
        var creatorId = Guid.NewGuid();
        var memberId = Guid.NewGuid();
        var projectId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var id = Guid.NewGuid();
        var acceptedAt = DateTime.UtcNow;
        var declinedAt = acceptedAt.AddHours(1);

        var dal = new DalApplication
        {
            Id = id,
            GroupId = groupId,
            Group = new DalGroup
            {
                Id = groupId,
                Name = "group",
                IsAzureAdGroup = false,
                CreatorId = creatorId,
                Creator = new DalUser
                {
                    Id = creatorId,
                    FirstName = "first",
                    LastName = "last",
                    Email = "user@example.com"
                },
                UserGroups = new List<DalUserGroup>
                {
                    new DalUserGroup()
                    {
                        Id = Guid.NewGuid(),
                        GroupId = groupId,
                        UserId = memberId,
                        Role = "developer",
                        User = new DalUser
                        {
                            Id = memberId,
                            FirstName = "first",
                            LastName = "last",
                            Email = "user@example.com"
                        }
                    }
                },
            },
            ProjectId = projectId,
            Project = new DalProject
            {
                Id = projectId,
                TitleInEstonian = "et",
                TitleInEnglish = "en",
            },
            UserId = userId,
            User = new DalUser
            {
                Id = userId,
                FirstName = "first",
                LastName = "last",
                Email = "user@example.com",
            },
            AcceptedAt = acceptedAt,
            DeclinedAt = declinedAt,
        };

        var bll = _sut.Map(dal)!;

        bll.Id.Should().Be(id);
        bll.GroupId.Should().Be(groupId);
        bll.Group.Should().NotBeNull();
        bll.Group!.Id.Should().Be(groupId);
        bll.Group.Name.Should().Be("group");
        bll.Group.IsAzureAdGroup.Should().BeFalse();
        bll.Group.CreatorId.Should().Be(creatorId);
        bll.Group.Creator.Should().NotBeNull();
        bll.Group.Creator!.Id.Should().Be(creatorId);
        bll.Group.Creator.FirstName.Should().Be("first");
        bll.Group.Creator.LastName.Should().Be("last");
        bll.Group.Creator.Email.Should().Be("user@example.com");
        bll.Group.UserGroups.Should().BeNull();
        bll.ProjectId.Should().Be(projectId);
        bll.Project.Should().NotBeNull();
        bll.Project!.Id.Should().Be(projectId);
        bll.Project.TitleInEstonian.Should().Be("et");
        bll.Project.TitleInEnglish.Should().Be("en");
        bll.UserId.Should().Be(userId);
        bll.User.Should().NotBeNull();
        bll.User!.Id.Should().Be(userId);
        bll.AcceptedAt.Should().Be(acceptedAt);
        bll.DeclinedAt.Should().Be(declinedAt);
    }

    [Fact]
    public void Map_BllToDal_AllFieldsCopied()
    {
        var groupId = Guid.NewGuid();
        var creatorId = Guid.NewGuid();
        var memberId = Guid.NewGuid();
        var projectId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var id = Guid.NewGuid();
        var acceptedAt = DateTime.UtcNow;
        var declinedAt = acceptedAt.AddHours(1);

        var bll = new Application
        {
            Id = id,
            GroupId = groupId,
            Group = new Group()
            {
                Id = groupId,
                Name = "group",
                IsAzureAdGroup = false,
                CreatorId = creatorId,
                Creator = new AppUser
                {
                    Id = creatorId,
                    FirstName = "first",
                    LastName = "last",
                    Email = "user@example.com",
                },
                UserGroups = new List<UserGroup>
                {
                    new UserGroup()
                    {
                        Id = Guid.NewGuid(),
                        GroupId = groupId,
                        UserId = memberId,
                        Role = "developer",
                        User = new AppUser
                        {
                            Id = memberId,
                            FirstName = "first",
                            LastName = "last",
                            Email = "user@example.com",
                        },
                    },
                },
            },
            ProjectId = projectId,
            Project = new Project
            {
                Id = projectId,
                TitleInEstonian = "et",
                TitleInEnglish = "en",
            },
            UserId = userId,
            User = new AppUser
            {
                Id = userId,
                FirstName = "first",
                LastName = "last",
                Email = "user@example.com",
            },
            AcceptedAt = acceptedAt,
            DeclinedAt = declinedAt,
        };

        var dal = _sut.Map(bll)!;

        dal.Id.Should().Be(id);
        dal.GroupId.Should().Be(groupId);
        dal.Group.Should().BeNull();
        dal.ProjectId.Should().Be(projectId);
        dal.Project.Should().BeNull();
        dal.UserId.Should().Be(userId);
        dal.User.Should().BeNull();
        dal.AcceptedAt.Should().Be(acceptedAt);
        dal.DeclinedAt.Should().Be(declinedAt);
    }

    [Fact]
    public void Map_NullDal_ReturnsNull()
    {
        _sut.Map((DalApplication?)null).Should().BeNull();
    }

    [Fact]
    public void Map_NullBll_ReturnsNull()
    {
        _sut.Map((Application?)null).Should().BeNull();
    }
}