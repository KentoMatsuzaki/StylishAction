using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Enum;

namespace Particle
{
    /// <summary>パーティクルを制御する抽象クラス</summary>
    public abstract class ParticleBase : MonoBehaviour
    {
        /// <summary>パーティクルの種類</summary>
        public ParticleEnums.ParticleType type;

        /// <summary>パーティクルの一覧</summary>
        private ParticleSystem[] _particles;
        
        //-------------------------------------------------------------------------------
        // 初期設定
        //-------------------------------------------------------------------------------
        
        protected void Awake()
        {
            _particles = GetComponentsInChildren<ParticleSystem>();
        }
        
        //-------------------------------------------------------------------------------
        // パーティクルの処理
        //-------------------------------------------------------------------------------

        /// <summary>パーティクルを有効化する</summary>
        public abstract void Activate();

        /// <summary>パーティクルを無効化する</summary>
        public void Deactivate()
        {
            foreach (var particle in _particles)
            {
                particle.Stop(); 
                particle.Clear();
            }
            
            SetInactiveAfterAllParticlesStop();
        }
        
        /// <summary>全てのパーティクルを再生する</summary>
        protected void PlayAllParticles()
        {
            foreach (var particle in _particles)
            {
                particle.Play();
            }
        }

        /// <summary>全てのパーティクルを再再生する</summary>
        protected void ReplayAllParticles()
        {
            foreach (var particle in _particles)
            {
                particle.Stop();
                particle.Clear();
                particle.Play();
            }
        }

        /// <summary>全てのパーティクルの再生完了後にゲームオブジェクトを無効化する</summary>
        protected async void SetInactiveAfterAllParticlesStop()
        {
            await UniTask.WaitUntil(() => _particles.All(p => !p.isPlaying));
            gameObject.SetActive(false);
        }
    }
}