namespace RogueLike
{
	public class GameActionContext
	{
		public GameActionContext(IGameEngine gameEngine)
		{
			Map = gameEngine.Map;
			Player = gameEngine.Player;
		}

		public Map Map { get; set; }
		public Player Player { get; set; }
	}
}