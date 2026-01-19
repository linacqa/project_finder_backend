using Base.DAL.EF;
using DAL.Contracts;
using DAL.DTO;
using DAL.EF.Mappers;

namespace DAL.EF.Repositories;

public class FolderTagRepository : BaseRepository<FolderTag, Domain.FolderTag>, IFolderTagRepository
{
    public FolderTagRepository(AppDbContext repositoryDbContext) : base(repositoryDbContext, new FolderTagUOWMapper())
    {
    }
}