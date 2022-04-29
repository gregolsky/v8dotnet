
function BuildV8NetLoader ( $projPath, $buildType ) {
    write-host "Building V8.Net.Loader"
    & dotnet build /p:SourceLinkCreate=true /p:GenerateDocumentationFile=true --configuration $buildType $projPath;
    CheckLastExitCode
}

function BuildV8NetSharedTypes ( $projPath, $buildType ) {
    write-host "Building V8.Net-SharedTypes"
    & dotnet build /p:SourceLinkCreate=true /p:GenerateDocumentationFile=true --configuration $buildType $projPath;
    CheckLastExitCode
}

function BuildV8NetProxyInterface ( $projPath, $buildType ) {
    write-host "Building V8.Net-ProxyInterface"
    & dotnet build /p:SourceLinkCreate=true /p:GenerateDocumentationFile=true --configuration $buildType $projPath;
    CheckLastExitCode
}

function BuildV8DotNet ( $projPath, $buildType ) {
    write-host "Building V8.Net"
    #& dotnet build /p:SourceLinkCreate=true /p:GenerateDocumentationFile=true --no-incremental $buildType $projPath;
    $v8netReleaseDir = [io.path]::combine($env:RELEASE_DIR, "V8.NET")
    mkdir -p $v8netReleaseDir
    & dotnet build /p:SourceLinkCreate=true /p:GenerateDocumentationFile=true --configuration $buildType -o $env:RELEASE_DIR $projPath;
    CheckLastExitCode
}

function BuildV8NetTest ( $projPath, $buildType ) {
    write-host "Building test $projPath"
    & dotnet build /p:SourceLinkCreate=true /p:GenerateDocumentationFile=true --configuration $buildType $projPath;
    CheckLastExitCode
}

function BuildV8NetProxy ( $srcPath, $targetPlatform, $buildType ) {
    write-host "Building V8.Net-Proxy"
    Push-Location $srcPath
    write-host "----------------------linux-64"
    write-host "----------cmake"

    try {
        $artifactsDir = "$env:RELEASE_DIR"
        $artifactsDir_linuxX64 = "$artifactsDir/v8netproxy/linux-x64"
        
        Remove-Item -Force -Recurse -ErrorAction SilentlyContinue build
        New-Item -ErrorAction 0 -ItemType Directory $artifactsDir_linuxX64

        cmake -B"$artifactsDir_linuxX64" -GNinja `
            -DCMAKE_TOOLCHAIN_FILE=./cmake/Toolchain_$targetPlatform_l4t.cmake `
            -DCMAKE_BUILD_TYPE="$buildType" `
            -DTARGET_PLATFORM="linux-x64" `
            -DV8_SRC="$env:V8_SRC" `
            -DBOOST_SRC="$env:BOOST_SRC" `
            -DBITNESS="x64" `
            -S.
        CheckLastExitCode

        write-host "----------ninja"
        ninja -C "$artifactsDir_linuxX64"
        CheckLastExitCode

        ls $artifactsDir_linuxX64
    # write-host "----------------------win64"
    # write-host "----------cmake"
    # cmake -Bbuild/win64 -GNinja `
    #     -D CMAKE_TOOLCHAIN_FILE=./cmake/Toolchain_win64_l4t.cmake `
    #     -D CMAKE_BUILD_TYPE="$buildType" `
    #     -D TARGET_PLATFORM="windows-x64" `
    #     -S.
    # CheckLastExitCode
    # write-host "----------ninja"
    # ninja -C build/win64
    # CheckLastExitCode

    # write-host "----------------------win32"
    # write-host "----------cmake"
    # #cmake -Bbuild/win32 -GNinja -DCMAKE_TOOLCHAIN_FILE=cmake/Toolchain_win32_l4t.cmake -DCMAKE_BUILD_TYPE="$buildType" -S.
    # CheckLastExitCode
    # write-host "----------ninja"
    # #ninja -C build/win32
    # CheckLastExitCode

    # write-host "----------------------mac64"
    # write-host "----------cmake"
    # #cmake -Bbuild/mac64 -GNinja -DCMAKE_TOOLCHAIN_FILE=cmake/Toolchain_mac64_l4t.cmake -DCMAKE_BUILD_TYPE="$buildType" -S.
    # CheckLastExitCode
    # write-host "----------ninja"
    # #ninja -C build/mac64
    # CheckLastExitCode

    write-host "----------------------arm64"
    write-host "----------cmake"
    # cmake -Bbuild/arm64 -GNinja -DCMAKE_TOOLCHAIN_FILE=cmake/Toolchain_aarch64_l4t.cmake -DCMAKE_BUILD_TYPE="$buildType" -S.
    # CheckLastExitCode
    write-host "----------ninja"
    # ninja -C build/arm64
    # CheckLastExitCode

    write-host "----------------------arm32"
    write-host "----------cmake"
    #CFLAGS=-m32 CXXFLAGS=-m32 
    # cmake -Bbuild/arm32 -GNinja -DCMAKE_TOOLCHAIN_FILE=cmake/Toolchain_aarch32_l4t.cmake -DCMAKE_BUILD_TYPE="$buildType" -S.
    # CheckLastExitCode
    write-host "----------ninja"
    # ninja -C build/arm32
    # CheckLastExitCode

    write-host "Building V8.Net-Proxy is completed"
    #CheckLastExitCode
    } finally {
        Pop-Location
    }
}

function NpmInstall () {
    write-host "Doing npm install..."

    foreach ($i in 1..3) {
        try {
            exec { npm install }
            CheckLastExitCode
            return
        }
        catch {
            write-host "Error doing npm install... Retrying."
        }
    }

    throw "npm install failed. Please see error above."
}

