using Base.Contracts;
using DAL.DTO;
using DAL.DTO.Identity;
using DAL.EF.Mappers.Identity;

namespace DAL.EF.Mappers;

public class CommentUOWMapper : IMapper<DAL.DTO.Comment, Domain.Comment>
{
    public Comment? Map(Domain.Comment? entity)
    {
        if (entity == null) return null;
        
        return new Comment()
        {
            Id = entity.Id,
            UserId = entity.UserId,
            User = entity.User != null ? new AppUser()
            {
                Id = entity.User.Id,
                FirstName = entity.User.FirstName,
                LastName = entity.User.LastName,
                Email = entity.User.Email,
                AzureObjectId = entity.User.AzureObjectId,
                AuthType = entity.User.AuthType,
            } : null,
            ProjectId = entity.ProjectId,
            Project = entity.Project != null ? new Project()
            {
                Id = entity.Project.Id,
                TitleInEstonian = entity.Project.TitleInEstonian,
                TitleInEnglish = entity.Project.TitleInEnglish,
                Description = entity.Project.Description,
                Client = entity.Project.Client,
                ExternalSupervisor = entity.Project.ExternalSupervisor,
                MinStudents = entity.Project.MinStudents,
                MaxStudents = entity.Project.MaxStudents,
                ProjectTypeId = entity.Project.ProjectTypeId,
                ProjectType = entity.Project.ProjectType != null ? new ProjectType()
                {
                    Id = entity.Project.ProjectType.Id,
                    Name = entity.Project.ProjectType.Name,
                } : null,
                ProjectStatusId = entity.Project.ProjectStatusId,
                ProjectStatus = entity.Project.ProjectStatus != null ? new ProjectStatus()
                {
                    Id = entity.Project.ProjectStatus.Id,
                    Name = entity.Project.ProjectStatus.Name,
                } : null,
                Deadline = entity.Project.Deadline,
                AttachmentsPaths = entity.Project.AttachmentsPaths,
            } : null,
            ReplyToCommentId = entity.ReplyToCommentId,
            ReplyToComment = entity.ReplyToComment != null ? Map(entity.ReplyToComment) : null,
            Content = entity.Content,
            CreatedAt = entity.CreatedAt,
        };
    }

    public Domain.Comment? Map(Comment? entity)
    {
        if (entity == null) return null;
        
        return new Domain.Comment()
        {
            Id = entity.Id,
            UserId = entity.UserId,
            ProjectId = entity.ProjectId,
            ReplyToCommentId = entity.ReplyToCommentId,
            Content = entity.Content,
        };
    }
}