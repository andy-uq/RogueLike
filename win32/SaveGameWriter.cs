using System.IO;
using LanguageExt;
using Newtonsoft.Json;
using static LanguageExt.Prelude;

namespace RogueLike.Win32
{
	public class SaveGameStore : ISaveGameStore
	{
		public Option<MapState> LoadMap()
		{
			if (File.Exists("player.txt"))
			{
				var json = File.ReadAllText("mapstate.txt");
				var map = Deserialise<MapState>(json);
				return Some(map);
			}

			return None;
		}

		public void Save(MapState map)
		{
			var json = Serialise(map);
			File.WriteAllText("mapstate.txt", json);
		}

		public void Save(PlayerState player)
		{
			var json = Serialise(player);
			File.WriteAllText("player.txt", json);
		}

		public Option<PlayerState> LoadPlayer()
		{
			if (File.Exists("player.txt"))
			{
				var json = File.ReadAllText("player.txt");
				var player = Deserialise<PlayerState>(json);
				return Some(player);
			}

			return None;
		}

		private static string Serialise(object obj)
		{
			return Newtonsoft.Json.JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
		}

		private static T Deserialise<T>(string json)
		{
			return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
		}
	}
}