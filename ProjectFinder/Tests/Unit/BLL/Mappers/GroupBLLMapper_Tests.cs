using AwesomeAssertions;
using BLL.DTO;
using BLL.DTO.Identity;
using BLL.Mappers;
using DalGroup = global::DAL.DTO.Group;
using DalUser = global::DAL.DTO.Identity.AppUser;
using DalUserGroup = global::DAL.DTO.UserGroup;

namespace Tests.Unit.BLL.Mappers;

public class GroupBLLMapper_Tests
{
    private readonly GroupBLLMapper _sut = new();

    [Fact]
    public void Map_DalToBll_AllFieldsCopied()
    {
        var groupId = Guid.NewGuid();
        var creatorId = Guid.NewGuid();
        var userGroupId = Guid.NewGuid();
        var memberId = Guid.NewGuid();

        var dal = new DalGroup
        {
            Id = groupId,
            Name = "group",
            IsAzureAdGroup = true,
            CreatorId = creatorId,
            Creator = new DalUser
            {
                Id = creatorId,
                FirstName = "first",
                LastName = "last",
                Email = "creator@example.com",
            },
            UserGroups = new List<DalUserGroup>
            {
                new()
                {
                    Id = userGroupId,
                    UserId = memberId,
                    GroupId = groupId,
                    Role = "developer",
                    User = new DalUser
                    {
                        Id = memberId,
                        FirstName = "member",
                        LastName = "user",
                        Email = "member@example.com",
                    },
                },
            },
        };

        var bll = _sut.Map(dal)!;

        bll.Id.Should().Be(groupId);
        bll.Name.Should().Be("group");
        bll.IsAzureAdGroup.Should().BeTrue();
        bll.CreatorId.Should().Be(creatorId);
        bll.Creator.Should().NotBeNull();
        bll.Creator!.Id.Should().Be(creatorId);
        bll.Creator.FirstName.Should().Be("first");
        bll.Creator.LastName.Should().Be("last");
        bll.Creator.Email.Should().Be("creator@example.com");
        bll.UserGroups.Should().NotBeNull();
        bll.UserGroups!.Count.Should().Be(1);
        bll.UserGroups.First().Id.Should().Be(userGroupId);
        bll.UserGroups.First().UserId.Should().Be(memberId);
        bll.UserGroups.First().GroupId.Should().Be(groupId);
        bll.UserGroups.First().Role.Should().Be("developer");
        bll.UserGroups.First().User.Should().NotBeNull();
        bll.UserGroups.First().User!.Id.Should().Be(memberId);
        bll.UserGroups.First().User!.FirstName.Should().Be("member");
        bll.UserGroups.First().User!.LastName.Should().Be("user");
        bll.UserGroups.First().User!.Email.Should().Be("member@example.com");
    }

    [Fact]
    public void Map_BllToDal_AllFieldsCopied()
    {
        var groupId = Guid.NewGuid();
        var creatorId = Guid.NewGuid();
        var userGroupId = Guid.NewGuid();
        var memberId = Guid.NewGuid();

        var bll = new Group
        {
            Id = groupId,
            Name = "group",
            IsAzureAdGroup = true,
            CreatorId = creatorId,
            Creator = new AppUser
            {
                Id = creatorId,
                FirstName = "first",
                LastName = "last",
                Email = "creator@example.com",
            },
            UserGroups = new List<UserGroup>
            {
                new()
                {
                    Id = userGroupId,
                    UserId = memberId,
                    GroupId = groupId,
                    Role = "developer",
                    User = new AppUser
                    {
                        Id = memberId,
                        FirstName = "member",
                        LastName = "user",
                        Email = "member@example.com",
                    },
                },
            },
        };

        var dal = _sut.Map(bll)!;

        dal.Id.Should().Be(groupId);
        dal.Name.Should().Be("group");
        dal.IsAzureAdGroup.Should().BeTrue();
        dal.CreatorId.Should().Be(creatorId);
        dal.Creator.Should().BeNull();
        dal.UserGroups.Should().BeNull();
    }

    [Fact]
    public void Map_NullDal_ReturnsNull()
    {
        _sut.Map((DalGroup?)null).Should().BeNull();
    }

    [Fact]
    public void Map_NullBll_ReturnsNull()
    {
        _sut.Map((Group?)null).Should().BeNull();
    }
}