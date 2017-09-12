using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using System.Net.WebSockets;
using CommonLibs;

namespace CommonNetwork
{
    public class UserManager<T> : IUserManager<T> where T:UserData
    {
        ConcurrentDictionary<int, int> m_useridBySocket = new ConcurrentDictionary<int, int>();
        ConcurrentDictionary<int, T> m_usersById = new ConcurrentDictionary<int, T>();

        object m_lock = new object();

        public virtual T UpdateUser(WebSocket socket, int userId, UserTypeEnum userType, string token, double expiresIn)
        {
            T data = default(T);
            if (socket != null && userId > 0)
            {
                int handle = socket.GetHashCode();
                m_useridBySocket.AddOrUpdate(handle, userId, (key, oldValue)=> userId);
                
                lock (m_lock)
                {
                    if (m_usersById.ContainsKey(userId))
                    {
                        data = m_usersById[userId];
                        data.SocketHandle = handle;
                        data.Token = token;
                        data.ExpireTime = DateTime.Now.AddSeconds(expiresIn);
                    }
                    else
                    {
                        data = (T)Activator.CreateInstance(typeof(T));
                        data.ID = userId;
                        data.Type = userType;
                        data.SocketHandle = handle;
                        data.Token = token;
                        data.ExpireTime = DateTime.Now.AddSeconds(expiresIn);
                        m_usersById[userId] = data;
                    }
                }
            }
            return data;
        }

        public virtual int RemoveUser(WebSocket socket)
        {
            int id = 0;
            lock (m_lock)
            {
                int handle = socket.GetHashCode();
                if (m_useridBySocket.ContainsKey(handle))
                {
                    id = m_useridBySocket[handle];

                    T data = null;
                    if (m_usersById.ContainsKey(id))
                        m_usersById.TryRemove(id, out data);

                    int tid = 0;
                    m_useridBySocket.TryRemove(handle, out tid);
                }
            }
            return id;
        }
        public virtual bool RemoveUser(int id)
        {
            bool ret = false;
            lock (m_lock)
            {
                if (m_usersById.ContainsKey(id))
                {
                    ret = true;
                    var data = m_usersById[id];

                    int tid = 0;
                    if (m_useridBySocket.ContainsKey(data.SocketHandle))
                        m_useridBySocket.TryRemove(data.SocketHandle, out tid);

                    T tdata = null;
                    m_usersById.TryRemove(id, out tdata);
                }
            }
            return ret;
        }

        public bool ValidSocket(WebSocket socket)
        {
            int handle = socket.GetHashCode();
            return m_useridBySocket.ContainsKey(handle);
        }

        public virtual T GetUserData(WebSocket socket)
        {
            T user = null;
            lock(m_lock)
            {
                int id = 0;
                int handle = socket.GetHashCode();
                m_useridBySocket.TryGetValue(handle, out id);
                if (id > 0)
                    m_usersById.TryGetValue(id, out user);
            }
            return user;
        }
        public virtual T GetUserData(int id)
        {
            T user = null;
            lock (m_lock)
            {
                m_usersById.TryGetValue(id, out user);
            }
            return user;
        }
    }
}
