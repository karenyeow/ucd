using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Common.Helpers.Extensions
{
    public static class ByteExtensions
    {
        public static Stream ConvertTo(this Byte[] byteArray)
        {
            Stream stream = new MemoryStream(byteArray)
            {
                Position = 0
            };
            return stream;
        }

        public static void WriteToStream(this byte[] byteArray, Stream str)
        {
            //BinaryWriter writer = new BinaryWriter(str);
            //writer.Write(byteArray);

            long length = byteArray.Length;
            int times = 0;


            int readBytes = (int)length / 20; //number of bytes to read per block. Each file is read into the buffer in 20 blocks of data.

            //in case of very small file, the readBytes must be at least 1. 
            if (readBytes > 0)
            {
                while (length > readBytes)
                {// once read 1024 bytes for example
                    str.Write(byteArray, readBytes * times, readBytes);

                    length -= readBytes;
                    times++;
                }
            }

            str.Write(byteArray, readBytes * times, ((int)byteArray.Length) - readBytes * times);
            str.Seek(0, SeekOrigin.Begin);
        }
    }
}
