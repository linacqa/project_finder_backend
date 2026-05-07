using AwesomeAssertions;
using DAL.EF.Mappers;

using DalGroup = global::DAL.DTO.Group;
using DalUser = global::DAL.DTO.Identity.AppUser;
using DalUserGroup = global::DAL.DTO.UserGroup;

using DomainGroup = global::Domain.Group;
using DomainAppUser = global::Domain.Identity.AppUser;
using DomainUserGroup = global::Domain.UserGroup;

namespace Tests.Unit.DAL.Mappers;

public class GroupDALMapper_Tests
{
    private readonly GroupUOWMapper _sut = new();

    [Fact]
    public void Map_DomainToDal_AllFieldsCopied()
    {
        var groupId = Guid.NewGuid();
        var creatorId = Guid.NewGuid();
        var userGroupId = Guid.NewGuid();
        var memberId = Guid.NewGuid();

        var domain = new DomainGroup
        {
            Id = groupId,
            Name = "group",
            IsAzureAdGroup = true,
            UserId = creatorId,
            User = new DomainAppUser
            {
                Id = creatorId,
                FirstName = "first",
                LastName = "last",
                Email = "creator@example.com",
            },
            UserGroups = new List<DomainUserGroup>
            {
                new()
                {
                    Id = userGroupId,
                    UserId = memberId,
                    GroupId = groupId,
                    Role = "developer",
                    User = new DomainAppUser
                    {
                        Id = memberId,
                        FirstName = "member",
                        LastName = "user",
                        Email = "member@example.com",
                    },
                },
            },
        };

        var dal = _sut.Map(domain)!;

        dal.Id.Should().Be(groupId);
        dal.Name.Should().Be("group");
        dal.IsAzureAdGroup.Should().BeTrue();
        dal.CreatorId.Should().Be(creatorId);
        dal.Creator.Should().NotBeNull();
        dal.Creator!.Id.Should().Be(creatorId);
        dal.Creator.FirstName.Should().Be("first");
        dal.Creator.LastName.Should().Be("last");
        dal.Creator.Email.Should().Be("creator@example.com");
        dal.UserGroups.Should().NotBeNull();
        dal.UserGroups!.Count.Should().Be(1);
        dal.UserGroups.First().Id.Should().Be(userGroupId);
        dal.UserGroups.First().UserId.Should().Be(memberId);
        dal.UserGroups.First().GroupId.Should().Be(groupId);
        dal.UserGroups.First().Role.Should().Be("developer");
        dal.UserGroups.First().User.Should().NotBeNull();
        dal.UserGroups.First().User!.Id.Should().Be(memberId);
        dal.UserGroups.First().User!.FirstName.Should().Be("member");
        dal.UserGroups.First().User!.LastName.Should().Be("user");
        dal.UserGroups.First().User!.Email.Should().Be("member@example.com");
    }

    [Fact]
    public void Map_DalToDomain_AllFieldsCopied()
    {
        var dal = new DalGroup
        {
            Id = Guid.NewGuid(),
            Name = "group",
            IsAzureAdGroup = true,
            CreatorId = Guid.NewGuid(),
        };

        var domain = _sut.Map(dal)!;

        domain.Should().NotBeNull();
        domain.Id.Should().Be(dal.Id);
        domain.Name.Should().Be("group");
        domain.IsAzureAdGroup.Should().BeTrue();
        domain.UserId.Should().Be(dal.CreatorId);
    }

    [Fact]
    public void Map_NullDomain_ReturnsNull()
    {
        _sut.Map((DomainGroup?)null).Should().BeNull();
    }

    [Fact]
    public void Map_NullDal_ReturnsNull()
    {
        _sut.Map((DalGroup?)null).Should().BeNull();
    }
}