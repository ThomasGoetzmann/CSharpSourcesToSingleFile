# CSharpSourcesToSingleFile

Takes all .cs files in a directory and combines them in a single .cs output file (console application).

The search is done in the **given directory** and is **recursive** but **excludes** the ```/bin/``` and ```/obj``` folders.

## Usage

You can run the executable with different args and use the --help argument for more details.

```bash
.\CSharpSourcesToSingleFile.exe --help
```

Hint: Add the folder containing the .exe to your Path for an easier usage.

## Screenshots

![Usage example](img/usage-example.png)

## Build

You can download and extract the .zip archive containing the compiled files, or you can build it yourself with the following commands.

**WARNING:** The project uses **.Net 5.0**

```bash
# Restores the NuGet package dependencies
dotnet restore

# Build the .exe
dotnet build
```
