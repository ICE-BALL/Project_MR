using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    public class RecvBuffer
    {
        ArraySegment<byte> _buffer;

        int _readPos;
        int _writePos;

        public RecvBuffer(int buffSize)
        {
            _buffer = new ArraySegment<byte>(new byte[buffSize], 0, buffSize);
        }

        public int DataSize { get { return _writePos - _readPos; } }
        public int FreeSize { get { return _buffer.Count - _writePos; } }

        public ArraySegment<byte> ReadSegment
        {
            get { return new ArraySegment<byte>(_buffer.Array, _buffer.Offset + _readPos, DataSize); }
        }

        public ArraySegment<byte> WriteSegment
        {
            get { return new ArraySegment<byte>(_buffer.Array, _buffer.Offset + _writePos, FreeSize); }
        }

        public void Clean()
        {
            int dataSize = DataSize;
            if (dataSize == 0)
            {
                _readPos = _writePos = 0;
            }
            else
            {
                Array.Copy(_buffer.Array, _buffer.Offset + _readPos, _buffer.Array, _buffer.Offset, dataSize);
                _readPos = 0;
                _writePos = dataSize;
            }
        }

        public bool OnRead(int numOfbytes)
        {
            if (numOfbytes > DataSize)
                return false;

            _readPos += numOfbytes;
            return true;
        }

        public bool OnWrite(int numOfbytes)
        {
            if (numOfbytes > FreeSize)
                return false;

            _writePos += numOfbytes;
            return true;
        }
    }
}
