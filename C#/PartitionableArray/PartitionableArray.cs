using System;
//using System.Collections.Generic;
//using System.Linq.Expressions;

namespace PartitionableArray
{
	public class PartitionableArray<T>
	{
		/// <summary>
		/// Representation of an element in the array.
		/// </summary>			
		private class Element
		{
			public T value;
			public readonly int index; // Only way to do constant-time reverse lookup

			// Keep track of this element in the list of interesting elements
			public Element prev;
			public Element next;

			// Make sure the index gets set
			public Element(int i) { index = i; }
		}

		/// <summary>
		/// Internal array representation.
		/// </summary>
		private Element[] arr;

		/// <summary>
		/// List of interesting elements.
		/// </summary>
		private Element interesting = null;

		/// <summary>
		/// Given a value, decide if it is interesting.
		/// </summary>
		public delegate bool PartitionFn(T val);
		/// <summary>
		/// Stored partition function.
		/// </summary>
		private PartitionFn test;

		/// <summary>
		/// Initializes a new instance of the PartitionableArray class.
		/// </summary>
		/// <param name='fn'>
		/// A partition function to use to determine if the elements of the
		/// array are interesting.
		/// </param>
		/// <param name='capacity'>
		/// The fixed maximum capacity of the array.
		/// </param>
		public PartitionableArray (PartitionFn fn, int capacity)
		{
			test = fn;
			arr = new Element[capacity];
			for (int i = 0; i < capacity; i++) {
				arr [i] = new Element (i);
			}
		}

		/// <summary>
		/// Determines whether one or more interesting values exist.
		/// </summary>
		/// <returns>
		/// <c>true</c> if one or more values are interesting; otherwise, <c>false</c>.
		/// </returns>
		public bool IsInteresting() { return interesting != null; }

		/// <summary>
		/// Gets the index of an interesting element.
		/// <para>No guarantees are made as to which element's index will be
		/// returned, just that the index will be that of an interesting
		/// element.</para>
		/// </summary>
		/// <returns>
		/// The index of an interesting element.
		/// </returns>
		public int InterestingElement() { return interesting.index; }

		/// <summary>
		/// Gets or sets the value at a certain index in the array.
		/// </summary>
		/// <param name='index'>
		/// The index in the array.
		/// </param>
		public T this[int index] {
			get { return arr[index].value; }
			set {
				arr[index].value = value;

				// Update list of interesting elements
				if(test(value)) { // Is interesting
					if(!(arr[index].prev != null
					     || arr[index].next != null)) { // Is not already in list
						// Add to interesting list
						if(interesting != null) {
							interesting.prev = arr[index];
							arr[index].next = interesting;
						}
						interesting = arr[index];
					}
				} else { // Is not interesting
					if(interesting != null
					   && interesting.index == index) { // Is list head
						// Remove list head
						interesting = arr[index].next;
						arr[index].next = null;
						if(interesting != null) { // List is not empty
							interesting.prev = null;
						}
					} else if(arr[index].prev != null) { // Elsewhere in list
						// Remove from list
						arr[index].prev.next = arr[index].next;
						if(arr[index].next != null) { // Is not tail
							arr[index].next.prev = arr[index].prev;
							arr[index].next = null;
						}
						arr[index].prev = null;
					}
					// Else is not already in list, no need to remove
				}
			}
		}
	}
}

