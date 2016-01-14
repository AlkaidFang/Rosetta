using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alkaid
{
    public class NetStream
    {
        private TBuffer<Byte> mReadBuffer;
        private Byte[] mReadBufferTemp;
        private TBuffer<Byte> mWriteBuffer;

        private volatile bool mPipeInIdle;
        private volatile bool mPipeOutIdle;

        public NetStream(int bufferSize)
        {
            mReadBuffer = new TBuffer<Byte>(bufferSize); // 主读数据区
            mReadBufferTemp = new Byte[bufferSize / 2]; // 读缓存区
            mWriteBuffer = new TBuffer<Byte>(bufferSize); // 主写数据区

            mPipeInIdle = true;
            mPipeOutIdle = true;
        }

        public Byte[] AsyncPipeIn
        {
            get
            {
                mPipeInIdle = false;
                return mReadBufferTemp;
            }
        }

        public bool AsyncPipeInIdle
        {
            get
            {
                return mPipeInIdle;
            }
        }

        public void FinishedIn(int length)
        {
            mReadBuffer.Push(mReadBufferTemp, length);
            mPipeInIdle = true;
        }

        public Byte[] AsyncPipeOut
        {
            get
            {
                mPipeOutIdle = false;
                return mWriteBuffer.Buffer();
            }
        }

        public bool AsyncPipeOutIdle
        {
            get
            {
                return mPipeOutIdle;
            }
        }

        public void FinishedOut(int length)
        {
            mWriteBuffer.Pop(length);
            mPipeOutIdle = false;
        }

        public Byte[] InStream
        {
            get
            {
                return mReadBuffer.Buffer();
            }
        }

        public int InStreamLength
        {
            get
            {
                return mReadBuffer.DataSize();
            }
        }

        public Byte[] OutStream
        {
            get
            {
                return mWriteBuffer.Buffer();
            }
        }

        public int OutStreamLength
        {
            get
            {
                return mWriteBuffer.DataSize();
            }
        }

        public void PushInStream(byte[] buffer)
        {
            mReadBuffer.Push(buffer);
        }

        public void PopInStream(int length)
        {
            mReadBuffer.Pop(length);
        }

        public void PushOutStream(byte[] buffer)
        {
            mWriteBuffer.Push(buffer);
        }

        public void PopOutStream(int length)
        {
            mWriteBuffer.Pop(length);
        }

        public void Clear()
        {
            mReadBuffer.Clear();
            mWriteBuffer.Clear();
        }
    }
}
