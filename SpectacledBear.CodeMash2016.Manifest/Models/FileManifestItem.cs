using System;

namespace SpectacledBear.CodeMash2016.Manifest.Models
{
    public class FileManifestItem : IComparable<FileManifestItem>
    {
        public string File { get; set; }

        public string Version { get; set; }

        public string Checksum { get; set; }

        public int CompareTo(FileManifestItem item)
        {
            int result = 0;

            if (item.Checksum != Checksum)
            {
                return 1;
            }

            // Diff is failing when version is null!!!
            if (!string.IsNullOrEmpty(Version) && string.IsNullOrEmpty(item.Version))
            {
                return 1;
            }

            if (!string.IsNullOrEmpty(item.Version) && string.IsNullOrEmpty(Version))
            {
                return -1;
            }

            if (string.IsNullOrEmpty(item.Version) && string.IsNullOrEmpty(Version))
            {
                return 0;
            }

            string[] separator = new string[] { "." };
            string[] versionSegments = Version.Split(separator, StringSplitOptions.None);
            string[] itemVersionSegments = item.Version.Split(separator, StringSplitOptions.None);

            for (int i = 0; (i < versionSegments.Length && i < itemVersionSegments.Length); i++)
            {
                int segmentResult = versionSegments[i].CompareTo(itemVersionSegments[i]);
                if (segmentResult != 0)
                {
                    result = segmentResult;
                    break;
                }
            }

            return result;
        }
    }
}
