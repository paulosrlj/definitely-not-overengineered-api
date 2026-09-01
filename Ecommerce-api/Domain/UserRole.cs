using System.Text.Json.Serialization;

namespace Ecommerce_api.Domain;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UserRole
{
    Admin,
    Customer
}