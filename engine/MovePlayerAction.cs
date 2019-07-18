namespace RogueLike
{
	public class MovePlayerAction : IPlayerAction
	{
		private readonly Point _position;

		public MovePlayerAction(Point position)
		{
			_position = position;
		}

		public void Act(GameActionContext context)
		{
			context.Player.Position = _position;
		}
	}
}