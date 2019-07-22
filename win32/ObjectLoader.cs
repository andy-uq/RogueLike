using System.Collections.Generic;
using System.Linq;

namespace RogueLike.Win32
{
	public class ObjectLoader : IObjectLoader
	{
		private static string GetDataPath(string name)
		{
			return System.IO.Path.Combine("data", name);
		}

		public T Load<T>(string name)
		{
			var path = GetDataPath(name);
			var json = System.IO.File.ReadAllText(path);
			return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
		}

		public List<T> LoadAll<T>(string name)
		{
			var path = GetDataPath(name);
			var json = System.IO.File.ReadAllText(path);
			return Newtonsoft.Json.JsonConvert.DeserializeObject<List<T>>(json);
		}

		public char[][] LoadTiles(string mapName)
		{
			var path = GetDataPath(mapName);
			return System.IO.File
				.ReadAllLines(path)
				.Select(t => t.ToArray())
				.ToArray();
		}
	}
}