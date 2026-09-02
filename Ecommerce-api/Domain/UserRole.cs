using System.Text.Json.Serialization;

namespace Ecommerce_api.Domain;

// Convert the incoming String into Enum
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UserRole
{
    Admin,
    Customer
}