using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace 上野迅_インターン_20250513
{
    [DataContract]
    public class AudioFilesInfo
    {
        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public string Date { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public string Size { get; set; }
        [DataMember]
        public string FilePath { get; set; }
        [DataMember]
        public double LastPositionSeconds { get; set; }
    }

    public static class AudioUtils
    {
        public static void SaveListToFile(string filePath, List<AudioFilesInfo> list)
        {
            var serializer = new DataContractJsonSerializer(typeof(List<AudioFilesInfo>));
            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                serializer.WriteObject(fs, list);
            }
        }

        public static List<AudioFilesInfo> LoadListFromFile(string filePath)
        {
            if (!File.Exists(filePath)) return new List<AudioFilesInfo>();
            var serializer = new DataContractJsonSerializer(typeof(List<AudioFilesInfo>));
            using (var fs = new FileStream(filePath, FileMode.Open))
            {
                return (List<AudioFilesInfo>)serializer.ReadObject(fs);
            }
        }

        public static AudioFilesInfo CreateAudioFilesInfo(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            return new AudioFilesInfo
            {
                FileName = fileInfo.Name,
                Date = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                Type = "." + fileInfo.Extension.TrimStart('.').ToLower(),
                Size = (fileInfo.Length / 1024.0 / 1024.0).ToString("F1") + "MB",
                FilePath = filePath
            };
        }
    }
}
