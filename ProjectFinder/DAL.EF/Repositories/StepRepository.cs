using Base.DAL.EF;
using DAL.Contracts;
using DAL.DTO;
using DAL.EF.Mappers;

namespace DAL.EF.Repositories;

public class StepRepository : BaseRepository<Step, Domain.Step>, IStepRepository
{
    public StepRepository(AppDbContext repositoryDbContext) : base(repositoryDbContext, new StepUOWMapper())
    {
    }
}