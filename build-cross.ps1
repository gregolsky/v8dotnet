
docker build `
	--progress=plain `
	-t v8repo `
	-f ./scripts/cross/Dockerfile.v8repo ./

if ($LASTEXITCODE -ne 0) {
	throw "See error above"
}

New-Item -ItemType Directory -Force artifacts

docker build `
	--progress=plain `
	-t v8net-proxy-build `
	-f ./scripts/cross/Dockerfile ./

if ($LASTEXITCODE -ne 0) {
	throw "See error above"
}

docker run --rm `
	-v "$(Resolve-Path artifacts):/build/artifacts" `
	-it v8net-proxy-build 