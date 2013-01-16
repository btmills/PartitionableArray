#include <stdlib.h>
#include "pa.h"

/*
 * Structure representing a single element in an array.
 * It may or may not be considered interesting. If so, interesting will point to
 * its respective node in a doubly-linked list of all interesting elements.
 */
typedef struct PAElement {
	int value;
	int index;
	struct PAElement* prev;
	struct PAElement* next;
} PAElement;

/*
 * Structure representing a partitionable array.
 * It has a fixed capacity, a doubly-linked list of all interesting elements,
 * an array of elements, and a function used to determine if an element is
 * interesting.
 */
struct PArray {
	unsigned int capacity;
	PAElement* interesting;
	char (*test)(int);
	PAElement* arr;
};

PArray* pa_new()
{
	return malloc(sizeof(PArray));
}

PArray* pa_ctor(PArray* this, char (*fn)(int), unsigned int capacity)
{
	unsigned int i;

	this->capacity = capacity;
	this->test = fn;
	this->arr = calloc(capacity, sizeof(PAElement));
	for(i = 0; i < capacity; i++)
	{
		this->arr[i].index = i;
	}

	return this; // Facilitate chaining or nesting
}

void* pa_dtor(PArray* this)
{
	this->interesting = NULL; // Prevent dangling pointer
	// Free array
	free(this->arr);

	return NULL;
}

int pa_get(PArray* this, unsigned int index)
{
	return this->arr[index].value;
}

void pa_set(PArray* this, unsigned int index, int value)
{
	this->arr[index].value = value;

	if(this->test(value)) // Is interesting
	{
		if(!(this->arr[index].prev != NULL
			|| this->arr[index].next != NULL)) // Is not already in list
		{
			// Add element to beginning of interesting list
			if(this->interesting != NULL)
			{
				this->interesting->prev = &(this->arr[index]);
				this->arr[index].next = this->interesting;
			}
			this->interesting = &(this->arr[index]);
		}
	}
	else // Is not interesting
	{
		if(this->interesting != NULL
		   && this->interesting->index == index) // Is list head
		{
			// Remove list head
			this->interesting = this->arr[index].next;
			this->arr[index].next = NULL;
			if(this->interesting != NULL) // List still isn't empty
			{
				this->interesting->prev = NULL;
			}
		}
		else if(this->arr[index].prev != NULL) // Is elsewhere in list
		{
			// Remove from list
			this->arr[index].prev->next = this->arr[index].next;
			if(this->arr[index].next != NULL) // Is not tail
			{
				this->arr[index].next->prev = this->arr[index].prev;
				this->arr[index].next = NULL;
			}
			this->arr[index].prev = NULL;
		}
		// Else is not already in list, no need to remove
	}
}

char pa_interesting(PArray* this)
{
	return this->interesting != NULL;
}

unsigned int pa_any(PArray* this)
{
	return this->interesting->index;
}
