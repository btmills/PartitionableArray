#ifndef PA_H
#define PA_H

struct PA;

struct PA* pa_new();
struct PA* pa_ctor(struct PA* this, char (*fn)(int), unsigned int capacity);
void pa_dtor(struct PA* this);
int pa_get(unsigned int index);
int pa_set(unsigned int index, int value);
unsigned int pa_any();

#endif /* PA_H */