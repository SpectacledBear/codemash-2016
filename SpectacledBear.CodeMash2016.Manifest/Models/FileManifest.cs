using System.Collections.Generic;

namespace SpectacledBear.CodeMash2016.Manifest.Models
{
    public class FileManifest
    {
        [Newtonsoft.Json.JsonProperty("Manifest")]
        public List<FileManifestItem>Items { get; set; }
    }
}
