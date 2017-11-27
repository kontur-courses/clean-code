using System;
using System.IO;

namespace Samples
{
	internal class DataSaver_Refactored2
	{
		public static void SaveData_Refactored(string filename, byte[] data)
		{
			File.WriteAllBytes(filename, data);
			File.WriteAllBytes(Path.ChangeExtension(filename, "bkp"), data);
			File.WriteAllBytes(filename + ".time", BitConverter.GetBytes(DateTime.Now.Ticks));
		}
	}
}