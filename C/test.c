#include <stdio.h>
#include <stdlib.h>
#include "pa.h"

#define CAPACITY 20

int main(int argc, char** argv)
{
	bool even(int val)
	{
		return val != 0 && val % 2 == 0;
	}

	PArray* arr = NULL;
	int i = 0;

	arr = pa_ctor(pa_new(), &even, CAPACITY);

	for(i = 0; i < CAPACITY; i++)
	{
		pa_set(arr, i, i);
		printf("Added %d.\n", pa_get(arr, i));
		if(pa_interesting(arr))
			printf("%d is interesting.\n", pa_get(arr, pa_any(arr)));
	}

	printf("Array contents:\n");
	for(i = 0; i < CAPACITY; i++)
	{
		printf("%2d: %2u\n", i, pa_get(arr, i));
	}

	// Set all interesting elements to 0
	while(pa_interesting(arr))
	{
		int index = pa_any(arr);
		printf("%2d is interesting. Setting equal to 0...\n", pa_get(arr, index));
		pa_set(arr, index, 0);
	}
	printf("Array is %sinteresting.\n", pa_interesting(arr) ? "" : "not ");

	arr = pa_dtor(arr);

	return 0;
}