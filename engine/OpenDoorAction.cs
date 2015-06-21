namespace RogueLike
{
	public class OpenDoorAction : IPlayerAction
	{
		private readonly Point _position;

		public OpenDoorAction(Point position)
		{
			_position = position;
		}

		public void Act(GameActionContext context)
		{
			context.Map.OpenDoor(_position);
		}
	}
}