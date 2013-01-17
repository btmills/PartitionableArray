#ifndef bool
#define bool char
#endif
#ifndef true
#define true 1
#endif
#ifndef false
#define false 0
#endif

#ifndef PA_H
#define PA_H

typedef struct PArray PArray;

/*
 * Allocates memory for a partitionable array.
 *
 * returns: Pointer to a PArray
 */
PArray* pa_new();

/*
 * Constructs a new partitionable array with a specified evaluation function and
 * a fixed capacity.
 *
 * this: An allocated but uninitialized PArray
 * fn: Function to be used to determine if an element is interesting
 * capacity: Number of elements the array should be able to store
 *
 * returns: this, unaltered, to facilitate chaining or nesting
 */
PArray* pa_ctor(PArray* this, bool (*fn)(int), int length);

/*
 * Destructs a PArray and frees its memory.
 * Don't forget to set the pointer to NULL after calling this.
 *
 * this: PArray to destruct
 *
 * returns: NULL
 */
void* pa_dtor(PArray* this);

/*
 * Gets the capacity of the array.
 *
 * this: PArray structure.
 *
 * returns: The capacity of the array.
 */
int pa_len(PArray* this);

/*
 * Gets the number of interesting elements in the array.
 *
 * this: PArray structure.
 *
 * returns: The number of interesting elements in the array.
 */
int pa_count(PArray* this);

/*
 * Gets the value of the element in the array at the specified index.
 *
 * this: An initialized PArray
 * index: Index of the element to get
 *        0 <= index < capacity
 *
 * returns: Value of the array element at index
 */
int pa_get(PArray* this, int index);

/*
 * Sets the value of the element in the array at the specified index.
 *
 * this: An initialized PArray
 * index: Index of the element to set
 *        0 <= index < capacity
 * value: New value of the element at index
 */
void pa_set(PArray* this, int index, int value);

/*
 * Determines if there are any interesting elements in a PArray.
 *
 * this: PArray that may or may not contain interesting elements
 *
 * returns: true if the PArray contains interesting elements, false otherwise
 */
bool pa_interesting(PArray* this);

/*
 * Gets the index of an interesting element of the array.
 * Requires that the array contains at least one interesting element.
 *
 * this: PArray containing at least one interesting element
 *
 * returns: Index of an interesting element in the array
 */
int pa_any(PArray* this);

#endif /* PA_H */