using System.IO;
using Newtonsoft.Json;
using SpectacledBear.CodeMash2016.Manifest.Models;

namespace SpectacledBear.CodeMash2016.Manifest
{
    internal class ManifestReader
    {
        internal static FileManifest ReadManifest(string filePath)
        {
            string json = File.ReadAllText(filePath);

            FileManifest manifest = JsonConvert.DeserializeObject<FileManifest>(json);

            return manifest;
        }
    }
}
