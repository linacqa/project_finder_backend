using Base.Contracts;
using DAL.DTO;

namespace DAL.EF.Mappers;

public class FolderUOWMapper : IMapper<DAL.DTO.Folder, Domain.Folder>
{
    public Folder? Map(Domain.Folder? entity)
    {
        if (entity == null) return null;
        
        return new Folder()
        {
            Id = entity.Id,
            Name = entity.Name,
            IsVisible =  entity.IsVisible,
        };
    }

    public Domain.Folder? Map(Folder? entity)
    {
        if (entity == null) return null;
        
        return new Domain.Folder()
        {
            Id = entity.Id,
            Name = entity.Name,
            IsVisible =  entity.IsVisible,
        };
    }
}