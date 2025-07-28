using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public class SaveService : ISaveService
{
    private const string SAVE_FILE = "game_save.json";
    
    public void Save(GameModel model)
    {
        var saveData = new {
            TowerCubes = model.TowerCubes.Select(c => new {
                Position = c.Position,
                Color = new float[] { c.Color.r, c.Color.g, c.Color.b, c.Color.a }
            }).ToList()
        };
        
        File.WriteAllText(GetSavePath(), JsonConvert.SerializeObject(saveData));
    }

    public void Load(GameModel model)
    {
        string path = GetSavePath();
        if (!File.Exists(path)) return;
        
        var json = File.ReadAllText(path);
        var saveData = JsonConvert.DeserializeObject<SaveData>(json);
        
        foreach (var cubeData in saveData.TowerCubes)
        {
            model.TowerCubes.Add(new CubeModel {
                Position = cubeData.Position,
                Color = new Color(
                    cubeData.Color[0], 
                    cubeData.Color[1], 
                    cubeData.Color[2], 
                    cubeData.Color[3]
                ),
                IsInTower = true
            });
        }
    }
    
    private string GetSavePath() => 
        Path.Combine(Application.persistentDataPath, SAVE_FILE);
    
    [System.Serializable]
    private class SaveData
    {
        public List<CubeSaveData> TowerCubes;
    }
    
    [System.Serializable]
    private class CubeSaveData
    {
        public Vector3 Position;
        public float[] Color;
    }
}