using System;
using System.Runtime.InteropServices;

namespace carl
{
	internal static class Win32Console
	{
		public const int STD_OUTPUT_HANDLE = -0xb;
		public const uint GENERIC_READ = (0x80000000);
		public const uint GENERIC_WRITE = (0x40000000);
		public const uint GENERIC_EXECUTE = (0x20000000);
		public const uint GENERIC_ALL = (0x10000000);

		[DllImport("Kernel32.dll")]
		public static extern IntPtr GetStdHandle(int handle);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr CreateConsoleScreenBuffer(
			uint dwDesiredAccess,
			uint dwShareMode,
			IntPtr lpSecurityAttributes,
			uint dwFlags,
			IntPtr lpScreenBufferData
			);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool SetConsoleActiveScreenBuffer(
			IntPtr hConsoleOutput
			);

		[DllImport("kernel32.dll", EntryPoint = "WriteConsoleOutputW", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern int WriteConsoleOutput(
			IntPtr hConsoleOutput,
			/* This pointer is treated as the origin of a two-dimensional array of CHAR_INFO structures
				  whose size is specified by the dwBufferSize parameter.*/
			[MarshalAs(UnmanagedType.LPArray), In] CharInfo[,] lpBuffer,
			Coord dwBufferSize,
			Coord dwBufferCoord,
			ref SmallRect lpWriteRegion);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool FillConsoleOutputAttribute(
			IntPtr hConsoleOutput,
			ushort wAttribute,
			uint nLength,
			Coord dwWriteCoord,
			out uint lpNumberOfAttrsWritten
			);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool FillConsoleOutputCharacter(
			IntPtr hConsoleOutput,
			char cCharacter,
			uint nLength,
			Coord dwWriteCoord,
			out uint lpNumberOfCharsWritten
			);

		[DllImport("kernel32.dll")]
		public static extern bool SetConsoleCursorPosition(IntPtr hConsoleOutput,
			Coord dwCursorPosition);

		[StructLayout(LayoutKind.Explicit)]
		public struct CharInfo
		{
			[FieldOffset(0)] public char UnicodeChar;
			[FieldOffset(0)] public char AsciiChar;

			[FieldOffset(2)] //2 bytes seems to work properly
			public UInt16 Attributes;
		}

		//COORD struct 
		[StructLayout(LayoutKind.Sequential)]
		public struct Coord
		{
			public Coord(int x, int y)
			{
				X = (short) x;
				Y = (short) y;
			}

			public short X;
			public short Y;
		}

		//SMALL_RECT struct
		[StructLayout(LayoutKind.Sequential)]
		public struct SmallRect
		{
			public SmallRect(int left, int top, int right, int bottom)
			{
				Left = (short) left;
				Top = (short) top;
				Right = (short) right;
				Bottom = (short) bottom;
			}

			public short Left;
			public short Top;
			public short Right;
			public short Bottom;
		}
	}
}