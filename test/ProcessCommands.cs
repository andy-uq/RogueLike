using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LanguageExt;
using static LanguageExt.Prelude;
using Moq;
using NUnit.Framework;

namespace RogueLike.Tests
{
	public class ProcessCommands
	{
		[TestCase(1, 0, true)]
		[TestCase(-1, 0, false)]
		[TestCase(0, -1, false)]
		[TestCase(1, 1, true)]
		public async Task Move(int x, int y, bool success)
		{
			var origin = new Point(1, 1);
			var player = new Player(position: origin);
			var actionQueue = new Queue<IPlayerAction>();

			var gameEngine = new Mock<IGameEngine>(MockBehavior.Strict);
			gameEngine.SetupGet(_ => _.Map).Returns(Data.Maps.Small);
			gameEngine.SetupGet(_ => _.Player).Returns(player);
			gameEngine.Setup(_ => _.EnqueueActionAsync(It.IsAny<IPlayerAction>()))
				.Returns<IPlayerAction>(action => Task.FromResult(Some(action)))
				.Callback<IPlayerAction>(action => actionQueue.Enqueue(action));
			
			var cp = new CommandProcessor(gameEngine.Object);
			var move = await cp.Move(x, y);

			if (success)
			{
				move.Match(
					m => m.Should().BeOfType<MovePlayerAction>(),
					() => { throw new InvalidOperationException(); });
			}
			else
			{
				move.Some(m => m.Should().BeNull());
			}
		}

		[Test]
		public async Task OpenDoor()
		{
			var player = new Player(position: new Point(1, 1));
			var actionQueue = new Queue<IPlayerAction>();
			var map = Data.Maps.HasDoor();

			var gameEngine = new Mock<IGameEngine>();
			gameEngine.SetupGet(_ => _.Map).Returns(map);
			gameEngine.SetupGet(_ => _.Player).Returns(player);
			gameEngine.Setup(_ => _.EnqueueActionAsync(It.IsAny<IPlayerAction>()))
				.Returns<IPlayerAction>(action => Task.FromResult(Some(action)))
				.Callback<IPlayerAction>(action => actionQueue.Enqueue(action));

			var cp = new CommandProcessor(gameEngine.Object);
			var move = await cp.Move(1, 0);

			move.Match(
				m => m.Should().BeOfType<OpenDoorAction>(),
				() => { throw new InvalidOperationException(); });
		}

		[Test]
		public void Quit()
		{
			var gameEngine = new Mock<IGameEngine>();
			var cp = new CommandProcessor(gameEngine.Object);

			cp.Add("quit");

			gameEngine.Verify(x => x.EndGame());
		}

		[Test]
		public void UnknownCommand()
		{
			string status = null;

			var gameEngine = new Mock<IGameEngine>();
			gameEngine.Setup(x => x.SetStatus(Moq.It.IsAny<string>(), It.IsAny<object[]>()))
				.Callback<string, object[]>((format, args) => status = string.Format(format, args));

			var cp = new CommandProcessor(gameEngine.Object);

			cp.Add("bleh");

			status.Should().Be("Unknown command: bleh");
		}
	}
}