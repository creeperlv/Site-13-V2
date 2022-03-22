using Newtonsoft.Json;
using Site13Kernel.IO.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Site13Kernel.IO
{
    public static class FileIO
    {
        public readonly static JsonSerializerSettings settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Include,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SerializeToFile<T>(T value, StorageFile file)
        {
            WriteAllText(file, JsonConvert.SerializeObject(value, settings));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T DeserializeFromFile<T>(StorageFile file)
        {
            return JsonConvert.DeserializeObject<T>(ReadAllText(file));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ReadAllText(StorageFile file)
        {
            using (var s = file.OpenRead())
            {
                using (StreamReader sr = new StreamReader(s))
                {
                    return sr.ReadToEnd();
                }
            }
        }
        /// <summary>
        /// Perform: Delete then write.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="content"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteAllText(StorageFile file, string content)
        {
            if (file.Exist)
                file.Delete();
            using (var s = file.Open())
            {
                using (StreamWriter SW = new StreamWriter(s))
                {
                    SW.Write(content);
                    SW.Flush();
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadLines(StorageFile file, Action<string> Processor)
        {
            using (var s = file.OpenRead())
            {
                using (StreamReader sr = new StreamReader(s))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        Processor(line);
                    }
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<string> ReadLines(StorageFile file)
        {
            List<string> contents = new List<string>();
            using (var s = file.OpenRead())
            {
                using (StreamReader sr = new StreamReader(s))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        contents.Add(line);
                    }
                }
            }
            return contents;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<string> PipedReadLines(StorageFile file, Func<string, string> Pipe)
        {
            List<string> contents = new List<string>();
            using (var s = file.OpenRead())
            {
                using (StreamReader sr = new StreamReader(s))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        contents.Add(Pipe(line));
                    }
                }
            }
            return contents;
        }
    }
}
