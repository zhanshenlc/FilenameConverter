using WMPLib;
using System;
using System.IO;
using System.Linq;

namespace FilenameConverter {
    class SongRating {

        public static string[] GetAllFiles(string rootPath) {
            string[] flacs = Directory.GetFiles(rootPath, "*.flac", SearchOption.AllDirectories);
            string[] wavs = Directory.GetFiles(rootPath, "*.wav", SearchOption.AllDirectories);
            string[] mp3s = Directory.GetFiles(rootPath, "*.mp3", SearchOption.AllDirectories);

            return flacs.Concat(wavs).ToArray().Concat(mp3s).ToArray();
        }

        public static void QueryFileData(string rootPath, string storePath) {

            string[] filenames = GetAllFiles(rootPath);

            WindowsMediaPlayer wmp = new WindowsMediaPlayer();

            int wmpPlayCountAtom = wmp.mediaCollection.getMediaAtom("UserPlayCount");
            int wmpRatingAtom = wmp.mediaCollection.getMediaAtom("UserRating");

            using (StreamWriter writer = new StreamWriter(storePath + "UserMusicCollectionDatabase.txt")) {
                foreach (string filename in filenames) {
                    IWMPPlaylist wmpTrackSearch = wmp.mediaCollection.getByAttribute("SourceURL", filename);
                    try {
                        IWMPMedia wmpTrack = wmpTrackSearch.get_Item(0);

                        string wmpPlayCount = wmpTrack.getItemInfoByAtom(wmpPlayCountAtom);
                        string wmpRating = wmpTrack.getItemInfoByAtom(wmpRatingAtom);

                        writer.WriteLine(filename + "\t" + wmpPlayCount + "\t" + wmpRating);
                    } catch(Exception e) {
                        Console.WriteLine(e.Message);
                        Console.WriteLine(wmpTrackSearch.count);
                    }
                    wmpTrackSearch.clear();
                }
            }

            wmp.close();
        }

        public static void EditSongData(string databasePath) {

            WindowsMediaPlayer wmp = new WindowsMediaPlayer();

            try {
                foreach (string line in File.ReadLines(databasePath)) {
                    string[] info = line.Split('\t');

                    IWMPPlaylist wmpTrackSearch = wmp.mediaCollection.getByAttribute("SourceURL", info[0]);
                    try {
                        IWMPMedia wmpTrack = wmpTrackSearch.get_Item(0);

                        wmpTrack.setItemInfo("UserPlayCount", info[1]);
                        wmpTrack.setItemInfo("UserRating", info[2]);
                    } catch (Exception e) {
                        Console.WriteLine(e.Message);
                        Console.WriteLine(wmpTrackSearch.count);
                    }
                    wmpTrackSearch.clear();
                }
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }

            wmp.close();
        }

    }
}
