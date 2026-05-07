using AwesomeAssertions;
using DAL.EF.Mappers;

using DalProject = global::DAL.DTO.Project;
using DalTag = global::DAL.DTO.Tag;
using DalUserProject = global::DAL.DTO.UserProject;
using DalUser = global::DAL.DTO.Identity.AppUser;
using DalUserProjectRole = global::DAL.DTO.UserProjectRole;

using DomainProject = global::Domain.Project;
using DomainTag = global::Domain.Tag;
using DomainUserProject = global::Domain.UserProject;
using DomainAppUser = global::Domain.Identity.AppUser;
using DomainUserProjectRole = global::Domain.UserProjectRole;

namespace Tests.Unit.DAL.Mappers;

public class ProjectDALMapper_Tests
{
    private readonly ProjectUOWMapper _sut = new();

    [Fact]
    public void Map_DomainToDal_AllFieldsCopied()
    {
        var projectId = Guid.NewGuid();
        var tagId = Guid.NewGuid();
        var userProjectId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var roleId = Guid.NewGuid();

        var domain = new DomainProject
        {
            Id = projectId,
            TitleInEstonian = "et",
            TitleInEnglish = "en",
            Description = "desc",
            Client = "client",
            Supervisor = "sup",
            ExternalSupervisor = "ext",
            MinStudents = 1,
            MaxStudents = 3,
            ProjectTypeId = Guid.NewGuid(),
            ProjectType = new Domain.ProjectType { Id = Guid.NewGuid(), Name = "type" },
            ProjectStatusId = Guid.NewGuid(),
            ProjectStatus = new Domain.ProjectStatus { Id = Guid.NewGuid(), Name = "status" },
            Deadline = DateTime.UtcNow.Date,
            ProjectTags = new List<Domain.ProjectTag>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    ProjectId = projectId,
                    TagId = tagId,
                    Tag = new DomainTag { Id = tagId, Name = "tag" },
                }
            },
            UserProjects = new List<DomainUserProject>
            {
                new()
                {
                    Id = userProjectId,
                    UserId = userId,
                    ProjectId = projectId,
                    User = new DomainAppUser { Id = userId, FirstName = "u-first", LastName = "u-last", Email = "u@e" },
                    UserProjectRoleId = roleId,
                    UserProjectRole = new DomainUserProjectRole { Id = roleId, Name = "student" },
                }
            },
            CreatedAt = DateTime.UtcNow,
        };

        var dal = _sut.Map(domain)!;

        dal.Should().NotBeNull();
        dal.Id.Should().Be(projectId);
        dal.TitleInEstonian.Should().Be("et");
        dal.TitleInEnglish.Should().Be("en");
        dal.Description.Should().Be("desc");
        dal.Client.Should().Be("client");
        dal.Supervisor.Should().Be("sup");
        dal.ExternalSupervisor.Should().Be("ext");
        dal.MinStudents.Should().Be(1);
        dal.MaxStudents.Should().Be(3);
        dal.ProjectTypeId.Should().Be(domain.ProjectTypeId);
        dal.ProjectType.Should().NotBeNull();
        dal.ProjectType!.Name.Should().Be("type");
        dal.ProjectStatusId.Should().Be(domain.ProjectStatusId);
        dal.ProjectStatus.Should().NotBeNull();
        dal.ProjectStatus!.Name.Should().Be("status");
        dal.Deadline.Should().Be(domain.Deadline);
        dal.ProjectTags.Should().NotBeNull();
        dal.ProjectTags!.Count.Should().Be(1);
        dal.ProjectTags.First().Tag.Should().NotBeNull();
        dal.ProjectTags.First().Tag!.Name.Should().Be("tag");
        dal.UserProjects.Should().NotBeNull();
        dal.UserProjects!.Count.Should().Be(1);
        dal.UserProjects.First().User.Should().NotBeNull();
        dal.UserProjects.First().User!.Id.Should().Be(userId);
        dal.UserProjects.First().UserProjectRole.Should().NotBeNull();
        dal.UserProjects.First().UserProjectRole!.Name.Should().Be("student");
        dal.CreatedAt.Should().Be(domain.CreatedAt);
    }

    [Fact]
    public void Map_DalToDomain_AllFieldsCopied()
    {
        var dal = new DalProject
        {
            Id = Guid.NewGuid(),
            TitleInEstonian = "et",
            TitleInEnglish = "en",
            Description = "desc",
            Client = "client",
            Supervisor = "sup",
            ExternalSupervisor = "ext",
            MinStudents = 1,
            MaxStudents = 3,
            ProjectTypeId = Guid.NewGuid(),
            ProjectStatusId = Guid.NewGuid(),
            Deadline = DateTime.UtcNow.Date,
        };

        var domain = _sut.Map(dal)!;

        domain.Should().NotBeNull();
        domain.Id.Should().Be(dal.Id);
        domain.TitleInEstonian.Should().Be("et");
        domain.TitleInEnglish.Should().Be("en");
        domain.Description.Should().Be("desc");
        domain.Client.Should().Be("client");
        domain.Supervisor.Should().Be("sup");
        domain.ExternalSupervisor.Should().Be("ext");
        domain.MinStudents.Should().Be(1);
        domain.MaxStudents.Should().Be(3);
        domain.ProjectTypeId.Should().Be(dal.ProjectTypeId);
        domain.ProjectStatusId.Should().Be(dal.ProjectStatusId);
        domain.Deadline.Should().Be(dal.Deadline);
    }

    [Fact]
    public void Map_NullDomain_ReturnsNull()
    {
        _sut.Map((DomainProject?)null).Should().BeNull();
    }

    [Fact]
    public void Map_NullDal_ReturnsNull()
    {
        _sut.Map((DalProject?)null).Should().BeNull();
    }
}

