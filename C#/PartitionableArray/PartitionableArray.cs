using System;
using System.Diagnostics.Contracts;

namespace PartitionableArray
{
	public class PartitionableArray<T>
	{
		#region Internal

		private struct Element
		{
			public T value;

			// Keep track of this element in the list of interesting elements
			public bool interesting;
			public int prev;
			public int next;
		}

		private Element[] arr;

		private PartitionFn test;

		private int sentinel
		{
			get { return arr.Length - 1; }
		}

		[ContractInvariantMethod]
		private void invariant()
		{
			Contract.Invariant(test != null);
			Contract.Invariant(arr != null);
			Contract.Invariant(arr.Length >= 1);
			Contract.Invariant(sentinel == arr.Length - 1);
		}

		private void consider(int index)
		{
			Contract.Requires<IndexOutOfRangeException>(
				0 <= index && index < Length,
				"The index must be within the bounds of the array.");
			Contract.Assume(index < arr.Length);

			if (test(arr[index].value)) // Is interesting
			{
				if (!arr[index].interesting) // Is not in list
				{
					// Add to list
					count++;
					arr[index].interesting = true;

					arr[index].next = sentinel;
					arr[index].prev = arr[sentinel].prev;
					arr[arr[sentinel].prev].next = index;
					arr[sentinel].prev = index;
				}
				// Else already in list, no need to add
			}
			else // Is not interesting
			{
				if (arr[index].interesting) // Is in list
				{
					// Remove from list
					count--;
					arr[index].interesting = false;

					arr[arr[index].prev].next = arr[index].next;
					arr[arr[index].next].prev = arr[index].prev;
				}
				// Else is not already in list, no need to remove
			}
		}

		#endregion

		#region Public

		/// <summary>
		/// Given a value, decide if it is interesting.
		/// </summary>
		[Pure]
		public delegate bool PartitionFn(T val);

		/// <summary>
		/// Gets maximum the number of elements in the array.
		/// </summary>
		/// <value>
		/// The maximum number of elements in the array.
		/// </value>
		public int Length
		{
			get { return arr.Length - 1; }
		}

		int count = 0;
		/// <summary>
		/// Gets the number of interesting elements in the array.
		/// </summary>
		/// <value>
		/// The number of interesting elements in the array.
		/// </value>
		public int Count
		{
			get { return count; }
		}

		/// <summary>
		/// Initializes a new instance of the PartitionableArray class.
		/// </summary>
		/// <param name='fn'>
		/// A partition function to use to determine if the elements of the
		/// array are interesting.
		/// </param>
		/// <param name='length'>
		/// The fixed maximum number of elements in the array.
		/// </param>
		public PartitionableArray(PartitionFn fn, int length)
		{
			Contract.Requires<ArgumentOutOfRangeException>(0 <= length,
				"Length must be a non-negative integer.");
			Contract.Ensures(test == fn);
			Contract.Ensures(Length == length);

			test = fn;
			arr = new Element[length + 1]; // One extra element for sentinel
			arr[sentinel].prev = arr[sentinel].next = sentinel; // Set up the sentinel
			for (int i = 0; i < Length; i++)
			{
				consider(i);
			}
		}

		/// <summary>
		/// Gets the index of an interesting element.
		/// <para>No guarantees are made as to which element's index will be
		/// returned, just that the index will be that of an interesting
		/// element.</para>
		/// </summary>
		/// <returns>
		/// The index of an interesting element.
		/// </returns>
		public int AnyInteresting()
		{
			Contract.Requires<NullReferenceException>(Count > 0,
				"The array must contain interesting elements.");
			Contract.Ensures(0 <= Contract.Result<int>()
				&& Contract.Result<int>() < Length,
				"The returned index must be within the bounds of the array.");
			Contract.Ensures(test(this[Contract.Result<int>()]),
				"The returned index must be that of an interesting element.");

			return arr[sentinel].next;
		}

		/// <summary>
		/// Gets or sets the value at a certain index in the array.
		/// </summary>
		/// <param name='index'>
		/// The index in the array.
		/// </param>
		public T this[int index]
		{
			get
			{
				Contract.Requires<IndexOutOfRangeException>(
					0 <= index && index < Length,
					"Index must be within array bounds.");
				Contract.Assume(index < this.arr.Length);

				return arr[index].value;
			}
			set
			{
				Contract.Requires<IndexOutOfRangeException>(
					0 <= index && index < Length,
					"Index must be within array bounds.");
				Contract.Assume(index < arr.Length);

				arr[index].value = value;

				consider(index); // Update list of interesting elements
			}
		}

		#endregion
	}
}

