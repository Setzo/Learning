#!/usr/bin/env bash

# If first argument not provided.
[[ ! $1 ]] && { echo 'Missing argument'; exit 1; }

#if [[ ! $1 ]]; then
#    echo 'Missing argument.'
#    exit 1
#fi

# If file already exists.
if [[ -e "$filename" ]]; then
    echo "File ${filename} already exists."
    exit 1
fi

# If there is another command with this name
if type "$scriptname" > /dev/null 2>&1; then
    echo "There already is command with name ${scriptname}"
    exit 1
fi
declare -r bindir="$HOME/Repos/Learning/Bash"

# If bin directory does not exist.
if [[ ! -d "$bindir" ]]; then

    # Create bin directory.
    if mkdir "$bindir"; then
        echo "Created ${bindir}"
    else
        echo "Could not create ${bindir}"
        exit 1
    fi
fi

while [[ $1 ]]; do
    scriptname=$1
    filename="${bindir}/${scriptname}"

    # If file already exists.
    if [[ -e "$filename" ]]; then
        echo "File ${filename} already exists"
        shift
        continue
    fi

    # If there is another command with this name
    if type "$scriptname" > /dev/null 2>&1; then
        echo "There already is command with name ${scriptname}"
        shift
        continue
    fi

    # Create script.
    echo '#!/usr/bin/env bash' > "$filename"
    # Rights.
    chmod u+x "$filename"

    echo "Script ${scriptname} created"
    shift
done

exit 0
