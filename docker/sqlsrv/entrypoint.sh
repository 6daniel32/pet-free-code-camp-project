#!/bin/bash
# Start the script to create the DB and user
# Run the following command on the command line to verify the code in this script works
# /bin/bash -c 'if [[ $(uname -a | grep Darwin) == "" ]]; then echo "linux"; else echo "mac"; fi'
if [[ $(uname -a | grep Darwin) == "" ]]
then
    # Other platforms like Linux
    /usr/config/configure-db.sh &
else
    # Mac OS X platform
    echo "Mac OS X platform"
fi

# Start SQL Server
/opt/mssql/bin/sqlservr