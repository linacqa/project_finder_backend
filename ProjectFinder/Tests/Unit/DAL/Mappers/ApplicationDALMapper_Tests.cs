using DAL.EF.Mappers;
using AwesomeAssertions;

using DalApplication = global::DAL.DTO.Application;
using DomainApplication = global::Domain.Application;
using DomainAppUser = global::Domain.Identity.AppUser;
using DomainGroup = global::Domain.Group;
using DomainProject = global::Domain.Project;
using DomainUserGroup = global::Domain.UserGroup;
using DomainAuthType = global::Domain.Identity.AuthType;

namespace Tests.Unit.DAL.Mappers;

public class ApplicationDALMapper_Tests
{
    private readonly ApplicationUOWMapper _sut = new();

    [Fact]
    public void Map_DomainToDal_AllFieldsCopied()
    {
        var userId = Guid.NewGuid();
        var groupUserId = Guid.NewGuid();
        var groupId = Guid.NewGuid();
        var projectId = Guid.NewGuid();

        var domain = new DomainApplication
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            User = new DomainAppUser
            {
                Id = userId,
                FirstName = "first",
                LastName = "last",
                Email = "e@e",
                AzureObjectId = "az",
                AuthType = DomainAuthType.Local,
            },
            GroupId = groupId,
            Group = new DomainGroup
            {
                Id = groupId,
                Name = "group-name",
                IsAzureAdGroup = true,
                UserId = groupUserId,
                User = new DomainAppUser
                {
                    Id = groupUserId,
                    FirstName = "g-first",
                    LastName = "g-last",
                    AzureObjectId = "g-az",
                    AuthType = DomainAuthType.AzureAD,
                },
                UserGroups = new List<DomainUserGroup>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        UserId = Guid.NewGuid(),
                        GroupId = groupId,
                        Role = "admin",
                        User = new DomainAppUser
                        {
                            Id = Guid.NewGuid(),
                            FirstName = "member-first",
                            LastName = "member-last",
                            Email = "member@example.com",
                        },
                    }
                },
            },
            ProjectId = projectId,
            Project = new DomainProject
            {
                Id = projectId,
                TitleInEstonian = "et",
            },
            AcceptedAt = DateTime.UtcNow,
            DeclinedAt = null,
        };

        var dal = _sut.Map(domain)!;

        dal.Should().NotBeNull();
        dal.Id.Should().Be(domain.Id);
        dal.UserId.Should().Be(domain.UserId);
        dal.User.Should().NotBeNull();
        dal.User!.Id.Should().Be(userId);
        dal.User.FirstName.Should().Be("first");
        dal.GroupId.Should().Be(groupId);
        dal.Group.Should().NotBeNull();
        dal.Group!.Name.Should().Be("group-name");
        dal.Group.UserGroups.Should().NotBeNull();
        dal.Group.UserGroups!.Count.Should().Be(1);
        dal.Group.UserGroups.First().Role.Should().Be("admin");
        dal.Group.UserGroups.First().User.Should().NotBeNull();
        dal.Group.UserGroups.First().User!.FirstName.Should().Be("member-first");
        dal.Group.UserGroups.First().User!.LastName.Should().Be("member-last");
        dal.Group.UserGroups.First().User!.Email.Should().Be("member@example.com");
        dal.ProjectId.Should().Be(projectId);
        dal.Project.Should().NotBeNull();
        dal.Project!.TitleInEstonian.Should().Be("et");
        dal.AcceptedAt.Should().Be(domain.AcceptedAt);
        dal.DeclinedAt.Should().BeNull();
    }

    [Fact]
    public void Map_DalToDomain_AllFieldsCopied()
    {
        var dal = new DalApplication
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            GroupId = Guid.NewGuid(),
            ProjectId = Guid.NewGuid(),
            AcceptedAt = DateTime.UtcNow,
            DeclinedAt = null,
        };

        var domain = _sut.Map(dal)!;

        domain.Should().NotBeNull();
        domain.Id.Should().Be(dal.Id);
        domain.UserId.Should().Be(dal.UserId);
        domain.GroupId.Should().Be(dal.GroupId);
        domain.ProjectId.Should().Be(dal.ProjectId);
        domain.AcceptedAt.Should().Be(dal.AcceptedAt);
        domain.DeclinedAt.Should().BeNull();
    }

    [Fact]
    public void Map_NullDomain_ReturnsNull()
    {
        _sut.Map((DomainApplication?)null).Should().BeNull();
    }

    [Fact]
    public void Map_NullDal_ReturnsNull()
    {
        _sut.Map((DalApplication?)null).Should().BeNull();
    }
}