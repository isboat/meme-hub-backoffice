using Microsoft.Azure.SignalR.Management;

namespace Partners.Management.Web.Services
{
    public interface ISignalrService
    {
        Task<ServiceHubContext> GetHubContextAsync();

        Task SendMessage(ChangeMessage changeMessage);
    }
}
