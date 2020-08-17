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
