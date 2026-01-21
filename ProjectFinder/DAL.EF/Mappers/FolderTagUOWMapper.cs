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
            Folder = entity.Folder != null ? new Folder()
            {
                Id = entity.Folder.Id,
                Name = entity.Folder.Name,
            } : null,
            TagId = entity.TagId,
            Tag = entity.Tag != null ? new Tag()
            {
                Id = entity.Tag.Id,
                Name = entity.Tag.Name,
            } : null,
        };
    }

    public Domain.FolderTag? Map(FolderTag? entity)
    {
        if (entity == null) return null;
        
        return new Domain.FolderTag()
        {
            Id = entity.Id,
            FolderId = entity.FolderId,
            Folder = entity.Folder != null ? new Domain.Folder()
            {
                Id = entity.Folder.Id,
                Name = entity.Folder.Name,
            } : null,
            TagId = entity.TagId,
            Tag = entity.Tag != null ? new Domain.Tag()
            {
                Id = entity.Tag.Id,
                Name = entity.Tag.Name,
            } : null,
        };
    }
}