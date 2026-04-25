using Base.DAL.EF;
using DAL.Contracts;
using DAL.EF.Repositories;

namespace DAL.EF;

public class AppUOW : BaseUOW<AppDbContext>, IAppUOW
{
    public AppUOW(AppDbContext uowDbContext) : base(uowDbContext)
    {
    }

    private IApplicationRepository? _applicationRepository;
    public IApplicationRepository ApplicationRepository =>
        _applicationRepository ??= new ApplicationRepository(UOWDbContext);
    
    private ICommentRepository? _commentRepository;
    public ICommentRepository CommentRepository =>
        _commentRepository ??= new CommentRepository(UOWDbContext);
    
    private IFolderRepository? _folderRepository;
    public IFolderRepository FolderRepository =>
        _folderRepository ??= new FolderRepository(UOWDbContext);
    
    private IFolderTagRepository? _folderTagRepository;
    public IFolderTagRepository FolderTagRepository =>
        _folderTagRepository ??= new FolderTagRepository(UOWDbContext);
    
    private IGroupRepository? _groupRepository;
    public IGroupRepository GroupRepository =>
        _groupRepository ??= new GroupRepository(UOWDbContext);
    
    private IInvitationRepository? _invitationRepository;
    public IInvitationRepository InvitationRepository =>
        _invitationRepository ??= new InvitationRepository(UOWDbContext);
    
    private INotificationRepository? _notificationRepository;
    public INotificationRepository NotificationRepository =>
        _notificationRepository ??= new NotificationRepository(UOWDbContext);
    
    private IProjectFolderRepository? _projectFolderRepository;
    public IProjectFolderRepository ProjectFolderRepository =>
        _projectFolderRepository ??= new ProjectFolderRepository(UOWDbContext);
    
    private IProjectRepository? _projectRepository;
    public IProjectRepository ProjectRepository =>
        _projectRepository ??= new ProjectRepository(UOWDbContext);
    
    private IProjectStatusRepository? _projectStatusRepository;
    public IProjectStatusRepository ProjectStatusRepository =>
        _projectStatusRepository ??= new ProjectStatusRepository(UOWDbContext);
    
    private IProjectStepRepository? _projectStepRepository;
    public IProjectStepRepository ProjectStepRepository =>
        _projectStepRepository ??= new ProjectStepRepository(UOWDbContext);
    
    private IProjectTagRepository? _projectTagRepository;
    public IProjectTagRepository ProjectTagRepository =>
        _projectTagRepository ??= new ProjectTagRepository(UOWDbContext);
    
    private IProjectTypeRepository? _projectTypeRepository;
    public IProjectTypeRepository ProjectTypeRepository =>
        _projectTypeRepository ??= new ProjectTypeRepository(UOWDbContext);
    
    private IStepRepository? _stepRepository;
    public IStepRepository StepRepository =>
        _stepRepository ??= new StepRepository(UOWDbContext);
    
    private IStepStatusRepository? _stepStatusRepository;
    public IStepStatusRepository StepStatusRepository =>
        _stepStatusRepository ??= new StepStatusRepository(UOWDbContext);
    
    private ITagRepository? _tagRepository;
    public ITagRepository TagRepository =>
        _tagRepository ??= new TagRepository(UOWDbContext);
    
    private IUserGroupRepository? _userGroupRepository;
    public IUserGroupRepository UserGroupRepository =>
        _userGroupRepository ??= new UserGroupRepository(UOWDbContext);
    
    private IUserProjectRepository? _userProjectRepository;
    public IUserProjectRepository UserProjectRepository =>
        _userProjectRepository ??= new UserProjectRepository(UOWDbContext);
    
    private IUserProjectRoleRepository? _userProjectRoleRepository;
    public IUserProjectRoleRepository UserProjectRoleRepository =>
        _userProjectRoleRepository ??= new UserProjectRoleRepository(UOWDbContext);
    
    private IUserTagRepository? _userTagRepository;
    public IUserTagRepository UserTagRepository =>
        _userTagRepository ??= new UserTagRepository(UOWDbContext);
}
