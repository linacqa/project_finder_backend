using AwesomeAssertions;
using BLL.DTO;
using BLL.DTO.Identity;
using BLL.Mappers;
using BLL.Services;
using DAL.Contracts;
using Moq;
using DalInvitation = global::DAL.DTO.Invitation;
using DalGroup = global::DAL.DTO.Group;
using DalUserGroup = global::DAL.DTO.UserGroup;
using DalAppUser = global::DAL.DTO.Identity.AppUser;

namespace Tests.Unit.BLL.Services;

public class InvitationService_Tests
{
    private readonly Mock<IAppUOW> _uow = new();
    private readonly Mock<IInvitationRepository> _repository = new();
    private readonly Mock<IUserGroupRepository> _userGroupRepository = new();
    private readonly Mock<IGroupRepository> _groupRepository = new();
    private readonly InvitationBLLMapper _mapper = new();
    private readonly InvitationService _sut;

    public InvitationService_Tests()
    {
        _uow.SetupGet(u => u.InvitationRepository).Returns(_repository.Object);
        _uow.SetupGet(u => u.UserGroupRepository).Returns(_userGroupRepository.Object);
        _uow.SetupGet(u => u.GroupRepository).Returns(_groupRepository.Object);
        _sut = new InvitationService(_uow.Object, _mapper);
    }

    [Fact]
    public async Task AllAsync_MapsResults()
    {
        var dalItems = new List<DalInvitation>
        {
            new() { Id = Guid.NewGuid(), FromUserId = Guid.NewGuid(), ToUserId = Guid.NewGuid(), GroupId = Guid.NewGuid() },
            new() { Id = Guid.NewGuid(), FromUserId = Guid.NewGuid(), ToUserId = Guid.NewGuid(), GroupId = Guid.NewGuid() },
        };
        _repository.Setup(r => r.AllAsync()).ReturnsAsync(dalItems);

        var bllItems = (await _sut.AllAsync()).ToList();

        bllItems.Should().HaveCount(2);
        _repository.Verify(r => r.AllAsync(), Times.Once);
    }

    [Fact]
    public async Task FindAsync_ReturnsMappedEntity()
    {
        var id = Guid.NewGuid();
        var fromUserId = Guid.NewGuid();
        var toUserId = Guid.NewGuid();
        var groupId = Guid.NewGuid();

        _repository.Setup(r => r.FindAsync(id))
            .ReturnsAsync(new DalInvitation
            {
                Id = id,
                FromUserId = fromUserId,
                ToUserId = toUserId,
                GroupId = groupId,
                Role = "Member"
            });

        var result = await _sut.FindAsync(id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
        result.FromUserId.Should().Be(fromUserId);
        result.Role.Should().Be("Member");
        _repository.Verify(r => r.FindAsync(id), Times.Once);
    }

    [Fact]
    public async Task FindAsync_WhenRepositoryReturnsNull_ReturnsNull()
    {
        var id = Guid.NewGuid();
        _repository.Setup(r => r.FindAsync(id)).ReturnsAsync((DalInvitation?)null);

        var result = await _sut.FindAsync(id);

        result.Should().BeNull();
    }

    [Fact]
    public async Task FindAsync_WithNestedObjects_MapsCompleteStructure()
    {
        var id = Guid.NewGuid();
        var fromUserId = Guid.NewGuid();
        var toUserId = Guid.NewGuid();
        var groupId = Guid.NewGuid();
        var creatorId = Guid.NewGuid();
        var memberId = Guid.NewGuid();

        _repository.Setup(r => r.FindAsync(id))
            .ReturnsAsync(new DalInvitation
            {
                Id = id,
                FromUserId = fromUserId,
                ToUserId = toUserId,
                GroupId = groupId,
                Role = "Admin",
                AcceptedAt = DateTime.UtcNow,
                FromUser = new DalAppUser
                {
                    Id = fromUserId,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john@example.com"
                },
                ToUser = new DalAppUser
                {
                    Id = toUserId,
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane@example.com"
                },
                Group = new DalGroup
                {
                    Id = groupId,
                    Name = "Team A",
                    IsAzureAdGroup = false,
                    CreatorId = creatorId,
                    Creator = new DalAppUser
                    {
                        Id = creatorId,
                        FirstName = "Admin",
                        LastName = "User",
                        Email = "admin@example.com"
                    },
                    UserGroups = new List<DalUserGroup>
                    {
                        new()
                        {
                            Id = Guid.NewGuid(),
                            UserId = memberId,
                            GroupId = groupId,
                            Role = "Member",
                            User = new DalAppUser
                            {
                                Id = memberId,
                                FirstName = "Member",
                                LastName = "User",
                                Email = "member@example.com"
                            }
                        }
                    }
                }
            });

        var result = await _sut.FindAsync(id);

        result.Should().NotBeNull();
        result!.FromUser!.Email.Should().Be("john@example.com");
        result.ToUser!.Email.Should().Be("jane@example.com");
        result.Group!.Creator!.FirstName.Should().Be("Admin");
        result.Group.UserGroups.Should().HaveCount(1);
        result.Group.UserGroups!.First().User!.Email.Should().Be("member@example.com");
    }

    [Fact]
    public void Add_WithValidInput_DelegatesToRepository()
    {
        var userId = Guid.NewGuid();
        var toUserId = Guid.NewGuid();
        var groupId = Guid.NewGuid();
        var bll = new Invitation
        {
            Id = Guid.NewGuid(),
            FromUserId = userId,
            ToUserId = toUserId,
            GroupId = groupId,
            Role = "Member"
        };

        _groupRepository.Setup(r => r.Find(groupId))
            .Returns(new DalGroup
            {
                Id = groupId,
                CreatorId = userId,
                UserGroups = new List<DalUserGroup> { new() }
            });

        _userGroupRepository.Setup(r => r.UserInGroup(toUserId, groupId))
            .Returns(false);

        _sut.Add(bll, userId);

        _repository.Verify(
            r => r.Add(It.Is<DalInvitation>(x => x.Id == bll.Id && x.ToUserId == toUserId), userId),
            Times.Once);
    }

    [Fact]
    public void Add_WhenGroupNotFound_ThrowsInvalidOperationException()
    {
        var userId = Guid.NewGuid();
        var toUserId = Guid.NewGuid();
        var groupId = Guid.NewGuid();
        var bll = new Invitation
        {
            Id = Guid.NewGuid(),
            FromUserId = userId,
            ToUserId = toUserId,
            GroupId = groupId
        };

        _groupRepository.Setup(r => r.Find(groupId))
            .Returns((DalGroup?)null);

        Assert.Throws<InvalidOperationException>(() => _sut.Add(bll, userId));

        _repository.Verify(r => r.Add(It.IsAny<DalInvitation>(), It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public void Add_WhenUserIsNotGroupCreator_ThrowsUnauthorizedAccessException()
    {
        var userId = Guid.NewGuid();
        var creatorId = Guid.NewGuid();
        var toUserId = Guid.NewGuid();
        var groupId = Guid.NewGuid();
        var bll = new Invitation
        {
            Id = Guid.NewGuid(),
            FromUserId = userId,
            ToUserId = toUserId,
            GroupId = groupId
        };

        _groupRepository.Setup(r => r.Find(groupId))
            .Returns(new DalGroup
            {
                Id = groupId,
                CreatorId = creatorId,
                UserGroups = new List<DalUserGroup>()
            });

        Assert.Throws<UnauthorizedAccessException>(() => _sut.Add(bll, userId));

        _repository.Verify(r => r.Add(It.IsAny<DalInvitation>(), It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public void Add_WhenGroupIsFull_ThrowsInvalidOperationException()
    {
        var userId = Guid.NewGuid();
        var toUserId = Guid.NewGuid();
        var groupId = Guid.NewGuid();
        var bll = new Invitation
        {
            Id = Guid.NewGuid(),
            FromUserId = userId,
            ToUserId = toUserId,
            GroupId = groupId
        };

        _groupRepository.Setup(r => r.Find(groupId))
            .Returns(new DalGroup
            {
                Id = groupId,
                CreatorId = userId,
                UserGroups = Enumerable.Range(0, 10).Select(i => new DalUserGroup()).ToList()
            });

        Assert.Throws<InvalidOperationException>(() => _sut.Add(bll, userId));

        _repository.Verify(r => r.Add(It.IsAny<DalInvitation>(), It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public void Add_WhenUserAlreadyInGroup_ThrowsInvalidOperationException()
    {
        var userId = Guid.NewGuid();
        var toUserId = Guid.NewGuid();
        var groupId = Guid.NewGuid();
        var bll = new Invitation
        {
            Id = Guid.NewGuid(),
            FromUserId = userId,
            ToUserId = toUserId,
            GroupId = groupId
        };

        _groupRepository.Setup(r => r.Find(groupId))
            .Returns(new DalGroup
            {
                Id = groupId,
                CreatorId = userId,
                UserGroups = new List<DalUserGroup> { new() }
            });

        _userGroupRepository.Setup(r => r.UserInGroup(toUserId, groupId))
            .Returns(true);

        Assert.Throws<InvalidOperationException>(() => _sut.Add(bll, userId));

        _repository.Verify(r => r.Add(It.IsAny<DalInvitation>(), It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_WhenAcceptingAndValidating_CreatesUserGroupAndReturnsInvitation()
    {
        var invitationId = Guid.NewGuid();
        var toUserId = Guid.NewGuid();
        var fromUserId = Guid.NewGuid();
        var groupId = Guid.NewGuid();

        var bll = new Invitation
        {
            Id = invitationId,
            FromUserId = fromUserId,
            ToUserId = toUserId,
            GroupId = groupId,
            Role = "Member",
            AcceptedAt = DateTime.UtcNow
        };

        var updatedDalEntity = new DalInvitation
        {
            Id = invitationId,
            FromUserId = fromUserId,
            ToUserId = toUserId,
            GroupId = groupId,
            Role = "Member",
            AcceptedAt = DateTime.UtcNow
        };

        _repository.Setup(r => r.UpdateAsync(It.IsAny<DalInvitation>(), It.IsAny<Guid>()))
            .ReturnsAsync(updatedDalEntity);

        _userGroupRepository.Setup(r => r.UserInGroup(toUserId, groupId))
            .Returns(false);

        _groupRepository.Setup(r => r.FindAsync(groupId))
            .ReturnsAsync(new DalGroup
            {
                Id = groupId,
                UserGroups = new List<DalUserGroup> { new() }
            });

        var result = await _sut.UpdateAsync(bll, toUserId);

        result!.Id.Should().Be(invitationId);
        _userGroupRepository.Verify(
            r => r.Add(
                It.Is<DalUserGroup>(x =>
                    x.UserId == toUserId &&
                    x.GroupId == groupId &&
                    x.Role == "Member"),
                toUserId),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhenNotRecipient_ThrowsUnauthorizedAccessException()
    {
        var invitationId = Guid.NewGuid();
        var toUserId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();
        var groupId = Guid.NewGuid();

        var bll = new Invitation
        {
            Id = invitationId,
            FromUserId = Guid.NewGuid(),
            ToUserId = toUserId,
            GroupId = groupId,
            AcceptedAt = DateTime.UtcNow
        };

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _sut.UpdateAsync(bll, otherUserId));

        _repository.Verify(r => r.UpdateAsync(It.IsAny<DalInvitation>(), It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_WhenAcceptingButAlreadyMember_ThrowsInvalidOperationException()
    {
        var invitationId = Guid.NewGuid();
        var toUserId = Guid.NewGuid();
        var groupId = Guid.NewGuid();

        var bll = new Invitation
        {
            Id = invitationId,
            FromUserId = Guid.NewGuid(),
            ToUserId = toUserId,
            GroupId = groupId,
            AcceptedAt = DateTime.UtcNow
        };

        _userGroupRepository.Setup(r => r.UserInGroup(toUserId, groupId))
            .Returns(true);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _sut.UpdateAsync(bll, toUserId));

        _repository.Verify(r => r.UpdateAsync(It.IsAny<DalInvitation>(), It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_WhenAcceptingButGroupFull_ThrowsInvalidOperationException()
    {
        var invitationId = Guid.NewGuid();
        var toUserId = Guid.NewGuid();
        var groupId = Guid.NewGuid();

        var bll = new Invitation
        {
            Id = invitationId,
            FromUserId = Guid.NewGuid(),
            ToUserId = toUserId,
            GroupId = groupId,
            AcceptedAt = DateTime.UtcNow
        };

        _userGroupRepository.Setup(r => r.UserInGroup(toUserId, groupId))
            .Returns(false);

        _groupRepository.Setup(r => r.FindAsync(groupId))
            .ReturnsAsync(new DalGroup
            {
                Id = groupId,
                UserGroups = Enumerable.Range(0, 10).Select(i => new DalUserGroup()).ToList()
            });

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _sut.UpdateAsync(bll, toUserId));

        _repository.Verify(r => r.UpdateAsync(It.IsAny<DalInvitation>(), It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_WhenDeclining_DoesNotCreateUserGroup()
    {
        var invitationId = Guid.NewGuid();
        var toUserId = Guid.NewGuid();
        var groupId = Guid.NewGuid();

        var bll = new Invitation
        {
            Id = invitationId,
            FromUserId = Guid.NewGuid(),
            ToUserId = toUserId,
            GroupId = groupId,
            DeclinedAt = DateTime.UtcNow
        };

        var updatedDalEntity = new DalInvitation
        {
            Id = invitationId,
            ToUserId = toUserId,
            GroupId = groupId,
            DeclinedAt = DateTime.UtcNow,
            AcceptedAt = null
        };

        _groupRepository.Setup(r => r.FindAsync(groupId))
            .ReturnsAsync(new DalGroup
            {
                Id = groupId,
                UserGroups = new List<DalUserGroup> { new() }
            });

        _repository.Setup(r => r.UpdateAsync(It.IsAny<DalInvitation>(), It.IsAny<Guid>()))
            .ReturnsAsync(updatedDalEntity);

        var result = await _sut.UpdateAsync(bll, toUserId);

        result!.Id.Should().Be(invitationId);
        _userGroupRepository.Verify(
            r => r.Add(It.IsAny<DalUserGroup>(), It.IsAny<Guid>()),
            Times.Never);
    }

    [Fact]
    public void Remove_WhenUserIsSenderAndNotAccepted_RemovesInvitation()
    {
        var invitationId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var bll = new Invitation { Id = invitationId, FromUserId = userId };

        _repository.Setup(r => r.Find(invitationId, userId))
            .Returns(new DalInvitation
            {
                Id = invitationId,
                FromUserId = userId,
                AcceptedAt = null
            });

        _sut.Remove(bll, userId);

        _repository.Verify(
            r => r.Remove(It.Is<DalInvitation>(x => x.Id == invitationId), userId),
            Times.Once);
    }

    [Fact]
    public void Remove_WhenUserIsNotSender_ThrowsUnauthorizedAccessException()
    {
        var invitationId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var senderId = Guid.NewGuid();
        var bll = new Invitation { Id = invitationId, FromUserId = senderId };

        _repository.Setup(r => r.Find(invitationId, userId))
            .Returns(new DalInvitation
            {
                Id = invitationId,
                FromUserId = senderId,
                AcceptedAt = null
            });

        Assert.Throws<UnauthorizedAccessException>(() => _sut.Remove(bll, userId));

        _repository.Verify(r => r.Remove(It.IsAny<DalInvitation>(), It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public void Remove_WhenInvitationIsAccepted_ThrowsInvalidOperationException()
    {
        var invitationId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var bll = new Invitation { Id = invitationId, FromUserId = userId };

        _repository.Setup(r => r.Find(invitationId, userId))
            .Returns(new DalInvitation
            {
                Id = invitationId,
                FromUserId = userId,
                AcceptedAt = DateTime.UtcNow
            });

        Assert.Throws<InvalidOperationException>(() => _sut.Remove(bll, userId));

        _repository.Verify(r => r.Remove(It.IsAny<DalInvitation>(), It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task RemoveAsync_WhenUserIsSenderAndNotAccepted_RemovesInvitation()
    {
        var invitationId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _repository.Setup(r => r.FindAsync(invitationId, userId))
            .ReturnsAsync(new DalInvitation
            {
                Id = invitationId,
                FromUserId = userId,
                AcceptedAt = null
            });

        await _sut.RemoveAsync(invitationId, userId);

        _repository.Verify(r => r.RemoveAsync(invitationId, userId), Times.Once);
    }

    [Fact]
    public async Task RemoveAsync_WhenUserIsNotSender_ThrowsUnauthorizedAccessException()
    {
        var invitationId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var senderId = Guid.NewGuid();

        _repository.Setup(r => r.FindAsync(invitationId, userId))
            .ReturnsAsync(new DalInvitation
            {
                Id = invitationId,
                FromUserId = senderId,
                AcceptedAt = null
            });

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _sut.RemoveAsync(invitationId, userId));

        _repository.Verify(r => r.RemoveAsync(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task RemoveAsync_WhenInvitationIsAccepted_ThrowsInvalidOperationException()
    {
        var invitationId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _repository.Setup(r => r.FindAsync(invitationId, userId))
            .ReturnsAsync(new DalInvitation
            {
                Id = invitationId,
                FromUserId = userId,
                AcceptedAt = DateTime.UtcNow
            });

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _sut.RemoveAsync(invitationId, userId));

        _repository.Verify(r => r.RemoveAsync(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task AllAsyncToUser_MapsResults()
    {
        var userId = Guid.NewGuid();
        var dalItems = new List<DalInvitation>
        {
            new() { Id = Guid.NewGuid(), ToUserId = userId, FromUserId = Guid.NewGuid(), GroupId = Guid.NewGuid() },
            new() { Id = Guid.NewGuid(), ToUserId = userId, FromUserId = Guid.NewGuid(), GroupId = Guid.NewGuid() },
        };

        _repository.Setup(r => r.AllAsyncToUser(userId))
            .ReturnsAsync(dalItems);

        var bllItems = (await _sut.AllAsyncToUser(userId)).ToList();

        bllItems.Should().HaveCount(2);
        _repository.Verify(r => r.AllAsyncToUser(userId), Times.Once);
    }

    [Fact]
    public async Task AllAsyncByGroupId_WhenGroupNotFound_ThrowsInvalidOperationException()
    {
        var groupId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _groupRepository.Setup(r => r.FindAsync(groupId))
            .ReturnsAsync((DalGroup?)null);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _sut.AllAsyncByGroupId(groupId, userId));

        _repository.Verify(r => r.AllAsyncByGroupId(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task AllAsyncByGroupId_WhenUserNotInGroup_ThrowsUnauthorizedAccessException()
    {
        var groupId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _groupRepository.Setup(r => r.FindAsync(groupId))
            .ReturnsAsync(new DalGroup { Id = groupId });

        _userGroupRepository.Setup(r => r.UserInGroup(userId, groupId))
            .Returns(false);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _sut.AllAsyncByGroupId(groupId, userId));

        _repository.Verify(r => r.AllAsyncByGroupId(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task AllAsyncByGroupId_WhenAuthorized_MapsResults()
    {
        var groupId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _groupRepository.Setup(r => r.FindAsync(groupId))
            .ReturnsAsync(new DalGroup { Id = groupId });

        _userGroupRepository.Setup(r => r.UserInGroup(userId, groupId))
            .Returns(true);

        var dalItems = new List<DalInvitation>
        {
            new() { Id = Guid.NewGuid(), GroupId = groupId, FromUserId = Guid.NewGuid(), ToUserId = Guid.NewGuid() },
            new() { Id = Guid.NewGuid(), GroupId = groupId, FromUserId = Guid.NewGuid(), ToUserId = Guid.NewGuid() },
        };

        _repository.Setup(r => r.AllAsyncByGroupId(groupId, userId))
            .ReturnsAsync(dalItems);

        var bllItems = (await _sut.AllAsyncByGroupId(groupId, userId)).ToList();

        bllItems.Should().HaveCount(2);
        _repository.Verify(r => r.AllAsyncByGroupId(groupId, userId), Times.Once);
    }
}
