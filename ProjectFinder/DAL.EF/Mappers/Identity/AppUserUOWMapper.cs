namespace DAL.EF.Mappers.Identity;

public class AppUserUOWMapper
{
    public DAL.DTO.Identity.AppUser? Map(Domain.Identity.AppUser? entity)
    {
        if (entity == null) return null;

        return new DAL.DTO.Identity.AppUser()
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email,
            AzureObjectId = entity.AzureObjectId,
            AuthType = entity.AuthType,
        };
    }

    public Domain.Identity.AppUser? Map(DAL.DTO.Identity.AppUser? entity)
    {
        if (entity == null) return null;

        return new Domain.Identity.AppUser()
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            AzureObjectId = entity.AzureObjectId,
            AuthType = entity.AuthType,
        };
    }
}