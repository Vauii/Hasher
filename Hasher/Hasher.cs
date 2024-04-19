using System;
using System.IO;

namespace FileHasher
{
    class Hasher
    {
        static void Main(string[] args)
        {
            if (args.Length == 0 || args[0] == "-h")
            {
                PrintHelp();
                return;
            }

            string filename = "";
            string mode = "";

            for (int i = 0; i < args.Length; i += 2)
            {
                if (args[i] == "-f")
                {
                    filename = args[i + 1];
                }
                else if (args[i] == "-m")
                {
                    mode = args[i + 1];
                }
            }

            if (string.IsNullOrEmpty(filename) || string.IsNullOrEmpty(mode))
            {
                Console.WriteLine("Ошибка: Необходимо указать имя файла и режим.");
                PrintHelp();
                return;
            }

            if (!File.Exists(filename))
            {
                Console.WriteLine("Ошибка: Файл не найден.");
                return;
            }

            try
            {
                switch (mode)
                {
                    case "crc32":
                        Console.WriteLine($"CRC32 хеш файла {filename}: {CalculateCRC32(filename)}");
                        break;
                    case "sum32":
                        Console.WriteLine($"Сумма 32-битных блоков данных файла {filename}: {CalculateSum32(filename)}");
                        break;
                    default:
                        Console.WriteLine("Ошибка: Неподдерживаемый режим.");
                        PrintHelp();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static string CalculateCRC32(string filename)
        {
            using (var stream = File.OpenRead(filename))
            {
                var crc32Calculator = new CRC32Calculator();
                var hash = crc32Calculator.ComputeHash(stream);
                return hash.ToString("X").ToLower();
            }
        }

        static uint CalculateSum32(string filename)
        {
            using (var stream = File.OpenRead(filename))
            {
                var sum32Calculator = new Sum32Calculator();
                return sum32Calculator.ComputeHash(stream);
            }
        }

        static void PrintHelp()
        {
            Console.WriteLine("Аргументы программы:");
            Console.WriteLine("./Hasher.exe -h");
            Console.WriteLine("./Hasher.exe -f <filename> -m <mode>");
            Console.WriteLine("./Hasher.exe -m <mode> -f <filename>");
            Console.WriteLine("Где <mode> может принимать значения из {crc32, sum32}");
        }
    }
}
