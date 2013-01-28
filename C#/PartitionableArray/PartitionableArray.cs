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
			Contract.Invariant(Length == arr.Length - 1);
			Contract.Invariant(0 <= count);
			Contract.Invariant(count <= Length);
			Contract.Invariant(count < arr.Length);
			Contract.Invariant(Count == count);
		}

		// Remove an interesting element from the list
		private void remove(int index)
		{
			Contract.Requires<IndexOutOfRangeException>(
				0 <= index && index < Length,
				"The index must be within the bounds of the array.");
			Contract.Requires(arr[index].interesting == true);
			Contract.Requires(test(arr[index].value) == true);
			Contract.Ensures(arr[index].interesting == false);
			Contract.Ensures(Contract.OldValue(Count) - 1 == Count);

			count--;
			arr[index].interesting = false;

			arr[arr[index].prev].next = arr[index].next;
			arr[arr[index].next].prev = arr[index].prev;
		}

		// Add an interesting element to the list
		private void add(int index)
		{
			Contract.Requires<IndexOutOfRangeException>(
				0 <= index && index < Length,
				"The index must be within the bounds of the array.");
			Contract.Requires(arr[index].interesting == false);
			Contract.Requires(test(arr[index].value) == true);
			Contract.Ensures(arr[index].interesting == true);
			Contract.Ensures(Contract.OldValue(Count) + 1 == Count);

			count++;
			arr[index].interesting = true;

			arr[index].next = sentinel;
			arr[index].prev = arr[sentinel].prev;
			arr[arr[sentinel].prev].next = index;
			arr[sentinel].prev = index;
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
			Contract.Ensures(
				// If default value is interesting, all elements are interesting
				(test(arr[sentinel].value) && Count == length)
				// Otherwise, no elements are interesting
				|| Count == 0);

			test = fn;
			arr = new Element[length + 1]; // One extra element for sentinel
			arr[sentinel].prev = arr[sentinel].next = sentinel; // Set up the sentinel
			for (int i = 0; i < Length; i++) // Check for interesting default values
				if (test(arr[i].value))
					add(i);
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
				Contract.Ensures(
					// If old and new values are interesting, Count is unchanged
					((test(Contract.OldValue(arr[index].value))
						== test(arr[index].value))
						&& Contract.OldValue(Count) == Count)
					// If old was uninteresting and new value is interesting, Count increments
					|| (!test(Contract.OldValue(arr[index].value))
						&& test(arr[index].value)
						&& Contract.OldValue(Count) + 1 == Count)
					// If old value was interesting and new value is uninteresting, Count decrements
					|| (test(Contract.OldValue(arr[index].value))
						&& !test(arr[index].value)
						&& Contract.OldValue(Count) - 1 == Count));
				Contract.Assume(index < arr.Length);

				if (test(arr[index].value)) remove(index);
				arr[index].value = value;
				if (test(arr[index].value)) add(index);
			}
		}

		#endregion
	}
}

