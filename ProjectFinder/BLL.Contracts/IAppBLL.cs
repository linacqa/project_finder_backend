using Base.BLL.Contracts;

namespace BLL.Contracts;

public interface IAppBLL : IBaseBLL
{
    IApplicationService ApplicationService { get; }
    ICommentService CommentService { get; }
    IFolderService FolderService { get; }
    IFolderTagService FolderTagService { get; }
    IGroupProjectService GroupProjectService { get; }
    IGroupService GroupService { get; }
    IInvitationService InvitationService { get; }
    INotificationService NotificationService { get; }
    IProjectFolderService ProjectFolderService { get; }
    IProjectService ProjectService { get; }
    IProjectStatusService ProjectStatusService { get; }
    IProjectStepService ProjectStepService { get; }
    IProjectTagService ProjectTagService { get; }
    IProjectTypeService ProjectTypeService { get; }
    IStepService StepService { get; }
    IStepStatusService StepStatusService { get; }
    ITagService TagService { get; }
    IUserGroupService UserGroupService { get; }
    IUserProjectService UserProjectService { get; }
    IUserTagService UserTagService { get; }
}