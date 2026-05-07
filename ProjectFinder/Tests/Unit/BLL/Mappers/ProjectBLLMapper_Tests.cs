using AwesomeAssertions;
using BLL.DTO;
using BLL.DTO.Identity;
using BLL.Mappers;
using DalProject = global::DAL.DTO.Project;
using DalProjectType = global::DAL.DTO.ProjectType;
using DalProjectStatus = global::DAL.DTO.ProjectStatus;
using DalProjectTag = global::DAL.DTO.ProjectTag;
using DalTag = global::DAL.DTO.Tag;
using DalUserProject = global::DAL.DTO.UserProject;
using DalUser = global::DAL.DTO.Identity.AppUser;
using DalUserProjectRole = global::DAL.DTO.UserProjectRole;

namespace Tests.Unit.BLL.Mappers;

public class ProjectBLLMapper_Tests
{
    private readonly ProjectBLLMapper _sut = new();

    [Fact]
    public void Map_DalToBll_AllFieldsCopied()
    {
        var id = Guid.NewGuid();
        var titleInEstonian = "et";
        var titleInEnglish = "en";
        var description = "description";
        var client = "client";
        var primarySupervisor = "primarySupervisor";
        var externalSupervisor = "externalSupervisor";
        var minStudents = 1;
        var maxStudents = 10;
        var projectTypeId = Guid.NewGuid();
        var projectStatusId = Guid.NewGuid();
        var deadline = DateTime.UtcNow;
        var createdAt = DateTime.UtcNow;
        
        var tagId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var userProjectRoleId = Guid.NewGuid();

        var dal = new DalProject
        {
            Id = id,
            TitleInEstonian = titleInEstonian,
            TitleInEnglish = titleInEnglish,
            Description = description,
            Client = client,
            Supervisor = primarySupervisor,
            ExternalSupervisor = externalSupervisor,
            MinStudents = minStudents,
            MaxStudents = maxStudents,
            ProjectTypeId = projectTypeId,
            ProjectType = new DalProjectType
            {
                Id = projectTypeId,
                Name = "type",
            },
            ProjectStatusId = projectStatusId,
            ProjectStatus = new DalProjectStatus
            {
                Id = projectStatusId,
                Name = "status",
            },
            Deadline = deadline,
            ProjectTags = new List<DalProjectTag>
            {
                new DalProjectTag
                {
                    Id = Guid.NewGuid(),
                    ProjectId = id,
                    TagId = tagId,
                    Tag = new DalTag
                    {
                        Id = tagId,
                        Name = "tag",
                    }
                }
            },
            UserProjects = new List<DalUserProject>
            {
                new DalUserProject
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    ProjectId = id,
                    User = new DalUser
                    {
                        Id = userId,
                        FirstName = "first",
                        LastName = "last",
                        Email = "user@example.com",
                    },
                    UserProjectRoleId = userProjectRoleId,
                    UserProjectRole = new DalUserProjectRole
                    {
                        Id = userProjectRoleId,
                        Name = "role",
                    }
                }
            },
            CreatedAt = createdAt,
        };

        var bll = _sut.Map(dal)!;

        bll.Id.Should().Be(id);
        bll.TitleInEstonian.Should().Be(titleInEstonian);
        bll.TitleInEnglish.Should().Be(titleInEnglish);
        bll.Description.Should().Be(description);
        bll.Client.Should().Be(client);
        bll.PrimarySupervisor.Should().Be(primarySupervisor);
        bll.ExternalSupervisor.Should().Be(externalSupervisor);
        bll.MinStudents.Should().Be(minStudents);
        bll.MaxStudents.Should().Be(maxStudents);
        bll.ProjectTypeId.Should().Be(projectTypeId);
        bll.ProjectType!.Id.Should().Be(projectTypeId);
        bll.ProjectType.Name.Should().Be("type");
        bll.ProjectStatusId.Should().Be(projectStatusId);
        bll.ProjectStatus!.Id.Should().Be(projectStatusId);
        bll.ProjectStatus.Name.Should().Be("status");
        bll.Deadline.Should().Be(deadline);
        bll.CreatedAt.Should().Be(createdAt);
        bll.ProjectTags!.Count.Should().Be(1);
        bll.ProjectTags.First().TagId.Should().Be(tagId);
        bll.ProjectTags.First().Tag!.Name.Should().Be("tag");
        bll.UserProjects!.Count.Should().Be(1);
        bll.UserProjects.First().UserId.Should().Be(userId);
        bll.UserProjects.First().User!.FirstName.Should().Be("first");
        bll.UserProjects.First().User!.LastName.Should().Be("last");
        bll.UserProjects.First().User!.Email.Should().Be("user@example.com");
    }

    [Fact]
    public void Map_BllToDal_AllFieldsCopied()
    {
        var id = Guid.NewGuid();
        var titleInEstonian = "et";
        var titleInEnglish = "en";
        var description = "description";
        var client = "client";
        var primarySupervisor = "primarySupervisor";
        var externalSupervisor = "externalSupervisor";
        var minStudents = 1;
        var maxStudents = 10;
        var projectTypeId = Guid.NewGuid();
        var projectStatusId = Guid.NewGuid();
        var deadline = DateTime.UtcNow;
        var createdAt = DateTime.UtcNow;
        
        var tagId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var userProjectRoleId = Guid.NewGuid();

        var bll = new Project
        {
            Id = id,
            TitleInEstonian = titleInEstonian,
            TitleInEnglish = titleInEnglish,
            Description = description,
            Client = client,
            PrimarySupervisor = primarySupervisor,
            ExternalSupervisor = externalSupervisor,
            MinStudents = minStudents,
            MaxStudents = maxStudents,
            ProjectTypeId = projectTypeId,
            ProjectType = new ProjectType
            {
                Id = projectTypeId,
                Name = "type",
            },
            ProjectStatusId = projectStatusId,
            ProjectStatus = new ProjectStatus
            {
                Id = projectStatusId,
                Name = "status",
            },
            Deadline = deadline,
            ProjectTags = new List<ProjectTag>
            {
                new ProjectTag
                {
                    Id = Guid.NewGuid(),
                    ProjectId = id,
                    TagId = tagId,
                    Tag = new Tag
                    {
                        Id = tagId,
                        Name = "tag",
                    }
                }
            },
            UserProjects = new List<UserProject>
            {
                new UserProject
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    ProjectId = id,
                    User = new AppUser
                    {
                        Id = userId,
                        FirstName = "first",
                        LastName = "last",
                        Email = "user@example.com",
                    },
                    UserProjectRoleId = userProjectRoleId,
                    UserProjectRole = new UserProjectRole
                    {
                        Id = userProjectRoleId,
                        Name = "role",
                    }
                }
            },
            CreatedAt = createdAt,
        };

        var dal = _sut.Map(bll)!;

        dal.Id.Should().Be(id);
        dal.TitleInEstonian.Should().Be(titleInEstonian);
        dal.TitleInEnglish.Should().Be(titleInEnglish);
        dal.Description.Should().Be(description);
        dal.Client.Should().Be(client);
        dal.Supervisor.Should().Be(primarySupervisor);
        dal.ExternalSupervisor.Should().Be(externalSupervisor);
        dal.MinStudents.Should().Be(minStudents);
        dal.MaxStudents.Should().Be(maxStudents);
        dal.ProjectTypeId.Should().Be(projectTypeId);
        dal.ProjectStatusId.Should().Be(projectStatusId);
        dal.Deadline.Should().Be(deadline);
    }

    [Fact]
    public void Map_NullDal_ReturnsNull()
    {
        _sut.Map((DalProject?)null).Should().BeNull();
    }

    [Fact]
    public void Map_NullBll_ReturnsNull()
    {
        _sut.Map((Project?)null).Should().BeNull();
    }
}