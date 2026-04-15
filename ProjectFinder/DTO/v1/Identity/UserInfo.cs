using Base.Contracts;

namespace DTO.v1.Identity;

public class UserInfo : IDomainId
{
    public Guid Id { get; set; }
    public string Email { get; set; } = default!;
    public string? PhoneNumber { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;

    public string Role { get; set; } = default!;
    // public string? UniId { get; set; }
}