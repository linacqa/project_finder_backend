namespace DAL.EF.DataSeeding;

public static class InitialData
{
    public static readonly (string roleName, Guid? id)[]
        Roles =
        [
            ("admin", null),
            ("user", null),
            ("student", null),
            ("teacher", null),
        ];

    // TODO: Change before deploying to production
    public static readonly (string name, string firstName, string lastName, string password, Guid? id, string[] roles)[]
        Users =
        [
            ("admin@taltech.ee", "admin", "taltech", "Foo.Bar.1", null, ["admin"]),
            ("user@taltech.ee", "user", "taltech", "Foo.Bar.2", null, ["user"]),
            ("student@taltech.ee", "student", "taltech", "Foo.Bar.3", null, ["student"]),
            ("teacher@taltech.ee", "teacher", "taltech", "Foo.Bar.4", null, ["teacher"]),
        ];
    
    public static readonly (string name, bool isVisible)[] Folders =
    [
        ("2026/2027", true),
        ("hidden-folder", false),
    ];
    
    public static readonly (string name, Guid? id)[] StepStatuses =
    [
        ("Not Started", Guid.Parse("00000000-0000-0000-0000-000000000001")),
        ("In Progress", Guid.Parse("00000000-0000-0000-0000-000000000002")),
        ("Completed", Guid.Parse("00000000-0000-0000-0000-000000000003")),
    ];

    public static readonly (string name, Guid? id)[] ProjectTypes =
    [
        ("Lõputöö", Guid.Parse("00000000-0000-0000-0000-000000000001")),
        ("Praktika projekt", Guid.Parse("00000000-0000-0000-0000-000000000002")),
        ("Praktika projekt + Lõputöö", Guid.Parse("00000000-0000-0000-0000-000000000003")),
    ];

    public static readonly (string name, Guid? id)[] ProjectStatuses =
    [
        ("Draft", Guid.Parse("00000000-0000-0000-0000-000000000001")),
        ("Open", Guid.Parse("00000000-0000-0000-0000-000000000002")),
        ("Closed", Guid.Parse("00000000-0000-0000-0000-000000000003")),
        ("Archived", Guid.Parse("00000000-0000-0000-0000-000000000004")),
        ("Completed", Guid.Parse("00000000-0000-0000-0000-000000000005")),
    ];

    public static readonly (string name, Guid? id)[] UserProjectRoles = [
        ("Author", Guid.Parse("00000000-0000-0000-0000-000000000001")),
        ("Supervisor", Guid.Parse("00000000-0000-0000-0000-000000000002")),
        ("External Supervisor", Guid.Parse("00000000-0000-0000-0000-000000000003")),
        ("Executor", Guid.Parse("00000000-0000-0000-0000-000000000004")),
    ];
}
