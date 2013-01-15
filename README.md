# Partitionable Array

This is an implementation of an array whose elements can be classified by a function as either interesting or uninteresting. Element values can be set and retreived in constant time, and an interesting element can be retrieved in constant time.

## Implementations

Only one implementation has been completed so far, in C. See the C directory for more information.

## Design Tradeoffs

Had I not designed it this way intentionally, these would also be known as shortcomings.

- Works only on `int` types
	Note: Soon to be changed
- Can only find the most recently set interesting element
	Note: Could easily get list of all interesting elements or nth interesting element in linear time

## Next Steps

- Use generic types (void* in C or generics in C#)