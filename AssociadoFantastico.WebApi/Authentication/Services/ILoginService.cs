using AssociadoFantastico.WebApi.ViewModels;

namespace AssociadoFantastico.WebApi.Authentication.Services
{
    public interface ILoginService
    {
        AuthInfoViewModel Login(string email, string matricula);
    }
}
