namespace RogueLike
{
	public interface IGameEngine
	{
		void EndGame();
		void SetStatus(string format, params object[] args);
		Map Map { get; }
		Player Player { get; }
		IObjectLoader ObjectLoader { get; }
	}
}