using System;
using System.Net.WebSockets;
using CommonLibs;

namespace CommonNetwork
{
    public interface IUserManager<T>
    {
        T UpdateUser(WebSocket socket, int accountId, string token, double expiresIn);
        int RemoveUser(WebSocket socket);
        bool RemoveUser(int id);
        bool ValidSocket(WebSocket socket);
        T GetUserData(WebSocket socket);
        T GetUserData(int id);
    }
}
