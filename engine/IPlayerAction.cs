namespace RogueLike
{
	public interface IPlayerAction
	{
		void Act(GameActionContext context);
	}

	public interface IMonsterAction
	{
		void Act(GameActionContext context);
	}
}