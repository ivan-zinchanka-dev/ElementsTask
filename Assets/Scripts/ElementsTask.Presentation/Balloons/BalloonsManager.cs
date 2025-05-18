using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using ElementsTask.Common.Animations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ElementsTask.Presentation.Balloons
{
    public class BalloonsManager : IDisposable
    {
        private const int MaxBalloons = 3;
        private readonly BalloonViewsFactory _balloonViewsFactory;
        private readonly List<BalloonView> _activeBalloons = new();
        
        public BalloonsManager(BalloonViewsFactory balloonViewsFactory)
        {
            _balloonViewsFactory = balloonViewsFactory;
        }
        
        public async UniTask StartAsync(Transform parent, CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_activeBalloons.Count < MaxBalloons)
                {
                    BalloonView balloon = _balloonViewsFactory.CreateBalloonView(BalloonKind.Blue, parent);

                    balloon.Animation.FlyToRightAsync(new HorizontalSinusAnimation.Data()).ContinueWith(() =>
                    {
                        _activeBalloons.Remove(balloon);
                        DestroyBalloon(balloon);
                    });

                    _activeBalloons.Add(balloon);
                }

                await UniTask.Delay(TimeSpan.FromSeconds(5), DelayType.DeltaTime, cancellationToken: stoppingToken);
            }
        }

        private void DestroyBalloon(BalloonView balloon)
        {
            if (balloon != null)
            {
                Object.Destroy(balloon.gameObject);
            }
        }

        public void Dispose()
        {
            foreach (BalloonView balloon in _activeBalloons)
            {
                DestroyBalloon(balloon);
            }
            
            _activeBalloons.Clear();
        }
    }
}