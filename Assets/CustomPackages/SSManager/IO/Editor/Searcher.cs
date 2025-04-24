using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SSManager.IO
{
    public class Searcher
    {
        public enum PathType
        {
            Absolute,
            Relative
        }

        private static List<FileInfo> SearchFile(DirectoryInfo dir, string fileName)
        {
            List<FileInfo> foundItems = dir.GetFiles(fileName).ToList();
            DirectoryInfo[] dis = dir.GetDirectories();
            foreach (DirectoryInfo di in dis)
            {
                foundItems.AddRange(SearchFile(di, fileName));
            }

            return foundItems;
        }

        public static string SearchFileInProject(string fileName, PathType pathType = PathType.Absolute)
        {
            string result = SearchFileInPackages(fileName, pathType);
            result ??= SearchFileInAssets(fileName, pathType);

            return result;
        }

        public static string SearchFileInAssets(string fileName, PathType pathType = PathType.Absolute)
        {
            DirectoryInfo di = new DirectoryInfo(Application.dataPath);
            List<FileInfo> fis = SearchFile(di, fileName);

            if (fis.Count >= 1)
            {
                switch (pathType)
                {
                    case PathType.Absolute:
                        return fis[0].FullName;

                    case PathType.Relative:
                        var fullPath = fis[0].FullName;
                        var assetIndex = fullPath.LastIndexOf("Assets");

                        if (assetIndex >= 0)
                        {
                            return fullPath.Substring(assetIndex);
                        }
                        return fullPath;
                }
            }

            return null;
        }

        public static string SearchFileInPackages(string fileName, PathType pathType = PathType.Absolute)
        {
            string dataPath = Application.dataPath;
            dataPath = dataPath.Replace("Assets", "Library/PackageCache");
            DirectoryInfo di = new DirectoryInfo(dataPath);
            DirectoryInfo[] dis = di.GetDirectories();
            DirectoryInfo packageDir = dis.FirstOrDefault(d => d.Name.Contains("thanhdv.ssmanager"));

            if (packageDir == null) return null;

            List<FileInfo> fis = SearchFile(packageDir, fileName);

            if (fis.Count >= 1)
            {
                switch (pathType)
                {
                    case PathType.Absolute:
                        return fis[0].FullName;

                    case PathType.Relative:
                        var fullPath = fis[0].FullName;
                        var assetIndex = fullPath.LastIndexOf("Packages");

                        if (assetIndex >= 0)
                        {
                            return fullPath.Substring(assetIndex);
                        }
                        return fullPath;
                }
            }

            return null;
        }
    }
}
