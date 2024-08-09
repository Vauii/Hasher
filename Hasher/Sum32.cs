namespace FileHasher
{
    public class Sum32Calculator
    {
        private readonly uint _bufferSize;
        public Sum32Calculator(uint bufferSize = 4096)
        {
            _bufferSize = bufferSize;
        }

        public uint ComputeHash(Stream inputStream)
        {
            uint sum = 0;
            byte[] buffer = new byte[_bufferSize];
            int bytesRead;

            while ((bytesRead = inputStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                int i = 0;
                while (i <= bytesRead - 4)
                {
                    sum += BitConverter.ToUInt32(buffer, i);
                    i += 4;
                }

                int residue = bytesRead % 4;
                if (residue > 0)
                {
                    uint temp;
                    Array.Copy(buffer, bytesRead - residue, buffer, 0, residue);
                    Array.Clear(buffer, residue, 4 - residue);
                    temp = BitConverter.ToUInt32(buffer, 0);
                    sum += temp;
                }
            }

            return sum;
        }
    }
}
