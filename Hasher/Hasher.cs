using System;
using System.IO;

using FileHasher;


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

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-f" && i + 1 < args.Length)
                {
                    filename = args[i + 1];
                }
                else if (args[i] == "-m" && i + 1 < args.Length)
                {
                    mode = args[i + 1];
                }
            }

            if (filename == null || mode == null)
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
                var crc32 = new Crc32();
                var hash = crc32.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }

        static long CalculateSum32(string filename)
        {
            using (var stream = File.OpenRead(filename))
            {
                long sum = 0;
                int bytesRead;
                byte[] buffer = new byte[4];

                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    for (int i = 0; i < bytesRead; i += 4)
                    {
                        sum += BitConverter.ToInt32(buffer, i);
                    }
                }

                return sum;
            }
        }

        static void PrintHelp()
        {
            Console.WriteLine("Аргументы программы:");
            Console.WriteLine("./hasher -h");
            Console.WriteLine("./hasher -f <filename> -m <mode>");
            Console.WriteLine("./hasher -m <mode> -f <filename>");
            Console.WriteLine("Где <mode> может принимать значения из {crc32, sum32}");
        }
    }
}
