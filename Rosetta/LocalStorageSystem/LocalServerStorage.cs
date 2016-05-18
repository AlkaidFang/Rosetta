using System;
using System.Collections.Generic;
using System.Text;
using Alkaid;

namespace Rosetta
{
    public class LocalServerStorage : Singleton<LocalServerStorage>, ILocalStorage
    {
        public int m_iServerId = 0;                 // 上次登录服务器ID
        public string m_sServerIP = string.Empty;             // 上次登录服务器IP
        public int m_iServerPort = 0;               // 上次登录服务器端口

        public string Name()
        {
            return "LocalServerStorage";
        }

        public void Save(LocalStorageSystem manager)
        {
            manager.PutInt(m_iServerId);
            manager.PutString(m_sServerIP);
            manager.PutInt(m_iServerPort);
        }

        public void Load(LocalStorageSystem manager)
        {
            m_iServerId = manager.GetInt();
            m_sServerIP = manager.GetString();
            m_iServerPort = manager.GetInt();
        }
    }
}
