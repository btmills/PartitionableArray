#include <stdlib.h>
#include "pa.h"

/*
 * Structure representing a single element in an array.
 * It may or may not be considered interesting. If so, interesting will point to
 * its respective node in a doubly-linked list of all interesting elements.
 */
typedef struct PAElement {
	int value;

	bool interesting;
	int prev;
	int next;
} PAElement;

/*
 * Structure representing a partitionable array.
 * It has a fixed capacity, a doubly-linked list of all interesting elements,
 * an array of elements, and a function used to determine if an element is
 * interesting.
 */
struct PArray {
	int length;
	int count;
	int sentinel;
	char (*test)(int);
	PAElement* arr;
};

PArray* pa_new() { return malloc(sizeof(PArray)); }

PArray* pa_ctor(PArray* this, char (*fn)(int), int length)
{
	this->test = fn;
	this->length = length;
	this->arr = calloc(length + 1, sizeof(PAElement)); // One extra element for sentinel
	this->arr[length].prev = this->arr[length].next = this->sentinel = length; // Set up the sentinel

	return this; // Facilitate chaining or nesting
}

void* pa_dtor(PArray* this)
{
	// Free array
	free(this->arr);
	free(this);

	return NULL;
}

int pa_len(PArray* this) { return this->length; }

int pa_count(PArray* this) { return this->count; }

int pa_get(PArray* this, int index)
{
	if(!(0 <= index && index < this->length))
		return -1;

	return this->arr[index].value;
}

void pa_set(PArray* this, int index, int value)
{
	if(!(0 <= index && index < this->length))
		return;

	this->arr[index].value = value;

	// Update list of interesting elements
	if(this->test(value)) // Is interesting
	{
		if(!this->arr[index].interesting) // Is not in list
		{
			// Add to list
			this->count++;
			this->arr[index].interesting = true;

			this->arr[index].next = this->sentinel;
			this->arr[index].prev = this->arr[this->sentinel].prev;
			this->arr[this->arr[this->sentinel].prev].next = index;
			this->arr[this->sentinel].prev = index;
		}
		// Else already in list, no need to add
	}
	else // Is not interesting
	{
		if(this->arr[index].interesting) // Is in list
		{
			// Remove from list
			this->count--;
			this->arr[index].interesting = false;

			this->arr[this->arr[index].prev].next = this->arr[index].next;
			this->arr[this->arr[index].next].prev = this->arr[index].prev;
		}
		// Else is not already in list, no need to remove
	}
}

char pa_interesting(PArray* this) { return this->count > 0; }

int pa_any(PArray* this) { return this->arr[this->sentinel].next; }
