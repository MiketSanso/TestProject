using System;
using UniRx;
using UnityEngine;

public interface ICubeView
{
    Vector3 Position { get; }
    void Initialize(Color color, bool isInTower);
    void MoveToPosition(Vector3 position, Action onComplete);
    void MoveDown(float height);
    void Destroy();
    
    IObservable<Unit> DragStarted { get; }
    IObservable<Vector2> Dragging { get; }
    IObservable<Unit> DragEnded { get; }
}