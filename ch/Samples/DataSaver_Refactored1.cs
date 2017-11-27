using System;
using System.IO;

namespace Samples
{
	internal class DataSaver_Refactored1
	{
		public static void SaveData(string filename, byte[] data)
		{
			using (var fs1 = new FileStream(filename, FileMode.OpenOrCreate))
			{
				var backupName = Path.ChangeExtension(filename, "bkp");
				using (var fs2 = new FileStream(backupName, FileMode.OpenOrCreate))
				{
					fs1.Write(data, 0, data.Length);
					fs2.Write(data, 0, data.Length);
				}

				var tf = filename + ".time";
				using (var fs3 = new FileStream(tf, FileMode.OpenOrCreate))
				{
					var t = BitConverter.GetBytes(DateTime.Now.Ticks);
					fs3.Write(t, 0, t.Length);
				}
			}
		}
	}
}