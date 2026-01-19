using Base.DAL.EF;
using DAL.Contracts;
using DAL.DTO;
using DAL.EF.Mappers;

namespace DAL.EF.Repositories;

public class ApplicationRepository : BaseRepository<Application, Domain.Application>, IApplicationRepository
{
    public ApplicationRepository(AppDbContext repositoryDbContext) : base(repositoryDbContext, new ApplicationUOWMapper())
    {
    }
}
