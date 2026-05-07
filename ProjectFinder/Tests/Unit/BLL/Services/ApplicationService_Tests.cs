using AwesomeAssertions;
using BLL.DTO;
using BLL.Mappers;
using BLL.Services;
using DAL.Contracts;
using Moq;
using DalAppUser = global::DAL.DTO.Identity.AppUser;
using DalApplication = global::DAL.DTO.Application;
using DalGroup = global::DAL.DTO.Group;
using DalProject = global::DAL.DTO.Project;
using DalUserGroup = global::DAL.DTO.UserGroup;
using DalUserProject = global::DAL.DTO.UserProject;

namespace Tests.Unit.BLL.Services;

public class ApplicationService_Tests
{
    private readonly Mock<IAppUOW> _uow = new();
    private readonly Mock<IApplicationRepository> _repository = new();
    private readonly Mock<IUserProjectRepository> _userProjectRepository = new();
    private readonly Mock<IProjectRepository> _projectRepository = new();
    private readonly ApplicationBLLMapper _mapper = new();
    private readonly ApplicationService _sut;
    
    // Well-known GUIDs from ApplicationService.cs
    private readonly Guid _executorRoleId = Guid.Parse("00000000-0000-0000-0000-000000000004");
    private readonly Guid _openStatusId = Guid.Parse("00000000-0000-0000-0000-000000000002");

    public ApplicationService_Tests()
    {
        _uow.SetupGet(u => u.ApplicationRepository).Returns(_repository.Object);
        _uow.SetupGet(u => u.UserProjectRepository).Returns(_userProjectRepository.Object);
        _uow.SetupGet(u => u.ProjectRepository).Returns(_projectRepository.Object);
        _sut = new ApplicationService(_uow.Object, _mapper);
    }

    [Fact]
    public async Task AllAsync_MapsResults()
    {
        var userId = Guid.NewGuid();
        var projectId = Guid.NewGuid();
        
        var dalItems = new List<DalApplication>
        {
            new() { Id = Guid.NewGuid(), UserId = userId, ProjectId = projectId },
            new() { Id = Guid.NewGuid(), UserId = userId, ProjectId = projectId },
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
        var userId = Guid.NewGuid();
        var projectId = Guid.NewGuid();
        
        _repository.Setup(r => r.FindAsync(id))
            .ReturnsAsync(new DalApplication
            {
                Id = id,
                UserId = userId,
                ProjectId = projectId,
                AcceptedAt = null,
                DeclinedAt = null
            });

        var result = await _sut.FindAsync(id);

        result.Should().NotBeNull();
        result.Id.Should().Be(id);
        result.UserId.Should().Be(userId);
        _repository.Verify(r => r.FindAsync(id), Times.Once);
    }

    [Fact]
    public async Task FindAsync_WhenRepositoryReturnsNull_ReturnsNull()
    {
        var id = Guid.NewGuid();
        _repository.Setup(r => r.FindAsync(id)).ReturnsAsync((DalApplication?)null);

        var result = await _sut.FindAsync(id);

        result.Should().BeNull();
    }

    [Fact]
    public async Task FindAsync_WithNestedObjects_MapsCompleteStructure()
    {
        var id = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var projectId = Guid.NewGuid();
        var groupId = Guid.NewGuid();
        var creatorId = Guid.NewGuid();

        _repository.Setup(r => r.FindAsync(id))
            .ReturnsAsync(new DalApplication
            {
                Id = id,
                UserId = userId,
                ProjectId = projectId,
                GroupId = groupId,
                AcceptedAt = DateTime.UtcNow,
                DeclinedAt = null,
                User = new DalAppUser
                {
                    Id = userId,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john@example.com"
                },
                Project = new DalProject
                {
                    Id = projectId,
                    TitleInEstonian = "Projekt",
                    TitleInEnglish = "Project"
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
                        FirstName = "Jane",
                        LastName = "Smith",
                        Email = "jane@example.com"
                    }
                }
            });

        var result = await _sut.FindAsync(id);

        result.Should().NotBeNull();
        var mapped = result ?? throw new InvalidOperationException();
        var user = mapped.User ?? throw new InvalidOperationException();
        var project = mapped.Project ?? throw new InvalidOperationException();
        var group = mapped.Group ?? throw new InvalidOperationException();
        var creator = group.Creator ?? throw new InvalidOperationException();

        user.Email.Should().Be("john@example.com");
        project.TitleInEnglish.Should().Be("Project");
        creator.FirstName.Should().Be("Jane");
        mapped.AcceptedAt.Should().NotBeNull();
    }

    [Fact]
    public void Add_DelegatesToRepository()
    {
        var userId = Guid.NewGuid();
        var projectId = Guid.NewGuid();
        var bll = new Application
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            ProjectId = projectId
        };

        _sut.Add(bll);

        _repository.Verify(
            r => r.Add(It.Is<DalApplication>(x => x.Id == bll.Id && x.UserId == userId), default),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithoutAcceptance_ReturnsUpdatedEntity()
    {
        var applicationId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var projectId = Guid.NewGuid();
        
        var bll = new Application
        {
            Id = applicationId,
            UserId = userId,
            ProjectId = projectId,
            AcceptedAt = null
        };

        _repository.Setup(r => r.UpdateAsync(It.IsAny<DalApplication>()))
            .ReturnsAsync((DalApplication e, Guid _) => e);

        var result = await _sut.UpdateAsync(bll);

        result!.Id.Should().Be(applicationId);
        _repository.Verify(r => r.UpdateAsync(It.IsAny<DalApplication>()), Times.Once);
        _userProjectRepository.Verify(r => r.Add(It.IsAny<DalUserProject>(), It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_WithAcceptanceAndGroup_AddsUserProjectsForAllGroupMembers()
    {
        var applicationId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var projectId = Guid.NewGuid();
        var groupId = Guid.NewGuid();
        var member1Id = Guid.NewGuid();
        var member2Id = Guid.NewGuid();
        
        var bll = new Application
        {
            Id = applicationId,
            UserId = userId,
            ProjectId = projectId,
            GroupId = groupId,
            AcceptedAt = DateTime.UtcNow
        };

        var updatedDalEntity = new DalApplication
        {
            Id = applicationId,
            UserId = userId,
            ProjectId = projectId,
            GroupId = groupId,
            AcceptedAt = DateTime.UtcNow,
            Group = new DalGroup
            {
                Id = groupId,
                Name = "Team",
                UserGroups = new List<DalUserGroup>
                {
                    new() { UserId = member1Id, GroupId = groupId, Role = "Member" },
                    new() { UserId = member2Id, GroupId = groupId, Role = "Member" }
                }
            }
        };

        _repository.Setup(r => r.UpdateAsync(It.IsAny<DalApplication>()))
            .ReturnsAsync(updatedDalEntity);

        var result = await _sut.UpdateAsync(bll);

        var mapped = result ?? throw new InvalidOperationException();
        mapped.Id.Should().Be(applicationId);
        _userProjectRepository.Verify(
            r => r.Add(
                It.Is<DalUserProject>(x =>
                    x.UserId == member1Id &&
                    x.ProjectId == projectId &&
                    x.UserProjectRoleId == _executorRoleId),
                member1Id),
            Times.Once);
        _userProjectRepository.Verify(
            r => r.Add(
                It.Is<DalUserProject>(x =>
                    x.UserId == member2Id &&
                    x.ProjectId == projectId &&
                    x.UserProjectRoleId == _executorRoleId),
                member2Id),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithAcceptanceAndNoGroup_AddsUserProjectForApplicant()
    {
        var applicationId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var projectId = Guid.NewGuid();
        
        var bll = new Application
        {
            Id = applicationId,
            UserId = userId,
            ProjectId = projectId,
            GroupId = null,
            AcceptedAt = DateTime.UtcNow
        };

        var updatedDalEntity = new DalApplication
        {
            Id = applicationId,
            UserId = userId,
            ProjectId = projectId,
            GroupId = null,
            AcceptedAt = DateTime.UtcNow,
        };

        _repository.Setup(r => r.UpdateAsync(It.IsAny<DalApplication>()))
            .ReturnsAsync(updatedDalEntity);

        var result = await _sut.UpdateAsync(bll);

        var mapped = result ?? throw new InvalidOperationException();
        mapped.Id.Should().Be(applicationId);
        _userProjectRepository.Verify(
            r => r.Add(
                It.Is<DalUserProject>(x =>
                    x.UserId == userId &&
                    x.ProjectId == projectId &&
                    x.UserProjectRoleId == _executorRoleId),
                userId),
            Times.Once);
    }

    [Fact]
    public async Task RemoveAsync_WhenUserIsOwnerAndNotAccepted_RemovesApplication()
    {
        var applicationId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _repository.Setup(r => r.FindAsync(applicationId, userId))
            .ReturnsAsync(new DalApplication
            {
                Id = applicationId,
                UserId = userId,
                AcceptedAt = null,
                DeclinedAt = null
            });

        await _sut.RemoveAsync(applicationId, userId);

        _repository.Verify(r => r.RemoveAsync(applicationId, userId), Times.Once);
    }

    [Fact]
    public async Task RemoveAsync_WhenUserIsNotOwner_ThrowsUnauthorizedAccessException()
    {
        var applicationId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var ownerId = Guid.NewGuid();

        _repository.Setup(r => r.FindAsync(applicationId, userId))
            .ReturnsAsync(new DalApplication
            {
                Id = applicationId,
                UserId = ownerId,
                AcceptedAt = null
            });

        await Assert.ThrowsAsync<UnauthorizedAccessException>(
            () => _sut.RemoveAsync(applicationId, userId));

        _repository.Verify(r => r.RemoveAsync(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task RemoveAsync_WhenApplicationIsAccepted_ThrowsInvalidOperationException()
    {
        var applicationId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _repository.Setup(r => r.FindAsync(applicationId, userId))
            .ReturnsAsync(new DalApplication
            {
                Id = applicationId,
                UserId = userId,
                AcceptedAt = DateTime.UtcNow
            });

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _sut.RemoveAsync(applicationId, userId));

        _repository.Verify(r => r.RemoveAsync(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public void Remove_WhenUserIsOwnerAndNotAccepted_RemovesApplication()
    {
        var applicationId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var bll = new Application { Id = applicationId, UserId = userId };

        _repository.Setup(r => r.Find(applicationId, userId))
            .Returns(new DalApplication
            {
                Id = applicationId,
                UserId = userId,
                AcceptedAt = null,
                DeclinedAt = null
            });

        _sut.Remove(bll, userId);

        _repository.Verify(
            r => r.Remove(It.Is<DalApplication>(x => x.Id == applicationId), userId),
            Times.Once);
    }

    [Fact]
    public void Remove_WhenUserIsNotOwner_ThrowsUnauthorizedAccessException()
    {
        var applicationId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var ownerId = Guid.NewGuid();
        var bll = new Application { Id = applicationId, UserId = ownerId };

        _repository.Setup(r => r.Find(applicationId, userId))
            .Returns(new DalApplication
            {
                Id = applicationId,
                UserId = ownerId,
                AcceptedAt = null
            });

        Assert.Throws<UnauthorizedAccessException>(() => _sut.Remove(bll, userId));

        _repository.Verify(r => r.Remove(It.IsAny<DalApplication>(), It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public void Remove_WhenApplicationIsAccepted_ThrowsInvalidOperationException()
    {
        var applicationId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var bll = new Application { Id = applicationId, UserId = userId };

        _repository.Setup(r => r.Find(applicationId, userId))
            .Returns(new DalApplication
            {
                Id = applicationId,
                UserId = userId,
                AcceptedAt = DateTime.UtcNow
            });

        Assert.Throws<InvalidOperationException>(() => _sut.Remove(bll, userId));

        _repository.Verify(r => r.Remove(It.IsAny<DalApplication>(), It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task FindAsyncByProjectId_ReturnsMappedEntity()
    {
        var projectId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var applicationId = Guid.NewGuid();

        _repository.Setup(r => r.FindAsyncByProjectId(projectId, userId))
            .ReturnsAsync(new DalApplication
            {
                Id = applicationId,
                ProjectId = projectId,
                UserId = userId
            });

        var result = await _sut.FindAsyncByProjectId(projectId, userId);

        result.Should().NotBeNull();
        result.ProjectId.Should().Be(projectId);
        result.UserId.Should().Be(userId);
        _repository.Verify(r => r.FindAsyncByProjectId(projectId, userId), Times.Once);
    }

    [Fact]
    public async Task FindAsyncByProjectId_WhenRepositoryReturnsNull_ReturnsNull()
    {
        var projectId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        _repository.Setup(r => r.FindAsyncByProjectId(projectId, userId))
            .ReturnsAsync((DalApplication?)null);

        var result = await _sut.FindAsyncByProjectId(projectId, userId);

        result.Should().BeNull();
    }

    [Fact]
    public async Task AddWithValidationAsync_WhenProjectNotFound_ThrowsInvalidOperationException()
    {
        var userId = Guid.NewGuid();
        var projectId = Guid.NewGuid();
        var bll = new Application { Id = Guid.NewGuid(), ProjectId = projectId, UserId = userId };

        _projectRepository.Setup(r => r.FindAsync(projectId, userId))
            .ReturnsAsync((DalProject?)null);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _sut.AddWithValidationAsync(bll, userId));

        _repository.Verify(r => r.Add(It.IsAny<DalApplication>(), It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task AddWithValidationAsync_WhenProjectNotOpen_ThrowsInvalidOperationException()
    {
        var userId = Guid.NewGuid();
        var projectId = Guid.NewGuid();
        var bll = new Application { Id = Guid.NewGuid(), ProjectId = projectId, UserId = userId };

        var closedStatusId = Guid.NewGuid();
        _projectRepository.Setup(r => r.FindAsync(projectId, userId))
            .ReturnsAsync(new DalProject
            {
                Id = projectId,
                ProjectStatusId = closedStatusId // Not open status
            });

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _sut.AddWithValidationAsync(bll, userId));

        _repository.Verify(r => r.Add(It.IsAny<DalApplication>(), It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task AddWithValidationAsync_WhenUserAlreadyApplied_ThrowsInvalidOperationException()
    {
        var userId = Guid.NewGuid();
        var projectId = Guid.NewGuid();
        var bll = new Application { Id = Guid.NewGuid(), ProjectId = projectId, UserId = userId };

        _projectRepository.Setup(r => r.FindAsync(projectId, userId))
            .ReturnsAsync(new DalProject
            {
                Id = projectId,
                ProjectStatusId = _openStatusId
            });

        _repository.Setup(r => r.FindAsyncByProjectId(projectId, userId))
            .ReturnsAsync(new DalApplication { Id = Guid.NewGuid() });

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _sut.AddWithValidationAsync(bll, userId));

        _repository.Verify(r => r.Add(It.IsAny<DalApplication>(), It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task AddWithValidationAsync_WithValidInputs_AddsApplication()
    {
        var userId = Guid.NewGuid();
        var projectId = Guid.NewGuid();
        var applicationId = Guid.NewGuid();
        var bll = new Application { Id = applicationId, ProjectId = projectId, UserId = userId };

        _projectRepository.Setup(r => r.FindAsync(projectId, userId))
            .ReturnsAsync(new DalProject
            {
                Id = projectId,
                ProjectStatusId = _openStatusId
            });

        _repository.Setup(r => r.FindAsyncByProjectId(projectId, userId))
            .ReturnsAsync((DalApplication?)null);

        await _sut.AddWithValidationAsync(bll, userId);

        _repository.Verify(
            r => r.Add(
                It.Is<DalApplication>(x => x.Id == applicationId && x.ProjectId == projectId),
                userId),
            Times.Once);
    }
}
