# Partitionable Array

This is an implementation of an array whose elements can be classified by a function as either interesting or uninteresting. Element values can be set and retreived in constant time, and an interesting element can be retrieved in constant time.

## Requirements

- Every element in an array can be classified as interesting or uninteresting.
    - This classification is determined by a function that maps an element's value as input to the interesting or uninteresting Boolean output.
- In constant time, it should be possible to:
    - set the value of an element in the array.
    - get the value of an element in the array.
    - get the index of an interesting element in the array.
        - There is neither any guarantee nor any assumption that any specific index will be returned or that subsequent calls will or will not return different indices. The only guarantee is that the returned index is that of an interesting element, provided such an element exists.
- Setting element values should not require dynamic memory allocation.

## Implementations

Two implementations have been tried so far: C and C#. See the corresponding directories for more information.

## Design Tradeoffs

Had I not designed it this way intentionally, these would also be known as shortcomings.

- C implementation works only on `int` types
	Note: Soon to be changed
- Can only find the most recently set interesting element
	Note: Could easily get list of all interesting elements or nth interesting element in linear time

## Next Steps

- Use generic types in C implementation (void*)
- Get more than just most recently set interesting value
- How is this useful?