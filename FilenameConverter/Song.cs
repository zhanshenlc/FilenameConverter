using System;
using System.Collections.Generic;
using System.IO;

namespace FilenameConverter {
    class Song {

        public static void ConvertSong() {
            List<string> arrHeaders = new List<string>();
            Shell32.Shell shell = new Shell32.Shell();
            Shell32.Folder objFolder;
            objFolder = shell.NameSpace(Directory.GetCurrentDirectory());
            for (int i = 0; i < short.MaxValue; i++) {
                string header = objFolder.GetDetailsOf(null, i);
                if (String.IsNullOrEmpty(header))
                    break;
                arrHeaders.Add(header);
            }

            int count = 0;
            foreach (Shell32.FolderItem2 item in objFolder.Items()) {
                string[] fileExtensions = { ".flac", ".mp3" };
                foreach (string fileExtension in fileExtensions) {
                    if (Path.GetExtension(item.Name).CompareTo(fileExtension) == 0) {
                        string title = "";
                        string number = "";
                        string discNumber = "";
                        for (int i = 0; i < arrHeaders.Count; i++) {
                            if (arrHeaders[i].CompareTo("标题") == 0) {
                                title = objFolder.GetDetailsOf(item, i);
                            } else if (arrHeaders[i].CompareTo("#") == 0) {
                                number = objFolder.GetDetailsOf(item, i);
                            } else if (arrHeaders[i].CompareTo("部分设置") == 0) {
                                discNumber = objFolder.GetDetailsOf(item, i);
                            }
                        }
                        if (title.CompareTo("") == 1 && number.CompareTo("") == 1) {
                            if (number.Length == 1) {
                                number = "0" + number;
                            }
                            if (discNumber.Length > 0) {
                                discNumber += ".";
                            }
                            title.Replace(':', '：');
                            title.Replace('/', '／');
                            title.Replace('*', '＊');
                            File.Move(item.Name, discNumber + number + " - " + title + fileExtension);
                            count++;
                        }
                    }
                }          
            }
            Console.WriteLine("{0} files converted", count);
        }

    }
}
