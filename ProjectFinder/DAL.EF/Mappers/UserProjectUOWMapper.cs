using Base.Contracts;
using DAL.DTO;
using DAL.EF.Mappers.Identity;

namespace DAL.EF.Mappers;

public class UserProjectUOWMapper : IMapper<DAL.DTO.UserProject, Domain.UserProject>
{
    public UserProject? Map(Domain.UserProject? entity)
    {
        if (entity == null) return null;
        
        return new UserProject()
        {
            Id = entity.Id,
            UserId = entity.UserId,
            User = entity.User != null ? new AppUserUOWMapper().Map(entity.User) : null,
            ProjectId = entity.ProjectId,
            Project = entity.Project != null ? new ProjectUOWMapper().Map(entity.Project) : null,
        };
    }

    public Domain.UserProject? Map(UserProject? entity)
    {
        if (entity == null) return null;
        
        return new Domain.UserProject()
        {
            Id = entity.Id,
            UserId = entity.UserId,
            User = entity.User != null ? new AppUserUOWMapper().Map(entity.User) : null,
            ProjectId = entity.ProjectId,
            Project = entity.Project != null ? new ProjectUOWMapper().Map(entity.Project) : null,
        };
    }
}