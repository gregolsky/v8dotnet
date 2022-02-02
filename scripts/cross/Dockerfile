FROM v8repo

# basic build tools
RUN curl -sL https://deb.nodesource.com/setup_14.x | bash - && apt-get install -y nodejs && node --version \
 && curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --channel 6.0 --install-dir /opt/dotnet

# # v8 specific build tools
# WORKDIR /build
# RUN git clone https://chromium.googlesource.com/chromium/tools/depot_tools.git 
# ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1 PATH=/build/depot_tools:/opt/dotnet:$PATH
# RUN rm -rf v8 && fetch v8

COPY scripts/cross/assets/install-build-deps_chromium.sh /build

RUN apt update \
  && bash ./install-build-deps_chromium.sh --no-prompt --arm --no-chromeos-fonts
RUN sudo apt install -y vim mingw-w64 bzip2 g++-multilib libc++-11-dev

COPY scripts/cross/assets/build_v8.sh scripts/cross/assets/v8 ./

RUN bash -c "/build/build_v8.sh windows-x64"

COPY . ./ 

RUN mkdir -p boost && tar xjf scripts/cross/assets/boost.tar.bz2 -C boost --strip-components=2

RUN rm -rf /build/Source/V8.NET-Proxy/V8/v8 \
  && mv /build/v8 /build/Source/V8.NET-Proxy/V8/v8 

CMD pwsh ./build.ps1 && ls artifacts


