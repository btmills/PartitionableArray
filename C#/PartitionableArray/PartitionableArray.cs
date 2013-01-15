using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PartitionableArray
{
	public class PartitionableArray<T>
	{
		private struct Element
		{
			public T value;
			public LinkedListNode<int> interesting;
		}

		public delegate bool EvalFn(T val);
		private EvalFn test;
		
		private Element[] arr;

		private LinkedList<int> interesting = new LinkedList<int>();
		
		public T this[int index] {
			get { return arr[index].value; }
			set {
				arr[index].value = value;

				if(test(value)) { // Is interesting
					if(!(arr[index].interesting != null)) { // Is not in list
						// Add to interesting list
						interesting.AddFirst(index);
						arr[index].interesting = interesting.First;
					}
				} else { // Is not interesting
					if(arr[index].interesting != null) { // Is in list
						// Remove from interesting list
						interesting.Remove(arr[index].interesting);
						arr[index].interesting = null;
					}
				}
			}
		}
		
		public PartitionableArray (EvalFn fn, int capacity)
		{
			test = fn;
			arr = new Element[capacity];
		}
		
		public bool IsInteresting()
		{
			return interesting.Count > 0;
		}

		public int InterestingElement()
		{
			return interesting.First.Value;
		}
	}
}

