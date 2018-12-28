#!/bin/bash

cat protocols/protocols.txt | while read line
do
  if [ -z ${line// } ]; then
    continue
  fi
  split=($line)
  proto="protocols/${split[0]}"
  dst="generated/${split[1]}"

  set -x
  dotnet run -- $proto $dst
  { set +x; } 2>/dev/null
done

set -x
cp generated/* ../../src/Backends/Wayland/ --verbose

