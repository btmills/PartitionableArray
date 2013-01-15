This partitionable array implementation uses a `PArray` (Partitionable Array) structure to store the array.

## Operations

### New

`PArray* pa_new()`

#### Description

Allocates memory for a partitionable array.

#### Returns

Pointer to memory for a PArray.

### Constructor

`PArray* pa_ctor(PArray* this, bool (*fn)(int), unsigned int capacity)`

#### Description

Constructs a new partitionable array with a specified evaluation function and a fixed capacity.

#### Arguments

- `this` An allocated but ininitialized `PArray`.
- `fn` Function to be used to determine if an element is interesting.
- `capacity` Number of elements the array should be able to store.

#### Returns

`this` argument, unaltered, to facilitate single-line creation of the form `PArray* arr = pa_ctor(pa_new(), {fn}, {capacity});`

### Destructor

`void* pa_dtor(PArray* this)`

#### Description

Destructs an array and frees its memory. Don't forget to set the array pointer to `NULL` after calling this.

#### Arguments

- `this` Array to destruct.

#### Returns

`NULL`

### Get

`int pa_get(PArray* this, unsigned int index)`

#### Description

Gets the value of the element in the array at the specified index.

#### Arguments

- `this` The array.
- `index` Index of the element whose value should be retrieved. 0 <= index < capacity.

#### Returns

The value at the specified index in the array.

### Set

`void pa_set(PArray* this, unsigned int index, int value)`

#### Description

Sets the value of the element in the array at the specified index.

#### Arguments

- `this` The array.
- `index` Index of the array whose value should be set. 0 <= index < capacity.
- `value` The new value.

### Interesting

`bool pa_interesting(PArray* this)`

#### Description

Determines if there are any interesting elements in an array.

#### Arguments

- `this` The array.

#### Returns

`true` if at least one interesting element exists in the array. `false` otherwise.

### Any

`unsigned int pa_any(PArray* this)`

#### Description

Gets the index of an interesting element of the array. Requires that the array contains at least one interesting element, which can be verified by a `pa_interesting()`.

#### Arguments

- `this` The array.

#### Returns

The index of an interesting element in the array.