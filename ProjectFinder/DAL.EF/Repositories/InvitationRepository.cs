using Base.DAL.EF;
using DAL.Contracts;
using DAL.DTO;
using DAL.EF.Mappers;

namespace DAL.EF.Repositories;

public class InvitationRepository : BaseRepository<Invitation, Domain.Invitation>, IInvitationRepository
{
    public InvitationRepository(AppDbContext repositoryDbContext) : base(repositoryDbContext, new InvitationUOWMapper())
    {
    }
}