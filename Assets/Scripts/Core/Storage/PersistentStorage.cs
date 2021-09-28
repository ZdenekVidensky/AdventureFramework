namespace TVB.Core.Storage
{
    using System.IO;

    using UnityEngine;

    public static class PersistentStorage
    {
        public static void Save(object data, string relativePath)
        {
            string json    = JsonUtility.ToJson(data, false);
            string path = GetPath(relativePath);

            File.WriteAllText(path, json);
        }

        public static T Load<T>(string relativePath)
        {
            string path = GetPath(relativePath);

            if (File.Exists(path) == false)
                return default;

            return JsonUtility.FromJson<T>(File.ReadAllText(path));
        }

        public static bool Exists(string relativePath)
        {
            string path = GetPath(relativePath);
            return File.Exists(path);
        }

        private static string GetPath(string relativePath)
        {
            return $"{Application.persistentDataPath}/{relativePath}";
        }
    }
}
