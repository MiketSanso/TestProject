using UnityEngine;

public interface IGameView
{
    void ShowMessage(string message);
    Vector3 CalculateTowerPosition(CubeModel lastCube);
    bool IsPointInHole(Vector3 position);
    bool IsTowerFull(Vector3 position);
    float GetCubeHeight();
}