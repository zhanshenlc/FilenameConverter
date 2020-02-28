using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FilenameConverter {
    class Anime {

        public static void ConvertAnime() {
            string currentDirectory = Directory.GetCurrentDirectory();
            string[] filenames = Directory.GetFiles(currentDirectory);
            List<String> videos = new List<string>(), subtitles = new List<string>();
            string videoExtension = "", subtitleExtension = "";
            foreach (string filename in filenames) {
                if (Path.GetExtension(filename) == ".ass") {
                    if (subtitleExtension.CompareTo("") == 0) {
                        subtitleExtension = ".ass";
                    } else if (subtitleExtension.CompareTo(".ass") == 1) {
                        Console.WriteLine("Mutiple subtitle types found");
                        return;
                    }
                    subtitles.Add(filename);
                } else if (Path.GetExtension(filename) == ".srt") {
                    if (subtitleExtension.CompareTo("") == 0) {
                        subtitleExtension = ".srt";
                    } else if (subtitleExtension.CompareTo(".srt") == 1) {
                        Console.WriteLine("Mutiple subtitle types found");
                        return;
                    }
                    subtitles.Add(filename);
                } else if (Path.GetExtension(filename) == ".mkv") {
                    if (videoExtension.CompareTo("") == 0) {
                        videoExtension = ".mkv";
                    } else if (videoExtension.CompareTo(".mkv") == 1) {
                        Console.WriteLine("Mutiple video types found");
                        return;
                    }
                    videos.Add(filename);
                } else if (Path.GetExtension(filename) == ".mp4") {
                    if (videoExtension.CompareTo("") == 0) {
                        videoExtension = ".mp4";
                    } else if (videoExtension.CompareTo(".mp4") == 1) {
                        Console.WriteLine("Mutiple video types found");
                        return;
                    }
                    videos.Add(filename);
                }
            }
            if (videos.Count != subtitles.Count) {
                Console.WriteLine("Different number of video and subtitle files");
                return;
            }
            videos.Sort();
            subtitles.Sort();
            for (int i = 0; i < videos.Count; i++) {
                File.Move(subtitles[i], Path.GetFileNameWithoutExtension(videos[i]) + subtitleExtension);
            }
            Console.WriteLine("{0} files converted", videos.Count);
        }
    }
}
