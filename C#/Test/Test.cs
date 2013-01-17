using System;
using PartitionableArray;

namespace Test
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			PartitionableArray<int> arr = new PartitionableArray<int> ((val) => (val != 0 && val % 2 == 0), 20);

			for (int i = 0; i < arr.Length; i++) {
				arr [i] = i;
				Console.WriteLine ("Added {0}.", arr [i]);
				if (arr.IsInteresting ()) {
					Console.WriteLine ("{0} is interesting.", arr [arr.AnyInteresting()]);
				}
			}

			Console.WriteLine("Array contains {0} interesting elements.", arr.Count);
			Console.WriteLine ("Array contents:");
			for (int i = 0; i < arr.Length; i++) {
				Console.WriteLine ("{0}: {1}", i, arr [i]);
			}

			while (arr.IsInteresting()) {
				int index = arr.AnyInteresting();
				Console.WriteLine ("{0} is interesting. Setting equal to 0...", arr [index]);
				arr [index] = 0;
				Console.WriteLine("Array contains {0} interesting elements.", arr.Count);
			}
			Console.WriteLine("Array is {0}interesting.", arr.IsInteresting() ? "" : "not ");
		}
	}
}
