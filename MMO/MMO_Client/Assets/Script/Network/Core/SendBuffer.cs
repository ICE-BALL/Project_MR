using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
    public class SendBufferHelper
    {
        public static ThreadLocal<SendBuffer> _sendBuffer = new ThreadLocal<SendBuffer>(() => { return null; });

        public static int ChunkSize { get; set; } = 4096 * 100;

        public static ArraySegment<byte> Open(int reserveSize)
        {
            if (_sendBuffer.Value == null)
                _sendBuffer.Value = new SendBuffer(ChunkSize);
            if (reserveSize > _sendBuffer.Value.FreeSize)
                _sendBuffer.Value = new SendBuffer(ChunkSize);

            return _sendBuffer.Value.Open(reserveSize);
        }

        public static ArraySegment<byte> Close(int usedSize)
        {
            return _sendBuffer.Value.Close(usedSize);
        }
    }

    public class SendBuffer
    {
        byte[] _buffer;
        int _usedSize;

        public int FreeSize { get { return _buffer.Length - _usedSize; } }

        public SendBuffer(int ChunkSize)
        {
            _buffer = new byte[ChunkSize];
        }

        public ArraySegment<byte> Open(int reserverSize)
        {
            if (reserverSize > FreeSize)
                return null;

            return new ArraySegment<byte>(_buffer, _usedSize, reserverSize);
        }

        public ArraySegment<byte> Close(int usedSize)
        {
            ArraySegment<byte> Buffer = new ArraySegment<byte>(_buffer, _usedSize, usedSize);
            _usedSize += usedSize;
            return Buffer;
        }
    }
}
