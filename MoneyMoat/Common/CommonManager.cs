using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyMoat
{
    public class CommonManager
    {
        public CommonManager()
        {
        }


        private object m_lock_me = new object();
        private Dictionary<string, Dictionary<string, object>> m_locks = new Dictionary<string, Dictionary<string, object>>();

        public ConcurrentDictionary<int, AutoResetEvent> RequestHnadles = new ConcurrentDictionary<int, AutoResetEvent>();

        public object GetLockerById<T>(int id)
        {
            return GetLockerById<T>(id.ToString());
        }
        public object GetLockerById<T>(string id)
        {
            object locker = null;
            lock (m_lock_me)
            {
                string typeName = typeof(T).Name;
                if (!m_locks.ContainsKey(typeName))
                    m_locks[typeName] = new Dictionary<string, object>();
                if (!m_locks[typeName].ContainsKey(id))
                    m_locks[typeName][id] = new object();
                locker = m_locks[typeName][id];
            }
            return locker;
        }

    }
}
