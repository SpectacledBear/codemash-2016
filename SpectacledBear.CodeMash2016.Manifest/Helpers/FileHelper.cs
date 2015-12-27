using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace SpectacledBear.CodeMash2016.Manifest.Helpers
{
    internal static class FileHelper
    {
        internal static List<string> GetDirsFromRootDir(string rootDirPath)
        {
            List<string> dirs = new List<string>();

            dirs.Add(rootDirPath);
            dirs.AddRange(GetDirsFromPath(rootDirPath));

            dirs.Distinct().ToList();
            dirs.Sort();

            return dirs;
        }

        internal static List<string> GetFilesFromDirs(List<string> dirs)
        {
            List<string> files = new List<string>();

            foreach (string dir in dirs)
            {
                files.AddRange(Directory.GetFiles(dir));
            }

            return files;
        }

        internal static string GetVersionsFromFile(string filePath)
        {
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(filePath);
            string version = versionInfo.FileVersion;

            return version;
        }

        internal static string GetChecksumFromFile(string filePath)
        {
            byte[] fileData = File.ReadAllBytes(filePath);

            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(fileData);
                string checksum = BitConverter.ToString(hash).Replace("-", string.Empty);

                return checksum;
            }
        }

        private static List<string> GetDirsFromPath(string dirPath)
        {
            string[] dirs = Directory.GetDirectories(dirPath);

            if (dirs.Length == 0)
            {
                return new List<string>();
            }

            List<string> dirList = new List<string>(dirs);

            foreach (string dir in dirs)
            {
                dirList.AddRange(GetDirsFromPath(dir));
            }

            return dirList;
        }
    }
}
