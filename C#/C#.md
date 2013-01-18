The C# implementation uses the PartitionableArray class.

## Usage

### Constructor

`PartitionableArray<int> arr = new PartitionableArray<int>((val) => (val % 2 == 0), 20)` creates a partitionable array that selects even numbers (plus 0) and has a capacity of 20 elements.

### Length

`int PartitionableArray.Length` gets the number of elements in the array.

### Count

`int PartitionableArray.Count` gets the number of interesting elements in the array.

### Accessor

The partitionable array's elements are accessed just like elements of a regular array: `arr[4] = arr[5]` copies the value at index 5 to index 4.

### AnyInteresting

`int PartitionableArray.AnyInteresting()` returns the index of an interesting element, provided one exists (which can be checked using the `Count` property described above). There are no gaurantees about which element's index is returned other than that it is interesting.