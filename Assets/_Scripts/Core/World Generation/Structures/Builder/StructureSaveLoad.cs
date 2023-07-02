using HerosJourney.Core.WorldGeneration.Voxels;
using HerosJourney.Utils;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace HerosJourney.Core.WorldGeneration.Structures.Builder
{
    public static class StructureSaveLoad
    {
        public const string RESOURCES_PATH = "/Resources/";
        public const string EXTENSION = ".json";

        public static void SaveStructure(List<VoxelSaveData> voxelSaveData, string fileName)
        {
            string directory = Application.dataPath + RESOURCES_PATH;

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            string json = JsonConvert.SerializeObject(voxelSaveData);
            File.WriteAllText(directory + fileName + EXTENSION, json);
        }

        public static List<VoxelSaveData> LoadStructure(string fileName)
        {
            List<VoxelSaveData> voxelSaveData = new List<VoxelSaveData>();
            string path = Application.dataPath + RESOURCES_PATH + fileName + EXTENSION;

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                voxelSaveData = JsonConvert.DeserializeObject<List<VoxelSaveData>>(json);
            }
            else
            {
                Debug.LogError($"File {path} does not exist");
            }

            return voxelSaveData;
        }

        public static List<VoxelSaveData> LoadStructure(TextAsset json) => JsonConvert.DeserializeObject<List<VoxelSaveData>>(json.text);
    }
}