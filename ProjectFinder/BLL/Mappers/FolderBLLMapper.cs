using Base.Contracts;
using BLL.DTO;

namespace BLL.Mappers;

public class FolderBLLMapper : IMapper<BLL.DTO.Folder, DAL.DTO.Folder>
{
    public Folder? Map(DAL.DTO.Folder? entity)
    {
        if (entity == null) return null;
        
        var result = new Folder()
        {
            Id = entity.Id,
            Name = entity.Name,
            IsVisible = entity.IsVisible,
        };
        
        return result;
    }

    public DAL.DTO.Folder? Map(Folder? entity)
    {
        if (entity == null) return null;
        
        var result = new DAL.DTO.Folder()
        {
            Id = entity.Id,
            Name = entity.Name,
            IsVisible = entity.IsVisible,
        };
        
        return result;
    }
}