using Base.BLL;
using Base.Contracts;
using BLL.Contracts;
using BLL.DTO;
using DAL.Contracts;

namespace BLL.Services;

public class CommentService : BaseService<BLL.DTO.Comment, DAL.DTO.Comment, DAL.Contracts.ICommentRepository>, ICommentService
{
    private readonly IUserProjectRepository _userProjectRepository;
    
    public CommentService(
        IAppUOW serviceUOW, 
        IMapper<Comment, DAL.DTO.Comment, Guid> mapper) : base(serviceUOW, serviceUOW.CommentRepository, mapper)
    {
        _userProjectRepository = serviceUOW.UserProjectRepository;
    }
    
    public async Task<IEnumerable<BLL.DTO.Comment>> AllAsyncByProjectId(Guid projectId, Guid userId)
    {
        var userInProject = await _userProjectRepository.UserInProject(projectId, userId);
        
        if (!userInProject)
        {
            throw new UnauthorizedAccessException("User is not part of the project.");
        }
        
        var entities = await ServiceRepository.AllAsyncByProjectId(projectId, userId);
        return entities.Select(e => Mapper.Map(e)!).ToList();
    }
    
    public async Task AddAsync(BLL.DTO.Comment entity, Guid userId = default)
    {
        var userInProject = await _userProjectRepository.UserInProject(entity.ProjectId, userId);
        
        if (!userInProject)
        {
            throw new UnauthorizedAccessException("User is not part of the project.");
        }
        
        var dalEntity = Mapper.Map(entity);
        ServiceRepository.Add(dalEntity!, userId);
    }

    public override void Remove(Comment entity, Guid userId = default)
    {
        Remove(entity.Id, userId);
    }

    public override void Remove(Guid id, Guid userId = default)
    {
        
        var entity = ServiceRepository.Find(id, userId);
        if (entity != null)
        {
            if (ServiceRepository.CommentHasReplies(id).Result)
            {
                entity.Content = "Deleted comment.";
                ServiceRepository.Update(entity);
                return;
            }
            ServiceRepository.Remove(entity, userId);
        }
    }

    public override async Task RemoveAsync(Guid id, Guid userId = default)
    {
        var entity = await ServiceRepository.FindAsync(id, userId);
        if (entity != null)
        {
            if (await ServiceRepository.CommentHasReplies(id))
            {
                entity.Content = "Deleted comment.";
                await ServiceRepository.UpdateAsync(entity);
                return;
            }
            await ServiceRepository.RemoveAsync(id, userId);
        }
    }
}