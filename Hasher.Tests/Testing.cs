using NUnit.Framework;
using System.Diagnostics;
using System.IO;

namespace Hasher.Tests
{
    public class HasherTests
    {
        [Test]
        public void TestCRC32Hash()
        {
            string testFile = "test.txt";
            string testContent = "This is a test file.";

            File.WriteAllText(testFile, testContent);

            string expectedCRC32 = "d2da1783";
            string actualCRC32 = CalculateCRC32(testFile);

            Assert.That(expectedCRC32, Is.EqualTo(actualCRC32));
        }

        [Test]
        public void TestSum32Hash()
        {
            string testFile = "test.txt";
            string testContent = "This is a test file.";

            File.WriteAllText(testFile, testContent);

            long expectedSum32 = 1754;
            long actualSum32 = CalculateSum32(testFile);

            Assert.That(expectedSum32, Is.EqualTo(actualSum32));
        }

        private string CalculateCRC32(string filename)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "Hasher.exe",
                Arguments = $"-f \"{filename}\" -m crc32",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(processStartInfo))
            {
                string result = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                return result.Trim();
            }
        }

        private long CalculateSum32(string filename)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "Hasher.exe",
                Arguments = $"-f \"{filename}\" -m sum32",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(processStartInfo))
            {
                string result = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                return long.Parse(result.Trim());
            }
        }
    }
}
