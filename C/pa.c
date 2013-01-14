#include <stdlib.h>
#include "pa.h"

/*
 * Structure representing a node in a doubly-linked list.
 * Contains a single value, index, representing an index in an array.
 */
struct node {
	struct node* prev;
	struct node* next;
	unsigned int index;
};

struct PA {
