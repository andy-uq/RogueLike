using System.Linq;

namespace RogueLike
{
	public class Mobile
	{
		public Mobile(int id, Point position)
		{
			Id = id;
			Position = position;
		}

		public int Id { get; }
		public Point Home { get; set; }
		public Point Position { get; set; }
		public Point? Target { get; set; }
		public char Glyph { get; set; }

		public void Act(GameActionContext context)
		{
			if (context.Map.CanSee(Position, context.Player.Position))
			{
				Target = context.Player.Position;
			}

			if (Target == null)
			{
				return;
			}

			var newPosition = MoveTowards(Target.Value);

			if (newPosition == context.Player.Position)
			{
				AttackPlayer(context);
				return;
			}

			MoveTo(context, newPosition);

			if (Position == Target)
			{
				Target = (Position == Home)
					? (Point?) null
					: Home;
			}
		}

		private void AttackPlayer(GameActionContext context)
		{
		}

		private void MoveTo(GameActionContext context, Point newPosition)
		{
			if (context.Map.IsOccupied(newPosition) || context.Map.Mobiles.Select(m => m.Position).Contains(newPosition))
				return;

			Position = newPosition;
		}

		private Point MoveTowards(Point target)
		{
			int x = Position.X, y = Position.Y;
			var p = new Point(target.X - x, target.Y - y);

			if (p.X > 0) x++;
			if (p.X < 0) x--;
			if (p.Y > 0) y++;
			if (p.Y < 0) y--;

			return new Point(x, y);
		}

		public void Load(MobileState state)
		{
			if (Id != state.Id)
				return;

			Position = new Point(state.Position.X, state.Position.Y);
		}

		public MobileState Save()
		{
			return new MobileState
			{
				Id = Id,
				Position = new PointXY(Position.X, Position.Y)
			};
		}
	}
}