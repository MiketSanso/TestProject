using UnityEngine;
using Zenject;

public class GameView : MonoBehaviour, IGameView
{
    [SerializeField] private Collider2D _holeCollider;
    [SerializeField] private Transform _towerBase;
    
    private GameConfig _config;
    private float _cubeHeight;

    [Inject]
    private void Construct(GameConfig config)
    {
        _config = config;
        _cubeHeight = config.CubePrefab.GetComponent<SpriteRenderer>().bounds.size.y;
    }

    public void ShowMessage(string message) => Debug.Log(message);

    public Vector3 CalculateTowerPosition(CubeModel lastCube)
    {
        if (lastCube == null) return _towerBase.position;
        
        float offset = Random.Range(-_config.MaxOffset, _config.MaxOffset);
        return new Vector3(
            lastCube.Position.x + offset,
            lastCube.Position.y + _cubeHeight,
            0
        );
    }

    public bool IsPointInHole(Vector3 position) => _holeCollider.OverlapPoint(position);

    public bool IsTowerFull(Vector3 position) => position.y > _config.MaxTowerHeight;

    public float GetCubeHeight() => _cubeHeight;
}