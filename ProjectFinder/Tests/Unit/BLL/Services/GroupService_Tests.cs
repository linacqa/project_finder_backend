using AwesomeAssertions;
using BLL.DTO;
using BLL.Mappers;
using BLL.Services;
using DAL.Contracts;
using Moq;
using DalAppUser = global::DAL.DTO.Identity.AppUser;
using DalGroup = global::DAL.DTO.Group;
using DalUserGroup = global::DAL.DTO.UserGroup;

namespace Tests.Unit.BLL.Services;

public class GroupService_Tests
{
    private readonly Mock<IAppUOW> _uow = new();
    private readonly Mock<IGroupRepository> _repository = new();
    private readonly Mock<IUserGroupRepository> _userGroupRepository = new();
    private readonly GroupBLLMapper _mapper = new();
    private readonly GroupService _sut;

    public GroupService_Tests()
    {
        _uow.SetupGet(u => u.GroupRepository).Returns(_repository.Object);
        _uow.SetupGet(u => u.UserGroupRepository).Returns(_userGroupRepository.Object);
        _sut = new GroupService(_uow.Object, _mapper);
    }

    [Fact]
    public async Task AllAsync_MapsResults()
    {
        var dalItems = new List<DalGroup>
        {
            new() { Id = Guid.NewGuid(), Name = "Alpha Group", IsAzureAdGroup = false },
            new() { Id = Guid.NewGuid(), Name = "Beta Group", IsAzureAdGroup = true },
        };
        _repository.Setup(r => r.AllAsync()).ReturnsAsync(dalItems);

        var bllItems = (await _sut.AllAsync()).ToList();

        bllItems.Should().HaveCount(2);
        bllItems[0].Name.Should().Be("Alpha Group");
        bllItems[0].IsAzureAdGroup.Should().BeFalse();
        bllItems[1].Name.Should().Be("Beta Group");
        bllItems[1].IsAzureAdGroup.Should().BeTrue();
        _repository.Verify(r => r.AllAsync(), Times.Once);
    }

    [Fact]
    public async Task AllAsync_WithUserId_MapsResults()
    {
        var userId = Guid.NewGuid();
        var dalItems = new List<DalGroup>
        {
            new() { Id = Guid.NewGuid(), Name = "Group 1", CreatorId = userId },
        };
        _repository.Setup(r => r.AllAsync(userId)).ReturnsAsync(dalItems);

        var bllItems = (await _sut.AllAsync(userId)).ToList();

        bllItems.Should().HaveCount(1);
        bllItems[0].CreatorId.Should().Be(userId);
        _repository.Verify(r => r.AllAsync(userId), Times.Once);
    }

    [Fact]
    public async Task FindAsync_ReturnsMappedEntity()
    {
        var id = Guid.NewGuid();
        var creatorId = Guid.NewGuid();
        _repository.Setup(r => r.FindAsync(id))
            .ReturnsAsync(new DalGroup
            {
                Id = id,
                Name = "Test Group",
                CreatorId = creatorId,
                IsAzureAdGroup = false,
                Creator = new DalAppUser
                {
                    Id = creatorId,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john@example.com"
                }
            });

        var result = await _sut.FindAsync(id);

        result.Should().NotBeNull();
        var mapped = result ?? throw new InvalidOperationException();
        var creator = mapped.Creator ?? throw new InvalidOperationException();

        mapped.Id.Should().Be(id);
        mapped.Name.Should().Be("Test Group");
        creator.FirstName.Should().Be("John");
        creator.Email.Should().Be("john@example.com");
        _repository.Verify(r => r.FindAsync(id), Times.Once);
    }

    [Fact]
    public async Task FindAsync_WhenRepositoryReturnsNull_ReturnsNull()
    {
        var id = Guid.NewGuid();
        _repository.Setup(r => r.FindAsync(id)).ReturnsAsync((DalGroup?)null);

        var result = await _sut.FindAsync(id);

        result.Should().BeNull();
    }

    [Fact]
    public async Task FindAsync_WithUserGroups_MapsBothGroupAndMembers()
    {
        var groupId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var memberId = Guid.NewGuid();

        _repository.Setup(r => r.FindAsync(groupId))
            .ReturnsAsync(new DalGroup
            {
                Id = groupId,
                Name = "Team Group",
                CreatorId = userId,
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
                            FirstName = "Jane",
                            LastName = "Smith",
                            Email = "jane@example.com"
                        }
                    }
                }
            });

        var result = await _sut.FindAsync(groupId);

        result.Should().NotBeNull();
        var mapped = result ?? throw new InvalidOperationException();
        var userGroups = mapped.UserGroups ?? throw new InvalidOperationException();
        var userGroup = userGroups.First();
        var user = userGroup.User ?? throw new InvalidOperationException();

        userGroups.Should().HaveCount(1);
        user.FirstName.Should().Be("Jane");
        userGroup.Role.Should().Be("Member");
    }

    [Fact]
    public void Add_DelegatesToRepository()
    {
        var groupId = Guid.NewGuid();
        var creatorId = Guid.NewGuid();
        var bll = new Group
        {
            Id = groupId,
            Name = "New Group",
            IsAzureAdGroup = false,
            CreatorId = creatorId
        };

        _sut.Add(bll);

        _repository.Verify(
            r => r.Add(It.Is<DalGroup>(x => x.Id == groupId && x.Name == "New Group"), default),
            Times.Once);
    }

    [Fact]
    public void Add_WithCreatorRole_CreatesUserGroupAndAddsToGroup()
    {
        var groupId = Guid.NewGuid();
        var creatorId = Guid.NewGuid();
        var bll = new Group
        {
            Id = groupId,
            Name = "New Group",
            IsAzureAdGroup = false,
            CreatorId = creatorId,
            CreatorRoleInGroup = "Admin"
        };

        _sut.Add(bll, creatorId);

        _repository.Verify(
            r => r.Add(It.Is<DalGroup>(x => x.Id == groupId && x.Name == "New Group"), creatorId),
            Times.Once);
        
        _userGroupRepository.Verify(
            r => r.Add(
                It.Is<DalUserGroup>(x =>
                    x.UserId == creatorId &&
                    x.GroupId == groupId &&
                    x.Role == "Admin"),
                creatorId),
            Times.Once);
    }

    [Fact]
    public void Add_WithNullCreatorRole_DoesNotCreateUserGroup()
    {
        var groupId = Guid.NewGuid();
        var creatorId = Guid.NewGuid();
        var bll = new Group
        {
            Id = groupId,
            Name = "New Group",
            IsAzureAdGroup = false,
            CreatorId = creatorId,
            CreatorRoleInGroup = null
        };

        _sut.Add(bll, creatorId);

        _repository.Verify(
            r => r.Add(It.IsAny<DalGroup>(), creatorId),
            Times.Once);
        
        _userGroupRepository.Verify(
            r => r.Add(It.IsAny<DalUserGroup>(), It.IsAny<Guid>()),
            Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_DelegatesToRepository_AndReturnsMapped()
    {
        var id = Guid.NewGuid();
        var bll = new Group { Id = id, Name = "Updated Group", IsAzureAdGroup = false };
        
        _repository.Setup(r => r.UpdateAsync(It.IsAny<DalGroup>()))
            .ReturnsAsync((DalGroup e, Guid _) => e);

        var result = await _sut.UpdateAsync(bll);

        result!.Id.Should().Be(id);
        result.Name.Should().Be("Updated Group");
        _repository.Verify(r => r.UpdateAsync(It.IsAny<DalGroup>()), Times.Once);
    }

    [Fact]
    public async Task RemoveAsync_DelegatesToRepository()
    {
        var id = Guid.NewGuid();

        _repository.Setup(r => r.FindAsync(id))
            .ReturnsAsync(new DalGroup { Id = id, Name = "Group to Remove" });

        await _sut.RemoveAsync(id);

        _repository.Verify(r => r.FindAsync(id), Times.Once);
        _repository.Verify(r => r.RemoveAsync(id, default), Times.Once);
    }

    [Fact]
    public void Remove_DelegatesToRepository()
    {
        var id = Guid.NewGuid();
        var bll = new Group { Id = id, Name = "Group to Remove" };

        _repository.Setup(r => r.Find(id, default))
            .Returns(new DalGroup { Id = id, Name = "Group to Remove" });

        _sut.Remove(bll);

        _repository.Verify(
            r => r.Find(id, default),
            Times.Once);
        _repository.Verify(
            r => r.Remove(It.Is<DalGroup>(x => x.Id == id), default),
            Times.Once);
    }

    [Fact]
    public async Task AllAsyncMatchingTeamSize_MapsResults()
    {
        var minStudents = 3;
        var maxStudents = 5;
        var userId = Guid.NewGuid();
        
        var dalItems = new List<DalGroup>
        {
            new() { Id = Guid.NewGuid(), Name = "Team A", IsAzureAdGroup = false, CreatorId = userId },
            new() { Id = Guid.NewGuid(), Name = "Team B", IsAzureAdGroup = false, CreatorId = Guid.NewGuid() },
        };
        
        _repository.Setup(r => r.AllAsyncMatchingTeamSize(minStudents, maxStudents, userId))
            .ReturnsAsync(dalItems);

        var bllItems = (await _sut.AllAsyncMatchingTeamSize(minStudents, maxStudents, userId)).ToList();

        bllItems.Should().HaveCount(2);
        bllItems[0].Name.Should().Be("Team A");
        bllItems[1].Name.Should().Be("Team B");
        _repository.Verify(
            r => r.AllAsyncMatchingTeamSize(minStudents, maxStudents, userId),
            Times.Once);
    }

    [Fact]
    public async Task AllAsyncMatchingTeamSize_WithoutUserId_MapsResults()
    {
        var minStudents = 2;
        var maxStudents = 4;
        
        var dalItems = new List<DalGroup>
        {
            new() { Id = Guid.NewGuid(), Name = "Group 1", IsAzureAdGroup = true },
        };
        
        _repository.Setup(r => r.AllAsyncMatchingTeamSize(minStudents, maxStudents, default))
            .ReturnsAsync(dalItems);

        var bllItems = (await _sut.AllAsyncMatchingTeamSize(minStudents, maxStudents)).ToList();

        bllItems.Should().HaveCount(1);
        bllItems[0].IsAzureAdGroup.Should().BeTrue();
        _repository.Verify(
            r => r.AllAsyncMatchingTeamSize(minStudents, maxStudents, default),
            Times.Once);
    }

    [Fact]
    public async Task AllAsyncMatchingTeamSize_ReturnsEmptyList_WhenNoGroupsMatch()
    {
        var minStudents = 10;
        var maxStudents = 20;
        
        _repository.Setup(r => r.AllAsyncMatchingTeamSize(minStudents, maxStudents, default))
            .ReturnsAsync(new List<DalGroup>());

        var bllItems = (await _sut.AllAsyncMatchingTeamSize(minStudents, maxStudents)).ToList();

        bllItems.Should().BeEmpty();
    }
}
