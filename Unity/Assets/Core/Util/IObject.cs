using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alkaid
{
    public class IObject
    {
        private static long _instance_id_index_dump_ = 0;
        private long mInstanceId;

        private long _INSTANCEID_()
        {
            return ++_instance_id_index_dump_;
        }

        public IObject()
        {
            mInstanceId = _INSTANCEID_();
        }

        ~IObject()
        {

        }

        public long GetInstanceId()
        {
            return mInstanceId;
        }
    }
}
