using System;
//using System.Collections.Generic;
//using System.Linq.Expressions;

namespace PartitionableArray
{
	public class PartitionableArray<T>
	{
		#region Internal representation

		/// <summary>
		/// Representation of an element in the array.
		/// </summary>			
		private struct Element
		{
			public T value;

			// Keep track of this element in the list of interesting elements
			public bool interesting;
			public int prev;
			public int next;
		}

		/// <summary>
		/// Internal array representation.
		/// </summary>
		private Element[] arr;

		/// <summary>
		/// List of interesting elements.
		/// </summary>
		private int sentinel;
		
		/// <summary>
		/// Stored partition function.
		/// </summary>
		private PartitionFn test;

		#endregion

		#region Interface

		/// <summary>
		/// Given a value, decide if it is interesting.
		/// </summary>
		public delegate bool PartitionFn(T val);

		private int length = 0;
		/// <summary>
		/// Gets maximum the number of elements in the array.
		/// </summary>
		/// <value>
		/// The maximum number of elements in the array.
		/// </value>
		public int Length {
			get { return length; }
		}

		private int count = 0;
		/// <summary>
		/// Gets the number of interesting elements in the array.
		/// </summary>
		/// <value>
		/// The number of interesting elements in the array.
		/// </value>
		public int Count {
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
		public PartitionableArray (PartitionFn fn, int length)
		{
			this.test = fn;
			this.length = length;
			arr = new Element[length + 1]; // One extra element for sentinel
			arr[length].prev = arr[length].next = sentinel = length; // Set up the sentinel
		}

		/// <summary>
		/// Determines whether one or more interesting values exist.
		/// </summary>
		/// <returns>
		/// <c>true</c> if one or more values are interesting; otherwise, <c>false</c>.
		/// </returns>
		public bool IsInteresting() { return count > 0; }

		/// <summary>
		/// Gets the index of an interesting element.
		/// <para>No guarantees are made as to which element's index will be
		/// returned, just that the index will be that of an interesting
		/// element.</para>
		/// </summary>
		/// <returns>
		/// The index of an interesting element.
		/// </returns>
		public int AnyInteresting() { return arr[sentinel].next; }

		/// <summary>
		/// Gets or sets the value at a certain index in the array.
		/// </summary>
		/// <param name='index'>
		/// The index in the array.
		/// </param>
		public T this[int index] {
			get {
				if(!(0 <= index && index < length)) {
					throw new IndexOutOfRangeException();
				}

				return arr[index].value;
			}
			set {
				if(!(0 <= index && index < length)) {
					throw new IndexOutOfRangeException();
				}

				arr[index].value = value;

				// Update list of interesting elements
				if(test(value)) { // Is interesting
					if(!arr[index].interesting) { // Is not in list
						// Add to list
						count++;
						arr[index].interesting = true;

						arr[index].next = sentinel;
						arr[index].prev = arr[sentinel].prev;
						arr[arr[sentinel].prev].next = index;
						arr[sentinel].prev = index;
					}
					// Else already in list, no need to add
				} else { // Is not interesting
					if(arr[index].interesting) { // Is in list
						// Remove from list
						count--;
						arr[index].interesting = false;

						arr[arr[index].prev].next = arr[index].next;
						arr[arr[index].next].prev = arr[index].prev;
					}
					// Else is not already in list, no need to remove
				}
			}
		}

		#endregion
	}
}

