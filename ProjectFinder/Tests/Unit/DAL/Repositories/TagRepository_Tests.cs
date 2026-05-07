using AwesomeAssertions;
using Base.Contracts;
using DAL.EF;
using DAL.EF.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

using DalTag = global::DAL.DTO.Tag;

namespace Tests.Unit.DAL.Repositories;

public class TagRepository_Tests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly TagRepository _sut;

    public TagRepository_Tests()
    {
        var connectionString = "DataSource=:memory:";
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connectionString)
            .Options;

        var userNameResolverMock = new Mock<IUserNameResolver>();
        userNameResolverMock.Setup(e => e.CurrentUserName).Returns("test");
        var loggerMock = new Mock<ILogger<AppDbContext>>();

        _context = new AppDbContext(options, userNameResolverMock.Object, loggerMock.Object);
        _context.Database.OpenConnection();
        _context.Database.EnsureCreated();
        _sut = new TagRepository(_context);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public async Task AllAsync_ReturnsAllTags()
    {
        // Arrange
        var tag1 = new global::Domain.Tag { Id = Guid.NewGuid(), Name = "tag1"};
        var tag2 = new global::Domain.Tag { Id = Guid.NewGuid(), Name = "tag2"};
        _context.Tags.AddRange(tag1, tag2);
        await _context.SaveChangesAsync();

        // Act
        var result = (await _sut.AllAsync()).ToList();

        // Assert
        result.Should().HaveCount(2);
        result.Should().ContainEquivalentOf(new DalTag { Id = tag1.Id, Name = "tag1" });
        result.Should().ContainEquivalentOf(new DalTag { Id = tag2.Id, Name = "tag2" });
    }

    [Fact]
    public void All_ReturnsAllTags()
    {
        // Arrange
        var tag1 = new global::Domain.Tag { Id = Guid.NewGuid(), Name = "tag1" };
        var tag2 = new global::Domain.Tag { Id = Guid.NewGuid(), Name = "tag2" };
        _context.Tags.AddRange(tag1, tag2);
        _context.SaveChangesAsync();

        // Act
        var result = _sut.All().ToList();

        // Assert
        result.Should().HaveCount(2);
        result.Should().ContainEquivalentOf(new DalTag { Id = tag1.Id, Name = "tag1" });
    }

    [Fact]
    public async Task FindAsync_WithValidId_ReturnsTag()
    {
        // Arrange
        var tagId = Guid.NewGuid();
        var tag = new global::Domain.Tag { Id = tagId, Name = "test-tag" };
        _context.Tags.Add(tag);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sut.FindAsync(tagId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(tagId);
        result.Name.Should().Be("test-tag");
    }

    [Fact]
    public void Find_WithValidId_ReturnsTag()
    {
        // Arrange
        var tagId = Guid.NewGuid();
        var tag = new global::Domain.Tag { Id = tagId, Name = "test-tag" };
        _context.Tags.Add(tag);
        _context.SaveChangesAsync();

        // Act
        var result = _sut.Find(tagId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(tagId);
        result.Name.Should().Be("test-tag");
    }

    [Fact]
    public async Task FindAsync_WithInvalidId_ReturnsNull()
    {
        // Act
        var result = await _sut.FindAsync(Guid.NewGuid());

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Find_WithInvalidId_ReturnsNull()
    {
        // Act
        var result = _sut.Find(Guid.NewGuid());

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Add_AddsTagToRepository()
    {
        // Arrange
        var dalTag = new DalTag { Id = Guid.NewGuid(), Name = "new-tag" };

        // Act
        _sut.Add(dalTag);
        _context.SaveChangesAsync();

        // Assert
        var result = _context.Tags.FirstOrDefault(t => t.Id == dalTag.Id);
        result.Should().NotBeNull();
        result.Name.Should().Be("new-tag");
    }

    [Fact]
    public async Task UpdateAsync_UpdatesExistingTag()
    {
        // Arrange
        var tagId = Guid.NewGuid();
        var tag = new global::Domain.Tag { Id = tagId, Name = "original-tag" };
        _context.Tags.Add(tag);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        var updatedDalTag = new DalTag { Id = tagId, Name = "updated-tag" };

        // Act
        var result = await _sut.UpdateAsync(updatedDalTag);
        await _context.SaveChangesAsync();

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("updated-tag");
        var dbTag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == tagId);
        dbTag.Should().NotBeNull();
        dbTag!.Name.Should().Be("updated-tag");
    }

    [Fact]
    public void Update_UpdatesExistingTag()
    {
        // Arrange
        var tagId = Guid.NewGuid();
        var tag = new global::Domain.Tag { Id = tagId, Name = "original-tag" };
        _context.Tags.Add(tag);
        _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        var updatedDalTag = new DalTag { Id = tagId, Name = "updated-tag" };

        // Act
        var result = _sut.Update(updatedDalTag);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("updated-tag");
    }

    [Fact]
    public async Task RemoveAsync_WithValidId_RemovesTag()
    {
        // Arrange
        var tagId = Guid.NewGuid();
        var tag = new global::Domain.Tag { Id = tagId, Name = "tag-to-remove" };
        _context.Tags.Add(tag);
        await _context.SaveChangesAsync();

        // Act
        await _sut.RemoveAsync(tagId);
        await _context.SaveChangesAsync();

        // Assert
        var result = await _context.Tags.FirstOrDefaultAsync(t => t.Id == tagId);
        result.Should().BeNull();
    }

    [Fact]
    public void Remove_WithValidId_RemovesTag()
    {
        // Arrange
        var tagId = Guid.NewGuid();
        var tag = new Domain.Tag { Id = tagId, Name = "tag-to-remove" };
        _context.Tags.Add(tag);
        _context.SaveChangesAsync();

        // Act
        _sut.Remove(tagId, default);
        _context.SaveChangesAsync();

        // Assert
        var result = _context.Tags.FirstOrDefault(t => t.Id == tagId);
        result.Should().BeNull();
    }

    [Fact]
    public void Remove_WithEntity_RemovesTag()
    {
        // Arrange
        var tag = new global::Domain.Tag { Id = Guid.NewGuid(), Name = "tag-to-remove" };
        _context.Tags.Add(tag);
        _context.SaveChangesAsync();

        var dalTag = new DalTag { Id = tag.Id, Name = tag.Name };

        // Act
        _sut.Remove(dalTag);
        _context.SaveChangesAsync();

        // Assert
        var result = _context.Tags.FirstOrDefault(t => t.Id == tag.Id);
        result.Should().BeNull();
    }

    [Fact]
    public async Task ExistsAsync_WithValidId_ReturnsTrue()
    {
        // Arrange
        var tagId = Guid.NewGuid();
        var tag = new global::Domain.Tag { Id = tagId, Name = "test-tag" };
        _context.Tags.Add(tag);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sut.ExistsAsync(tagId);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task ExistsAsync_WithInvalidId_ReturnsFalse()
    {
        // Act
        var result = await _sut.ExistsAsync(Guid.NewGuid());

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Exists_WithValidId_ReturnsTrue()
    {
        // Arrange
        var tagId = Guid.NewGuid();
        var tag = new global::Domain.Tag { Id = tagId, Name = "test-tag" };
        _context.Tags.Add(tag);
        _context.SaveChangesAsync();

        // Act
        var result = _sut.Exists(tagId);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Exists_WithInvalidId_ReturnsFalse()
    {
        // Act
        var result = _sut.Exists(Guid.NewGuid());

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task AllAsync_WithEmptyRepository_ReturnsEmptyCollection()
    {
        // Act
        var result = (await _sut.AllAsync()).ToList();

        // Assert
        result.Should().BeEmpty();
    }
}