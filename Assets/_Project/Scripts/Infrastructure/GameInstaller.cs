using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private RectTransform _holeArea;
    [SerializeField] private RectTransform _towerContainer;
    [SerializeField] private RectTransform _bottomContainer;
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private TMPro.TextMeshProUGUI _messageText;

    public override void InstallBindings()
    {
        // Конфиг
        Container.BindInstance(_gameConfig);
        
        // Модели
        Container.Bind<GameModel>().AsSingle();
        
        // Сервисы
        Container.Bind<ICubeFactory>().To<CubeFactory>().AsSingle();
        Container.Bind<ISaveService>().To<SaveService>().AsSingle();
        
        // Сигналы
        Container.DeclareSignal<CubeDroppedSignal>();
        Container.DeclareSignal<ScrollLockSignal>();
        
        // UI элементы
        Container.BindInstance(_holeArea).WithId("HoleArea");
        Container.BindInstance(_towerContainer).WithId("TowerContainer");
        Container.BindInstance(_bottomContainer).WithId("BottomContainer");
        Container.BindInstance(_messageText).WithId("MessageText");
        
        // Контроллеры
        Container.BindInterfacesAndSelfTo<ScrollController>().AsSingle()
            .WithArguments(_scrollRect);
        
        // Презентер
        Container.BindInterfacesAndSelfTo<GamePresenter>().AsSingle().NonLazy();
    }
}