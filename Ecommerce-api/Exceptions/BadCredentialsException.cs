namespace Ecommerce_api.Exceptions;

public class BadCredentialsException : Exception
{
    public BadCredentialsException(string message) : base(message)
    {
    }
}