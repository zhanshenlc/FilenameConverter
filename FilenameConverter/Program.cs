using System;
using System.IO;

namespace FilenameConverter {
    class Program {
        [STAThread]

        static void Main(string[] _) {
            while (true) {
                Console.WriteLine("Filename Converter\nPress 1 for songs\nPress 2 for anime\nPress Esc to exit");
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                if (keyInfo.Key == ConsoleKey.Escape) {
                    return;
                } else if (keyInfo.Key == ConsoleKey.D1 || keyInfo.Key == ConsoleKey.NumPad1) {
                    Console.WriteLine();
                    Song.ConvertSong();
                    break;
                } else if (keyInfo.Key == ConsoleKey.D2 || keyInfo.Key == ConsoleKey.NumPad2) {
                    Console.WriteLine();
                    Anime.ConvertAnime();
                    break;
                } else {
                    Console.WriteLine("Invalid Input");
                }
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
