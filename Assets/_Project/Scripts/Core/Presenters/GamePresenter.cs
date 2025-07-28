using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;

public class GamePresenter : IInitializable, IDisposable
{
    private readonly GameModel _model;
    private readonly IGameView _view;
    private readonly ICubeFactory _cubeFactory;
    private readonly ISaveService _saveService;
    private readonly SignalBus _signalBus;
    private readonly CompositeDisposable _disposables = new();

    [Inject]
    public GamePresenter(
        GameModel model,
        IGameView view,
        ICubeFactory cubeFactory,
        ISaveService saveService,
        SignalBus signalBus)
    {
        _model = model;
        _view = view;
        _cubeFactory = cubeFactory;
        _saveService = saveService;
        _signalBus = signalBus;
    }

    public void Initialize()
    {
        _signalBus.GetStream<CubeDroppedSignal>()
            .Subscribe(HandleCubeDrop)
            .AddTo(_disposables);
        
        _model.Message.Subscribe(_view.ShowMessage).AddTo(_disposables);
        
        _saveService.Load(_model);
        CreateInitialCubes();
    }

    private void CreateInitialCubes()
    {
        for (int i = 0; i < _view.Config.InitialCubeCount; i++)
        {
            CreateBottomCube();
        }
    }

    private void CreateBottomCube()
    {
        var cube = _cubeFactory.CreateCube(false);
        var model = new CubeModel
        {
            Position = cube.RectTransform.anchoredPosition,
            Color = cube.Color,
            Index = cube.Index
        };
        
        _model.BottomCubes.Add(model);
    }

    private void HandleCubeDrop(CubeDroppedSignal signal)
    {
        var cube = signal.CubeView;
        var screenPos = _view.GetScreenPosition(cube.RectTransform);
        
        if (_view.IsPointInHole(screenPos)) 
        {
            RemoveCubeFromTower(cube);
            return;
        }
        
        if (_view.IsTowerFull(screenPos))
        {
            _model.Message.Value = "Tower is full!";
            cube.Destroy();
            CreateBottomCube();
            return;
        }
        
        if (_view.IsOverTower(screenPos))
        {
            AddCubeToTower(cube);
        }
        else
        {
            _model.Message.Value = "Cube missed!";
            cube.Destroy();
            CreateBottomCube();
        }
    }

    private void AddCubeToTower(CubeView cube)
    {
        var lastCube = _model.TowerCubes.LastOrDefault();
        var position = _view.CalculateTowerPosition(lastCube);
        
        cube.MoveToPosition(position);
        
        var model = new CubeModel
        {
            Position = position,
            Color = cube.Color,
            Index = cube.Index,
            IsInTower = true
        };
        
        _model.TowerCubes.Add(model);
        _model.Message.Value = "Cube added to tower!";
        _saveService.Save(_model);
        CreateBottomCube();
    }

    private void RemoveCubeFromTower(CubeView cube)
    {
        var cubeModel = _model.TowerCubes.FirstOrDefault(c => c.Index == cube.Index);
        if (cubeModel == null) return;
        
        int index = _model.TowerCubes.IndexOf(cubeModel);
        _model.TowerCubes.RemoveAt(index);
        
        cube.Destroy();
        _model.Message.Value = "Cube removed from tower!";
        
        for (int i = index; i < _model.TowerCubes.Count; i++)
        {
            var cubeToMove = _cubeFactory.GetCubeView(_model.TowerCubes[i]);
            if (cubeToMove != null)
            {
                var newPos = new Vector2(
                    _model.TowerCubes[i].Position.x,
                    _model.TowerCubes[i].Position.y - _view.Config.CubeSize
                );
                
                cubeToMove.MoveToPosition(newPos);
                _model.TowerCubes[i].Position = newPos;
            }
        }
        
        _saveService.Save(_model);
    }

    public void Dispose() => _disposables.Dispose();
}