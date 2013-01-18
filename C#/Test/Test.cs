using System;
using PartitionableArray;

namespace Test
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			PartitionableArray<int> arr = new PartitionableArray<int>((val) => (/*val != 0 && */val % 2 == 0), 20);
			Console.WriteLine("Array contains {0} interesting elements.", arr.Count);

			for (int i = 1; i < arr.Length; i++)
			{
				arr[i] = i;
				Console.WriteLine("Added {0}.", arr[i]);
				Console.WriteLine("Array contains {0} interesting elements.", arr.Count);
			}

			Console.WriteLine("Array contents:");
			for (int i = 0; i < arr.Length; i++)
			{
				Console.WriteLine("{0}: {1}", i, arr[i]);
			}

			while (arr.Count > 0)
			{
				int index = arr.AnyInteresting();
				Console.WriteLine("{0} is interesting. Setting equal to -1...", arr[index]);
				arr[index] = -1;
				Console.WriteLine("Array contains {0} interesting elements.", arr.Count);
			}
			Console.WriteLine("Array is {0}interesting.", arr.Count > 0 ? "" : "not ");
			Console.WriteLine("Press any key to continue...");
			Console.ReadKey();
		}
	}
}
