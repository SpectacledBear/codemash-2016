using System.IO;
using Newtonsoft.Json;
using SpectacledBear.CodeMash2016.Manifest.Models;

namespace SpectacledBear.CodeMash2016.Manifest
{
    internal class FileWriter
    {
        internal static void WriteManifest(FileManifest manifest, string filePath)
        {
            string json = JsonConvert.SerializeObject(manifest, Formatting.Indented);

            File.WriteAllText(filePath, json);
        }

        internal static void WriteReport(DiffReport report, string filePath)
        {
            string json = JsonConvert.SerializeObject(report, Formatting.Indented);

            File.WriteAllText(filePath, json);
        }
    }
}
