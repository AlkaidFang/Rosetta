using System;
using System.Collections.Generic;
using System.Text;
using Alkaid;

namespace Rosetta
{
    public class IAPPayment
    {
        public enum PLATFORM : int
        {
            UNKNOW = 0,
            ANDROID,
            IOS,
            WP8,
        }

        public int mPlatform;           // 平台类型
        public string mServerId;        // 服务器
        public string mPlayerId;        // 玩家
        public string mProductId;       // 商品
        public int mGoodId;             // 商品 , for cyou payment
        public string mOrderId;         // 订单号
        public string mOrderReceipt;       // 订单回执
        public int mVerifyFaildedTimes; // 订单校验失败次数

        public IAPPayment()
        {
            mPlatform = (int)PLATFORM.UNKNOW;
            mServerId = "";
            mPlayerId = "";
            mProductId = "";
            mGoodId = 0;
            mOrderId = "";
            mOrderReceipt = "";
            mVerifyFaildedTimes = 0;
        }
    }

    public class LocalUnVerifyIAPStorage : Singleton<LocalUnVerifyIAPStorage>, ILocalStorage
    {
        private IDictionary<string, IAPPayment> m_lUnVerifyList = new Dictionary<string, IAPPayment>();

        public LocalUnVerifyIAPStorage()
        {
            m_lUnVerifyList.Clear();
        }

        public string Name()
        {
            return "LocalUnVerifyIAPStorage";
        }

        public void Save(LocalStorageSystem manager)
        {
            manager.PutInt(m_lUnVerifyList.Count);

            IAPPayment temp;
            foreach (string orderId in m_lUnVerifyList.Keys)
            {
                temp = m_lUnVerifyList[orderId];
                manager.PutInt(temp.mPlatform);
                manager.PutString(temp.mServerId);
                manager.PutString(temp.mPlayerId);
                manager.PutString(temp.mProductId);
                manager.PutInt(temp.mGoodId);
                manager.PutString(temp.mOrderId);
                manager.PutString(temp.mOrderReceipt);
                manager.PutInt(temp.mVerifyFaildedTimes);
            }
        }

        public void Load(LocalStorageSystem manager)
        {
            int count = 0;
            manager.GetInt();

            IAPPayment temp;
            for(int i = 0; i < count; ++i)
            {
                temp = new IAPPayment();
                temp.mPlatform = manager.GetInt();
                temp.mServerId = manager.GetString();
                temp.mPlayerId = manager.GetString();
                temp.mProductId = manager.GetString();
                temp.mGoodId = manager.GetInt();
                temp.mOrderId = manager.GetString();
                temp.mOrderReceipt = manager.GetString();
                temp.mVerifyFaildedTimes = manager.GetInt();
                m_lUnVerifyList.Add(temp.mOrderId, temp);
            }
        }

        public void InsertPayment(string serverId, string playerId, string productId, int goodId, string orderId, string orderReceipt)
        {
            // note: 需要注意的是在插入完支付信息之后请调用存储数据（LocalPlayerDataManager.NeedSaveToDisk()）
            // 一定要存盘

            IAPPayment payment = new IAPPayment();
            payment.mPlatform = (int)IAPPayment.PLATFORM.UNKNOW;  //需调用系统方法返回各种不同的平台
            payment.mServerId = serverId;
            payment.mPlayerId = playerId;
            payment.mProductId = productId;
            payment.mGoodId = goodId;
            payment.mOrderId = orderId;
            payment.mOrderReceipt = orderReceipt;

            m_lUnVerifyList.Add(payment.mOrderId, payment);
        }

        public void DeletePayment(string orderId)
        {
            // note： 需要注意，在删除完订单信息之后，需要调用存盘
            if (!m_lUnVerifyList.ContainsKey(orderId))
            {
                return;
            }

            m_lUnVerifyList.Remove(orderId);
        }

        public IDictionary<string, IAPPayment> GetPaymentList()
        {
            return this.m_lUnVerifyList;
        }

    }
}
