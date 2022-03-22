#!/bin/bash

set -e
set -x

ARGS_GN_SRC_DIR=/build
ARGS_GN_TARGET_DIR=/build/v8

cd $ARGS_GN_TARGET_DIR

platform=$1

gn gen "out.gn/$platform.release"
#python tools/dev/v8gen.py "$platform.release"

cp -v "$ARGS_GN_SRC_DIR/args.gn.${platform}.release" "$ARGS_GN_TARGET_DIR/out.gn/${platform}.release/args.gn"

gn gen "out.gn/$platform.release"

ninja -C "out.gn/${platform}.release"
  