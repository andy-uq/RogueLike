namespace RogueLike.Definitions
{
	public class Level
	{
		public Level(string map, string monsters)
		{
			Map = map;
			Monsters = monsters;
		}

		public string Map { get; set; }
		public string Monsters { get; set; }
	}
}