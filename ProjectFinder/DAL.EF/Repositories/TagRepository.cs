using Base.DAL.EF;
using DAL.Contracts;
using DAL.DTO;
using DAL.EF.Mappers;

namespace DAL.EF.Repositories;

public class TagRepository : BaseRepository<Tag, Domain.Tag>, ITagRepository
{
    public TagRepository(AppDbContext repositoryDbContext) : base(repositoryDbContext, new TagUOWMapper())
    {
    }
}