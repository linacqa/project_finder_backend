using Base.DAL.EF;
using DAL.Contracts;
using DAL.DTO;
using DAL.EF.Mappers;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF.Repositories;

public class InvitationRepository : BaseRepository<Invitation, Domain.Invitation>, IInvitationRepository
{
    public InvitationRepository(AppDbContext repositoryDbContext) : base(repositoryDbContext, new InvitationUOWMapper())
    {
    }
    
    public async Task<IEnumerable<Invitation>> AllAsyncToUser(Guid userId = default!)
    {
        return (await GetQuery(userId)
                .Where(e => e.ToUserId == userId)
                .Include(e => e.ToUser)
                .Include(e => e.User)
                .Include(e => e.Group)
                    .ThenInclude(e => e.UserGroups)!
                        .ThenInclude(u => u.User)
                .ToListAsync())
            .Select(e => Mapper.Map(e)!);
    }
    
    public async Task<IEnumerable<Invitation>> AllAsyncByGroupId(Guid groupId, Guid userId = default)
    {
        return (await GetQuery(userId)
                .Where(e => e.GroupId == groupId)
                .Include(e => e.ToUser)
                .Include(e => e.User)
                .Include(e => e.Group)
                    .ThenInclude(e => e.UserGroups)!
                        .ThenInclude(u => u.User)
                .ToListAsync())
            .Select(e => Mapper.Map(e)!);
    }
}