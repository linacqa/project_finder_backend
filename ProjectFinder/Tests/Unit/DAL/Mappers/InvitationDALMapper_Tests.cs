using AwesomeAssertions;
using DAL.EF.Mappers;

using DalInvitation = global::DAL.DTO.Invitation;
using DalAppUser = global::DAL.DTO.Identity.AppUser;

using DomainInvitation = global::Domain.Invitation;
using DomainAppUser = global::Domain.Identity.AppUser;
using DomainGroup = global::Domain.Group;

namespace Tests.Unit.DAL.Mappers;

public class InvitationDALMapper_Tests
{
    private readonly InvitationUOWMapper _sut = new();

    [Fact]
    public void Map_DomainToDal_AllFieldsCopied()
    {
        var groupId = Guid.NewGuid();
        var fromUserId = Guid.NewGuid();
        var toUserId = Guid.NewGuid();
        var userGroupId = Guid.NewGuid();
        var memberId = Guid.NewGuid();

        var domain = new DomainInvitation
        {
            Id = Guid.NewGuid(),
            GroupId = groupId,
            Group = new DomainGroup
            {
                Id = groupId,
                Name = "group-name",
                IsAzureAdGroup = true,
                UserId = fromUserId,
                User = new DomainAppUser
                {
                    Id = fromUserId,
                    FirstName = "creator-first",
                    LastName = "creator-last",
                    Email = "creator@example.com",
                },
                UserGroups = new List<global::Domain.UserGroup>
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
                            FirstName = "member-first",
                            LastName = "member-last",
                            Email = "member@example.com",
                        },
                    }
                },
            },
            ToUserId = toUserId,
            ToUser = new DomainAppUser
            {
                Id = toUserId,
                FirstName = "to-first",
                LastName = "to-last",
                Email = "to@example.com",
            },
            UserId = fromUserId,
            User = new DomainAppUser
            {
                Id = fromUserId,
                FirstName = "from-first",
                LastName = "from-last",
                Email = "from@example.com",
            },
            AcceptedAt = DateTime.UtcNow,
            DeclinedAt = null,
            Role = "member",
        };

        var dal = _sut.Map(domain)!;

        dal.Should().NotBeNull();
        dal.Id.Should().Be(domain.Id);
        dal.GroupId.Should().Be(groupId);
        dal.Group.Should().NotBeNull();
        dal.Group!.Id.Should().Be(groupId);
        dal.Group.Name.Should().Be("group-name");
        dal.Group.IsAzureAdGroup.Should().BeTrue();
        dal.Group.UserGroups.Should().NotBeNull();
        dal.Group.UserGroups!.Count.Should().Be(1);
        dal.Group.UserGroups.First().Id.Should().Be(userGroupId);
        dal.Group.UserGroups.First().UserId.Should().Be(memberId);
        dal.Group.UserGroups.First().GroupId.Should().Be(groupId);
        dal.Group.UserGroups.First().Role.Should().Be("developer");
        dal.Group.UserGroups.First().User.Should().NotBeNull();
        dal.Group.UserGroups.First().User!.Id.Should().Be(memberId);
        dal.Group.UserGroups.First().User!.FirstName.Should().Be("member-first");
        dal.Group.UserGroups.First().User!.LastName.Should().Be("member-last");
        dal.Group.UserGroups.First().User!.Email.Should().Be("member@example.com");
        dal.ToUserId.Should().Be(toUserId);
        dal.ToUser.Should().NotBeNull();
        dal.ToUser!.Id.Should().Be(toUserId);
        dal.FromUserId.Should().Be(fromUserId);
        dal.FromUser.Should().NotBeNull();
        dal.FromUser!.Id.Should().Be(fromUserId);
        dal.AcceptedAt.Should().Be(domain.AcceptedAt);
        dal.DeclinedAt.Should().BeNull();
        dal.Role.Should().Be("member");
    }

    [Fact]
    public void Map_DalToDomain_AllFieldsCopied()
    {
        var dal = new DalInvitation
        {
            Id = Guid.NewGuid(),
            GroupId = Guid.NewGuid(),
            ToUserId = Guid.NewGuid(),
            FromUserId = Guid.NewGuid(),
            AcceptedAt = DateTime.UtcNow,
            DeclinedAt = null,
            Role = "member",
        };

        var domain = _sut.Map(dal)!;

        domain.Should().NotBeNull();
        domain.Id.Should().Be(dal.Id);
        domain.GroupId.Should().Be(dal.GroupId);
        domain.ToUserId.Should().Be(dal.ToUserId);
        domain.UserId.Should().Be(dal.FromUserId);
        domain.AcceptedAt.Should().Be(dal.AcceptedAt);
        domain.DeclinedAt.Should().BeNull();
        domain.Role.Should().Be("member");
    }

    [Fact]
    public void Map_NullDomain_ReturnsNull()
    {
        _sut.Map((DomainInvitation?)null).Should().BeNull();
    }

    [Fact]
    public void Map_NullDal_ReturnsNull()
    {
        _sut.Map((DalInvitation?)null).Should().BeNull();
    }
}