using System.IO;

namespace Database.CD.Lib
{
    internal class Migration
    {
        private string _content;

        public string Name { get; private set; }

        public string FilePath { get; }

        public string Version { get; private set; }

        public string Content => _content ?? (_content = File.ReadAllText(FilePath));

        public Migration(FileInfo fileInfo)
        {
            var version = BuildVersionName(fileInfo.FullName);
            Version = version;
            Name = fileInfo.Name;
            FilePath = fileInfo.FullName;
        }

        private static string BuildVersionName(string fullname)
        {
            var names = fullname.Split('\\');
            var version = string.Empty;

            for (int i = names.Length - 1; i >= 0; i--)
            {
                if (names[i] == "Migrations" || names[i] == "Rollbacks")
                {
                    break;
                }

                version = string.IsNullOrEmpty(version) ? version : $".{version}";
                version = $"{names[i].Split('_')[0]}{version}";
            }

            return version;
        }
    }
}
