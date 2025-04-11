#!/bin/sh

# uses Mono to build and run examples/Main.cs, linked to Gnuplot binding

mono-csc -out:Main.exe examples/Main.cs Gnuplot.cs
mono Main.exe

exit 0