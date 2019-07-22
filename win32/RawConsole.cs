using System;

namespace RogueLike.Win32
{
	public class RawConsole : IConsole, IConsoleInput
	{
		private readonly IntPtr _primaryHandle;
		private IntPtr _active, _draw;
		private Win32Console.Coord _cursorPosition;

		private ConsoleColor _foreColour;
		private ConsoleColor _backColour;

		public RawConsole()
		{
			_primaryHandle = Win32Console.GetStdHandle(Win32Console.STD_OUTPUT_HANDLE);
			_active = Win32Console.CreateConsoleScreenBuffer(Win32Console.GENERIC_READ | Win32Console.GENERIC_WRITE, 0, IntPtr.Zero, 1, IntPtr.Zero);
			_draw = Win32Console.CreateConsoleScreenBuffer(Win32Console.GENERIC_READ | Win32Console.GENERIC_WRITE, 0, IntPtr.Zero, 1, IntPtr.Zero);

			_foreColour = ConsoleColor.Gray;
			_backColour = ConsoleColor.Black;

			if (_cursorPosition.X == 2 - 1 && _cursorPosition.Y == 25 - 1)
				return;

			_cursorPosition = new Win32Console.Coord(2 - 1, 25 - 1);

			Win32Console.SetConsoleCursorPosition(_draw, _cursorPosition);
			Win32Console.SetConsoleCursorPosition(_active, _cursorPosition);
		}

		public ConsoleKeyInfo ReadKey()
		{
			return Console.ReadKey(intercept: true);
		}

		public void SetCursorPosition(Point point)
		{
			var column = point.X;
			var row = point.Y;

			if (_cursorPosition.X == column - 1 && _cursorPosition.Y == row - 1)
				return;

			_cursorPosition = new Win32Console.Coord(column - 1, row - 1);
			Win32Console.SetConsoleCursorPosition(_draw, _cursorPosition);
			Win32Console.SetConsoleCursorPosition(_active, _cursorPosition);
		}

		public void SwapBuffers()
		{
			var temp = _draw;
			_draw = _active;
			_active = temp;
			
			Win32Console.GetConsoleCursorInfo(_active, out var cursorInfo);
			cursorInfo.Visible = false;
			Win32Console.SetConsoleCursorInfo(_active, ref cursorInfo);
			
			Win32Console.SetConsoleActiveScreenBuffer(_active);

			Clear();
		}

		private void Clear()
		{
			Win32Console.FillConsoleOutputCharacter(_draw, ' ', 80*24, new Win32Console.Coord(), out _);
			Win32Console.FillConsoleOutputAttribute(_draw, ToColourAttribute(ConsoleColor.Gray, ConsoleColor.Black), 80*24, new Win32Console.Coord(), out _);
		}

		public void SetColour(ConsoleColor colour, ConsoleColor? backColor = null)
		{
			_foreColour = colour;
			if (backColor == null)
				return;

			_backColour = backColor.Value;
		}

		public void Write(Point point, string text, ConsoleColor? color = null, ConsoleColor? backColor = null)
		{
			if (string.IsNullOrEmpty(text))
				return;

			var x = (short) (point.X - 1);
			var y = (short) (point.Y - 1);
			var length = (short) text.Length;

			var buffer = new Win32Console.CharInfo[text.Length, 1];
			for (var i = 0; i < text.Length; i++)
			{
				buffer[i, 0].UnicodeChar = text[i];
				buffer[i, 0].Attributes = ToColourAttribute(color, backColor);
			}

			var dwBufferCoord = new Win32Console.Coord {X = 0, Y = 0};
			var dwBufferSize = new Win32Console.Coord {X = length, Y = 1};
			var rect = new Win32Console.SmallRect() {Left = x, Right = (short) (x + length), Top = y, Bottom = (short) (y + 1)};

			Win32Console.WriteConsoleOutput(_draw, buffer, dwBufferSize, dwBufferCoord, ref rect);
		}

		private ushort ToColourAttribute(ConsoleColor? color, ConsoleColor? backColor)
		{
			return (ushort)
				(
					(byte) (backColor ?? _backColour) << 8 
					| (byte) (color ?? _foreColour)
				);
		}

		public void Restore()
		{
			if (_primaryHandle == IntPtr.Zero)
				return;

			Win32Console.SetConsoleActiveScreenBuffer(_primaryHandle);
		}
	}
}