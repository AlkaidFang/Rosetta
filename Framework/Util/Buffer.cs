/**
 * 一个缓冲区
 * 此处没有实现深层拷贝
 * */

using System;

namespace Alkaid
{
    public interface IBuffer<T>
    {
        // add all
        void Push(T[] data);
        // add element
        void Push(T[] data, int length);

        // pop all
        T[] Pop();

        // pop length
        T[] Pop(int length);

        // data at index
        T Get(int index);

        // clear
        void Clear();

        // get area
        T[] Buffer();

        // data size
        int DataSize();

        // size
        int Size();
    }

    public class TBuffer<T> : IBuffer<T>
    {
        private int _offset = 0;
        private T[] _data = null;
        private object _lock = null;

        public TBuffer(int length)
        {
            _offset = 0;
            _data = new T[length];
            _lock = new object();
        }

        private void Resize(int times)
        {
            T[] swi = _data;
            _data = new T[swi.Length * times];
            Array.Copy(swi, _data, swi.Length);
            swi = null;
        }

        public void Push(T[] data)
        {
            Push(data, data.Length);
        }

        public void Push(T[] data, int length)
        {
            lock(_lock)
            {
                if (_offset + length > _data.Length)
                {
                    int times = (_offset + length) / _data.Length + 1;
                    Resize(times);
                }
                Array.Copy(data, 0, _data, _offset, length);
                _offset += length;
            }
        }

        public T[] Pop()
        {
            return Pop(_offset);
        }

        public T[] Pop(int length)
        {
            if (length > _offset) return null;

            T[] ret = new T[length];
            lock(_lock)
            {
                Array.Copy(_data, ret, length);

                Array.Copy(_data, _offset, _data, 0, length);

                _offset -= length;
            }

            return ret;
        }

        public T Get(int index)
        {
            return index < _data.Length ? _data[index] : default(T);
        }

        public void Clear()
        {
            _offset = 0;
        }

        public T[] Buffer()
        {
            /*T[] ret = new T[_offset];
            Array.Copy(_data, ret, _offset);
            return ret;*/
            return _data;
        }

        public int DataSize()
        {
            return _offset;
        }

        public int Size()
        {
            return _data.Length;
        }
    }
}
