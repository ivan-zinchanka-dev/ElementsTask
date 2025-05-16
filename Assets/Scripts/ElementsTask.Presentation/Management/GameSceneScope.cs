using ElementsTask.Core.Services;
using ElementsTask.Presentation.Services.Factories;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Grid = ElementsTask.Presentation.Services.Grid;

namespace ElementsTask.Presentation.Management
{
    public class GameSceneScope : LifetimeScope
    {
        [SerializeField] 
        private BlockViewsFactory _blockViewsFactory;
        [SerializeField]
        private Grid _grid;
        [SerializeField]
        private GameSceneStartup _gameSceneStartup;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<BlockFieldCreator>(Lifetime.Singleton);
            builder.RegisterComponent<BlockViewsFactory>(_blockViewsFactory);
            builder.RegisterComponent<Grid>(_grid);
            builder.RegisterComponent<GameSceneStartup>(_gameSceneStartup);
        }
    }
}