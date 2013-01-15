The C# implementation uses the PartitionableArray class.

## Usage

### Constructor

`PartitionableArray<int> arr = new PartitionableArray<int>((val) => (val != 0 && val % 2 == 0), 20)` creates a partitionable array that selects even numbers and has a capacity of 20 elements.

### Accessor

The partitionable array's elements are accessed just like elements of a regular array: `arr[4] = arr[5]` copies the value at index 5 to index 4.

### IsInteresting

`bool PartitionableArray.IsInteresting()` returns true if there are one or more interesting elements in the array and false otherwise.

### InterestingElement

`int PartitionableArray.InterestingElement()` returns the index of an interesting element, provided one exists (which can be checked using `IsInteresting()` above). There are no gaurantees about which elements index is returned other than that it is interesting.