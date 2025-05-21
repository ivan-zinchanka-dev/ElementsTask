using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using ElementsTask.Common.Animations;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

namespace ElementsTask.Presentation.Balloons
{
    public class BalloonsDecorator : MonoBehaviour
    {
        [SerializeField]
        private int _maxBalloons = 3;
        [SerializeField]
        private HorizontalSinusAnimation.Data _animationData;
        [SerializeField] 
        private RectTransform _area;
        
        [Inject]
        private BalloonViewsFactory _balloonViewsFactory;
        
        private readonly List<BalloonView> _activeBalloons = new();
        private Vector3[] _cachedAreaCorners;
        
        public async UniTask StartAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_activeBalloons.Count < _maxBalloons)
                {
                    if (TwoVariantsRandom())
                    {
                        BalloonView balloon = _balloonViewsFactory.CreateBalloonView(
                            GetRandomBalloonKind(), 
                            GetRandomPosition(false),
                            Quaternion.identity,
                            transform);
                        
                        balloon.Animation.FlyToRightAsync(_animationData).ContinueWith(() =>
                        {
                            _activeBalloons.Remove(balloon);
                            DestroyBalloon(balloon);
                        });
                        
                        _activeBalloons.Add(balloon);
                    }
                    else
                    {
                        BalloonView balloon = _balloonViewsFactory.CreateBalloonView(
                            GetRandomBalloonKind(), 
                            GetRandomPosition(true),
                            Quaternion.identity,
                            transform);
                        
                        balloon.Animation.FlyToLeftAsync(_animationData).ContinueWith(() =>
                        {
                            _activeBalloons.Remove(balloon);
                            DestroyBalloon(balloon);
                        });
                        
                        _activeBalloons.Add(balloon);
                    }



                    
                }

                await UniTask.Delay(TimeSpan.FromSeconds(5), DelayType.DeltaTime, cancellationToken: stoppingToken);
            }
        }

        private void Start()
        {
            _cachedAreaCorners = new Vector3[4]; 
            _area.GetWorldCorners(_cachedAreaCorners);
            
            StartAsync(CancellationToken.None).Forget();
        }

        private BalloonKind GetRandomBalloonKind()
        {
            return (BalloonKind)Random.Range(1, 3);
        }
        
        private Vector3 GetRandomPosition(bool forRightBalloon)
        {
            if (forRightBalloon)
            {
                return new Vector3(
                    _cachedAreaCorners[2].x, 
                    Random.Range(_cachedAreaCorners[2].y, _cachedAreaCorners[3].y));
            }
            else
            {
                return new Vector3(
                    _cachedAreaCorners[0].x, 
                    Random.Range(_cachedAreaCorners[0].y, _cachedAreaCorners[1].y));
            }
        }

        private bool TwoVariantsRandom()
        {
            return Random.Range(0, 2) == 0;
        }

        private void DestroyBalloon(BalloonView balloon)
        {
            if (balloon != null)
            {
                Destroy(balloon.gameObject);
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