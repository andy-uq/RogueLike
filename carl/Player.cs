namespace carl
{
	public class Player
	{
		public Player(Point position = default(Point))
		{
			Position = position;
		}

		public Point Position { get; set; }
	}

	public class Mobile
	{
		public Mobile(Point position)
		{
			Position = position;
		}

		public Point Position { get; set; }
		public char Glyph { get; set; }
	}
}