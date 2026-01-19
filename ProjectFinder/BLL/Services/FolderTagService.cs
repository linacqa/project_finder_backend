using Base.BLL;
using Base.Contracts;
using BLL.Contracts;
using BLL.DTO;
using DAL.Contracts;

namespace BLL.Services;

public class FolderTagService : BaseService<BLL.DTO.FolderTag, DAL.DTO.FolderTag, DAL.Contracts.IFolderTagRepository>, IFolderTagService
{
    public FolderTagService(
        IAppUOW serviceUOW, 
        IMapper<FolderTag, DAL.DTO.FolderTag, Guid> mapper) : base(serviceUOW, serviceUOW.FolderTagRepository, mapper)
    {
    }
}