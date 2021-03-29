using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;
using Dir = System.IO.Directory;

namespace CSharpSourceCodeToSingleFile
{
    [Command(ExtendedHelpText = @"
Searches through all .cs files in a given folder (recursive) and aggregates them in a single .cs output file.")]
    class Program
    {
        static void Main(string[] args)
            => CommandLineApplication.Execute<Program>(args);

        [DirectoryExists]
        [Option(Description = "Folder path were all .cs files are located (search is recursive). Default is Current Directory.", ShortName = "d")]
        public string Directory { get; } = ".";

        [Required]
        [FileNotExists]
        [FileExtensions(Extensions = ".cs")]
        [Option(Description = "Single file name the code will be merged into", ShortName = "o")]
        public string Output { get; }

        [Option(CommandOptionType.NoValue)]
        public bool Verbose { get; }

        private void OnExecute()
        {
            var directory = Directory == "." ? Environment.CurrentDirectory : Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, Directory));
            var outputPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, Output));

            var objFolder = $"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}";
            var binFolder = $"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}";

            var files = Dir
                .EnumerateFiles(directory, "*.cs", SearchOption.AllDirectories)
                .Where(path => !path.Contains(binFolder)
                            && !path.Contains(objFolder));

            if(Verbose)
            {
                Console.WriteLine("File found:");
                foreach(var f in files)
                    Console.WriteLine(f);
            }

            var lines = files
                .SelectMany(file => File.ReadAllLines(file));

            var usings = lines
                .Where(line => line.StartsWith("using"))
                .Distinct();

            var classes = lines.Where(line => !usings.Contains(line));

            var outputDirectory = Path.GetDirectoryName(outputPath);
            System.IO.Directory.CreateDirectory(outputDirectory);

            using (var writer = File.CreateText(outputPath))
            {
                foreach (var line in usings)
                    writer.WriteLine(line);

                writer.WriteLine();

                foreach (var line in classes)
                    writer.WriteLine(line);
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"SUCCESS - Single source code file generated. {outputPath}");
            Console.ResetColor();
        }
    }
}
