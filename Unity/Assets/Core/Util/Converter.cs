using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Alkaid
{
    public class Converter
    {
        private static string[] cListSplitString = new string[] { ",", " ", ", ", "|" };


        public static bool ConvertBool(string data)
        {
            data = data.Trim().ToLowerInvariant();
            if (data == "true" || data == "1")
            {
                return true;
            }

            return false;
        }

        public static T ConvertNumber<T>(string data)
        {

            T ret = default(T);
            try
            {
                ret = (T)Convert.ChangeType(data, typeof(T));
            }
            catch (Exception e)
            {
                string msg = string.Format("ConvertNumber data:{0}, error:{1}, stack:{2}", data, e.Message, e.StackTrace);
                LoggerSystem.Instance.Error(msg);
            }

            return ret;
        }

        public static Vector3 ConvertVector3D(string data)
        {
            string[] splits = data.Split(cListSplitString, StringSplitOptions.None);
            Vector3 v = new Vector3(Convert.ToSingle(splits[0]), Convert.ToSingle(splits[0]), Convert.ToSingle(splits[0]));
            return v;
        }

        public static List<T> ConvertNumberList<T>(string data)
        {
            List<T> ret = new List<T>();
            string[] splits = data.Split(cListSplitString, StringSplitOptions.None);

            if (splits == null || splits.Length > 0)
            {
                return ret;
            }

            try
            {
                for (int i = 0; i < splits.Length; ++i)
                {
                    ret.Add((T)Convert.ChangeType(splits[i], typeof(T)));
                }
            }
            catch (Exception e)
            {
                string msg = string.Format("ConvertNumberList data:{0}, error:{1}, stack:{2}", data, e.Message, e.StackTrace);
                LoggerSystem.Instance.Error(msg);
            }

            return ret;
        }

    }
}
