CC = gcc

SHAREDFLAGS = -shared -fPIC

dlpack: dlpack.c dlpack.h
	$(CC) -o $@ $< $(CFLAGS)

libdlpack.so: dlpack.c dlpack.h
	$(CC) -o $@ $(SHAREDFLAGS) $< $(CFLAGS)
