using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CubeFactory : ICubeFactory
{
    private readonly DiContainer _container;
    private readonly GameConfig _config;
    private readonly Dictionary<CubeModel, ICubeView> _modelViewMap = new();
    private readonly Stack<ICubeView> _pool = new();

    public CubeFactory(DiContainer container, GameConfig config)
    {
        _container = container;
        _config = config;
    }

    public ICubeView CreateCube(bool isInTower)
    {
        ICubeView view = GetFromPool() ?? CreateNewCube();
        view.Initialize(GetRandomColor(), isInTower);
        return view;
    }

    private Color GetRandomColor() => 
        _config.CubeColors[Random.Range(0, _config.CubeColors.Length)];

    private ICubeView CreateNewCube()
    {
        var go = _container.InstantiatePrefab(_config.CubePrefab);
        return go.GetComponent<ICubeView>();
    }

    private ICubeView GetFromPool() => 
        _pool.Count > 0 ? _pool.Pop() : null;

    public ICubeView GetCubeView(CubeModel model) => 
        _modelViewMap.TryGetValue(model, out var view) ? view : null;

    public void ReturnToPool(ICubeView view)
    {
        _pool.Push(view);
        view.Destroy();
    }
}