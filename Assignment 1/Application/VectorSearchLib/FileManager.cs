using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace VectorSearchLib
{
    public static class FileManager
    {
        public static void SaveToJson<T>(IEnumerable<T> items, string path)
        {
            var json = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });
        //File.WriteAllText(path, json);
        // Ensure the directory exists before writing the file
        var directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            File.WriteAllText(path, json);

        }
        //C: \Users\kalsa\OneDrive\Backup storage\OneDrive\Desktop\MSc computer science\notes\dotnet - notes\Assignment 1\test

        public static List<T> LoadFromJson<T>(string path)
        {
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<T>>(json);
        }
    }
}
