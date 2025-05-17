using ElementsTask.Core.Services;
using ElementsTask.Presentation.Services.Factories;
using ElementsTask.Presentation.Views;
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
        private BlockViewsFactory _blockViewsFactory;
        [SerializeField]
        private BlockFieldView _blockFieldView;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<IPlayerProgressService, PlayerProgressService>(Lifetime.Singleton);
            builder.Register<ILevelLoader, CsvLevelLoader>(Lifetime.Singleton);
            builder.Register<BlockFieldCreator>(Lifetime.Singleton);
            
            builder.RegisterComponent<Camera>(_camera);
            builder.RegisterComponent<GameStateMachine>(_gameStateMachine);
            builder.RegisterComponent<BlockViewsFactory>(_blockViewsFactory);
            builder.RegisterComponent<BlockFieldView>(_blockFieldView);
        }
    }
}