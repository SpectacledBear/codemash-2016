namespace SpectacledBear.CodeMash2016.Manifest.Models
{
    internal class ManifestConfiguration
    {
        internal string RootDirectory { get; }

        internal string ManifestFilePath { get; }

        internal bool BuildManifest { get; }

        internal bool CheckManifest { get; }

        internal ManifestConfiguration(string rootDirectory, string manifestFilePath, bool buildManifest, bool checkManifest)
        {
            RootDirectory = rootDirectory;
            ManifestFilePath = manifestFilePath;
            BuildManifest = buildManifest;
            CheckManifest = checkManifest;
        }
    }
}
