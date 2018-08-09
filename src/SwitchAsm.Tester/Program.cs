using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using PlayerIOClient;
using PlayerIOClient.Fluent;
using SwitchAsm;
using TimedQuery;

namespace SwitchAsm.Tester {
	class Program {
		static void Main(string[] args) {
			var parser = new Parser();

			using (var fs = File.OpenRead("complicated.sasm"))
			using (var sr = new StreamReader(fs))
				while (!sr.EndOfStream)
					parser.ParseLine(sr.ReadLine());

			var modules = ModuleBuilder.CreateFrom(parser.GetCode());
			IEnumerable<IBlock> blocks = null;

			var email = Ask("What is your email address?");
			var password = Ask("What is your password?");
			var worldid = Ask("What is the World ID?");

			int worldWidth, worldHeight;

			var bot = PlayerIO.QuickConnect.SimpleConnect("everybody-edits-su9rn58o40itdbnw69plyw", email, password, null)
				.ApplyFluency() //make the client fluent

				.Multiplayer
					.CreateJoinRoom(worldid, "Everybodyedits237", false, null, null)
						.OnDisconnect((c, i, e) => {
							if (i) c.Reconnect().Send("init");
						})

						.On("init", (c, e) => {
							e.Get(18, out worldWidth)
							.Get(19, out worldHeight);

							blocks = modules.BuildWorld(worldWidth, worldHeight);

							c.Send("init2");
						})

						.On("init2", (c, e) => {
							var timedQuery = new QueryExecutioner<IBlock>();
							timedQuery.ProcessQueryItem += (i) => {
								var arguments = i.GetEEArgs(0, 0);
								c.Send("b", arguments);
								Thread.Sleep(10);
							};

							foreach (var i in blocks)
								timedQuery.AddQueryItem(i);
						})

						.Send("init");

			Console.ReadLine();
		}

		private static void TimedQuery_ProcessQueryItem(IBlock item) => throw new NotImplementedException();

		static string Ask(string question) {
			Console.WriteLine(question + " ");
			return Console.ReadLine();
		}
	}
}