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
        return (await RepositoryDbSet.AsQueryable()
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
        return (await RepositoryDbSet.AsQueryable()
                .Where(e => e.GroupId == groupId)
                .Include(e => e.ToUser)
                .Include(e => e.User)
                .Include(e => e.Group)
                    .ThenInclude(e => e.UserGroups)!
                        .ThenInclude(u => u.User)
                .ToListAsync())
            .Select(e => Mapper.Map(e)!);
    }
    
    public override DAL.DTO.Invitation? Update(DAL.DTO.Invitation entity, Guid userId = default!)
    {
        return Mapper.Map(RepositoryDbSet.Update(Mapper.Map(entity)!).Entity)!;
    }

    public override async Task<DAL.DTO.Invitation?> UpdateAsync(DAL.DTO.Invitation entity, Guid userId = default!)
    {
        var domainEntity = Mapper.Map(entity)!;

        return Mapper.Map(RepositoryDbSet.Update(domainEntity).Entity)!;
    }
}