using System;

namespace RogueLike
{
	public interface IConsole
	{
		void SetCursorPosition(Point point);
		void ClearCursorPosition();
		void SwapBuffers();
		void SetColour(ConsoleColor colour, ConsoleColor? backColor = null);
		void Write(Point point, string text, ConsoleColor? color = null, ConsoleColor? backColor = null);
		void Restore();
	}
}