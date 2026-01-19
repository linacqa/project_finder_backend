using Base.BLL;
using Base.Contracts;
using BLL.Contracts;
using BLL.DTO;
using DAL.Contracts;

namespace BLL.Services;

public class TagService : BaseService<BLL.DTO.Tag, DAL.DTO.Tag, DAL.Contracts.ITagRepository>, ITagService
{
    public TagService(
        IAppUOW serviceUOW, 
        IMapper<Tag, DAL.DTO.Tag, Guid> mapper) : base(serviceUOW, serviceUOW.TagRepository, mapper)
    {
    }
}