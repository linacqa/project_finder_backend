using Base.DAL.Contracts;

namespace DAL.Contracts;

public interface IAppUOW : IBaseUOW
{
    IApplicationRepository ApplicationRepository { get; }
    ICommentRepository CommentRepository { get; }
    IFolderRepository FolderRepository { get; }
    IFolderTagRepository FolderTagRepository { get; }
    IGroupProjectRepository GroupProjectRepository { get; }
    IGroupRepository GroupRepository { get; }
    IInvitationRepository InvitationRepository { get; }
    INotificationRepository NotificationRepository { get; }
    IProjectFolderRepository ProjectFolderRepository { get; }
    IProjectRepository ProjectRepository { get; }
    IProjectStepRepository ProjectStepRepository { get; }
    IProjectTagRepository ProjectTagRepository { get; }
    IStepRepository StepRepository { get; }
    ITagRepository TagRepository { get; }
    IUserGroupRepository UserGroupRepository { get; }
    IUserProjectRepository UserProjectRepository { get; }
    IUserTagRepository UserTagRepository { get; }
}