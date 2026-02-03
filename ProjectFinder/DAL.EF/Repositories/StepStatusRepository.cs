using Base.DAL.EF;
using DAL.Contracts;
using DAL.DTO;
using DAL.EF.Mappers;

namespace DAL.EF.Repositories;

public class StepStatusRepository : BaseRepository<StepStatus, Domain.StepStatus>, IStepStatusRepository
{
    public StepStatusRepository(AppDbContext repositoryDbContext) : base(repositoryDbContext, new StepStatusUOWMapper())
    {
    }
}