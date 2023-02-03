using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace CmdByteReplacer
{
    class Program
    {
        static List<byte> searchBytes = new List<byte>();
        static List<byte> replaceBytes = new List<byte>();

        static void Main(string[] args)
        {
            bool russian = CultureInfo.CurrentUICulture.EnglishName.IndexOf("russian", StringComparison.OrdinalIgnoreCase) >= 0;
            if (args.Length > 2 && File.Exists(args[0]))
            {
                hexToByte(searchBytes, args[1].Replace(" ", ""));
                hexToByte(replaceBytes, args[2].Replace(" ", ""));
                if (searchBytes.Count == replaceBytes.Count)
                {
                    try
                    {
                        byte[] bytesFile = File.ReadAllBytes(args[0]);
                        int fileSize = bytesFile.Length;
                        int searchSize = searchBytes.Count;
                        bool find = true;
                        for (int i = 0; i + searchSize < fileSize; i++)
                        {
                            find = true;
                            for (int j = 0; j < searchSize; j++)
                            {
                                if (bytesFile[i + j] != searchBytes[j])
                                {
                                    find = false;
                                    break;
                                }
                            }
                            if (find)
                            {
                                Buffer.BlockCopy(replaceBytes.ToArray(), 0, bytesFile, i, searchSize);
                                File.WriteAllBytes(args[0], bytesFile);
                                Console.WriteLine(russian ? "Операция замены выполнена." : "Replacement operation completed.");
                                break;
                            }
                        }
                        if (!find)
                        {
                            Console.WriteLine(russian ? "Операция замены не выполнена." : "Replacement operation not completed.");
                        }
                        bytesFile = null;
                    }
                    catch
                    {
                        Console.WriteLine(russian ? "Ошибка операции замены." : "Replace operation failed.");
                    }
                }
                else
                {
                    Console.WriteLine(russian ? "Поиск и замена не совпадают по размеру." : "Find and replace are not the same size.");
                }
            }
            else
            {
                Console.WriteLine(russian ? "Пример: путькфайлу \"00 00 00\" \"10 10 10\"" : "Example: filepath \"00 00 00\" \"10 10 10\"");
            }
        }
        private static void hexToByte(List<byte> list, string line)
        {
            int count = line.Length;
            for (int i = 0; i < count; i += 2)
            {
                list.Add(Convert.ToByte(line.Substring(i, 2), 16));
            }
        }
    }
}
