CC = gcc
CFLAGS = -Wall -fnested-functions
LDFLAGS = $(CFLAGS)

all: test
test: test.c pa.c pa.h
	   $(CC) $(CFLAGS) -o test test.c pa.c

clean:
	   rm -f *.o test
