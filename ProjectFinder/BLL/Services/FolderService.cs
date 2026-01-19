using Base.BLL;
using Base.Contracts;
using BLL.Contracts;
using BLL.DTO;
using DAL.Contracts;

namespace BLL.Services;

public class FolderService : BaseService<BLL.DTO.Folder, DAL.DTO.Folder, DAL.Contracts.IFolderRepository>, IFolderService
{
    public FolderService(
        IAppUOW serviceUOW, 
        IMapper<Folder, DAL.DTO.Folder, Guid> mapper) : base(serviceUOW, serviceUOW.FolderRepository, mapper)
    {
    }
}