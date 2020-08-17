P/Invoke bindings for [DLPack](http://github.com/dmlc/dlpack). It demonstrates consuming from C# a DLPack tensor produced in a C function.

Hopefully there will be official bindings: https://github.com/dmlc/dlpack/issues/56 and https://github.com/microsoft/onnxruntime/issues/4162.

Currently there is no example for constructing a DLPack tensor in C# and sending it to a C function.

Limitations: due to [`ReadOnlySpan<T>`](https://docs.microsoft.com/en-us/dotnet/api/system.readonlyspan-1.-ctor?view=netcore-3.1#System_ReadOnlySpan_1__ctor_System_Void__System_Int32_) supporting only Int32 lengths, the `ROS<T>`-returning method is called `DataSpanLessThan2Gb()`. Issue: https://github.com/dotnet/corefxlab/issues/896 and https://github.com/dotnet/apireviews/tree/master/2016/11-04-SpanOfT#spant-and-64-bit

### Install .NET Core 3.1 SDK on Ubuntu 18.04
From https://docs.microsoft.com/en-us/dotnet/core/install/linux-ubuntu#1804- :
```shell
wget https://packages.microsoft.com/config/ubuntu/18.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb

sudo apt-get update
sudo apt-get install -y apt-transport-https
sudo apt-get update 
sudo apt-get install -y dotnet-sdk-3.1
```

### Usage
```shell
make dlpack
./dlpack golden.bin

make libdlpack.so
dotnet run test.cs csharp.bin

#ndim 2
#shape 3 2
#(0, 0) = 0
#(0, 1) = 1
#(1, 0) = 2
#(1, 1) = 3
#(2, 0) = 4
#(2, 1) = 5
#Deleter calling
#Deleter called

diff golden.bin cscharp.bin
```
