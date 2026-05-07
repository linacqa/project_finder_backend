namespace Tests.Helpers;

public static class TestUsers
{
    public const string Password = "Test.Pass.1";

    public static readonly Guid UserAId = new("00000000-0000-0000-0000-000000000001");
    public const string UserAEmail = "user-a@test.ee";

    public static readonly Guid UserBId = new("00000000-0000-0000-0000-000000000002");
    public const string UserBEmail = "user-b@test.ee";
    
    public static readonly Guid AdminUserId = new("00000000-0000-0000-0000-000000000003");
    public const string AdminUserEmail = "admin@test.ee";
    
    public static readonly Guid StudentAId = new("00000000-0000-0000-0000-000000000004");
    public const string StudentAEmail = "student-a@test.ee";

    public static readonly Guid ProjectTypeAId = new("00000000-0000-0000-0000-000000000001");
    public const string ProjectTypeAName = "ProjectTypeA";
    
    public static readonly Guid ProjectStatusAId = new("00000000-0000-0000-0000-000000000001");
    public const string ProjectStatusAName = "ProjectStatusA";
    
    public static readonly Guid AuthorRoleId = new("00000000-0000-0000-0000-000000000001");
    public const string AuthorRoleName = "Author";
    
    public static readonly Guid SupervisorRoleId = new("00000000-0000-0000-0000-000000000002");
    public const string SupervisorRoleName = "Supervisor";
    
    public static readonly Guid ExternalSupervisorRoleId = new("00000000-0000-0000-0000-000000000003");
    public const string ExternalSupervisorRoleName = "External Supervisor";
    
    public static readonly Guid ExecutorRoleId = new("00000000-0000-0000-0000-000000000004");
    public const string ExecutorRoleName = "Executor";
}