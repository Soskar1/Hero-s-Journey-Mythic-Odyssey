using HerosJourney.Core.WorldGeneration.Voxels;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Structures.Builder
{
    public static class StructureSaveLoad
    {
        public const string RESOURCES_PATH = "/Resources/";
        public const string EXTENSION = ".json";

        public static void SaveStructure(Dictionary<Vector3Int, VoxelData> structureVoxelData, string fileName)
        {
            string directory = Application.dataPath + RESOURCES_PATH;

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            string json = JsonConvert.SerializeObject(structureVoxelData);
            File.WriteAllText(directory + fileName + EXTENSION, json);
        }

        public static Dictionary<Vector3Int, VoxelData> LoadStructure(string fileName)
        {
            Dictionary<Vector3Int, VoxelData> structureVoxelData = new Dictionary<Vector3Int, VoxelData>();

            string path = Application.dataPath + RESOURCES_PATH + fileName + EXTENSION;
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                structureVoxelData = JsonConvert.DeserializeObject<Dictionary<Vector3Int, VoxelData>>(json);
            }
            else 
            {
                Debug.LogError($"File {fileName + EXTENSION} does not exist. Path: {path}");
            }

            return structureVoxelData;  
        }
    }
}