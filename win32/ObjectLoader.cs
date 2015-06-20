using System.Collections.Generic;
using System.Linq;
using carl;

namespace win32
{
	public class ObjectLoader : IObjectLoader
	{
		public T Load<T>(string name)
		{
			var json = System.IO.File.ReadAllText(name);
			return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
		}

		public List<T> LoadAll<T>(string name)
		{
			var json = System.IO.File.ReadAllText(name);
			return Newtonsoft.Json.JsonConvert.DeserializeObject<List<T>>(json);
		}

		public char[][] LoadTiles(string mapName)
		{
			return System.IO.File
				.ReadAllLines(mapName)
				.Select(t => t.ToArray())
				.ToArray();
		}
	}
}