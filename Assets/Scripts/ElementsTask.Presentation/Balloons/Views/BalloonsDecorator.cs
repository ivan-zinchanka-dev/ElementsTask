using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using ElementsTask.Common.Animations;
using ElementsTask.Presentation.Balloons.Enums;
using ElementsTask.Presentation.Balloons.Factories;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

namespace ElementsTask.Presentation.Balloons.Views
{
    public class BalloonsDecorator : MonoBehaviour
    {
        [SerializeField]
        private int _maxBalloons = 3;
        
        [SerializeField]
        [MinMaxSlider(0f, 10f)]
        private Vector2 _spawnDelaySecondsRange = new Vector2(2f, 6f);
        
        [SerializeField]
        [MinMaxSlider(0f, 3f)]
        private Vector2 _balloonSpeedFactorRange = new Vector2(0.5f, 1.5f);
        
        [SerializeField] 
        private RectTransform _area;
        [SerializeField]
        private HorizontalSinusAnimation.Data _animationData;
        
        [Inject]
        private BalloonViewsFactory _balloonViewsFactory;
        
        private readonly List<BalloonView> _activeBalloons = new();
        private Vector3[] _cachedAreaCorners;

        private void Reset()
        {
            _area = GetComponent<RectTransform>();
            _animationData = new HorizontalSinusAnimation.Data()
            {
                Duration = 20f,
                Distance = 9f,
                Frequency = 1f,
                Amplitude = 0.75f,
            };
        }

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
                        
                        balloon.Animation.FlyToRightAsync(RandomizeBalloonSpeed(_animationData)).ContinueWith(() =>
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
                        
                        balloon.Animation.FlyToLeftAsync(RandomizeBalloonSpeed(_animationData)).ContinueWith(() =>
                        {
                            _activeBalloons.Remove(balloon);
                            DestroyBalloon(balloon);
                        });
                        
                        _activeBalloons.Add(balloon);
                    }
                }

                await UniTask.Delay(GetRandomDelay(), DelayType.DeltaTime, cancellationToken: stoppingToken);
            }
        }

        private void Start()
        {
            _cachedAreaCorners = new Vector3[4]; 
            _area.GetWorldCorners(_cachedAreaCorners);
            
            StartAsync(CancellationToken.None).Forget();
        }

        private HorizontalSinusAnimation.Data RandomizeBalloonSpeed(HorizontalSinusAnimation.Data animationData)
        {
            float duration = animationData.Duration *
                             Random.Range(_balloonSpeedFactorRange.x, _balloonSpeedFactorRange.y);
            
            Debug.Log($"duration: {duration}");
            
            return new HorizontalSinusAnimation.Data()
            {
                Duration = duration,
                Distance = animationData.Distance,
                Frequency = animationData.Frequency,
                Amplitude = animationData.Amplitude,
            };
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

        private TimeSpan GetRandomDelay()
        {
            return TimeSpan.FromSeconds(Random.Range(_spawnDelaySecondsRange.x, _spawnDelaySecondsRange.y));
        }

        private static bool TwoVariantsRandom()
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
        
    }
}