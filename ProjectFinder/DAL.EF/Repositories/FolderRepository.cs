using Base.DAL.EF;
using DAL.Contracts;
using DAL.DTO;
using DAL.EF.Mappers;

namespace DAL.EF.Repositories;

public class FolderRepository : BaseRepository<Folder, Domain.Folder>, IFolderRepository
{
    public FolderRepository(AppDbContext repositoryDbContext) : base(repositoryDbContext, new FolderUOWMapper())
    {
    }
}