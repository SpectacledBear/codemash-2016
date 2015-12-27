using System;
using System.Collections.Generic;

namespace SpectacledBear.CodeMash2016.Manifest.Models
{
    public class DiffReport
    {
        public List<FileManifestItem> EquivalentItems { get; set; }

        public List<Tuple<FileManifestItem, FileManifestItem>> DifferingItems { get; set; }

        public List<FileManifestItem> NewItems { get; set; }

        public List<FileManifestItem> MissingItems { get; set; }
    }
}
