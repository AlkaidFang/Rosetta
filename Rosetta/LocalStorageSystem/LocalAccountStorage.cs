using System;
using System.Collections.Generic;
using System.Text;
using Alkaid;

namespace Rosetta
{
    public class LocalAccountStorage : Singleton<LocalAccountStorage>, ILocalStorage
    {
        public string name = "";
        public string uid = "";
        public string AccountHistory = "";

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
