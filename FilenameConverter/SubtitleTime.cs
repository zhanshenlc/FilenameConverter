using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.IO;

namespace FilenameConverter {
    class SubtitleTime {

        private static string EditOneLine(string input, TimeSpan timeChange) {
			// Match Dialogue
			MatchCollection mc = Regex.Matches(input, @"^Dialogue: .*");
			if (mc.Count != 1) {
				return input;
            }

			// Match timeline
			mc = Regex.Matches(input, @"\d:\d\d:\d\d.\d\d");
			if (mc.Count != 2) {
				throw new Exception("Invalid content format");
            }
			CultureInfo cultureInfo = new CultureInfo("zh-CN");
			string startTimeStr = mc[0].ToString();
			string endTimeStr = mc[1].ToString();
			TimeSpan startTime = TimeSpan.Parse(startTimeStr, cultureInfo);
			TimeSpan endTime = TimeSpan.Parse(endTimeStr, cultureInfo);

			// Change timeline
			string newStartTimeStr, newEndTimeStr;
			startTime = startTime.Add(timeChange);
			endTime = endTime.Add(timeChange);
			if (TimeSpan.Compare(startTime, new TimeSpan(0, 0, 0)) == -1) {
				startTime = new TimeSpan(0, 0, 0, 0);
				newStartTimeStr = startTime.ToString().Remove(0, 1) + ".00";
			} else {
				newStartTimeStr = startTime.ToString().Remove(0, 1);
				newStartTimeStr = Truncate(newStartTimeStr, 10);
				if (newStartTimeStr.Length != 10) {
					newStartTimeStr += ".00";
                }
            }
			if (TimeSpan.Compare(endTime, new TimeSpan(0, 0, 0)) == -1) {
				endTime = new TimeSpan(0, 0, 0, 0);
				newEndTimeStr = endTime.ToString().Remove(0, 1) + ".00";
			} else {
				newEndTimeStr = endTime.ToString().Remove(0, 1);
				newEndTimeStr = Truncate(newEndTimeStr, 10);
				if (newEndTimeStr.Length != 10) {
					newEndTimeStr += ".00";
                }
            }

			string output = input;
			output = output.Replace(startTimeStr, newStartTimeStr);
			output = output.Replace(endTimeStr, newEndTimeStr);

			return output;
		}

		public static void EditSubtitleFile(String filename, TimeSpan timeChange) {
			string[] lines = File.ReadAllLines(filename);
			using (StreamWriter file = new StreamWriter(Path.GetFileNameWithoutExtension(filename) + "n.ass")) {
				foreach (string line in lines) {
					file.WriteLine(EditOneLine(line, timeChange));
				}
			}
		}

		public static void EditSubtitleFiles(TimeSpan timeChange) {
			string currentDirectory = Directory.GetCurrentDirectory();
			string[] filenames = Directory.GetFiles(currentDirectory);
			foreach (string filename in filenames) {
				if (Path.GetExtension(filename) == ".ass") {
					EditSubtitleFile(Path.Combine(currentDirectory, filename), timeChange);
				}
            }
			Console.WriteLine("{0} files edited", filenames.Length);
		}

		public static string Truncate(string value, int maxLength) {
			if (string.IsNullOrEmpty(value)) return value;
			return value.Length <= maxLength ? value : value.Substring(0, maxLength);
		}

	}
}
