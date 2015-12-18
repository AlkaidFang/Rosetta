using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace Alkaid
{
    public class TCPConnector : INetConnector, Lifecycle
    {
        private TcpClient mSocket;

        // 缓冲区
        private int mReadBufferOffset;
        private Byte[] mReadBuffer;
        private Byte[] mReadBufferTemp;
        private int mWriteBufferOffset;
        private Byte[] mWriteBuffer;
        // 读临时缓冲
        private int tempReadPacketLength = 0;
        private int tempReadPacketType = 0;
        private System.IO.MemoryStream tempReadPacketData = null;
        private System.Threading.Semaphore tempReadBufferSemaphore;


        public TCPConnector()
        {
            mReadBufferOffset = 0;
            mReadBuffer = new Byte[INetConnector.MAX_SOCKET_BUFFER_SIZE * 2]; // 主读数据区，默认长度，当某一次长度不够时直接自动延长，延长后不再收缩，先不用内存池的技术了
            mReadBufferTemp = new Byte[INetConnector.MAX_SOCKET_BUFFER_SIZE]; // 读缓存区
            mWriteBufferOffset = 0;
            mWriteBuffer = new Byte[INetConnector.MAX_SOCKET_BUFFER_SIZE * 2]; // 主写数据区，默认长度

            tempReadBufferSemaphore = null;
        }

        public bool Init()
        {
            return true;
        }

        public void Tick(float interval)
        {
            DecodeMessage();
        }

        public void Destroy()
        {

        }

        public override bool Connect(string address, int port)
        {
            base.Connect(address, port);
            mSocket = new TcpClient();
            try
            {
                mSocket.Connect(mNetHoster.GetAddress(), mNetHoster.GetPort());
            }
            catch(Exception e)
            {
                mIsConnected = false;
                return mIsConnected;
            }

            mIsConnected = true;
            mSocket.GetStream().BeginRead(mReadBufferTemp, 0, INetConnector.MAX_SOCKET_BUFFER_SIZE, new AsyncCallback(RecieveCallback), this);

            CallbackConnected();

            return mIsConnected;
        }

        private void RecieveCallback(IAsyncResult ar)
        {
            try
            {
                int readLength = mSocket.GetStream().EndRead(ar);
                LoggerSystem.Instance.Info("读取到数据字节数:" + readLength);
                if (readLength > 0)
                {
                    lock(mReadBuffer)
                    {
                        if (mReadBufferOffset + readLength > mReadBuffer.Length)
                        {
                            Byte[] swi = mReadBuffer;
                            mReadBuffer = new Byte[swi.Length * 2];
                            System.Array.Copy(swi, mReadBuffer, mReadBufferOffset);
                        }
                        System.Array.Copy(mReadBufferTemp, 0, mReadBuffer, mReadBufferOffset, readLength);
                    }
                    
                    mReadBufferOffset += readLength;

                    mSocket.GetStream().BeginRead(mReadBufferTemp, 0, INetConnector.MAX_SOCKET_BUFFER_SIZE, new AsyncCallback(RecieveCallback), this);
                }
                else
                {
                    // error
                    Disconnect();
                }
            }
            catch (Exception e)
            {
                LoggerSystem.Instance.Info("发生读取错误:" + e.Message);
                Disconnect();
            }
            
        }

        private void DecodeMessage()
        {
            lock(mReadBuffer)
            {

            }

        }

        private void SendMessage()
        {



        }


        private void Disconnect()
        {
            mSocket.GetStream().Close();
            mSocket.Close();
            mIsConnected = false;

            CallbackDisconnected();
        }
    }
}
