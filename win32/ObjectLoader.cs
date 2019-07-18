using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RogueLike.Win32
{
	public class ObjectLoader : IObjectLoader
	{
		public T Load<T>(string name)
		{
			var path = Path.Combine("data", name);
			var json = System.IO.File.ReadAllText(path);
			return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
		}

		public List<T> LoadAll<T>(string name)
		{
			var path = Path.Combine("data", name);
			var json = System.IO.File.ReadAllText(path);
			return Newtonsoft.Json.JsonConvert.DeserializeObject<List<T>>(json);
		}

		public char[][] LoadTiles(string mapName)
		{
			var path = Path.Combine("data", mapName);
			return System.IO.File
				.ReadAllLines(path)
				.Select(t => t.ToArray())
				.ToArray();
		}
	}
}