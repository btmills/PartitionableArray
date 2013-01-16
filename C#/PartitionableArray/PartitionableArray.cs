using System;
//using System.Collections.Generic;
//using System.Linq.Expressions;

namespace PartitionableArray
{
	public class PartitionableArray<T>
	{
		private class Element
		{
			public T value;
			public readonly int index;
			public Element prev;
			public Element next;

			public Element(int i) { index = i; }
		}

		public delegate bool EvalFn(T val);
		private EvalFn test;

		private Element[] arr;

		private Element interesting = null;
		
		public T this[int index] {
			get { return arr[index].value; }
			set {
				arr[index].value = value;

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
					if(arr[index].prev != null
					   || arr[index].next != null
					   || (interesting != null && interesting.index == index)) { // Is in list
						// Remove from interesting list
						if(interesting.index == index) { // Is head
							interesting = arr[index].next;
							arr[index].next = null;
							if(interesting != null) {
								interesting.prev = null;
							}
						} else { // Is not head
							arr[index].prev.next = arr[index].next;
							if(arr[index].next != null) { // Is not tail
								arr[index].next.prev = arr[index].prev;
								arr[index].next = null;
							}
							arr[index].prev = null;
						}
					}
				}
			}
		}
		
		public PartitionableArray (EvalFn fn, int capacity)
		{
			test = fn;
			arr = new Element[capacity];
			for (int i = 0; i < capacity; i++) {
				arr [i] = new Element (i);
			}
		}
		
		public bool IsInteresting()
		{
			return interesting != null;
		}

		public int InterestingElement()
		{
			return interesting.index;
		}
	}
}

