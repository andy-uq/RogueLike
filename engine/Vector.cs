using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;

namespace RogueLike
{
	[DebuggerDisplay("X={X}, Y={Y}, Z={Z} [{Magnitude}]")]
	public struct Vector : IEquatable<Vector>, IFormattable
	{
		public static readonly Vector Zero = new Vector(0, 0);

		private const string NUMBER_PATTERN = @"-?\d+(\.\d+)?";
		private const string VECTOR_PATTERN = @"\s*
												(?<x>" + NUMBER_PATTERN + @")
												,\s*(?<y>" + NUMBER_PATTERN + @")
												(,\s*(?<z>" + NUMBER_PATTERN + @"))?
											   \s*";

		private static readonly Regex s_parseVector =
			new Regex(
				@"^
										(
											" + Delimited("(", ")") + @"
											|" + Delimited("{", "}") + @"
											|" + VECTOR_PATTERN + @"
										)
									$",
				RegexOptions.IgnorePatternWhitespace);

		private static string Delimited(string left, string right)
		{
			left = left == null ? "" : Regex.Escape(left);
			right = right == null ? "" : Regex.Escape(right);

			return $@"({left}{VECTOR_PATTERN}{right})";
		}

		public Vector(double x, double y, double z = 0d)
			: this()
		{
			X = x;
			Y = y;
			Z = z;
		}

		public double X { get; }
		public double Y { get; }
		public double Z { get; }

		public double Magnitude
		{
			get
			{
				double raw = Math.Sqrt((X*X) + (Y*Y) + (Z*Z));
				return Math.Round(raw, 4);
			}
		}

		#region IEquatable<Vector> Members

		public bool Equals(Vector obj)
		{
			return 
				Units.Equal(obj.X, X) 
				&& Units.Equal(obj.Y, Y) 
				&& Units.Equal(obj.Z, Z);
		}

		#endregion

		#region IFormattable Members

		[Pure]
		public string ToString(string format, IFormatProvider? fp = null)
		{
			if (String.IsNullOrEmpty(format))
				format = "G";

			// If G format specifier is passed, display like this: (x, y).
			if (format.ToLower() == "g")
				return $"({X:n4}, {Y:n4}, {Z:n4})";

			// If N format specifier is passed, display like this: (x, y).
			if (format.ToLower() == "n")
				return $"{X:n4} {Y:n4} {Z:n4}";

			// For "x" formatting, return just the x value as a string
			if (format.ToLower() == "x")
				return X.ToString("n4");

			// For "y" formatting, return just the y value as a string
			if (format.ToLower() == "y")
				return Y.ToString("n4");

			// For "y" formatting, return just the y value as a string
			if (format.ToLower() == "z")
				return Z.ToString("n4");

			// For "l" formatting, return just the magnitude as a string
			if (format.ToLower() == "l")
				return Magnitude.ToString("n4");

			// For any unrecognized format, throw an exception.
			throw new FormatException($"Invalid format string: '{format}'.");
		}

		#endregion

		public override bool Equals(object obj)
		{
			if (obj?.GetType() != typeof (Vector))
				return false;

			return Equals((Vector) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int result = X.GetHashCode();
				result = (result*397) ^ Y.GetHashCode();
				result = (result*397) ^ Z.GetHashCode();
				return result;
			}
		}

		public static Vector Parse(string value)
		{
			Match m = s_parseVector.Match(value.Trim());
			if (m.Success)
			{
				double x = double.Parse(m.Groups["x"].Value);
				double y = double.Parse(m.Groups["y"].Value);
				double z = m.Groups["z"].Success ? double.Parse(m.Groups["z"].Value) : 0d;
				return new Vector(x, y, z);
			}

			throw new FormatException("Unable to parse vector: " + value);
		}

		public override string ToString()
		{
			return ToString("g");
		}

		public static bool operator ==(Vector left, Vector right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Vector left, Vector right)
		{
			return !left.Equals(right);
		}

		[Pure]
		public Vector ToUnitVector()
		{
			var magnitude = Magnitude;

			if (Units.IsZero(magnitude))
				throw new ArgumentException("Cannot create a unit vector from the Zero vector");

			if (Units.Equal(magnitude, 1d))
				return new Vector(X, Y, Z);

			return new Vector(X/Magnitude, Y/magnitude, Z/magnitude);
		}

		[Pure]
		public Vector Scale(double scale)
		{
			return new Vector(X*scale, Y*scale, Z*scale);
		}

		[Pure]
		public double DotProduct(Vector b)
		{
			return (X*b.X) + (Y*b.Y) + (Z*b.Z);
		}

		[Pure]
		public static double Angle(Vector a, Vector b)
		{
			a = a.ToUnitVector();
			b = b.ToUnitVector();

			double dotProduct = Math.Round((a.X*b.X) + (a.Y*b.Y) + (a.Z*b.Z), 4);
			return Math.Acos(dotProduct);
		}

		[Pure]
		public Vector RotateZ(double radians)
		{
			double x = Math.Cos(radians)*X - Math.Sin(radians)*Y;
			double y = Math.Sin(radians)*X + Math.Cos(radians)*Y;

			return new Vector(x, y);
		}

		[Pure]
		public static Vector operator +(Vector a, Vector b)
		{
			return new Vector(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
		}

		[Pure]
		public static Vector operator -(Vector a, Vector b)
		{
			return new Vector(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
		}

		[Pure]
		public static Vector operator *(Vector a, Vector b)
		{
			return new Vector((a.Y*b.Z) - (b.Y*a.Z),(b.X*a.Z) - (b.Z*a.X), (a.X*b.Y) - (b.X*a.Y));
		}

		[Pure]
		public static Vector operator *(Vector a, double scale)
		{
			return a.Scale(scale);
		}

		[Pure]
		public static double Distance(Vector a, Vector b)
		{
			return (a - b).Magnitude;
		}

		[Pure]
		public static bool Intersects(Vector p0, double r0, Vector p1, double r1)
		{
			/*
				a = (r02 - r12 + d2 ) / (2 d)
				h2 = r02 - a2
				P2 = P0 + a ( P1 - P0 ) / d
				x3 = x2 +- h ( y1 - y0 ) / d
				y3 = y2 -+ h ( x1 - x0 ) / d
			*/

			var tmp = (p1 - p0);
			var d = tmp.Magnitude;

			if (d > (r0 + r1))
			{
				return false;
			}

			return true;
		}
	}
}