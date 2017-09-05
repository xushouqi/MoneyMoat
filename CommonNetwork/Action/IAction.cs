using System.Threading.Tasks;
using System.Net.WebSockets;
using CommonLibs;

namespace CommonNetwork
{
    public interface IAction
    {
        void Submit(WebSocket socket, int accountid, WebPackage package);
        Task DoAction();
        byte[] GetResponseData();
        Task AfterAction();
    }
}
