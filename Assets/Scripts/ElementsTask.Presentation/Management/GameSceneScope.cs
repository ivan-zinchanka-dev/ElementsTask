using ElementsTask.Data.BlockFieldCore.Services;
using ElementsTask.Data.Levels.Services;
using ElementsTask.Data.PlayerProgression;
using ElementsTask.Data.PlayerProgression.Services;
using ElementsTask.Presentation.Balloons;
using ElementsTask.Presentation.BlockFieldCore.Services.Factories;
using ElementsTask.Presentation.BlockFieldCore.Views;
using ElementsTask.Presentation.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace ElementsTask.Presentation.Management
{
    public class GameSceneScope : LifetimeScope
    {
        [SerializeField] 
        private Camera _camera;
        [SerializeField]
        private GameStateMachine _gameStateMachine;
        [SerializeField] 
        private HeadUpDisplay _headUpDisplay;
        [SerializeField] 
        private BalloonViewsFactory _balloonViewsFactory;
        [SerializeField] 
        private BlockViewsFactory _blockViewsFactory;
        [SerializeField]
        private BlockFieldView _blockFieldView;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<IPlayerProgressService, PlayerProgressService>(Lifetime.Singleton);
            builder.Register<ILevelLoader, CsvLevelLoader>(Lifetime.Singleton);
            builder.Register<BlockFieldCreator>(Lifetime.Singleton);
            builder.Register<BalloonsManager>(Lifetime.Transient);
            
            builder.RegisterComponent<Camera>(_camera);
            builder.RegisterComponent<GameStateMachine>(_gameStateMachine);
            builder.RegisterComponent<HeadUpDisplay>(_headUpDisplay);
            builder.RegisterComponent<BalloonViewsFactory>(_balloonViewsFactory);
            builder.RegisterComponent<BlockViewsFactory>(_blockViewsFactory);
            builder.RegisterComponent<BlockFieldView>(_blockFieldView);
        }
    }
}