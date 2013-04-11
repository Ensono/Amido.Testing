using System;
using System.IO;

namespace Amido.Azure.Storage.BlobStorage.Extensions
{
    public static class StreamExtensions
    {
        public static Byte[] ToByteArray(this Stream stream)
        {
            var length = stream.Length > Int32.MaxValue ? Int32.MaxValue : Convert.ToInt32(stream.Length);
            var buffer = new Byte[length];
            stream.Read(buffer, 0, length);
            return buffer;
        }
    }
}