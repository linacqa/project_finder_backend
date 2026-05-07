using AwesomeAssertions;
using Base.Contracts;
using DAL.EF;
using DAL.EF.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Tests.Helpers;
using DalGroup = global::DAL.DTO.Group;

namespace Tests.Unit.DAL.Repositories;

public class GroupRepository_Tests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly GroupRepository _sut;

    public GroupRepository_Tests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("DataSource=:memory:")
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution)
            .Options;

        var userNameResolverMock = new Mock<IUserNameResolver>();
        userNameResolverMock.Setup(e => e.CurrentUserName).Returns("test");

        var loggerMock = new Mock<ILogger<AppDbContext>>();

        _context = new AppDbContext(options, userNameResolverMock.Object, loggerMock.Object);
        _context.Database.OpenConnection();
        _context.Database.EnsureCreated();

        _sut = new GroupRepository(_context);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task AllAsync_ReturnsOnlyNonAzureAdGroups()
    {
        var user = CreateUser();
        var userId = user.Id;
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var group1 = CreateDomainGroup("group1", userId);
        var group2 = CreateDomainGroup("group2", userId);
        var azureGroup = CreateDomainGroup("azure-group", userId, isAzureAdGroup: true);

        _context.Groups.AddRange(group1, group2, azureGroup);
        await _context.SaveChangesAsync();

        var result = (await _sut.AllAsync()).ToList();

        result.Should().HaveCount(2);
        result.Should().Contain(g =>
            g.Id == group1.Id &&
            g.Name == "group1" &&
            g.CreatorId == userId &&
            g.IsAzureAdGroup == false);

        result.Should().Contain(g =>
            g.Id == group2.Id &&
            g.Name == "group2" &&
            g.CreatorId == userId &&
            g.IsAzureAdGroup == false);

        result.Should().NotContain(g => g.Id == azureGroup.Id);
    }

    [Fact]
    public void All_ReturnsOnlyNonAzureAdGroups()
    {
        var user = CreateUser();
        var userId = user.Id;
        _context.Users.Add(user);
        _context.SaveChangesAsync();

        var group1 = CreateDomainGroup("group1", userId);
        var group2 = CreateDomainGroup("group2", userId);
        var azureGroup = CreateDomainGroup("azure-group", userId, isAzureAdGroup: true);

        _context.Groups.AddRange(group1, group2, azureGroup);
        _context.SaveChangesAsync();

        var result = _sut.All().ToList();

        result.Should().HaveCount(2);
        result.Should().Contain(g => g.Id == group1.Id && g.Name == "group1");
        result.Should().Contain(g => g.Id == group2.Id && g.Name == "group2");
        result.Should().NotContain(g => g.Id == azureGroup.Id);
    }

    [Fact]
    public async Task AllAsync_WithEmptyRepository_ReturnsEmptyCollection()
    {
        var result = (await _sut.AllAsync()).ToList();

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task FindAsync_WithValidId_ReturnsGroup()
    {
        var user = CreateUser();
        var userId = user.Id;
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        var group = CreateDomainGroup("test-group", userId);

        _context.Groups.Add(group);
        await _context.SaveChangesAsync();

        var result = await _sut.FindAsync(group.Id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(group.Id);
        result.Name.Should().Be("test-group");
        result.CreatorId.Should().Be(userId);
    }

    [Fact]
    public void Find_WithValidId_ReturnsGroup()
    {
        var user = CreateUser();
        var userId = user.Id;
        _context.Users.Add(user);
        _context.SaveChangesAsync();
        var group = CreateDomainGroup("test-group", userId);

        _context.Groups.Add(group);
        _context.SaveChangesAsync();

        var result = _sut.Find(group.Id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(group.Id);
        result.Name.Should().Be("test-group");
        result.CreatorId.Should().Be(userId);
    }

    [Fact]
    public async Task FindAsync_WithInvalidId_ReturnsNull()
    {
        var result = await _sut.FindAsync(Guid.NewGuid());

        result.Should().BeNull();
    }

    [Fact]
    public void Find_WithInvalidId_ReturnsNull()
    {
        var result = _sut.Find(Guid.NewGuid());

        result.Should().BeNull();
    }

    [Fact]
    public async Task FindAsync_WithAzureAdGroup_ReturnsNull()
    {
        var user = CreateUser();
        var userId = user.Id;
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        var group = CreateDomainGroup("azure-group", userId, isAzureAdGroup: true);

        _context.Groups.Add(group);
        await _context.SaveChangesAsync();

        var result = await _sut.FindAsync(group.Id);

        result.Should().BeNull();
    }

    [Fact]
    public void Add_AddsGroupToRepository()
    {
        var user = CreateUser();
        var userId = user.Id;
        _context.Users.Add(user);
        _context.SaveChangesAsync();

        var dalGroup = new DalGroup
        {
            Id = Guid.NewGuid(),
            Name = "new-group",
            CreatorId = userId,
            IsAzureAdGroup = false
        };

        _sut.Add(dalGroup, userId);
        _context.SaveChangesAsync();

        var result = _context.Groups.FirstOrDefault(g => g.Id == dalGroup.Id);

        result.Should().NotBeNull();
        result!.Name.Should().Be("new-group");
        result.UserId.Should().Be(userId);
        result.IsAzureAdGroup.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateAsync_WhenCurrentUserOwnsGroup_UpdatesExistingGroup()
    {
        var user = CreateUser();
        var userId = user.Id;
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        var group = CreateDomainGroup("original-group", userId);

        _context.Groups.Add(group);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        var updatedDalGroup = new DalGroup
        {
            Id = group.Id,
            Name = "updated-group",
            CreatorId = userId,
            IsAzureAdGroup = false
        };
        
        _context.ChangeTracker.Clear();

        var result = await _sut.UpdateAsync(updatedDalGroup, userId);
        await _context.SaveChangesAsync();

        result.Should().NotBeNull();
        result!.Name.Should().Be("updated-group");

        var dbGroup = await _context.Groups.FirstOrDefaultAsync(g => g.Id == group.Id);
        dbGroup.Should().NotBeNull();
        dbGroup!.Name.Should().Be("updated-group");
    }

    [Fact]
    public void Update_WhenCurrentUserOwnsGroup_UpdatesExistingGroup()
    {
        var user = CreateUser();
        var userId = user.Id;
        _context.Users.Add(user);
        _context.SaveChangesAsync();
        var group = CreateDomainGroup("original-group", userId);

        _context.Groups.Add(group);
        _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        var updatedDalGroup = new DalGroup
        {
            Id = group.Id,
            Name = "updated-group",
            CreatorId = userId,
            IsAzureAdGroup = false
        };
        
        _context.ChangeTracker.Clear();

        var result = _sut.Update(updatedDalGroup, userId);
        _context.SaveChangesAsync();

        result.Should().NotBeNull();
        result!.Name.Should().Be("updated-group");
    }

    [Fact]
    public async Task UpdateAsync_WhenCurrentUserDoesNotOwnGroup_ThrowsUnauthorizedAccessException()
    {
        var owner = CreateUser();
        var ownerId = owner.Id;
        var otherUser = CreateUser();
        var otherUserId = otherUser.Id;
        _context.Users.AddRange(owner, otherUser);
        await _context.SaveChangesAsync();
        var group = CreateDomainGroup("original-group", ownerId);

        _context.Groups.Add(group);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        var updatedDalGroup = new DalGroup
        {
            Id = group.Id,
            Name = "updated-group",
            CreatorId = ownerId
        };

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _sut.UpdateAsync(updatedDalGroup, otherUserId));
    }

    [Fact]
    public void Update_WhenCurrentUserDoesNotOwnGroup_ThrowsUnauthorizedAccessException()
    {
        var ownerId = TestUsers.StudentAId;
        var otherUserId = TestUsers.UserAId;
        var group = CreateDomainGroup("original-group", ownerId);

        _context.Groups.Add(group);
        _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        var updatedDalGroup = new DalGroup
        {
            Id = group.Id,
            Name = "updated-group",
            CreatorId = ownerId
        };

        Assert.Throws<UnauthorizedAccessException>(
            () => _sut.Update(updatedDalGroup, otherUserId));
    }

    [Fact]
    public async Task RemoveAsync_WhenCurrentUserOwnsGroup_RemovesGroup()
    {
        var user = CreateUser();
        var userId = user.Id;
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        var group = CreateDomainGroup("group-to-remove", userId);

        _context.Groups.Add(group);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        await _sut.RemoveAsync(group.Id, userId);
        await _context.SaveChangesAsync();

        var result = await _context.Groups.FirstOrDefaultAsync(g => g.Id == group.Id);
        result.Should().BeNull();
    }

    [Fact]
    public void Remove_WhenCurrentUserOwnsGroup_RemovesGroup()
    {
        var user = CreateUser();
        var userId = user.Id;
        _context.Users.Add(user);
        _context.SaveChangesAsync();
        var group = CreateDomainGroup("group-to-remove", userId);

        _context.Groups.Add(group);
        _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        _sut.Remove(group.Id, userId);
        _context.SaveChangesAsync();

        var result = _context.Groups.FirstOrDefault(g => g.Id == group.Id);
        result.Should().BeNull();
    }

    [Fact]
    public void Remove_WithEntity_WhenCurrentUserOwnsGroup_RemovesGroup()
    {
        var user = CreateUser();
        var userId = user.Id;
        _context.Users.Add(user);
        _context.SaveChangesAsync();
        var group = CreateDomainGroup("group-to-remove", userId);

        _context.Groups.Add(group);
        _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        var dalGroup = new DalGroup
        {
            Id = group.Id,
            Name = group.Name,
            CreatorId = userId,
            IsAzureAdGroup = false
        };

        _sut.Remove(dalGroup, userId);
        _context.SaveChangesAsync();

        var result = _context.Groups.FirstOrDefault(g => g.Id == group.Id);
        result.Should().BeNull();
    }

    [Fact]
    public async Task RemoveAsync_WhenCurrentUserDoesNotOwnGroup_ThrowsUnauthorizedAccessException()
    {
        var owner = CreateUser();
        var ownerId = owner.Id;
        var otherUser = CreateUser();
        var otherUserId = otherUser.Id;
        _context.Users.AddRange(owner, otherUser);
        await _context.SaveChangesAsync();
        var group = CreateDomainGroup("group-to-remove", ownerId);

        _context.Groups.Add(group);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _sut.RemoveAsync(group.Id, otherUserId));
    }

    [Fact]
    public void Remove_WhenCurrentUserDoesNotOwnGroup_ThrowsUnauthorizedAccessException()
    {
        var owner = CreateUser();
        var ownerId = owner.Id;
        var otherUser = CreateUser();
        var otherUserId = otherUser.Id;
        _context.Users.AddRange(owner, otherUser);
        _context.SaveChangesAsync();
        var group = CreateDomainGroup("group-to-remove", ownerId);

        _context.Groups.Add(group);
        _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        Assert.Throws<UnauthorizedAccessException>(
            () => _sut.Remove(group.Id, otherUserId));
    }

    [Fact]
    public async Task AllAsyncMatchingTeamSize_ReturnsGroupsWithinRange()
    {
        var user = CreateUser();
        var userId = user.Id;
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var smallGroup = CreateDomainGroup("small", userId);
        var matchingGroup = CreateDomainGroup("matching", userId);
        var largeGroup = CreateDomainGroup("large", userId);

        _context.Groups.AddRange(smallGroup, matchingGroup, largeGroup);
        await _context.SaveChangesAsync();
        
        var member1 = CreateUser();
        var member2 = CreateUser();
        var member3 = CreateUser();
        var member4 = CreateUser();
        var member5 = CreateUser();
        var member6 = CreateUser();
        _context.Users.AddRange(member1, member2, member3, member4, member5, member6);
        await _context.SaveChangesAsync();

        _context.UserGroups.AddRange(
            CreateUserGroup(smallGroup.Id, member1.Id, "member"),

            CreateUserGroup(matchingGroup.Id, member2.Id, "member"),
            CreateUserGroup(matchingGroup.Id, member3.Id, "member"),

            CreateUserGroup(largeGroup.Id, member4.Id, "member"),
            CreateUserGroup(largeGroup.Id, member5.Id, "member"),
            CreateUserGroup(largeGroup.Id, member6.Id, "member")
        );

        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        var result = (await _sut.AllAsyncMatchingTeamSize(2, 2)).ToList();

        result.Should().ContainSingle();
        result[0].Id.Should().Be(matchingGroup.Id);
        result[0].Name.Should().Be("matching");
    }

    [Fact]
    public async Task AllAsyncMatchingTeamSize_DoesNotReturnAzureAdGroups()
    {
        var user = CreateUser();
        var userId = user.Id;
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var normalGroup = CreateDomainGroup("normal", userId);
        var azureGroup = CreateDomainGroup("azure", userId, isAzureAdGroup: true);

        _context.Groups.AddRange(normalGroup, azureGroup);
        await _context.SaveChangesAsync();

        var member1 = CreateUser();
        var member2 = CreateUser();
        var member3 = CreateUser();
        var member4 = CreateUser();
        _context.Users.AddRange(member1, member2, member3, member4);
        await _context.SaveChangesAsync();

        _context.UserGroups.AddRange(
            CreateUserGroup(normalGroup.Id, member1.Id, "member"),
            CreateUserGroup(normalGroup.Id, member2.Id, "member"),

            CreateUserGroup(azureGroup.Id, member3.Id, "member"),
            CreateUserGroup(azureGroup.Id, member4.Id, "member")
        );

        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        var result = (await _sut.AllAsyncMatchingTeamSize(2, 2)).ToList();

        result.Should().ContainSingle();
        result[0].Id.Should().Be(normalGroup.Id);
    }

    [Fact]
    public async Task ExistsAsync_WithValidId_ReturnsTrue()
    {
        var user = CreateUser();
        var userId = user.Id;
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        var group = CreateDomainGroup("existing-group", userId);

        _context.Groups.Add(group);
        await _context.SaveChangesAsync();

        var result = await _sut.ExistsAsync(group.Id);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task ExistsAsync_WithInvalidId_ReturnsFalse()
    {
        var result = await _sut.ExistsAsync(Guid.NewGuid());

        result.Should().BeFalse();
    }

    [Fact]
    public void Exists_WithValidId_ReturnsTrue()
    {
        var user = CreateUser();
        var userId = user.Id;
        _context.Users.Add(user);
        _context.SaveChangesAsync();
        var group = CreateDomainGroup("existing-group", userId);

        _context.Groups.Add(group);
        _context.SaveChangesAsync();

        var result = _sut.Exists(group.Id);

        result.Should().BeTrue();
    }

    [Fact]
    public void Exists_WithInvalidId_ReturnsFalse()
    {
        var result = _sut.Exists(Guid.NewGuid());

        result.Should().BeFalse();
    }

    private static global::Domain.Group CreateDomainGroup(
        string name,
        Guid userId,
        bool isAzureAdGroup = false,
        Guid? id = null)
    {
        return new global::Domain.Group
        {
            Id = id ?? Guid.NewGuid(),
            Name = name,
            UserId = userId,
            IsAzureAdGroup = isAzureAdGroup
        };
    }

    private static global::Domain.UserGroup CreateUserGroup(
        Guid groupId,
        Guid userId,
        string role)
    {
        return new global::Domain.UserGroup
        {
            Id = Guid.NewGuid(),
            GroupId = groupId,
            UserId = userId,
            Role = role
        };
    }
    
    private static global::Domain.Identity.AppUser CreateUser(Guid? id = null)
    {
        var userId = id ?? Guid.NewGuid();

        return new global::Domain.Identity.AppUser
        {
            Id = userId,
            UserName = $"{userId}@test.ee",
            NormalizedUserName = $"{userId}@TEST.EE",
            Email = $"{userId}@test.ee",
            NormalizedEmail = $"{userId}@TEST.EE",
            FirstName = "Test",
            LastName = "User",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString()
        };
    }
}