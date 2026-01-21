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
                ProjectType = entity.Project.ProjectType,
                ProjectStatus = entity.Project.ProjectStatus,
                Deadline = entity.Project.Deadline,
                AttachmentsPaths = entity.Project.AttachmentsPaths,
                CreatorId = entity.Project.UserId,
                Creator = entity.Project.User != null ? new AppUser()
                {
                    Id = entity.Project.User.Id,
                    FirstName = entity.Project.User.FirstName,
                    LastName = entity.Project.User.LastName,
                    AzureObjectId = entity.Project.User.AzureObjectId,
                    AuthType = entity.Project.User.AuthType,
                } : null,
            } : null,
            ReplyToCommentId = entity.ReplyToCommentId,
            ReplyToComment = entity.ReplyToComment != null ? Map(entity.ReplyToComment) : null,
            Content = entity.Content,
        };
    }

    public Domain.Comment? Map(Comment? entity)
    {
        if (entity == null) return null;
        
        return new Domain.Comment()
        {
            Id = entity.Id,
            UserId = entity.UserId,
            User = entity.User != null ? new Domain.Identity.AppUser()
            {
                Id = entity.User.Id,
                FirstName = entity.User.FirstName,
                LastName = entity.User.LastName,
                AzureObjectId = entity.User.AzureObjectId,
                AuthType = entity.User.AuthType,
            } : null,
            ProjectId = entity.ProjectId,
            Project = entity.Project != null ? new Domain.Project()
            {
                Id = entity.Project.Id,
                TitleInEstonian = entity.Project.TitleInEstonian,
                TitleInEnglish = entity.Project.TitleInEnglish,
                Description = entity.Project.Description,
                Client = entity.Project.Client,
                ExternalSupervisor = entity.Project.ExternalSupervisor,
                MinStudents = entity.Project.MinStudents,
                MaxStudents = entity.Project.MaxStudents,
                ProjectType = entity.Project.ProjectType,
                ProjectStatus = entity.Project.ProjectStatus,
                Deadline = entity.Project.Deadline,
                AttachmentsPaths = entity.Project.AttachmentsPaths,
                UserId = entity.Project.CreatorId,
                User = entity.Project.Creator != null ? new Domain.Identity.AppUser()
                {
                    Id = entity.Project.Creator.Id,
                    FirstName = entity.Project.Creator.FirstName,
                    LastName = entity.Project.Creator.LastName,
                    AzureObjectId = entity.Project.Creator.AzureObjectId,
                    AuthType = entity.Project.Creator.AuthType,
                } : null,
            } : null,
            ReplyToCommentId = entity.ReplyToCommentId,
            ReplyToComment = entity.ReplyToComment != null ? Map(entity.ReplyToComment) : null,
            Content = entity.Content,
        };
    }
}