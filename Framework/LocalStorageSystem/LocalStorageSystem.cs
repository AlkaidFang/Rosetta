/*
 * Name: LocalPlayerDataManager
 * Function: 提供游戏本地化存储游戏数据功能
 * Author: FangJun
 * Date: 2014-8-19
 * Framework: 
 *          version+data
 */

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Alkaid
{
    public sealed class LocalStorageSystem : Singleton<LocalStorageSystem>, Lifecycle
    {
        // attributes:
        private bool m_bNeedSaveDisk; // 存盘标识
        private string m_sAppVersion;       // 当前游戏版本
        private string m_sStorageVersion;   // 本地存储数据的版本，此数据用于更新版本时本地存储数据格式可能发生变化所用
        public IDictionary<string, ILocalStorage> m_lStorageList = new Dictionary<string, ILocalStorage>(); //所有注册存储的对象

        private int m_iTempIndex;   // 存储临时数据
        private string m_sTempName; // 存储临时数据

        // funcitons：
        public LocalStorageSystem()
        {
            m_sAppVersion = string.Empty;

            m_iTempIndex = 0;
            m_sTempName = string.Empty;
        }

        public bool Init()
        {
            LoggerSystem.Instance.Info("LocalPlayerDataManager    init   begin");
            // 注册存储服务信息, 在setup中进行了注册

            // 加载存储信息
            LoadStorage();

            LoggerSystem.Instance.Info("LocalPlayerDataManager    init   end");
            return true;
        }

        public void Tick(float interval)
        {
            if (!m_bNeedSaveDisk)
                return;

            SaveStorage();

            m_bNeedSaveDisk = false;
        }

        public void Destroy()
        {
            LoggerSystem.Instance.Info("LocalPlayerDataManager    destroy   begin");
            SaveStorage();
            m_lStorageList.Clear();
            LoggerSystem.Instance.Info("LocalPlayerDataManager    destroy   end");
        }

        public void SetAppVersion(string version)
        {
            m_sAppVersion = version;
        }

        public void RegisterLocalStorage(ILocalStorage storage)
        {
            m_lStorageList.Add(storage.Name(), storage);
        }

        private bool LoadStorage()
        {
            do
            {
                m_sStorageVersion = PlayerPrefs.GetString("_LocalStorageVersion_");
                if (!VarifyVersion(m_sAppVersion))
                {
                    LoggerSystem.Instance.Info("客户端版本不匹配，将删除本地存储数据");
                    DeleteStorage();
                    break;
                }

                foreach (string namekey in m_lStorageList.Keys)
                {
                    m_sTempName = m_lStorageList[namekey].Name();
                    m_iTempIndex = 0;

                    m_lStorageList[namekey].Load(this);
                }

                return true;
            } while (false);

            // 此处为什么要返回true：本地信息存储在没有版本号时肯定是不一致的，所以只是不用读取而已
            return true;
        }

        private bool SaveStorage()
        {
            LoggerSystem.Instance.Info("写入信息开始");
            // 数据版本信息
            PlayerPrefs.SetString("_LocalStorageVersion_", m_sAppVersion);

            // 用户信息
            foreach (string namekey in m_lStorageList.Keys)
            {
                m_sTempName = m_lStorageList[namekey].Name();
                m_iTempIndex = 0;

                m_lStorageList[namekey].Save(this);
            }

            PlayerPrefs.Save();

            LoggerSystem.Instance.Info("写入信息结束");
            return true;
        }

        private void DeleteStorage()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }

        public void NeedSaveToDisk()
        {
            m_bNeedSaveDisk = true;
        }

        public bool Varify()
        {
            return true;
        }

        public bool VarifyVersion(string appVersion)
        {
            // 只有同版本的数据可以被校验通过，不同版本数据需要被清空
            LoggerSystem.Instance.Info("本地数据版本：" + m_sStorageVersion + "  游戏版本：" + appVersion);

            if (m_sStorageVersion.Equals(appVersion))
            {
                return true;
            }

            return false;
        }

        public char GetChar()
        {
            return (char)PlayerPrefs.GetInt(m_sTempName + ++m_iTempIndex);
        }

        public short GetShort()
        {
            return (short)PlayerPrefs.GetInt(m_sTempName + ++m_iTempIndex);
        }

        public int GetInt()
        {
            return PlayerPrefs.GetInt(m_sTempName + ++m_iTempIndex);
        }

        public float GetFloat()
        {
            return PlayerPrefs.GetFloat(m_sTempName + ++m_iTempIndex);
        }

        public long GetLong()
        {
            // note: use for 8byte long size
            if (sizeof(long) == 2 * sizeof(int))
            {
                long temp1 = PlayerPrefs.GetInt(m_sTempName + ++m_iTempIndex);
                long temp2 = PlayerPrefs.GetInt(m_sTempName + ++m_iTempIndex);
                return ((temp1 << (8 * sizeof(int))) | temp2);
            }
            else
            {
                return PlayerPrefs.GetInt(m_sTempName + ++m_iTempIndex);
            }
        }

        public string GetString()
        {
            return PlayerPrefs.GetString(m_sTempName + ++m_iTempIndex);
        }

        public void PutChar(char data)
        {
            PlayerPrefs.SetInt(m_sTempName + ++m_iTempIndex, data);
        }

        public void PutShort(short data)
        {
            PlayerPrefs.SetInt(m_sTempName + ++m_iTempIndex, data);
        }

        public void PutInt(int data)
        {
            PlayerPrefs.SetInt(m_sTempName + ++m_iTempIndex, data);
        }

        public void PutFloat(float data)
        {
            PlayerPrefs.SetFloat(m_sTempName + ++m_iTempIndex, data);
        }

        public void PutLong(long data)
        {
            if (sizeof(long) == 2 * sizeof(int))
            {
                PlayerPrefs.SetInt(m_sTempName + ++m_iTempIndex, (int)(data >> (8 * sizeof(int))));
                PlayerPrefs.SetInt(m_sTempName + ++m_iTempIndex, (int)(data));
            }
            else
            {
                PlayerPrefs.SetInt(m_sTempName + ++m_iTempIndex, (int)(data));
            }
        }

        public void PutString(string data)
        {
            PlayerPrefs.SetString(m_sTempName + ++m_iTempIndex, data);
        }


    }
}
