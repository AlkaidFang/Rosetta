using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alkaid
{
    public class StreamBuffer
    {
        private TBuffer<Byte> mReadBuffer;
        private Byte[] mReadBufferTemp;
        private TBuffer<Byte> mWriteBuffer;

        public StreamBuffer(int bufferSize)
        {
            mReadBuffer = new TBuffer<Byte>(bufferSize); // 主读数据区
            mReadBufferTemp = new Byte[bufferSize / 2]; // 读缓存区
            mWriteBuffer = new TBuffer<Byte>(bufferSize); // 主写数据区
        }

        public Byte[] InPipe
        {
            get
            {
                return mReadBufferTemp;
            }
        }

        public void FinishedIn(int length)
        {
            mReadBuffer.Push(mReadBufferTemp, length);
        }

        public Byte[] OutPipe
        {
            get
            {
                return mWriteBuffer.Buffer();
            }
        }

        public void FinishedOut(int length)
        {
            mWriteBuffer.Pop(length);
        }

        public Byte[] StreamIn
        {
            get
            {
                return mReadBuffer.Buffer();
            }
        }

        public int StreamInLength
        {
            get
            {
                return mReadBuffer.DataSize();
            }
        }

        public Byte[] StreamOut
        {
            get
            {
                return mWriteBuffer.Buffer();
            }
        }

        public int StreamOutLength
        {
            get
            {
                return mWriteBuffer.DataSize();
            }
        }

        public void PopStreamIn(int length)
        {
            mReadBuffer.Pop(length);
        }

        public void PushStreamOut(byte[] buffer)
        {
            mWriteBuffer.Push(buffer);
        }

        public void Clear()
        {
            mReadBuffer.Clear();
            mWriteBuffer.Clear();
        }
    }
}
