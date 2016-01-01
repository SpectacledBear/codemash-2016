using System.Collections.Generic;
using System.Linq;
using SpectacledBear.CodeMash2016.Manifest.Models;

namespace SpectacledBear.CodeMash2016.Manifest
{
    internal class ManifestComparer
    {
        internal static Dictionary<string, int> Compare(FileManifest baselineManifest, FileManifest comparisonManifest) {
            List<FileManifestItem> comparableItems = new List<FileManifestItem>();
            foreach(FileManifestItem item in baselineManifest.Items)
            {
                if(comparisonManifest.Items.Any(i => i.File == item.File))
                {
                    comparableItems.Add(item);
                }
            }

            Dictionary<string, int> diffedItems = new Dictionary<string, int>();
            foreach (FileManifestItem item in comparableItems)
            {
                string filename = item.File;

                FileManifestItem comparableItem = comparisonManifest.Items.First(i => i.File == filename);

                int result = item.CompareTo(comparableItem);

                diffedItems.Add(filename, result);
            }

            return diffedItems;
        }
    }
}
