using FileHasher;

namespace HasherTests
{
    [TestFixture]
    public class HasherTests
    {
        private string _testFilePath;
        private string _testFileContent = "Hello, World!";
        private string _crc32ExpectedHash;
        private string _sum32ExpectedHash;

        [SetUp]
        public void Setup()
        {
            _testFilePath = Path.GetTempFileName();
            File.WriteAllText(_testFilePath, _testFileContent);

            _crc32ExpectedHash = "ec4ac3d0";
            _sum32ExpectedHash = "27f90447"; 
        }

        [Test]
        public void TestCRC32Hash()
        {
            var result = Hasher.CalculateCRC32(_testFilePath);
            Assert.That(_crc32ExpectedHash, Is.EqualTo(result.ToLower()));
        }

        [Test]
        public void TestSum32Hash()
        {
            var result = Hasher.CalculateSum32(_testFilePath);
            Assert.That(_sum32ExpectedHash, Is.EqualTo(result.ToString("x").ToLower()));
        }

        [Test]
        public void TestFileNotFound()
        {
            var invalidPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Assert.Throws<FileNotFoundException>(() => Hasher.CalculateCRC32(invalidPath));
            Assert.Throws<FileNotFoundException>(() => Hasher.CalculateSum32(invalidPath));
        }

        [Test]
        public void TestInvalidProgramArguments()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                string[] args = ["-k", _testFilePath, "-m", "crc32"];
                Hasher.Main(args);
                string expectedHelpMessage = "Ошибка: Неверные аргументы программы.";
                Assert.That(sw.ToString().Contains(expectedHelpMessage), Is.True);
            }
        }

        [Test]
        public void TestIncorrectMode()
        {
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                string[] args = ["-f", _testFilePath, "-m", "crcas32"];
                Hasher.Main(args);
                string expectedHelpMessage = "Ошибка: Неподдерживаемый режим.";
                Assert.That(sw.ToString().Contains(expectedHelpMessage), Is.True);
            }
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(_testFilePath))
                File.Delete(_testFilePath);
        }
    }
}
