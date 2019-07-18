using System;
using System.Collections.Generic;

namespace RogueLike
{
	public class PlayerState
	{
		public PointXY Position { get; set; }
		public List<PlayerItem> Inventory { get; set; }

		public PlayerState()
		{
			Inventory = new List<PlayerItem>();
		}
	}

	public class MapState
	{
		public List<TileState> Tiles { get; set; }
		public List<MobileState> Mobiles { get; set; }

		public MapState()
		{
			Tiles = new List<TileState>();
			Mobiles = new List<MobileState>();
		}
	}

	public class MobileState
	{
		public int Id { get; set; }
		public PointXY Position { get; set; }
	}

	public class TileState
	{
		public PointXY Coordinates { get; set; }
	}

	public struct PointXY : IEquatable<PointXY>
	{
		public int X { get; }
		public int Y { get; }

		public PointXY(int x, int y)
		{
			X = x;
			Y = y;
		}

		public bool Equals(PointXY other)
		{
			return X == other.X && Y == other.Y;
		}

		public override bool Equals(object obj)
		{
			return obj is PointXY other && Equals(other);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (X * 397) ^ Y;
			}
		}

		public static bool operator ==(PointXY left, PointXY right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(PointXY left, PointXY right)
		{
			return !left.Equals(right);
		}

		public void Deconstruct(out int x, out int y)
		{
			x = X;
			y = Y;
		}
	}
}