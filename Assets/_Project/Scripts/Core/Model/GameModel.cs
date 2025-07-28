using System.Collections.Generic;
using UniRx;

public class GameModel
{
    public ReactiveProperty<string> Message { get; } = new("");
    public List<CubeModel> TowerCubes { get; } = new();
    public List<CubeModel> BottomCubes { get; } = new();
    public ReactiveProperty<float> ScrollPosition { get; } = new(0);
}