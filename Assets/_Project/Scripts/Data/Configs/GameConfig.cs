using UnityEngine;

[CreateAssetMenu(menuName = "Configs/GameConfig")]
public class GameConfig : ScriptableObject
{
    public GameObject CubePrefab;
    public Color[] CubeColors;
    public int InitialCubeCount = 20;
    public float MaxTowerHeight = 8f;
    public float MaxOffset = 0.5f;
    public float CubeSize = 100f;
    public float ScrollSensitivity = 15f;
}