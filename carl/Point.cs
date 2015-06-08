using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;

namespace carl
{
	[DebuggerDisplay("X={X}, Y={Y} [{Magnitude}]")]
	public struct Point : IEquatable<Point>, IFormattable
	{
		public static readonly Point Zero = default(Point);

		private const string NUMBER_PATTERN = @"-?\d+(\.\d+)?";
		private const string POINT_PATTERN = @"\s*
												(?<x>" + NUMBER_PATTERN + @")
												,\s*(?<y>" + NUMBER_PATTERN + @")?
											   \s*";

		private static readonly Regex _parsePoint =
			new Regex(
				@"^
										(
											" + Delimited("(", ")") + @"
											|" + Delimited("{", "}") + @"
											|" + POINT_PATTERN + @"
										)
									$",
				RegexOptions.IgnorePatternWhitespace);

		private static string Delimited(string left, string right)
		{
			left = left == null ? "" : Regex.Escape(left);
			right = right == null ? "" : Regex.Escape(right);

			return $@"({left}{POINT_PATTERN}{right})";
		}

		public Point(int x, int y)
			: this()
		{
			X = x;
			Y = y;
		}

		public int X { get; }
		public int Y { get; }

		public double Magnitude
		{
			get
			{
				double raw = Math.Sqrt((X*X) + (Y*Y));
				return Math.Round(raw, 4);
			}
		}

		#region IEquatable<Point> Members

		public bool Equals(Point obj)
		{
			return 
				Equals(obj.X, X) 
				&& Equals(obj.Y, Y);
		}

		#endregion

		#region IFormattable Members

		[Pure]
		public string ToString(string format, IFormatProvider fp = null)
		{
			if (String.IsNullOrEmpty(format))
				format = "G";

			// If G format specifier is passed, display like this: (x, y).
			if (format.ToLower() == "g")
				return $"({X:d4}, {Y:d4})";

			// If N format specifier is passed, display like this: (x, y).
			if (format.ToLower() == "n")
				return $"{X:d4} {Y:d4}";

			// For "x" formatting, return just the x value as a string
			if (format.ToLower() == "x")
				return X.ToString("d4");

			// For "y" formatting, return just the y value as a string
			if (format.ToLower() == "y")
				return Y.ToString("d4");

			// For "l" formatting, return just the magnitude as a string
			if (format.ToLower() == "l")
				return Magnitude.ToString("n4");

			// For any unrecognized format, throw an exception.
			throw new FormatException($"Invalid format string: '{format}'.");
		}

		#endregion

		public override bool Equals(object obj)
		{
			if (obj.GetType() != typeof (Point))
				return false;

			return Equals((Point) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int result = X.GetHashCode();
				result = (result*397) ^ Y.GetHashCode();
				return result;
			}
		}

		public static Point Parse(string value)
		{
			Match m = _parsePoint.Match(value.Trim());
			if (m.Success)
			{
				int x = int.Parse(m.Groups["x"].Value);
				int y = int.Parse(m.Groups["y"].Value);
				return new Point(x, y);
			}

			throw new FormatException("Unable to parse Point: " + value);
		}

		public override string ToString()
		{
			return ToString("g");
		}

		public static bool operator ==(Point left, Point right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Point left, Point right)
		{
			return !left.Equals(right);
		}

		[Pure]
		public static implicit operator Vector(Point p)
		{
			return new Vector(p.X, p.Y);
		}

		[Pure]
		public static Point operator +(Point a, Point b)
		{
			return new Point(a.X + b.X, a.Y + b.Y);
		}

		[Pure]
		public static Point operator -(Point a, Point b)
		{
			return new Point(a.X - b.X, a.Y - b.Y);
		}
	}
}