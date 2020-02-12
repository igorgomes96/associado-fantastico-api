using Microsoft.AspNetCore.SignalR;

namespace AssociadoFantastico.WebApi.SignalR
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.Identity?.Name;
        }
    }
}
