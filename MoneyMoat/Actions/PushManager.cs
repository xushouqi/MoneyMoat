using System;
using System.Collections.Concurrent;
using CommonLibs;
using StockModels.ViewModels;

namespace CommonNetwork
{
    public class PushManager : IPushManager
    {
        protected ConcurrentDictionary<int, Action<WebPackage>> m_sockePusherById = null;
        protected ConcurrentDictionary<Type, int> m_actionIdByType = null;

        private readonly PackageManager m_packageManager;
        public PushManager(PackageManager packManager)
        {
            m_sockePusherById = new ConcurrentDictionary<int, Action<WebPackage>>();
            m_actionIdByType = new ConcurrentDictionary<Type, int>();
            m_packageManager = packManager;

            RegActionIds();
        }

        protected virtual void RegActionIds()
        {

        }

        public virtual void AddPushAction(UserData userData, Action<WebPackage> callback)
        {
            m_sockePusherById.TryAdd(userData.ID, callback);
        }
        public virtual void RemovePushAction(UserData userData)
        {
            Action<WebPackage> data = null;
            m_sockePusherById.TryRemove(userData.ID, out data);
        }

        /// <summary>
        /// 自动推送更新数据对象到客户端
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="data"></param>
        public void PushDataById<T>(int id, T data)
        {
            Type atype = typeof(T);
            if (m_sockePusherById.ContainsKey(id) && m_actionIdByType.ContainsKey(atype))
            {
                int actionId = m_actionIdByType[atype];
                var ret = ProtoBufUtils.Serialize(data);
                var package = m_packageManager.CreateActPackage(actionId, 0, 0, ret, ErrorCodeEnum.Success);

                m_sockePusherById[id](package);
            }
        }
    }
}
