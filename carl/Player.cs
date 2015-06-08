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
}