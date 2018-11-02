using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Interpreter
{
	internal static class Program
	{
		private const string Forward = "hold your horses now";
		private const string Back = "sleep until the sun goes down";
		private const string Add = "through the woods we ran";
		private const string Sub = "deep into the mountain sound";
		private const string Print = "don't listen to a word i say";
		private const string Input = "the screams sound all the same";
		private const string BeginLoop = "though the truth may vary";
		private const string EndLoop = "this ship will carry";

		private static readonly string RegexCommand =
			$"{Forward}|{Back}|{Add}|{Sub}|{Print}|{Input}|{BeginLoop}|{EndLoop}";

		private static string[] Parse(string code)
		{
			return Regex.Matches(code, RegexCommand).Select(m => m.ToString()).ToArray();
		}

		private static void Interpret(string code)
		{
			var mem = new int[65535];
			var dataPointer = 0;
			var scope = 0;
			var tokens = Parse(code);
			for (var i = 0; i < tokens.Length; i++)
			{
				switch (tokens[i])
				{
					case Forward:
						dataPointer = dataPointer < mem.Length - 1 ? dataPointer + 1 : 0;
						break;
					case Back:
						dataPointer = dataPointer > 0 ? dataPointer - 1 : mem.Length - 1;
						break;
					case Add:
						mem[dataPointer]++;
						break;
					case Sub:
						mem[dataPointer]--;
						break;
					case Print:
						Console.Write((char) mem[dataPointer]);
						break;
					case Input:
						mem[dataPointer] = Console.Read();
						break;
					case BeginLoop:
						if (mem[dataPointer] == 0)
						{
							i++;
							while (scope > 0 || !tokens[i].Equals(EndLoop))
							{
								if (tokens[i].Equals(BeginLoop)) scope++;
								if (tokens[i].Equals(EndLoop)) scope--;
								i++;
							}
						}

						break;
					case EndLoop:
						if (mem[dataPointer] != 0)
						{
							i--;
							while (scope > 0 || !tokens[i].Equals(BeginLoop))
							{
								if (tokens[i].Equals(EndLoop)) scope++;
								if (tokens[i].Equals(BeginLoop)) scope--;
								i--;
							}

							i--;
						}

						break;
				}
			}
		}

		private static void Main()
		{
			var code = File.ReadAllText("HelloWorld.omam");
			Interpret(code);
		}
	}
}