using Base.Contracts;
using BLL.DTO;

namespace BLL.Mappers;

public class GroupProjectBLLMapper : IMapper<BLL.DTO.GroupProject, DAL.DTO.GroupProject>
{
    public GroupProject? Map(DAL.DTO.GroupProject? entity)
    {
        if (entity == null) return null;
        
        var result = new GroupProject()
        {
            Id = entity.Id,
            ProjectId = entity.ProjectId,
            GroupId = entity.GroupId,
        };
        
        return result;
    }

    public DAL.DTO.GroupProject? Map(GroupProject? entity)
    {
        if (entity == null) return null;
        
        var result = new DAL.DTO.GroupProject()
        {
            Id = entity.Id,
            ProjectId = entity.ProjectId,
            GroupId = entity.GroupId,
        };
        
        return result;
    }
}