
docker run --rm `
	-v "$(Resolve-Path artifacts):/build/artifacts" `
	-it v8net-proxy-build bash