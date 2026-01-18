using Base.Contracts;
using DAL.DTO;

namespace DAL.EF.Mappers;

public class FolderTagUOWMapper : IMapper<DAL.DTO.FolderTag, Domain.FolderTag>
{
    public FolderTag? Map(Domain.FolderTag? entity)
    {
        if (entity == null) return null;
        
        return new FolderTag()
        {
            Id = entity.Id,
            FolderId = entity.FolderId,
            Folder = entity.Folder != null ? new FolderUOWMapper().Map(entity.Folder) : null,
            TagId = entity.TagId,
            Tag = entity.Tag != null ? new TagUOWMapper().Map(entity.Tag) : null,
        };
    }

    public Domain.FolderTag? Map(FolderTag? entity)
    {
        if (entity == null) return null;
        
        return new Domain.FolderTag()
        {
            Id = entity.Id,
            FolderId = entity.FolderId,
            Folder = entity.Folder != null ? new FolderUOWMapper().Map(entity.Folder) : null,
            TagId = entity.TagId,
            Tag = entity.Tag != null ? new TagUOWMapper().Map(entity.Tag) : null,
        };
    }
}