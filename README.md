# Arma3Ext_CSharpFeatureFlagsSample
Provides a working example of adding Feature Flags to a C# DLL Extension for Arma 3

The goal of this repository is simply to provide a working example of one of the many ways to include RVExtFeatureFlags directly into a C# DLL.
It uses the minimal setup simply to demonstrate the process.

What this is not:
  - A Feature-Complete example of a C# Extension
  - The only way to accomplish this

For this example, we assume use of Visual Studio and it's associated tools and compilers.
It is not required. Simply adjust the steps as needed to use your preferred environment.

## The Process
In this sample, we use C++ to provide the necessary external field to manipulate the RVExtFeatureFlags but other languages can be useed.

The .cpp file is located in the same directory as the C# project files but, it can be placed anywhere.

See [Arma3ExtFeatureFlags.cpp](Arma3ExtFeatureFlags.cpp) for an example of such a file

### Step 1 - Compile the .cpp file into a .obj file
  The goal is to compile and place an .obj file into the Intermediary Folder so we can link into the final C# DLL.

  The intermediary directory is typically 'obj\ (build) \ (target) \ (runtime)' or in this case *'obj\Release\net8.0\win-x64'*

  ```l /c /EHsc Arma3ExtFeatureFlags.cpp /Fo:obj\Release\net8.0\win-x64\Arma3ExtFeatureFlags.obj```

  This only has to happen once unless you need to modify the cpp file in which case recompilation is required.
  
  ### WARNING : For this to be x64 compatible, you have to use the 64-bit cl.exe or the 'x64 Native Tools Command Prompt'
  If the compilation results in an x86 .obj file as the normal Command Prompt would typically result in, the final compilation will fail. 

### Step 2 - Make sure you use LibraryImportAttribute (or DLLImport) in your .cs file
  LibraryImport is technically slightly faster and newer but, DLLImport is still supported and works
  - For LibraryImport - use partial classes and methods for the source generator to work.
  - For DLLImport - just use extern as normal
    
  See [Main.cs](Main.cs) for an example.

### Step 3 - Make sure your .csproj file includes an ItemGroup item to include the .obj file 
  - For net8.0 - NativeLibrary can be used
  - For net10.0 - LinkerArgs *must* be used
    
  See [SampleExt.csproj](SampleExt.csproj) for an example

### Step 4 - Compile the C# library
  e.g. ```dotnet publish -c Release -r win-x64```

### Step 5 - Verify all symbols are in the DLL using dumpbin or something equivalent
  e.g. ```dumpbin /exports SampleExt_x64.dll```
  
  should result in something like this:
```
  Section contains the following exports for SampleExt_x64.dll

    00000000 characteristics
    FFFFFFFF time date stamp
        0.00 version
           1 ordinal base
           6 number of functions
           6 number of names

    ordinal hint RVA      name

          1    0 0019FB10 DotNetRuntimeDebugHeader = DotNetRuntimeDebugHeader
          2    1 00002190 GetRVExtensionFeatureFlags = GetRVExtensionFeatureFlags
          3    2 00071E40 RVExtension = RVExtension
          4    3 00071F20 RVExtensionArgs = RVExtensionArgs
          5    4 0019F9F0 RVExtensionFeatureFlags = RVExtensionFeatureFlags
          6    5 00002170 SetRVExtensionFeatureFlags = SetRVExtensionFeatureFlags

  Summary

        E000 .data
       79000 .managed
        D000 .pdata
       79000 .rdata
        1000 .reloc
        1000 .rsrc
       70000 .text
       3C000 hydrated
```
  

