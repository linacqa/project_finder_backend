using AwesomeAssertions;
using BLL.DTO;
using BLL.Mappers;
using BLL.Services;
using DAL.Contracts;
using Moq;

using DalProject = global::DAL.DTO.Project;
using DalProjectFolder = global::DAL.DTO.ProjectFolder;
using DalProjectTag = global::DAL.DTO.ProjectTag;
using DalProjectStep = global::DAL.DTO.ProjectStep;
using DalUserProject = global::DAL.DTO.UserProject;

namespace Tests.Unit.BLL.Services;

public class ProjectService_Tests
{
    private readonly Mock<IAppUOW> _uow = new();
    private readonly Mock<IProjectRepository> _projectRepository = new();
    private readonly Mock<IProjectFolderRepository> _projectFolderRepository = new();
    private readonly Mock<IProjectTagRepository> _projectTagRepository = new();
    private readonly Mock<IProjectStepRepository> _projectStepRepository = new();
    private readonly Mock<IUserProjectRepository> _userProjectRepository = new();
    private readonly Mock<IFolderRepository> _folderRepository = new();
    private readonly Mock<ITagRepository> _tagRepository = new();
    private readonly Mock<IStepRepository> _stepRepository = new();
    
    private readonly ProjectBLLMapper _mapper = new();
    private readonly ProjectService _sut;

    public ProjectService_Tests()
    {
        _uow.SetupGet(u => u.ProjectRepository).Returns(_projectRepository.Object);
        _uow.SetupGet(u => u.ProjectFolderRepository).Returns(_projectFolderRepository.Object);
        _uow.SetupGet(u => u.ProjectTagRepository).Returns(_projectTagRepository.Object);
        _uow.SetupGet(u => u.ProjectStepRepository).Returns(_projectStepRepository.Object);
        _uow.SetupGet(u => u.UserProjectRepository).Returns(_userProjectRepository.Object);
        _uow.SetupGet(u => u.FolderRepository).Returns(_folderRepository.Object);
        _uow.SetupGet(u => u.TagRepository).Returns(_tagRepository.Object);
        _uow.SetupGet(u => u.StepRepository).Returns(_stepRepository.Object);
        
        _sut = new ProjectService(_uow.Object, _mapper);
    }

    [Fact]
    public async Task AllAsync_MapsResults()
    {
        var dalItems = new List<DalProject>
        {
            new()
            {
                Id = Guid.NewGuid(),
                TitleInEstonian = "projekt 1",
                Description = "desc1"
            },
            new()
            {
                Id = Guid.NewGuid(),
                TitleInEstonian = "projekt 2",
                Description = "desc2"
            }
        };
        _projectRepository.Setup(r => r.AllAsync()).ReturnsAsync(dalItems);

        var bllItems = (await _sut.AllAsync()).ToList();

        bllItems.Should().HaveCount(2);
        bllItems[0].TitleInEstonian.Should().Be("projekt 1");
        bllItems[1].TitleInEstonian.Should().Be("projekt 2");
        _projectRepository.Verify(r => r.AllAsync(), Times.Once);
    }

    [Fact]
    public async Task FindAsync_ReturnsMappedEntity()
    {
        var id = Guid.NewGuid();
        _projectRepository.Setup(r => r.FindAsync(id))
            .ReturnsAsync(new DalProject
            {
                Id = id,
                TitleInEstonian = "projekt",
                Description = "desc"
            });

        var result = await _sut.FindAsync(id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
        result.TitleInEstonian.Should().Be("projekt");
        _projectRepository.Verify(r => r.FindAsync(id), Times.Once);
    }

    [Fact]
    public async Task FindAsync_WhenRepositoryReturnsNull_ReturnsNull()
    {
        var id = Guid.NewGuid();
        _projectRepository.Setup(r => r.FindAsync(id)).ReturnsAsync((DalProject?)null);

        var result = await _sut.FindAsync(id);

        result.Should().BeNull();
    }

    [Fact]
    public async Task AllCurrentUserAsync_MapsResults()
    {
        var userId = Guid.NewGuid();
        var dalItems = new List<DalProject>
        {
            new() { Id = Guid.NewGuid(), TitleInEstonian = "user proj 1", Description = "desc1" },
            new() { Id = Guid.NewGuid(), TitleInEstonian = "user proj 2", Description = "desc2" }
        };
        _projectRepository.Setup(r => r.AllCurrentUserAsync(userId)).ReturnsAsync(dalItems);

        var bllItems = (await _sut.AllCurrentUserAsync(userId)).ToList();

        bllItems.Should().HaveCount(2);
        bllItems[0].TitleInEstonian.Should().Be("user proj 1");
        bllItems[1].TitleInEstonian.Should().Be("user proj 2");
        _projectRepository.Verify(r => r.AllCurrentUserAsync(userId), Times.Once);
    }

    [Fact]
    public async Task SearchAsync_ReturnsPaginatedResults()
    {
        var userId = Guid.NewGuid();
        var request = new DTO.v1.ProjectsSearchRequest { Page = 1, PageSize = 10 };
        var dalItems = new List<DalProject>
        {
            new() { Id = Guid.NewGuid(), TitleInEstonian = "search result 1", Description = "desc1" }
        };
        _projectRepository.Setup(r => r.SearchAsync(It.IsAny<DTO.v1.ProjectsSearchRequest>(), userId))
            .ReturnsAsync((dalItems, 1));

        var result = await _sut.SearchAsync(request, userId);

        result.Data.Should().HaveCount(1);
        result.TotalCount.Should().Be(1);
        result.Page.Should().Be(1);
        result.PageSize.Should().Be(10);
        result.Data.First().TitleInEstonian.Should().Be("search result 1");
    }

    [Fact]
    public void Add_WithValidFolders_AddsProjectAndRelatedEntities()
    {
        var projectId = Guid.NewGuid();
        var authorId = Guid.NewGuid();
        var folderId = Guid.NewGuid();
        var tagId = Guid.NewGuid();
        var stepId = Guid.NewGuid();

        var bll = new Project
        {
            Id = projectId,
            TitleInEstonian = "new project",
            Description = "desc",
            AuthorId = authorId,
            FolderIds = new List<Guid> { folderId },
            TagIds = new List<Guid> { tagId },
            StepIds = new List<Guid> { stepId }
        };

        _folderRepository.Setup(r => r.Exists(folderId)).Returns(true);
        _tagRepository.Setup(r => r.Exists(tagId)).Returns(true);
        _stepRepository.Setup(r => r.Exists(stepId)).Returns(true);

        _sut.Add(bll, authorId);

        _projectRepository.Verify(r => r.Add(It.IsAny<DalProject>(), authorId), Times.Once);
        _projectFolderRepository.Verify(r => r.Add(It.IsAny<DalProjectFolder>()), Times.Once);
        _projectTagRepository.Verify(r => r.Add(It.IsAny<DalProjectTag>()), Times.Once);
        _projectStepRepository.Verify(r => r.Add(It.IsAny<DalProjectStep>()), Times.Once);
        _userProjectRepository.Verify(r => r.Add(It.IsAny<DalUserProject>()), Times.AtLeastOnce);
    }

    [Fact]
    public void Add_WithNonExistentFolder_ThrowsArgumentException()
    {
        var projectId = Guid.NewGuid();
        var authorId = Guid.NewGuid();
        var nonExistentFolderId = Guid.NewGuid();

        var bll = new Project
        {
            Id = projectId,
            TitleInEstonian = "new project",
            Description = "desc",
            AuthorId = authorId,
            FolderIds = new List<Guid> { nonExistentFolderId }
        };

        _folderRepository.Setup(r => r.Exists(nonExistentFolderId)).Returns(false);

        var act = () => _sut.Add(bll, authorId);

        act.Should().Throw<ArgumentException>()
            .WithMessage($"*{nonExistentFolderId}*");
    }

    [Fact]
    public void Add_WithNonExistentTag_ThrowsArgumentException()
    {
        var projectId = Guid.NewGuid();
        var authorId = Guid.NewGuid();
        var nonExistentTagId = Guid.NewGuid();

        var bll = new Project
        {
            Id = projectId,
            TitleInEstonian = "new project",
            Description = "desc",
            AuthorId = authorId,
            TagIds = new List<Guid> { nonExistentTagId }
        };

        _tagRepository.Setup(r => r.Exists(nonExistentTagId)).Returns(false);

        var act = () => _sut.Add(bll, authorId);

        act.Should().Throw<ArgumentException>()
            .WithMessage($"*{nonExistentTagId}*");
    }

    [Fact]
    public void Add_WithNonExistentStep_ThrowsArgumentException()
    {
        var projectId = Guid.NewGuid();
        var authorId = Guid.NewGuid();
        var nonExistentStepId = Guid.NewGuid();

        var bll = new Project
        {
            Id = projectId,
            TitleInEstonian = "new project",
            Description = "desc",
            AuthorId = authorId,
            StepIds = new List<Guid> { nonExistentStepId }
        };

        _stepRepository.Setup(r => r.Exists(nonExistentStepId)).Returns(false);

        var act = () => _sut.Add(bll, authorId);

        act.Should().Throw<ArgumentException>()
            .WithMessage($"*{nonExistentStepId}*");
    }

    [Fact]
    public async Task UpdateAsync_ReturnsMappedEntity()
    {
        var projectId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var authorId = Guid.NewGuid();

        var bll = new Project
        {
            Id = projectId,
            TitleInEstonian = "updated project",
            Description = "updated desc",
            AuthorId = authorId,
            FolderIds = new List<Guid>(),
            TagIds = new List<Guid>(),
            StepIds = new List<Guid>()
        };

        var dalUpdated = new DalProject
        {
            Id = projectId,
            TitleInEstonian = "updated project",
            Description = "updated desc"
        };

        _projectRepository.Setup(r => r.UpdateAsync(It.IsAny<DalProject>(), userId))
            .ReturnsAsync(dalUpdated);
        _projectFolderRepository.Setup(r => r.AllAsyncByProjectId(projectId, userId))
            .ReturnsAsync(new List<DalProjectFolder>());
        _projectTagRepository.Setup(r => r.AllAsyncByProjectId(projectId, userId))
            .ReturnsAsync(new List<DalProjectTag>());
        _projectStepRepository.Setup(r => r.AllAsyncByProjectId(projectId, userId))
            .ReturnsAsync(new List<DalProjectStep>());
        _userProjectRepository.Setup(r => r.AllAsyncByProjectId(projectId, userId))
            .ReturnsAsync(new List<DalUserProject>());

        var result = await _sut.UpdateAsync(bll, userId);

        result.Should().NotBeNull();
        result!.Id.Should().Be(projectId);
        result.TitleInEstonian.Should().Be("updated project");
        _projectRepository.Verify(r => r.UpdateAsync(It.IsAny<DalProject>(), userId), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhenRepositoryReturnsNull_ReturnsNull()
    {
        var projectId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var authorId = Guid.NewGuid();

        var bll = new Project
        {
            Id = projectId,
            TitleInEstonian = "project",
            Description = "desc",
            AuthorId = authorId,
            FolderIds = new List<Guid>(),
            TagIds = new List<Guid>(),
            StepIds = new List<Guid>()
        };

        _projectRepository.Setup(r => r.UpdateAsync(It.IsAny<DalProject>(), userId))
            .ReturnsAsync((DalProject?)null);

        var result = await _sut.UpdateAsync(bll, userId);

        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_WithNonExistentFolder_ThrowsArgumentException()
    {
        var projectId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var authorId = Guid.NewGuid();
        var nonExistentFolderId = Guid.NewGuid();

        var bll = new Project
        {
            Id = projectId,
            TitleInEstonian = "project",
            Description = "desc",
            AuthorId = authorId,
            FolderIds = new List<Guid> { nonExistentFolderId },
            TagIds = new List<Guid>(),
            StepIds = new List<Guid>()
        };

        _projectRepository.Setup(r => r.UpdateAsync(It.IsAny<DalProject>(), userId))
            .ReturnsAsync(new DalProject { Id = projectId });
        _projectFolderRepository.Setup(r => r.AllAsyncByProjectId(projectId, userId))
            .ReturnsAsync(new List<DalProjectFolder>());
        _folderRepository.Setup(r => r.Exists(nonExistentFolderId)).Returns(false);

        var act = () => _sut.UpdateAsync(bll, userId);

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage($"*{nonExistentFolderId}*");
    }

    [Fact]
    public async Task UpdateAsync_WithNonExistentTag_ThrowsArgumentException()
    {
        var projectId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var authorId = Guid.NewGuid();
        var nonExistentTagId = Guid.NewGuid();

        var bll = new Project
        {
            Id = projectId,
            TitleInEstonian = "project",
            Description = "desc",
            AuthorId = authorId,
            FolderIds = new List<Guid>(),
            TagIds = new List<Guid> { nonExistentTagId },
            StepIds = new List<Guid>()
        };

        _projectRepository.Setup(r => r.UpdateAsync(It.IsAny<DalProject>(), userId))
            .ReturnsAsync(new DalProject { Id = projectId });
        _projectFolderRepository.Setup(r => r.AllAsyncByProjectId(projectId, userId))
            .ReturnsAsync(new List<DalProjectFolder>());
        _projectTagRepository.Setup(r => r.AllAsyncByProjectId(projectId, userId))
            .ReturnsAsync(new List<DalProjectTag>());
        _tagRepository.Setup(r => r.Exists(nonExistentTagId)).Returns(false);

        var act = () => _sut.UpdateAsync(bll, userId);

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage($"*{nonExistentTagId}*");
    }

    [Fact]
    public async Task UpdateAsync_WithNonExistentStep_ThrowsArgumentException()
    {
        var projectId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var authorId = Guid.NewGuid();
        var nonExistentStepId = Guid.NewGuid();

        var bll = new Project
        {
            Id = projectId,
            TitleInEstonian = "project",
            Description = "desc",
            AuthorId = authorId,
            FolderIds = new List<Guid>(),
            TagIds = new List<Guid>(),
            StepIds = new List<Guid> { nonExistentStepId }
        };

        _projectRepository.Setup(r => r.UpdateAsync(It.IsAny<DalProject>(), userId))
            .ReturnsAsync(new DalProject { Id = projectId });
        _projectFolderRepository.Setup(r => r.AllAsyncByProjectId(projectId, userId))
            .ReturnsAsync(new List<DalProjectFolder>());
        _projectTagRepository.Setup(r => r.AllAsyncByProjectId(projectId, userId))
            .ReturnsAsync(new List<DalProjectTag>());
        _projectStepRepository.Setup(r => r.AllAsyncByProjectId(projectId, userId))
            .ReturnsAsync(new List<DalProjectStep>());
        _stepRepository.Setup(r => r.Exists(nonExistentStepId)).Returns(false);

        var act = () => _sut.UpdateAsync(bll, userId);

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage($"*{nonExistentStepId}*");
    }

    [Fact]
    public async Task UpdateAsync_AddsNewFolders()
    {
        var projectId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var authorId = Guid.NewGuid();
        var newFolderId = Guid.NewGuid();

        var bll = new Project
        {
            Id = projectId,
            TitleInEstonian = "project",
            Description = "desc",
            AuthorId = authorId,
            FolderIds = new List<Guid> { newFolderId },
            TagIds = new List<Guid>(),
            StepIds = new List<Guid>()
        };

        _projectRepository.Setup(r => r.UpdateAsync(It.IsAny<DalProject>(), userId))
            .ReturnsAsync(new DalProject { Id = projectId });
        _projectFolderRepository.Setup(r => r.AllAsyncByProjectId(projectId, userId))
            .ReturnsAsync(new List<DalProjectFolder>());
        _projectTagRepository.Setup(r => r.AllAsyncByProjectId(projectId, userId))
            .ReturnsAsync(new List<DalProjectTag>());
        _projectStepRepository.Setup(r => r.AllAsyncByProjectId(projectId, userId))
            .ReturnsAsync(new List<DalProjectStep>());
        _userProjectRepository.Setup(r => r.AllAsyncByProjectId(projectId, userId))
            .ReturnsAsync(new List<DalUserProject>());
        _folderRepository.Setup(r => r.Exists(newFolderId)).Returns(true);

        await _sut.UpdateAsync(bll, userId);

        _projectFolderRepository.Verify(r => r.Add(It.Is<DalProjectFolder>(pf => pf.FolderId == newFolderId)), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_RemovesOldFolders()
    {
        var projectId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var authorId = Guid.NewGuid();
        var oldFolderId = Guid.NewGuid();
        var oldProjectFolderId = Guid.NewGuid();

        var bll = new Project
        {
            Id = projectId,
            TitleInEstonian = "project",
            Description = "desc",
            AuthorId = authorId,
            FolderIds = new List<Guid>(),
            TagIds = new List<Guid>(),
            StepIds = new List<Guid>()
        };

        var existingFolders = new List<DalProjectFolder>
        {
            new() { Id = oldProjectFolderId, FolderId = oldFolderId, ProjectId = projectId }
        };

        _projectRepository.Setup(r => r.UpdateAsync(It.IsAny<DalProject>(), userId))
            .ReturnsAsync(new DalProject { Id = projectId });
        _projectFolderRepository.Setup(r => r.AllAsyncByProjectId(projectId, userId))
            .ReturnsAsync(existingFolders);
        _projectTagRepository.Setup(r => r.AllAsyncByProjectId(projectId, userId))
            .ReturnsAsync(new List<DalProjectTag>());
        _projectStepRepository.Setup(r => r.AllAsyncByProjectId(projectId, userId))
            .ReturnsAsync(new List<DalProjectStep>());
        _userProjectRepository.Setup(r => r.AllAsyncByProjectId(projectId, userId))
            .ReturnsAsync(new List<DalUserProject>());

        await _sut.UpdateAsync(bll, userId);

        _projectFolderRepository.Verify(r => r.Remove(oldProjectFolderId), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_AddsNewTags()
    {
        var projectId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var authorId = Guid.NewGuid();
        var newTagId = Guid.NewGuid();

        var bll = new Project
        {
            Id = projectId,
            TitleInEstonian = "project",
            Description = "desc",
            AuthorId = authorId,
            FolderIds = new List<Guid>(),
            TagIds = new List<Guid> { newTagId },
            StepIds = new List<Guid>()
        };

        _projectRepository.Setup(r => r.UpdateAsync(It.IsAny<DalProject>(), userId))
            .ReturnsAsync(new DalProject { Id = projectId });
        _projectFolderRepository.Setup(r => r.AllAsyncByProjectId(projectId, userId))
            .ReturnsAsync(new List<DalProjectFolder>());
        _projectTagRepository.Setup(r => r.AllAsyncByProjectId(projectId, userId))
            .ReturnsAsync(new List<DalProjectTag>());
        _projectStepRepository.Setup(r => r.AllAsyncByProjectId(projectId, userId))
            .ReturnsAsync(new List<DalProjectStep>());
        _userProjectRepository.Setup(r => r.AllAsyncByProjectId(projectId, userId))
            .ReturnsAsync(new List<DalUserProject>());
        _tagRepository.Setup(r => r.Exists(newTagId)).Returns(true);

        await _sut.UpdateAsync(bll, userId);

        _projectTagRepository.Verify(r => r.Add(It.Is<DalProjectTag>(pt => pt.TagId == newTagId)), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_RemovesOldTags()
    {
        var projectId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var authorId = Guid.NewGuid();
        var oldTagId = Guid.NewGuid();
        var oldProjectTagId = Guid.NewGuid();

        var bll = new Project
        {
            Id = projectId,
            TitleInEstonian = "project",
            Description = "desc",
            AuthorId = authorId,
            FolderIds = new List<Guid>(),
            TagIds = new List<Guid>(),
            StepIds = new List<Guid>()
        };

        var existingTags = new List<DalProjectTag>
        {
            new() { Id = oldProjectTagId, TagId = oldTagId, ProjectId = projectId }
        };

        _projectRepository.Setup(r => r.UpdateAsync(It.IsAny<DalProject>(), userId))
            .ReturnsAsync(new DalProject { Id = projectId });
        _projectFolderRepository.Setup(r => r.AllAsyncByProjectId(projectId, userId))
            .ReturnsAsync(new List<DalProjectFolder>());
        _projectTagRepository.Setup(r => r.AllAsyncByProjectId(projectId, userId))
            .ReturnsAsync(existingTags);
        _projectStepRepository.Setup(r => r.AllAsyncByProjectId(projectId, userId))
            .ReturnsAsync(new List<DalProjectStep>());
        _userProjectRepository.Setup(r => r.AllAsyncByProjectId(projectId, userId))
            .ReturnsAsync(new List<DalUserProject>());

        await _sut.UpdateAsync(bll, userId);

        _projectTagRepository.Verify(r => r.Remove(oldProjectTagId), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_AddsNewSteps()
    {
        var projectId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var authorId = Guid.NewGuid();
        var newStepId = Guid.NewGuid();

        var bll = new Project
        {
            Id = projectId,
            TitleInEstonian = "project",
            Description = "desc",
            AuthorId = authorId,
            FolderIds = new List<Guid>(),
            TagIds = new List<Guid>(),
            StepIds = new List<Guid> { newStepId }
        };

        _projectRepository.Setup(r => r.UpdateAsync(It.IsAny<DalProject>(), userId))
            .ReturnsAsync(new DalProject { Id = projectId });
        _projectFolderRepository.Setup(r => r.AllAsyncByProjectId(projectId, userId))
            .ReturnsAsync(new List<DalProjectFolder>());
        _projectTagRepository.Setup(r => r.AllAsyncByProjectId(projectId, userId))
            .ReturnsAsync(new List<DalProjectTag>());
        _projectStepRepository.Setup(r => r.AllAsyncByProjectId(projectId, userId))
            .ReturnsAsync(new List<DalProjectStep>());
        _userProjectRepository.Setup(r => r.AllAsyncByProjectId(projectId, userId))
            .ReturnsAsync(new List<DalUserProject>());
        _stepRepository.Setup(r => r.Exists(newStepId)).Returns(true);

        await _sut.UpdateAsync(bll, userId);

        _projectStepRepository.Verify(r => r.Add(It.Is<DalProjectStep>(ps => ps.StepId == newStepId)), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_RemovesOldSteps()
    {
        var projectId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var authorId = Guid.NewGuid();
        var oldStepId = Guid.NewGuid();
        var oldProjectStepId = Guid.NewGuid();

        var bll = new Project
        {
            Id = projectId,
            TitleInEstonian = "project",
            Description = "desc",
            AuthorId = authorId,
            FolderIds = new List<Guid>(),
            TagIds = new List<Guid>(),
            StepIds = new List<Guid>()
        };

        var existingSteps = new List<DalProjectStep>
        {
            new() { Id = oldProjectStepId, StepId = oldStepId, ProjectId = projectId }
        };

        _projectRepository.Setup(r => r.UpdateAsync(It.IsAny<DalProject>(), userId))
            .ReturnsAsync(new DalProject { Id = projectId });
        _projectFolderRepository.Setup(r => r.AllAsyncByProjectId(projectId, userId))
            .ReturnsAsync(new List<DalProjectFolder>());
        _projectTagRepository.Setup(r => r.AllAsyncByProjectId(projectId, userId))
            .ReturnsAsync(new List<DalProjectTag>());
        _projectStepRepository.Setup(r => r.AllAsyncByProjectId(projectId, userId))
            .ReturnsAsync(existingSteps);
        _userProjectRepository.Setup(r => r.AllAsyncByProjectId(projectId, userId))
            .ReturnsAsync(new List<DalUserProject>());

        await _sut.UpdateAsync(bll, userId);

        _projectStepRepository.Verify(r => r.Remove(oldProjectStepId), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ManagesUserProjectsWithPrimarySupervisor()
    {
        var projectId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var authorId = Guid.NewGuid();
        var primarySupervisorId = Guid.NewGuid();

        var bll = new Project
        {
            Id = projectId,
            TitleInEstonian = "project",
            Description = "desc",
            AuthorId = authorId,
            PrimarySupervisorId = primarySupervisorId,
            FolderIds = new List<Guid>(),
            TagIds = new List<Guid>(),
            StepIds = new List<Guid>()
        };

        _projectRepository.Setup(r => r.UpdateAsync(It.IsAny<DalProject>(), userId))
            .ReturnsAsync(new DalProject { Id = projectId });
        _projectFolderRepository.Setup(r => r.AllAsyncByProjectId(projectId, userId))
            .ReturnsAsync(new List<DalProjectFolder>());
        _projectTagRepository.Setup(r => r.AllAsyncByProjectId(projectId, userId))
            .ReturnsAsync(new List<DalProjectTag>());
        _projectStepRepository.Setup(r => r.AllAsyncByProjectId(projectId, userId))
            .ReturnsAsync(new List<DalProjectStep>());
        _userProjectRepository.Setup(r => r.AllAsyncByProjectId(projectId, userId))
            .ReturnsAsync(new List<DalUserProject>());

        await _sut.UpdateAsync(bll, userId);

        _userProjectRepository.Verify(r => r.Add(It.Is<DalUserProject>(up => 
            up.UserId == primarySupervisorId && up.ProjectId == projectId)), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ManagesUserProjectsWithExternalSupervisor()
    {
        var projectId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var authorId = Guid.NewGuid();
        var externalSupervisorId = Guid.NewGuid();

        var bll = new Project
        {
            Id = projectId,
            TitleInEstonian = "project",
            Description = "desc",
            AuthorId = authorId,
            ExternalSupervisorId = externalSupervisorId,
            FolderIds = new List<Guid>(),
            TagIds = new List<Guid>(),
            StepIds = new List<Guid>()
        };

        _projectRepository.Setup(r => r.UpdateAsync(It.IsAny<DalProject>(), userId))
            .ReturnsAsync(new DalProject { Id = projectId });
        _projectFolderRepository.Setup(r => r.AllAsyncByProjectId(projectId, userId))
            .ReturnsAsync(new List<DalProjectFolder>());
        _projectTagRepository.Setup(r => r.AllAsyncByProjectId(projectId, userId))
            .ReturnsAsync(new List<DalProjectTag>());
        _projectStepRepository.Setup(r => r.AllAsyncByProjectId(projectId, userId))
            .ReturnsAsync(new List<DalProjectStep>());
        _userProjectRepository.Setup(r => r.AllAsyncByProjectId(projectId, userId))
            .ReturnsAsync(new List<DalUserProject>());

        await _sut.UpdateAsync(bll, userId);

        _userProjectRepository.Verify(r => r.Add(It.Is<DalUserProject>(up => 
            up.UserId == externalSupervisorId && up.ProjectId == projectId)), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_RemovesSupervisor_WhenSetToNull()
    {
        var projectId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var authorId = Guid.NewGuid();
        var primarySupervisorId = Guid.NewGuid();
        var supervisorUserProjectId = Guid.NewGuid();

        var bll = new Project
        {
            Id = projectId,
            TitleInEstonian = "project",
            Description = "desc",
            AuthorId = authorId,
            PrimarySupervisorId = null,
            FolderIds = new List<Guid>(),
            TagIds = new List<Guid>(),
            StepIds = new List<Guid>()
        };

        var existingUserProjects = new List<DalUserProject>
        {
            new() { Id = supervisorUserProjectId, UserId = primarySupervisorId, ProjectId = projectId, UserProjectRoleId = Guid.Parse("00000000-0000-0000-0000-000000000002") } // PrimarySupervisor role
        };

        _projectRepository.Setup(r => r.UpdateAsync(It.IsAny<DalProject>(), userId))
            .ReturnsAsync(new DalProject { Id = projectId });
        _projectFolderRepository.Setup(r => r.AllAsyncByProjectId(projectId, userId))
            .ReturnsAsync(new List<DalProjectFolder>());
        _projectTagRepository.Setup(r => r.AllAsyncByProjectId(projectId, userId))
            .ReturnsAsync(new List<DalProjectTag>());
        _projectStepRepository.Setup(r => r.AllAsyncByProjectId(projectId, userId))
            .ReturnsAsync(new List<DalProjectStep>());
        _userProjectRepository.Setup(r => r.AllAsyncByProjectId(projectId, userId))
            .ReturnsAsync(existingUserProjects);

        await _sut.UpdateAsync(bll, userId);

        _userProjectRepository.Verify(r => r.Remove(supervisorUserProjectId), Times.Once);
    }

    [Fact]
    public async Task RemoveAsync_DelegatesToRepository()
    {
        var id = Guid.NewGuid();

        _projectRepository.Setup(r => r.FindAsync(id))
            .ReturnsAsync(new DalProject { Id = id, TitleInEstonian = "proj", Description = "desc" });

        await _sut.RemoveAsync(id);

        _projectRepository.Verify(r => r.FindAsync(id), Times.Once);
        _projectRepository.Verify(r => r.RemoveAsync(id), Times.Once);
    }
}
