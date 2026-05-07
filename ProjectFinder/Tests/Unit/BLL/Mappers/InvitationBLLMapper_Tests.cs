using AwesomeAssertions;
using BLL.DTO;
using BLL.DTO.Identity;
using BLL.Mappers;
using DalInvitation = global::DAL.DTO.Invitation;
using DalGroup = global::DAL.DTO.Group;
using DalUser = global::DAL.DTO.Identity.AppUser;
using DalUserGroup = global::DAL.DTO.UserGroup;

namespace Tests.Unit.BLL.Mappers;

public class InvitationBLLMapper_Tests
{
    private readonly InvitationBLLMapper _sut = new();

    [Fact]
    public void Map_DalToBll_AllFieldsCopied()
    {
        var invitationId = Guid.NewGuid();
        var groupId = Guid.NewGuid();
        var creatorId = Guid.NewGuid();
        var userGroupId = Guid.NewGuid();
        var memberId = Guid.NewGuid();
        var fromUserId = Guid.NewGuid();
        var toUserId = Guid.NewGuid();
        var acceptedAt = DateTime.UtcNow;
        var declinedAt = acceptedAt.AddHours(1);

        var dal = new DalInvitation
        {
            Id = invitationId,
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
                    FirstName = "creator",
                    LastName = "user",
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
            },
            FromUserId = fromUserId,
            FromUser = new DalUser
            {
                Id = fromUserId,
                FirstName = "from",
                LastName = "user",
                Email = "from@example.com",
            },
            ToUserId = toUserId,
            ToUser = new DalUser
            {
                Id = toUserId,
                FirstName = "to",
                LastName = "user",
                Email = "to@example.com",
            },
            Role = "developer",
            AcceptedAt = acceptedAt,
            DeclinedAt = declinedAt,
        };

        var bll = _sut.Map(dal)!;

        bll.Id.Should().Be(invitationId);
        bll.GroupId.Should().Be(groupId);
        bll.Group.Should().NotBeNull();
        bll.Group!.Id.Should().Be(groupId);
        bll.Group.Name.Should().Be("group");
        bll.Group.IsAzureAdGroup.Should().BeFalse();
        bll.Group.CreatorId.Should().Be(creatorId);
        bll.Group.Creator.Should().NotBeNull();
        bll.Group.Creator!.Id.Should().Be(creatorId);
        bll.Group.Creator.FirstName.Should().Be("creator");
        bll.Group.Creator.LastName.Should().Be("user");
        bll.Group.Creator.Email.Should().Be("creator@example.com");
        bll.Group.UserGroups.Should().NotBeNull();
        bll.Group.UserGroups!.Count.Should().Be(1);
        bll.Group.UserGroups.First().Id.Should().Be(userGroupId);
        bll.Group.UserGroups.First().UserId.Should().Be(memberId);
        bll.Group.UserGroups.First().GroupId.Should().Be(groupId);
        bll.Group.UserGroups.First().Role.Should().Be("developer");
        bll.Group.UserGroups.First().User.Should().NotBeNull();
        bll.Group.UserGroups.First().User!.Id.Should().Be(memberId);
        bll.Group.UserGroups.First().User!.FirstName.Should().Be("member");
        bll.Group.UserGroups.First().User!.LastName.Should().Be("user");
        bll.Group.UserGroups.First().User!.Email.Should().Be("member@example.com");
        bll.FromUserId.Should().Be(fromUserId);
        bll.FromUser.Should().NotBeNull();
        bll.FromUser!.Id.Should().Be(fromUserId);
        bll.FromUser.FirstName.Should().Be("from");
        bll.FromUser.LastName.Should().Be("user");
        bll.FromUser.Email.Should().Be("from@example.com");
        bll.ToUserId.Should().Be(toUserId);
        bll.ToUser.Should().NotBeNull();
        bll.ToUser!.Id.Should().Be(toUserId);
        bll.ToUser.FirstName.Should().Be("to");
        bll.ToUser.LastName.Should().Be("user");
        bll.ToUser.Email.Should().Be("to@example.com");
        bll.Role.Should().Be("developer");
        bll.AcceptedAt.Should().Be(acceptedAt);
        bll.DeclinedAt.Should().Be(declinedAt);
    }

    [Fact]
    public void Map_BllToDal_AllFieldsCopied()
    {
        var invitationId = Guid.NewGuid();
        var groupId = Guid.NewGuid();
        var fromUserId = Guid.NewGuid();
        var toUserId = Guid.NewGuid();
        var acceptedAt = DateTime.UtcNow;
        var declinedAt = acceptedAt.AddHours(1);

        var bll = new Invitation
        {
            Id = invitationId,
            GroupId = groupId,
            Group = new Group
            {
                Id = groupId,
                Name = "group",
                IsAzureAdGroup = false,
            },
            FromUserId = fromUserId,
            FromUser = new AppUser
            {
                Id = fromUserId,
                FirstName = "from",
                LastName = "user",
                Email = "from@example.com",
            },
            ToUserId = toUserId,
            ToUser = new AppUser
            {
                Id = toUserId,
                FirstName = "to",
                LastName = "user",
                Email = "to@example.com",
            },
            Role = "developer",
            AcceptedAt = acceptedAt,
            DeclinedAt = declinedAt,
        };

        var dal = _sut.Map(bll)!;

        dal.Id.Should().Be(invitationId);
        dal.GroupId.Should().Be(groupId);
        dal.FromUserId.Should().Be(fromUserId);
        dal.ToUserId.Should().Be(toUserId);
        dal.Role.Should().Be("developer");
        dal.AcceptedAt.Should().Be(acceptedAt);
        dal.DeclinedAt.Should().Be(declinedAt);
        dal.Group.Should().BeNull();
        dal.FromUser.Should().BeNull();
        dal.ToUser.Should().BeNull();
    }

    [Fact]
    public void Map_NullDal_ReturnsNull()
    {
        _sut.Map((DalInvitation?)null).Should().BeNull();
    }

    [Fact]
    public void Map_NullBll_ReturnsNull()
    {
        _sut.Map((Invitation?)null).Should().BeNull();
    }
}