using System;
using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Enum;
using UnityEngine.Serialization;

namespace Particle
{
    /// <summary>パーティクルを制御する基底クラス</summary>
    public abstract class ParticleControllerBase : MonoBehaviour
    {
        /// <summary>パーティクルの持続時間</summary>
        [SerializeField] private float lifeTime;
        
        /// <summary>パーティクルの種類</summary>
        [SerializeField] private InGameEnum.ParticleLifeTimeType lifeTimeType;
        
        /// <summary>子のパーティクル配列</summary>
        private ParticleSystem[] _particles;
        
        //-------------------------------------------------------------------------------
        // 初期設定
        //-------------------------------------------------------------------------------
        
        private void Awake()
        {
            _particles = GetComponentsInChildren<ParticleSystem>();
        }
        
        //-------------------------------------------------------------------------------
        // パーティクルの処理
        //-------------------------------------------------------------------------------

        /// <summary>パーティクルを有効化する</summary>
        public virtual void Activate()
        {
            // アクティブ時
            if (gameObject.activeSelf)
            {
                ReplayParticles();
            }
            // 非アクティブ時
            else
            {
                HandleActivationByLifeTimeType();
            }
        }

        /// <summary>パーティクルを再再生する</summary>
        private void ReplayParticles()
        {
            foreach (var particle in _particles)
            {
                particle.Clear();
                particle.Play();
            }
        }

        /// <summary>パーティクルのライフタイムの種類に応じて有効化する</summary>
        private void HandleActivationByLifeTimeType()
        {
            switch (lifeTimeType)
            {
                case InGameEnum.ParticleLifeTimeType.OneShot :
                    WaitForAllParticlesThenDeactivate().Forget(); break;
                case InGameEnum.ParticleLifeTimeType.Lasting :
                    WaitForLifeTimeThenDeactivate().Forget(); break;
            }
        }

        /// <summary>パーティクルを有効化して、再生終了した後に非アクティブにする</summary>
        private async UniTaskVoid WaitForAllParticlesThenDeactivate()
        {
            gameObject.SetActive(true);
            await UniTask.WaitUntil(() => _particles.All(p => !p.isPlaying));
            gameObject.SetActive(false);
        }

        /// <summary>パーティクルを有効化して、持続時間の後に非アクティブにする</summary>
        private async UniTaskVoid WaitForLifeTimeThenDeactivate()
        {
            gameObject.SetActive(true);
            await UniTask.Delay(TimeSpan.FromSeconds(lifeTime));
            gameObject.SetActive(false);
        }
    }
}