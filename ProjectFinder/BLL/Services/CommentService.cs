using Base.BLL;
using Base.Contracts;
using BLL.Contracts;
using BLL.DTO;
using DAL.Contracts;

namespace BLL.Services;

public class CommentService : BaseService<BLL.DTO.Comment, DAL.DTO.Comment, DAL.Contracts.ICommentRepository>, ICommentService
{
    public CommentService(
        IAppUOW serviceUOW, 
        IMapper<Comment, DAL.DTO.Comment, Guid> mapper) : base(serviceUOW, serviceUOW.CommentRepository, mapper)
    {
    }
}