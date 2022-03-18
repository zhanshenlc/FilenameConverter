using System;
using System.IO;

namespace FilenameConverter {
    class Program {
        [STAThread]

        static void Main(string[] _) {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            while (true) {
                Console.WriteLine("Filename Converter\nPress 1 for songs\nPress 2 for anime\n" +
                    "Press 3 for editing subtitle timeline\nPress Esc to exit\n");
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                Console.WriteLine();
                if (keyInfo.Key == ConsoleKey.Escape) {
                    return;
                } 
                // Song
                else if (keyInfo.Key == ConsoleKey.D1 || keyInfo.Key == ConsoleKey.NumPad1) {
                    Song.ConvertSong();
                    break;
                }
                // Anime
                else if (keyInfo.Key == ConsoleKey.D2 || keyInfo.Key == ConsoleKey.NumPad2) {
                    while (true) {
                        Console.WriteLine("Anime Filename Converter\nPress 1 for matching\n" +
                            "Press 2 for replacing invalid characters\nPress 0 to return to last menu" +
                            "\nPress Esc to exit\n");
                        keyInfo = Console.ReadKey();
                        if (keyInfo.Key == ConsoleKey.Escape) {
                            return;
                        } else if (keyInfo.Key == ConsoleKey.D1 || keyInfo.Key == ConsoleKey.NumPad1) {
                            Anime.ConvertAnimeSubtitleFilename();
                            break;
                        } else if (keyInfo.Key == ConsoleKey.D2 || keyInfo.Key == ConsoleKey.NumPad2) {
                            Anime.ReplaceInvalidCharacters();
                            break;
                        } else if (keyInfo.Key == ConsoleKey.D0 || keyInfo.Key == ConsoleKey.NumPad0) {
                            break;
                        } else {
                            Console.WriteLine("Invalid Input\n");
                        }
                    }
                    break;
                }
                // Subtitle
                else if (keyInfo.Key == ConsoleKey.D3 || keyInfo.Key == ConsoleKey.NumPad3) {
                    while (true) {
                        Console.WriteLine("Subtitle TimeLine Editor\nPress 1 for 1 file\n" +
                            "Press 2 for all files in folder\nPress 0 to return to last menu" +
                            "\nPress Esc to exit\n");
                        keyInfo = Console.ReadKey();
                        if (keyInfo.Key == ConsoleKey.Escape) {
                            return;
                        } else if (keyInfo.Key == ConsoleKey.D1 || keyInfo.Key == ConsoleKey.NumPad1) {
                            string filename;
                            string currentDirectory = Directory.GetCurrentDirectory();
                            while (true) {
                                Console.WriteLine("\nInput the filename: ");
                                filename = Console.ReadLine();
                                filename = Path.Combine(currentDirectory, filename);
                                if (Path.IsPathFullyQualified(filename)) {
                                    break;
                                }
                            }
                            SubtitleTime.EditSubtitleFile(filename, readTimeChange());
                            break;
                        } else if (keyInfo.Key == ConsoleKey.D2 || keyInfo.Key == ConsoleKey.NumPad2) {
                            SubtitleTime.EditSubtitleFiles(readTimeChange());
                            break;
                        } else if (keyInfo.Key == ConsoleKey.D0 || keyInfo.Key == ConsoleKey.NumPad0) {
                            break;
                        } else {
                            Console.WriteLine("Invalid Input\n");
                        }
                    }
                } else {
                    Console.WriteLine("Invalid Input\n");
                }
            }

            Console.WriteLine("\nPress any key to exit");
            Console.ReadKey();
        }

        private static TimeSpan readTimeChange() {
            string sign, tmp;
            while (true) {
                Console.WriteLine("Positive or Negative? (Input 1 or 0): ");
                sign = Console.ReadLine();
                if (sign == "0" || sign == "1") {
                    break;
                }
                Console.WriteLine("Invalid Input");
            }

            int seconds;
            while (true) {
                Console.WriteLine("Seconds (range from 0 to 10): ");
                tmp = Console.ReadLine();
                seconds = Convert.ToInt32(tmp);
                if (seconds >= 0 && seconds <= 10) {
                    break;
                }
                Console.WriteLine("Invalid Input");
            }

            int milliseconds;
            while (true) {
                Console.WriteLine("Milliseconds (range from 10 to 999 or 0): ");
                tmp = Console.ReadLine();
                milliseconds = Convert.ToInt32(tmp);
                if (milliseconds >= 10 && milliseconds <= 999) {
                    break;
                }
                if (milliseconds == 0) {
                    break;
                }
                Console.WriteLine("Invalid Input");
            }

            if (sign == "0") {
                return new TimeSpan(0, 0, 0, -seconds, milliseconds);
            } else {
                return new TimeSpan(0, 0, 0, seconds, milliseconds);
            }
        }
    }
}
