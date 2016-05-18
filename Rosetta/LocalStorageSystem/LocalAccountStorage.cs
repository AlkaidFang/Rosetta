using System;
using System.Collections.Generic;
using System.Text;
using Alkaid;

namespace Rosetta
{
    public class LocalAccountStorage : Singleton<LocalAccountStorage>, ILocalStorage
    {
        public string name = string.Empty;
        public string uid = string.Empty;
        public string AccountHistory = string.Empty;

        public string Name()
        {
            return "LocalAccountStorage";
        }

        public void Save(LocalStorageSystem manager)
        {
            manager.PutString(name);
            manager.PutString(uid);
            manager.PutString(AccountHistory);
        }

        public void Load(LocalStorageSystem manager)
        {
            name = manager.GetString();
            uid = manager.GetString();
            AccountHistory = manager.GetString();
        }
    }
}
