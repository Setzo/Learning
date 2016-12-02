#!/usr/bin/env bash

[[ $# -ne 2 ]] && { echo 'Need exactly two arguments.' >&2; exit 1; }

#[[ $# -ne 2 ]] && {
#    echo 'Need exactly two arguments.' >&2
#    exit 1
#}

for f in *"$1"; do
    base=`basename "$f" "$1"`
    mv "$f" "${base}${2}"
done

exit 0