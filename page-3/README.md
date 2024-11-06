# Matrix Processing Project

This project demonstrates a .NET solution for processing matrices, with a focus on replacing zero elements with the nearest non-zero element. The solution comprises a class library (`MatrixProcessingCRT3lib`) that encapsulates the matrix processing logic, and a console application that provides a user interface for interacting with the library.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

![.NET SDK](https://img.shields.io/badge/.NET%20SDK-Compatible-blue) ![Visual Studio 2022](https://img.shields.io/badge/Visual%20Studio-2022-blue) ![NuGet CLI](https://img.shields.io/badge/NuGet-CLI-blue)

Before you begin, ensure you have the following installed:
- [.NET SDK](https://dotnet.microsoft.com/download), compatible with the version used in the project (`.NET 6`).
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) or a similar IDE that supports .NET development.
- [NuGet CLI](https://www.nuget.org/downloads): Essential for managing NuGet packages and configuring the local NuGet repository.

### Setting Up

1. **Clone the Repository:**

   ```bash
   git clone https://github.com/raccoon-hero/cross-platform-sketchbook.git
   cd cross-platform-programming/page-3
   ```

2. **Create the .nuspec File:**

   Create a .nuspec file for the `MatrixProcessingCRT3lib` class library. You can use the following content as a template:
   
   ```xml
   <?xml version="1.0" encoding="utf-8"?>
   <package xmlns="http://schemas.microsoft.com/packaging/2012/06/nuspec.xsd">
     <metadata>
       <id>KPetrachyk</id>
       <version>0.2.2</version>
       <authors>K.P.</authors>
       <description>Package Description</description>
       <dependencies>
         <group targetFramework="net6.0" />
       </dependencies>
     </metadata>
   </package>
   ```
   
   Save this file in the root of the MatrixProcessingCRT3lib project.
   
2. **Package the Class Library:**

   Before you use the class library in the console application, you'll need to package it. Navigate to the class library folder (`MatrixProcessingCRT3lib`) and use the following command:
   
   ```bash
   dotnet pack
   ```

   Alternatively, you can just right-click the project, and pack it. In both cases, this will create a NuGet package for the class library in the bin directory.

2. **Set Up the Local NuGet Repository:**
   If you don't have one already, proceed with the next steps:
   - Navigate to the `LocalRepo` folder.
   - Configure your local NuGet settings to use this folder as a source. You can do this by running the following command in the command prompt:
     ```bash
     nuget sources add -name "LocalRepo" -source "path_to_LocalRepo_folder"
     ```
   - Replace `"path_to_LocalRepo_folder"` with the actual path to your `LocalRepo` folder.

3. **Open the Application:**
   - Open the `Application` folder and locate the solution file (`CROSS_Task3.sln`).
   - Open this file in Visual Studio or your preferred IDE.

4. **Add the Class Library Package to the Console Application:**

   - In your IDE, right-click on the console application project within the solution.
   - Choose `Manage NuGet Packages`.
   - Go to the `Browse` tab and select your local NuGet repository (`LocalRepo`) as the package source.
   - Search for and install the `KPetrachyk` package.

4. **Build:**
   - Build the solution by pressing `Ctrl+Shift+B` or using the "Build" menu in your IDE.
