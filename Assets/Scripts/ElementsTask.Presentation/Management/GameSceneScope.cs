using ElementsTask.Common.Data.Crypto.Abstractions;
using ElementsTask.Common.Data.Crypto.ROS;
using ElementsTask.Data.BlockFieldCore.Services;
using ElementsTask.Data.Levels.Services;
using ElementsTask.Data.PlayerProgression.Services;
using ElementsTask.Presentation.Balloons;
using ElementsTask.Presentation.Balloons.Factories;
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
        [Header("Settings")]
        [SerializeField] 
        private bool _useCryptography;
        
        [Header("Services")]
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
        [SerializeField]
        private RosCryptoConfig _cryptoConfig;
        
        protected override void Configure(IContainerBuilder builder)
        {
            if (_useCryptography)
            {
                builder.RegisterInstance<IRosCryptoConfig>(_cryptoConfig);
                builder.Register<IStreamCryptoService, RosCryptoService>(Lifetime.Singleton);
            }
            else
            {
                builder.Register<IStreamCryptoService, FakeCryptoService>(Lifetime.Singleton);
            }

            builder.Register<IPlayerProgressService, PlayerProgressService>(Lifetime.Singleton);
            builder.Register<IBuildInLevelLoader, BuildInLevelLoader>(Lifetime.Singleton);
            builder.Register<ISavedLevelLoader, SavedLevelLoader>(Lifetime.Singleton);
            builder.Register<BlockFieldCreator>(Lifetime.Singleton);
            
            builder.RegisterComponent<Camera>(_camera);
            builder.RegisterComponent<GameStateMachine>(_gameStateMachine);
            builder.RegisterComponent<HeadUpDisplay>(_headUpDisplay);
            builder.RegisterComponent<BalloonViewsFactory>(_balloonViewsFactory);
            builder.RegisterComponent<BlockViewsFactory>(_blockViewsFactory);
            builder.RegisterComponent<BlockFieldView>(_blockFieldView);
        }
    }
}