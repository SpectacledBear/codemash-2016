using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SpectacledBear.CodeMash2016.Manifest.Models;

namespace SpectacledBear.CodeMash2016.Manifest
{
    class Program
    {
        private const string DIRECTORY_ARG = "directory";
        private const string MANIFEST_FILE_ARG = "file";
        private const string REPORT_FILE_ARG = "report";
        private const string BUILD_ARG = "build";
        private const string CHECK_ARG = "check";

        static int Main(string[] args)
        {
            ManifestConfiguration config = ParseArguments(args);

            if (!ValidateConfiguration(config))
            {
                PrintConsoleHelp();
                return -1;
            }

            string rootDirPath = Path.GetFullPath(config.RootDirectory);
            string manifestFilePath = Path.GetFullPath(config.ManifestFilePath);

            if (!Directory.Exists(rootDirPath))
            {
                Console.WriteLine("\nThe directory specified does not exist.");
                return -1;
            }

            if (config.CheckManifest && !File.Exists(manifestFilePath))
            {
                Console.WriteLine("\nThe manifest file does not exist.");
                return -1;
            }

            if (config.BuildManifest)
            {
                FileManifest manifest = FileManifestFactory.Create(rootDirPath);

                FileWriter.WriteManifest(manifest, manifestFilePath);
            }

            if (config.CheckManifest)
            {
                string reportFilePath = Path.GetFullPath(config.ReportFilePath);

                FileManifest baselineManifest = ManifestReader.ReadManifest(manifestFilePath);
                FileManifest currentManifest = FileManifestFactory.Create(rootDirPath);

                List<FileManifestItem> missingItems = new List<FileManifestItem>();
                foreach (FileManifestItem item in baselineManifest.Items)
                {
                    if (!currentManifest.Items.Any(i => i.File == item.File))
                    {
                        missingItems.Add(item);
                    }
                }

                List<FileManifestItem> newItems = new List<FileManifestItem>();
                foreach (FileManifestItem item in currentManifest.Items)
                {
                    if (!baselineManifest.Items.Any(i => i.File == item.File))
                    {
                        newItems.Add(item);
                    }
                }

                // Diff the manifest
                Dictionary<string, int> diff = ManifestComparer.Compare(baselineManifest, currentManifest);

                List<string> equalFiles = diff.Where(d => d.Value == 0).Select(d => d.Key).ToList();
                List<string> differingFiles = diff.Where(d => d.Value != 0).Select(d => d.Key).ToList();

                List<FileManifestItem> equalItems = baselineManifest.Items.Where(i => equalFiles.Any(f => i.File == f)).ToList();
                List<FileManifestItem> differingItems = baselineManifest.Items.Where(i => differingFiles.Any(f => i.File == f)).ToList();

                List<Tuple<FileManifestItem, FileManifestItem>> diffTuples = new List<Tuple<FileManifestItem, FileManifestItem>>();
                foreach (string differingFile in differingFiles)
                {
                    FileManifestItem baselineItem = baselineManifest.Items.First(i => i.File == differingFile);
                    FileManifestItem currentItem = currentManifest.Items.First(i => i.File == differingFile);
                    diffTuples.Add(new Tuple<FileManifestItem, FileManifestItem>(baselineItem, currentItem));
                }

                DiffReport report = new DiffReport()
                {
                    EquivalentItems = equalItems,
                    DifferingItems = diffTuples,
                    MissingItems = missingItems,
                    NewItems = newItems
                };

                // TODO: This needs to be an argument.
                FileWriter.WriteReport(report, reportFilePath);
            }

            return 0;
        }

        private static ManifestConfiguration ParseArguments(string[] args)
        {
            // Remove any dash prefix from arguments.
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].StartsWith("-"))
                {
                    string currentArg = args[i];
                    for (int j = 0; j < currentArg.Length; j++)
                    {
                        if (currentArg.Substring(j, 1) != "-")
                        {
                            args[i] = currentArg.Substring(j);
                            break;
                        }
                    }
                }
            }

            string rootDirectory = null;
            string manifestFilePath = null;
            string reportFilePath = null;
            bool build = false;
            bool check = false;

            if (args.Contains(DIRECTORY_ARG))
            {
                int directoryArgIndex = Array.IndexOf(args, DIRECTORY_ARG);

                if (directoryArgIndex != (args.Length - 1))
                {
                    rootDirectory = args[directoryArgIndex + 1];
                }
            }

            if (args.Contains(MANIFEST_FILE_ARG))
            {
                int fileArgIndex = Array.IndexOf(args, MANIFEST_FILE_ARG);

                if (fileArgIndex != (args.Length - 1))
                {
                    manifestFilePath = args[fileArgIndex + 1];
                }
            }

            if (args.Contains(REPORT_FILE_ARG))
            {
                int fileArgIndex = Array.IndexOf(args, REPORT_FILE_ARG);

                if (fileArgIndex != (args.Length - 1))
                {
                    reportFilePath = args[fileArgIndex + 1];
                }
            }

            if (args.Contains(BUILD_ARG))
            {
                build = true;
            }

            if (args.Contains(CHECK_ARG))
            {
                check = true;
            }

            ManifestConfiguration config = new ManifestConfiguration(rootDirectory, manifestFilePath, build, check, reportFilePath);

            return config;
        }

        private static bool ValidateConfiguration(ManifestConfiguration config)
        {
            if (string.IsNullOrEmpty(config.RootDirectory) ||
                string.IsNullOrEmpty(config.ManifestFilePath))
            {
                return false;
            }

            if (config.CheckManifest && string.IsNullOrEmpty(config.ReportFilePath))
            {
                return false;
            }

            if (config.BuildManifest == true && config.CheckManifest == true)
            {
                return false;
            }

            if (config.BuildManifest == false && config.CheckManifest == false)
            {
                return false;
            }

            return true;
        }

        private static void PrintConsoleHelp()
        {
            Console.WriteLine("\nManifest build and validation tool.");
            Console.WriteLine("\nParameters:");
            Console.WriteLine("\t--directory  The directory to build a manifest file from or compare a manifest file to.");
            Console.WriteLine("\t--file       The manifest file to create or use in comparison.");
            Console.WriteLine("\t--build      Instructs the application to build a manifest file.");
            Console.WriteLine("\t--check      Instructs the application to check a directory against a manifest file.");
            Console.WriteLine("\t--report     The report file to create. Only used with --check.");
            Console.WriteLine("\tNote: The build and check arguments cannot be used together.");
        }
    }
}
