using Base.Contracts;
using BLL.DTO;

namespace BLL.Mappers;

public class UserProjectBLLMapper : IMapper<BLL.DTO.UserProject, DAL.DTO.UserProject>
{
    public UserProject? Map(DAL.DTO.UserProject? entity)
    {
        if (entity == null) return null;
        
        var result = new UserProject()
        {
            Id = entity.Id,
            UserId = entity.UserId,
            ProjectId = entity.ProjectId,
        };
        
        return result;
    }

    public DAL.DTO.UserProject? Map(UserProject? entity)
    {
        if (entity == null) return null;
        
        var result = new DAL.DTO.UserProject()
        {
            Id = entity.Id,
            UserId = entity.UserId,
            ProjectId = entity.ProjectId,
        };
        
        return result;
    }
}