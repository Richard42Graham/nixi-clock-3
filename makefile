CC=gcc
CCFLAGS=-Wall
LDFLAGS=
SOURCES=$(wildcard ./src/*.c)
OBJECTS=$(SOURCES:.c=.o)
TARGET=a.out

all: $(TARGET)

$(TARGET): $(OBJECTS)
	$(CC) -o $@ $^ $(LDFLAGS)

./src/%.o: %.c %.h
	$(CC) $(CCFLAGS) -c $<

./src/%.o: %.c
	$(CC) $(CCFLAGS) -c $<

clean:
	rm -f **/*.o $(TARGET)
