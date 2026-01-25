namespace DAL.EF.DataSeeding;

public static class InitialData
{
    public static readonly (string roleName, Guid? id)[]
        Roles =
        [
            ("admin", null),
            ("user", Guid.Parse("00000000-0000-0000-0000-000000000002")),
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
    
    public static readonly string[] Steps =
    [
        "Not Started",
        "In Progress",
        "Completed",
    ];
}
