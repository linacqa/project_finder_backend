using Base.BLL;
using BLL.Contracts;
using BLL.Mappers;
using BLL.Services;
using DAL.Contracts;

namespace BLL;

public class AppBLL : BaseBLL<IAppUOW>, IAppBLL
{
    public AppBLL(IAppUOW uow) : base(uow)
    {
    }

    private IApplicationService? _applicationService;
    public IApplicationService ApplicationService =>
        _applicationService ??= new ApplicationService(
            BLLUOW, 
            new ApplicationBLLMapper()
        );
    
    private ICommentService? _commentService;
    public ICommentService CommentService =>
        _commentService ??= new CommentService(
            BLLUOW, 
            new CommentBLLMapper()
        );
    
    private IFolderService? _folderService;
    public IFolderService FolderService =>
        _folderService ??= new FolderService(
            BLLUOW, 
            new FolderBLLMapper()
        );
    
    private IFolderTagService? _folderTagService;
    public IFolderTagService FolderTagService =>
        _folderTagService ??= new FolderTagService(
            BLLUOW, 
            new FolderTagBLLMapper()
        );
    
    private IGroupProjectService? _groupProjectService;
    public IGroupProjectService GroupProjectService =>
        _groupProjectService ??= new GroupProjectService(
            BLLUOW, 
            new GroupProjectBLLMapper()
        );
    
    private IGroupService? _groupService;
    public IGroupService GroupService =>
        _groupService ??= new GroupService(
            BLLUOW, 
            new GroupBLLMapper()
        );
    
    private IInvitationService? _invitationService;
    public IInvitationService InvitationService =>
        _invitationService ??= new InvitationService(
            BLLUOW, 
            new InvitationBLLMapper()
        );
    
    private INotificationService? _notificationService;
    public INotificationService NotificationService =>
        _notificationService ??= new NotificationService(
            BLLUOW, 
            new NotificationBLLMapper()
        );
    
    private IProjectFolderService? _projectFolderService;
    public IProjectFolderService ProjectFolderService =>
        _projectFolderService ??= new ProjectFolderService(
            BLLUOW, 
            new ProjectFolderBLLMapper()
        );
    
    private IProjectService? _projectService;
    public IProjectService ProjectService =>
        _projectService ??= new ProjectService(
            BLLUOW, 
            new ProjectBLLMapper()
        );
    
    private IProjectStepService? _projectStepService;
    public IProjectStepService ProjectStepService =>
        _projectStepService ??= new ProjectStepService(
            BLLUOW, 
            new ProjectStepBLLMapper()
        );
    
    private IProjectTagService? _projectTagService;
    public IProjectTagService ProjectTagService =>
        _projectTagService ??= new ProjectTagService(
            BLLUOW, 
            new ProjectTagBLLMapper()
        );
    
    private IStepService? _stepService;
    public IStepService StepService =>
        _stepService ??= new StepService(
            BLLUOW, 
            new StepBLLMapper()
        );
    
    private ITagService? _tagService;
    public ITagService TagService =>
        _tagService ??= new TagService(
            BLLUOW, 
            new TagBLLMapper()
        );
    
    private IUserGroupService? _userGroupService;
    public IUserGroupService UserGroupService =>
        _userGroupService ??= new UserGroupService(
            BLLUOW, 
            new UserGroupBLLMapper()
        );
    
    private IUserProjectService? _userProjectService;
    public IUserProjectService UserProjectService =>
        _userProjectService ??= new UserProjectService(
            BLLUOW, 
            new UserProjectBLLMapper()
        );
    
    private IUserTagService? _userTagService;
    public IUserTagService UserTagService =>
        _userTagService ??= new UserTagService(
            BLLUOW, 
            new UserTagBLLMapper()
        );
}
