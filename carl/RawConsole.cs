using System;
using static carl.Win32Console;

namespace carl
{
	public class RawConsole
	{
		private readonly IntPtr _primaryHandle;
		private IntPtr _active, _draw;
		private Coord _cursorPosition;

		private ConsoleColor _foreColour;
		private ConsoleColor _backColour;

		public RawConsole()
		{
			_primaryHandle = GetStdHandle(STD_OUTPUT_HANDLE);
			_active = CreateConsoleScreenBuffer(GENERIC_READ | GENERIC_WRITE, 0, IntPtr.Zero, 1, IntPtr.Zero);
			_draw = CreateConsoleScreenBuffer(GENERIC_READ | GENERIC_WRITE, 0, IntPtr.Zero, 1, IntPtr.Zero);

			_foreColour = ConsoleColor.Gray;
			_backColour = ConsoleColor.Black;

			SetCursorPosition(2, 25);
		}

		public void SetCursorPosition(int column, int row)
		{
			if (_cursorPosition.X == column - 1 && _cursorPosition.Y == row - 1)
				return;

			_cursorPosition = new Coord(column - 1, row - 1);
			SetConsoleCursorPosition(_draw, _cursorPosition);
			SetConsoleCursorPosition(_active, _cursorPosition);
		}

		public void SwapBuffers()
		{
			var temp = _draw;
			_draw = _active;
			_active = temp;
			SetConsoleActiveScreenBuffer(_active);

			Clear();
		}

		private void Clear()
		{
			uint count;
			FillConsoleOutputCharacter(_draw, ' ', 80*24, new Coord(), out count);
			FillConsoleOutputAttribute(_draw, ToColourAttribute(ConsoleColor.Gray, ConsoleColor.Black), 80*24, new Coord(), out count);
		}

		public void SetColour(ConsoleColor colour, ConsoleColor? backColor = null)
		{
			_foreColour = colour;
			if (backColor == null)
				return;

			_backColour = backColor.Value;
		}

		public void Write(int column, int row, string text, ConsoleColor? color = null, ConsoleColor? backColor = null)
		{
			if (string.IsNullOrEmpty(text))
				return;

			var x = (short) (column - 1);
			var y = (short) (row - 1);
			var length = (short) text.Length;

			var buffer = new CharInfo[text.Length, 1];
			for (var i = 0; i < text.Length; i++)
			{
				buffer[i, 0].UnicodeChar = text[i];
				buffer[i, 0].Attributes = ToColourAttribute(color, backColor);
			}

			var dwBufferCoord = new Coord {X = 0, Y = 0};
			var dwBufferSize = new Coord {X = length, Y = 1};
			var rect = new SmallRect() {Left = x, Right = (short) (x + length), Top = y, Bottom = (short) (y + 1)};

			WriteConsoleOutput(_draw, buffer, dwBufferSize, dwBufferCoord, ref rect);
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

			SetConsoleActiveScreenBuffer(_primaryHandle);
		}
	}
}