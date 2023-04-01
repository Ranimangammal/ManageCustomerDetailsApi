using System;
using System.Linq;

namespace CustomerDetails.Test
{
	public static class RandomStringGenerator
	{
		private static readonly Random Random = new Random();

		public static string RandomString(int length)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			return new string(Enumerable.Repeat(chars, length)
							.Select(s => s[Random.Next(s.Length)]).ToArray());
		}
	}
}
