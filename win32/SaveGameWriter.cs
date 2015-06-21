using System.IO;
using LanguageExt;
using static LanguageExt.Prelude;

namespace RogueLike.Win32
{
	public class SaveGameStore : ISaveGameStore
	{
		public void Save(PlayerState player)
		{
			var json = Newtonsoft.Json.JsonConvert.SerializeObject(player);
			File.WriteAllText("player.txt", json);
		}

		public Option<PlayerState> Load()
		{
			if (File.Exists("player.txt"))
			{
				var json = File.ReadAllText("player.txt");
				var player = Newtonsoft.Json.JsonConvert.DeserializeObject<PlayerState>(json);
				return Some(player);
			}

			return None;
		}
	}
}