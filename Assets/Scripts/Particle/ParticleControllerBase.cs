using System;
using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Enum;
using UnityEngine.Serialization;

namespace Particle
{
    /// <summary>パーティクルを制御する抽象クラス</summary>
    public abstract class ParticleControllerBase : MonoBehaviour
    {
        /// <summary>パーティクルの表示時間</summary>
        [SerializeField] private float lifeTime;

        /// <summary>パーティクルを存続させるか</summary>
        [SerializeField] private bool isLasting;
        
        /// <summary>パーティクルの一覧</summary>
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
            // パーティクルが有効化されている場合
            if (gameObject.activeSelf)
            {
                // パーティクルをリスタートする
                ReplayParticles();
            }
            // パーティクルが無効化されている場合
            else
            {
                // パーティクルを有効化する
                HandleActivation();
            }
        }

        /// <summary>パーティクルを無効化する</summary>
        public virtual void Deactivate()
        {
            foreach (var particle in _particles)
            {
                particle.Stop();
                particle.Clear();
            }
        }

        /// <summary>パーティクルをリスタートする</summary>
        private void ReplayParticles()
        {
            foreach (var particle in _particles)
            {
                particle.Stop();
                particle.Clear();
                particle.Play();
            }
        }

        /// <summary>パーティクルを有効化する</summary>
        private void HandleActivation()
        {
            if (isLasting)
            {
                ActivateAndDeactivateAfterLifetime();
            }
            else
            {
                ActivateAndDeactivateAfterParticlesComplete();
            }
        }

        /// <summary>パーティクルを有効化して、表示時間が経過した後に無効化する</summary>
        private async void ActivateAndDeactivateAfterLifetime()
        {
            gameObject.SetActive(true);
            await UniTask.Delay(TimeSpan.FromSeconds(lifeTime));
            gameObject.SetActive(false);
        }
        
        /// <summary>パーティクルを有効化して、再生が完了した後に無効化する</summary>
        private async void ActivateAndDeactivateAfterParticlesComplete()
        {
            gameObject.SetActive(true);
            await UniTask.WaitUntil(() => _particles.All(p => !p.isPlaying));
            gameObject.SetActive(false);
        }
    }
}