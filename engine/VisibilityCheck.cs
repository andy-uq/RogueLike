using System;

namespace RogueLike
{
	public class VisibilityCheck
	{
		private readonly Tile[,] _map;

		public VisibilityCheck(Tile[,] map)
		{
			_map = map;
		}

		private bool IsOpenTile(int x, int y)
		{
			var tile = _map[x, y];
			return tile == Tiles.OpenDoor || tile == Tiles.Floor;
		}

		public bool CanSee(Point source, Point target)
		{
			var x = Math.Abs(target.X - source.X);
			var y = Math.Abs(target.Y - source.Y);

			if (x > y)
			{
				var dX = (target.X > source.X) ? 1 : -1;
				var dY = x == 0 ? double.NaN : (double)y / (double)x;
				if (target.Y < source.Y)
					dY *= -1;

				return ScanVertically(source, dX, dY, x);
			}
			else
			{
				var dY = (target.Y > source.Y) ? 1 : -1;
				var dX = x == 0 ? double.NaN : (double)y / (double)x;
				if (target.X < source.X)
					dX *= -1;

				return ScanHorizontally(source, dX, dY, y);
			}
		}

		private bool ScanHorizontally(Point from, double dX, int dY, int count)
		{
			double accumulator = 0;

			int x = from.X, y = from.Y;
			for (; count > 0; count--)
			{
				if (!IsOpenTile(x, y))
					return false;

				y += dY;
				accumulator += dX;
				if (accumulator > 1.0)
				{
					x += 1;
					accumulator -= 1.0;
				}
				else if (accumulator < -1.0)
				{
					x -= 1;
					accumulator += 1.0;
				}
			}

			return true;
		}

		private bool ScanVertically(Point from, int dX, double dY, int count)
		{
			double accumulator = 0;

			int x = from.X, y = from.Y;
			for (; count > 0; count--)
			{
				if (!IsOpenTile(x, y))
					return false;

				x += dX;
				accumulator += dY;

				if (accumulator > 1.0)
				{
					y++;
					accumulator -= 1.0;
				}
				else if (accumulator < -1.0)
				{
					y--;
					accumulator += 1.0;
				}
			}

			return true;
		}
	}
}