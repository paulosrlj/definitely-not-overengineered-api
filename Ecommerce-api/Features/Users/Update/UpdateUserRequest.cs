using System.ComponentModel.DataAnnotations;
using Ecommerce_api.Domain;

namespace Ecommerce_api.Features.Users.Update;

public record UpdateUserRequest(
    [property: MinLength(3)]
    [property: MaxLength(50)]
    string? Name = null,

    [property: EmailAddress]
    string? Email = null,

    [property: MinLength(3)]
    [property: MaxLength(50)]
    string? Password = null,

    UserRole? Role = null,

    string? Phone = null
);