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
        private BlockViewsFactory _blockViewsFactory;
        [SerializeField]
        private BlockFieldView _blockFieldView;
        [SerializeField]
        private GameSceneStartup _gameSceneStartup;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent<Camera>(_camera);
            builder.Register<BlockFieldCreator>(Lifetime.Singleton);
            builder.RegisterComponent<BlockViewsFactory>(_blockViewsFactory);
            builder.RegisterComponent<BlockFieldView>(_blockFieldView);
            builder.RegisterComponent<GameSceneStartup>(_gameSceneStartup);
        }
    }
}