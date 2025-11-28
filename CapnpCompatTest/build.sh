#!/bin/bash
set -e

# Compile the capnp schema
capnp compile -I . -I /usr/include -oc++ test.capnp

# Compile the C++ test runner
# We need C++14 or later.
# Linking against capnp-rpc, capnp, kj-async, kj, and pthread.
g++ -std=c++14 -o CapnpCompatTest CapnpCompatTest.cpp test.capnp.c++ \
    -lcapnp-rpc -lcapnp -lkj-async -lkj -lpthread

echo "Build successful! Executable is ./CapnpCompatTest"
