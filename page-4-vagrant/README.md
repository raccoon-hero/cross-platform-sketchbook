# App for Tasks 1/2/3 in One on Different Platforms

This project consists of a console application (`Application`) and a class library (`KPCore4`), packaged and distributed via a private NuGet repository, using BaGet. It includes deployment on different virtual machines using Vagrant.

The instructions below will get you a copy of the project up and running on your local machine for development and testing purposes.

## Prerequisites

![.NET SDK](https://img.shields.io/badge/.NET%20SDK-Compatible-blue)
![Visual Studio 2022](https://img.shields.io/badge/Visual%20Studio-2022-blue)
![NuGet CLI](https://img.shields.io/badge/NuGet-CLI-blue)
![Docker](https://img.shields.io/badge/Docker-Container%20Engine-blue)
![Vagrant](https://img.shields.io/badge/Vagrant-VM%20Manager-blue)

Before you begin, ensure you have the following installed:
- [**.NET SDK**](https://dotnet.microsoft.com/download), compatible with the version used in the project (`.NET 6`).
- [**Visual Studio 2022**](https://visualstudio.microsoft.com/vs/) or a similar IDE that supports .NET development.
- [**NuGet CLI**](https://www.nuget.org/downloads): Essential for managing NuGet packages and configuring the local NuGet repository.
- [**Docker**](https://www.docker.com/products/docker-desktop): Required for running the BaGet server in a container.
- [**Vagrant**](https://www.vagrantup.com/downloads.html): Used for setting up and managing virtual machines for deployment testing (consider having **up to 100 GB** of free space on your main drive). 

## Set up

### Setting Up a Private NuGet Repository (BaGet)

   1. **Pull BaGet Docker Image:**
      
      ```bash
      docker pull loicsharma/baget
      ```

   2. **Create a `.env` File for BaGet:**
      
      Create a `baget.env` file with the necessary configuration:
      ```bash
      ApiKey=kpadmin  # Or any other password
      Storage__Type=FileSystem
      Storage__Path=/var/baget/packages
      Database__Type=Sqlite
      Database__ConnectionString=Data Source=/var/baget/baget.db
      Search__Type=Database
      ```

   3. **Run BaGet Container:**
      
      ```bash
      docker run --rm --name kp-nuget-server -p 5555:80 --env-file baget.env -v ./baget-data:/var/baget loicsharma/baget
      ```

### Packaging and Publishing

   1. **Package Console Application and Class Library**
      
      ```bash
      dotnet pack --configuration Release
      ```

   2. **Publish App to BaGet:**
      
      Use the following command, while the Docker container is running:
      ```bash
      dotnet nuget push -s http://localhost:5555/v3/index.json -k kpadmin ./bin/Release/KPetrachyk.1.3.1.nupkg
      ```
   3. **Publish KPCore4 to BaGet**

      Use the following command, while the Docker container is running:
      ```bash
      dotnet nuget push -s http://localhost:5555/v3/index.json -k kpadmin ./bin/Release/KPCore4.1.1.0.nupkg
      ```

### Setting up VMs with Vagrant

   1. **Initialize Vagrant in the Project:**

      Navigate to the `Vagrant` directory within the project and run each of the following commands within specific folders (`Vagrant/Windows-10`, `Vagrant/Linux`, `Vagrant/MacOS`):
      ```bash
      cd ../Windows-10
      vagrant init gusztavvargadr/windows-10
      
      cd ../Linux
      vagrant init debian/bookworm64
      
      cd ../MacOS
      vagrant init yzgyyang/macOS-10.14
      ```
      You can find more Vagrant boxes on [Vagrant Cloud](https://app.vagrantup.com/boxes/search).
   2. **Configure the Vagrantfile:**
      
      `init` generates a `Vagrantfile` for selected VWs. Edit each Vagrantfile to set up the VMs, including network and shared folders. The ones used within this project are placed in the `Vagrant` folder. Here's an example of Vagrantfile configured for this project (``Windows 10`` VM set up with downloading `.NET6`):
      ```bash
      Vagrant.configure("2") do |config|
         config.vm.box = "gusztavvargadr/windows-10"
         config.vm.network "public_network"

         config.vm.provider "virtualbox" do |vb|
            vb.gui = true
         end
         
         config.vm.provision "shell", privileged: true, inline: <<-SHELL
            [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
            Invoke-WebRequest -Uri https://download.visualstudio.microsoft.com/download/pr/81531ad6-afa9-4b61-9d05-6a76dce81123/2885d26c1a58f37176fd7859f8cc80f1/dotnet-sdk-6.0.417-win-x64.exe -OutFile dotnet-sdk-6.0.417-win-x64.exe
            Start-Process -FilePath dotnet-sdk-6.0.417-win-x64.exe
         SHELL
      end
      ```
      If for some reason .NET6 wasn't downloaded, head to the [dotnet.microsoft.com](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) and download it within your VW manually.

      An example of Vagrantfile configured for this project (``Debian`` set up with downloading `.NET6` & `KPetrachyk` dotnet tool from a local private NuGet repository):
      ```bash
      Vagrant.configure("2") do |config|
        config.vm.box = "debian/bookworm64"
        config.vm.network "public_network"
      
        config.vm.provision "shell", inline: <<-SHELL
           apt-get update
           apt-get upgrade
      
           wget https://packages.microsoft.com/config/debian/12/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
           sudo dpkg -i packages-microsoft-prod.deb
           rm packages-microsoft-prod.deb
      
           sudo apt-get update && sudo apt-get install -y dotnet-sdk-6.0
           dotnet tool install --global --version 1.3.1 --add-source http://10.0.2.2:5555/v3/index.json --no-cache KPetrachyk
        SHELL
      end
      ```

      An example of Vagrantfile configured for this project (``MacOS`` set up):
      ```bash
      Vagrant.configure("2") do |config|
        config.vm.box="yzgyyang/macOS-10.14"
        config.vm.network "public_network"
      
        config.vm.synced_folder ".","/vagrant", :disabled =>true
        config.ssh.insert_key=false
      
        config.vm.provider "virtualbox" do|vb|
            vb.memory="4096"
            vb.customize ["modifyvm", :id, "--cpuidset", "1","000206a7","02100800","1fbae3bf","bfebfbff"]
      
            vb.customize ["setextradata", :id, "VBoxInternal/Devices/efi/0/Config/DmiSystemProduct", "MacBookPro11,3"]
            vb.customize ["setextradata", :id, "VBoxInternal/Devices/efi/0/Config/DmiSystemVersion", "1.0"]
            vb.customize ["setextradata", :id, "VBoxInternal/Devices/efi/0/Config/DmiBoardProduct", "Iloveapple"]
            vb.customize ["setextradata", :id, "VBoxInternal/Devices/smc/0/Config/DeviceKey", "ourhardworkbythesewordsguardedpleasedontsteal(c)AppleComputerInc"]
        end
      end
      ```

   4. **Start the VM:**
      ```bash
      vagrant up
      ```

   5. **Access the VM:**
      
      For GUI access, use the VirtualBox interface. For SSH (typically for Linux/Unix VMs):
      ```bash
      vagrant ssh
      ```

   6. **Installing our DotnetTool** (if not pre-configured in Vagrantfile):
      ```bash
      dotnet tool install --global --version 1.3.1 --add-source http://10.0.2.2:5555/v3/index.json --no-cache KPetrachyk
      ```
   7. **Other Useful Commands:**

      **\[All Tools\]:** After installing `KPetrachyk` dotnet tool, get the list of all installed dotnet tools, including the new one:
      ```bash
      dotnet tool list -g
      ```
      
      **\[Launch Tool\]:** Since the `KPetrachyk` dotnet tool command name is `kpcore4`, you should use it to activate the new console interface:
      ```bash
      kpcore4
      ```

      It should result as follows:
      ```bash
      CROSS_Task4 Started. Type 'exit' to quit.

      > [Enter command]: 
      ```

      **\[Delete Tool\]:** If you want to delete `KPetrachyk` dotnet tool:
      ```bash
      dotnet tool uninstall --global KPetrachyk
      ```
