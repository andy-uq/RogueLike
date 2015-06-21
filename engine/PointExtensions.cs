namespace RogueLike
{
	public static class PointExtensions
	{
		public static Point OffsetX(this Point point, int x)
		{
			return new Point(point.X + x, point.Y);
		}

		public static Point Add(this Point point, int x, int y)
		{
			return new Point(point.X + x, point.Y + y);
		}
	}
}