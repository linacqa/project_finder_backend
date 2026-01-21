using Base.Contracts;

namespace DTO.v1.Mappers;

public class FolderMapper : IMapper<DTO.v1.Folder, BLL.DTO.Folder>
{
    public Folder? Map(BLL.DTO.Folder? entity)
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

    public BLL.DTO.Folder? Map(Folder? entity)
    {
        if (entity == null) return null;
        
        var result = new BLL.DTO.Folder()
        {
            Id = entity.Id,
            Name = entity.Name,
            IsVisible = entity.IsVisible,
        };
        
        return result;
    }

    public BLL.DTO.Folder Map(FolderCreate entity)
    {
        var result = new BLL.DTO.Folder()
        {
            Id = Guid.NewGuid(),
            Name = entity.Name,
            IsVisible = entity.IsVisible,
        };
        
        return result;
    }
}