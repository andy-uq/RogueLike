﻿using System;
using System.IO;
using Newtonsoft.Json;

namespace RogueLike.Win32
{
	public class SaveGameStore : ISaveGameStore
	{
		public MapState? LoadMap(Action<MapState> action)
		{
			if (File.Exists("player.txt"))
			{
				var path = Path.Combine("data/save", "mapstate.txt");
				var json = File.ReadAllText(path);
				var map = Deserialise<MapState>(json);
				action(map);
				return map;
			}

			return null;
		}

		public void Save(MapState map)
		{
			var path = Path.Combine("data/save", "mapstate.txt");
			var json = Serialise(map);
			File.WriteAllText(path, json);
		}

		public void Save(PlayerState player)
		{
			var path = Path.Combine("data/save", "player.txt");
			var json = Serialise(player);
			File.WriteAllText(path, json);
		}

		public PlayerState? LoadPlayer(Action<PlayerState> action)
		{
			var path = Path.Combine("data/save", "player.txt");
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
			return Newtonsoft.Json.JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
		}

		private static T Deserialise<T>(string json)
		{
			return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
		}
	}
}