using Services.Identity.Data;

namespace Services.Identity.Authentication
{
    public interface IAuthenticationService
    {
        string GetToken(User customer);
    }
}