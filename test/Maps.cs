using carl;
using FluentAssertions;
using NUnit.Framework;

namespace test
{
	[TestFixture]
	public class Maps
	{
		[TestCase(-1, -1, true)]
		[TestCase(-1, 1, true)]
		[TestCase(1, -1, true)]
		[TestCase(0, 0, true)]
		[TestCase(10, 10, true)]
		[TestCase(1, 1, false)]
		public void OutOfBounds(int x, int y, bool outOfBounds)
		{
			var map = Data.Maps.Small();
			map.IsOccupied(x, y).Should().Be(outOfBounds);
		}
	}
}