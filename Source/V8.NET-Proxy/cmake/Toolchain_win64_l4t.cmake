# the name of the target operating system
set(CMAKE_SYSTEM_NAME Windows)
set(CMAKE_SYSTEM_VERSION 1)
set(CMAKE_SYSTEM_PROCESSOR x86_64)
set(TARGET_ARCH x86_64-w64-mingw32)
set(CMAKE_LIBRARY_ARCHITECTURE ${TARGET_ARCH} CACHE STRING "" FORCE)

# which compilers to use
# https://stackoverflow.com/questions/17242516/mingw-w64-threads-posix-vs-win32
set(CMAKE_C_COMPILER     "${TARGET_ARCH}-gcc-posix")
set(CMAKE_CXX_COMPILER   "${TARGET_ARCH}-g++-posix")
#find_program(CMAKE_C_COMPILER ${TARGET_ARCH}-gcc)
#find_program(CMAKE_CXX_COMPILER ${TARGET_ARCH}-g++)
if(NOT CMAKE_C_COMPILER OR NOT CMAKE_CXX_COMPILER)
    message(FATAL_ERROR "Can't find suitable C/C++ cross compiler for ${TARGET_ARCH}")
endif()


set(CMAKE_FIND_ROOT_PATH "/usr/${TARGET_ARCH}")

# adjust the default behavior of the find commands:
# search headers and libraries in the target environment
set(CMAKE_FIND_ROOT_PATH_MODE_INCLUDE ONLY)
set(CMAKE_FIND_ROOT_PATH_MODE_LIBRARY ONLY)
 
# search programs in the host environment
set(CMAKE_FIND_ROOT_PATH_MODE_PROGRAM NEVER)

set(CMAKE_CROSS_COMPILING TRUE)

