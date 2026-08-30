using System.Text.Json.Serialization;

namespace Ecommerce_api.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UserRole
{
    Admin,
    Customer
}