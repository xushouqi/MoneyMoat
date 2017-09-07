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

        public PushManager()
        {
            m_sockePusherById = new ConcurrentDictionary<int, Action<WebPackage>>();
            m_actionIdByType = new ConcurrentDictionary<Type, int>();

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

        private int m_id = 90000000;
        private WebPackage CreatePackage(int actionId, byte[] param, int accountId = 0)
        {
            var package = new WebPackage
            {
                ActionId = actionId,
                Uid = accountId,
                ID = System.Threading.Interlocked.Increment(ref m_id),
                Params = param,
            };
            return package;
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
                var package = CreatePackage(actionId, null);
                package.ErrorCode = (int)ErrorCodeEnum.Success;
                package.Return = ProtoBufUtils.Serialize(data);

                m_sockePusherById[id](package);
            }
        }
    }
}
