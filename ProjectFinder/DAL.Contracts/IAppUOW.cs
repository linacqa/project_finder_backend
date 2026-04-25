using Base.DAL.Contracts;

namespace DAL.Contracts;

public interface IAppUOW : IBaseUOW
{
    IApplicationRepository ApplicationRepository { get; }
    ICommentRepository CommentRepository { get; }
    IFolderRepository FolderRepository { get; }
    IFolderTagRepository FolderTagRepository { get; }
    IGroupRepository GroupRepository { get; }
    IInvitationRepository InvitationRepository { get; }
    INotificationRepository NotificationRepository { get; }
    IProjectFolderRepository ProjectFolderRepository { get; }
    IProjectRepository ProjectRepository { get; }
    IProjectStatusRepository ProjectStatusRepository { get; }
    IProjectStepRepository ProjectStepRepository { get; }
    IProjectTagRepository ProjectTagRepository { get; }
    IProjectTypeRepository ProjectTypeRepository { get; }
    IStepRepository StepRepository { get; }
    IStepStatusRepository StepStatusRepository { get; }
    ITagRepository TagRepository { get; }
    IUserGroupRepository UserGroupRepository { get; }
    IUserProjectRepository UserProjectRepository { get; }
    IUserProjectRoleRepository UserProjectRoleRepository { get; }
    IUserTagRepository UserTagRepository { get; }
}