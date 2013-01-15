#include <stdlib.h>
#include "pa.h"

/*
 * Structure representing a single element in an array.
 * It may or may not be considered interesting. If so, interesting will point to
 * its respective node in a doubly-linked list of all interesting elements.
 */
typedef struct PAElement {
	int value;
	struct DLLNode* interesting;
} PAElement;

/*
 * Structure representing a node in a doubly-linked list.
 * Contains a single value, index, representing an index in an array.
 */
typedef struct DLLNode {
	struct DLLNode* prev;
	struct DLLNode* next;
	unsigned int index;
} DLLNode;

/*
 * Structure representing a partitionable array.
 * It has a fixed capacity, a doubly-linked list of all interesting elements,
 * an array of elements, and a function used to determine if an element is
 * interesting.
 */
struct PArray {
	unsigned int capacity;
	DLLNode* interesting;
	char (*test)(int);
	PAElement* elements;
};

PArray* pa_new()
{
	return malloc(sizeof(PArray));
}

PArray* pa_ctor(PArray* this, char (*fn)(int), unsigned int capacity)
{
	this->capacity = capacity;
	this->test = fn;
	this->elements = calloc(capacity, sizeof(PAElement));

	return this; // Facilitate chaining or nesting
}

void* pa_dtor(PArray* this)
{
	// Free interesting list
	while(this->interesting)
	{
		DLLNode* next = this->interesting->next;
		free(this->interesting);
		this->interesting = next;
	}
	// Free array
	free(this->elements);

	return NULL;
}

int pa_get(PArray* this, unsigned int index)
{
	return this->elements[index].value;
}

void pa_set(PArray* this, unsigned int index, int value)
{
	this->elements[index].value = value;

	if(this->test(value))
	{
		if(!this->elements[index].interesting)
		{
			// Add element to beginning of interesting list
			DLLNode* node = malloc(sizeof(DLLNode));
			node->index = index;
			node->prev = NULL;
			if(this->interesting)
			{
				node->next = this->interesting;
				this->interesting->prev = node;
			}
			else
			{
				node->next = NULL;
			}
			this->interesting = node;
			// Point this->elements[index].interesting to added element
			this->elements[index].interesting = node;
		}
	}
	else
	{
		if(this->elements[index].interesting)
		{
			// Remove from interesting list
			DLLNode* node = this->elements[index].interesting;
			if(!node->prev) // Node is head
			{
				this->interesting = node->next;
				if(node->next) // Element is not only interesting element
				{
					node->next->prev = NULL;
				}
			}
			else
			{
				node->prev->next = node->next;
				if(node->next)
				{
					node->next->prev = node->prev;
				}
			}
			free(node);
		}
	}
}

char pa_interesting(PArray* this)
{
	return this->interesting ? 1 : 0;
}

unsigned int pa_any(PArray* this)
{
	return this->interesting->index;
}
