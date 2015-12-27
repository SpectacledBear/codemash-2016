using System.Collections.Generic;
using System.IO;
using SpectacledBear.CodeMash2016.Manifest.Helpers;
using SpectacledBear.CodeMash2016.Manifest.Models;

namespace SpectacledBear.CodeMash2016.Manifest
{
    internal static class FileManifestFactory
    {
        internal static FileManifest Create(string rootDirPath)
        {
            List<string> dirs = FileHelper.GetDirsFromRootDir(rootDirPath);
            List<string> files = FileHelper.GetFilesFromDirs(dirs);
            List<FileManifestItem> items = CreateManifestItemsFromFiles(files, rootDirPath);

            foreach (FileManifestItem item in items)
            {
                item.Version = FileHelper.GetVersionsFromFile(rootDirPath + @"\" + item.File);
                item.Checksum = FileHelper.GetChecksumFromFile(rootDirPath + @"\" + item.File);
            }

            FileManifest manifest = new FileManifest { Items = items };

            return manifest;
        }

        private static List<FileManifestItem> CreateManifestItemsFromFiles(List<string> files, string rootDirPath)
        {
            List<FileManifestItem> items = new List<FileManifestItem>();

            foreach (string file in files)
            {
                string finalFilePath = file;

                if (file.StartsWith(rootDirPath))
                {
                    finalFilePath = finalFilePath.Substring(rootDirPath.Length + 1);
                }

                FileManifestItem item = new FileManifestItem { File = finalFilePath };
                items.Add(item);
            }

            return items;
        }
    }
}
