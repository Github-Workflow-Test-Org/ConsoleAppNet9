#!/bin/zsh
name=$(basename ${PWD})
echo ${name}
rm -rf ${name}
docker build . -t ${name:l}
CID=$(docker run -d ${name:l})
docker cp "$CID":/app ${name:l}
docker container stop "$CID"
docker container rm "$CID"

#remove some additional files
#rm -rf ${name}/*.${name} ${name}/*.db WebApp/web.config

#zip output to binaries directory; will probably only work on MacOSX
zip -r ../binaries/${name}-binaries.zip ${name}

#remove directory
rm -rf ${name}  