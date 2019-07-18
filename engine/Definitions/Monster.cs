namespace RogueLike.Definitions
{
	public class Monster
	{
		public Monster(int id, string glyph)
		{
			Id = id;
			Glyph = glyph;
		}

		public int Id { get; set; }
		public string Glyph { get; set; }
	}
}