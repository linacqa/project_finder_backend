using Base.Contracts;
using BLL.DTO;

namespace BLL.Mappers;

public class FolderTagBLLMapper : IMapper<BLL.DTO.FolderTag, DAL.DTO.FolderTag>
{
    public FolderTag? Map(DAL.DTO.FolderTag? entity)
    {
        if (entity == null) return null;
        
        var result = new FolderTag()
        {
            Id = entity.Id,
            FolderId = entity.FolderId,
            TagId = entity.TagId,
        };
        
        return result;
    }

    public DAL.DTO.FolderTag? Map(FolderTag? entity)
    {
        if (entity == null) return null;
        
        var result = new DAL.DTO.FolderTag()
        {
            Id = entity.Id,
            FolderId = entity.FolderId,
            TagId = entity.TagId,
        };
        
        return result;
    }
}