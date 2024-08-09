namespace FileHasher
{
    public class CRC32Calculator
    {
        private static readonly uint[] crcTable = new uint[256];
        private readonly uint _bufferSize;

        public CRC32Calculator(uint bufferSize = 4096)
        {
            _bufferSize = bufferSize;
            const uint poly = 0xEDB88320;
            for (uint i = 0; i < crcTable.Length; ++i)
            {
                uint crc = i;
                for (int j = 8; j > 0; --j)
                {
                    if ((crc & 1) == 1)
                        crc = (crc >> 1) ^ poly;
                    else
                        crc >>= 1;
                }
                crcTable[i] = crc;
            }
        }

        public uint ComputeHash(Stream inputStream)
        {
            uint crc = 0xFFFFFFFF;
            int bytesRead;
            byte[] buffer = new byte[_bufferSize];

            while ((bytesRead = inputStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                for (int i = 0; i < bytesRead; i++)
                {
                    byte index = (byte)((crc ^ buffer[i]) & 0xff);
                    crc = (crc >> 8) ^ crcTable[index];
                }
            }

            return crc ^ 0xFFFFFFFF;
        }

    }
}

