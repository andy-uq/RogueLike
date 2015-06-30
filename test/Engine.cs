using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace RogueLike.Tests
{
	class Engine
	{
		[Test]
		public void StartEngine()
		{
			MakeEngine();
		}

		[Test]
		public void SetStatusLine()
		{
			var engine = MakeEngine();
			engine.SetStatus("Hello world");
			engine.StatusLine.Should().Be("Hello world");
			engine.StatusTtl.Should().BeGreaterThan(0);
		}

		[Test]
		public void StatusLineClears()
		{
			var engine = MakeEngine();
			engine.SetStatus("Hello world");

			var ttl = engine.StatusTtl;
			while (ttl > 0)
			{
				var status = engine.GetStatusLine();
				status.IsNone.Should().BeFalse();
				status.IfSome(s => s.Should().Be("Hello world"));
				ttl--;
			}

			engine.GetStatusLine().IsNone.Should().BeTrue();
			engine.StatusTtl.Should().Be(0);
		}

		[Test]
		public async Task QueueNextAction()
		{
			var engine = MakeEngine();
			var movePlayerAction = new MovePlayerAction(new Point(1, 1));
			await engine.EnqueueActionAsync(movePlayerAction);
			var task = await engine.TakeNextActionAsync();
			task.Should().Be(movePlayerAction);
		}

		[Test]
		public async Task EndGameEndsQueue()
		{
			var engine = MakeEngine();
			var ended = false;
			var t = Task.Run(async () =>
			{
				while (engine.IsActive)
				{
					await engine.TakeNextActionAsync();
				}

				ended = true;
			});


			engine.EndGame();
			await t;

			ended.Should().BeTrue();
		}

		private static GameEngine MakeEngine()
		{
			return new GameEngine();
		}
	}
}
