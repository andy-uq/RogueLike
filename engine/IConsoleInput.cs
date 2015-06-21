using System;

namespace RogueLike
{
	public interface IConsoleInput
	{
		ConsoleKeyInfo ReadKey();
	}
}