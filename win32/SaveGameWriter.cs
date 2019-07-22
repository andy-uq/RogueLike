using System;
using System.IO;
using Newtonsoft.Json;

namespace RogueLike.Win32
{
	public class SaveGameStore : ISaveGameStore
	{
		private static readonly JsonSerializerSettings s_jsonSerializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
		
		private readonly string _savePath;

		public SaveGameStore()
		{
			var save = Directory.CreateDirectory(Path.Combine("data/save"));
			_savePath = save.FullName;
		}

		public MapState? LoadMap(Action<MapState> action)
		{
			if (File.Exists(Path.Combine(_savePath, "player.txt")))
			{
				var path = Path.Combine(_savePath, "mapstate.txt");
				var json = File.ReadAllText(path);
				var map = Deserialise<MapState>(json);
				action(map);
				return map;
			}

			return null;
		}

		public void Save(MapState map)
		{
			var path = Path.Combine(_savePath, "mapstate.txt");
			var json = Serialise(map);
			File.WriteAllText(path, json);
		}

		public void Save(PlayerState player)
		{
			var path = Path.Combine(_savePath, "player.txt");
			var json = Serialise(player);
			File.WriteAllText(path, json);
		}

		public PlayerState? LoadPlayer(Action<PlayerState> action)
		{
			var path = Path.Combine(_savePath, "player.txt");
			if (File.Exists(path))
			{
				var json = File.ReadAllText(path);
				var player = Deserialise<PlayerState>(json);
				action(player);
				return player;
			}

			return null;
		}

		private static string Serialise(object obj)
		{
			return JsonConvert.SerializeObject(obj, Formatting.Indented, s_jsonSerializerSettings);
		}

		private static T Deserialise<T>(string json)
		{
			return JsonConvert.DeserializeObject<T>(json, s_jsonSerializerSettings);
		}
	}
}