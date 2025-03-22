using UnityEngine;

namespace Effect
{
    /// <summary>パーティクルを制御する基底クラス</summary>
    public abstract class ParticleControllerBase : MonoBehaviour
    {
        /// <summary>全ての子のパーティクル</summary>
        private ParticleSystem[] _particles;

        private void Awake()
        {
            _particles = GetComponentsInChildren<ParticleSystem>();
        }

        private void Update()
        {
            var allStopped = true;

            foreach (var particle in _particles)
            {
                if (particle.isPlaying)
                {
                    allStopped = false; break;
                }
            }
            
            // 全ての子のパーティクルが再生完了していたら、オブジェクトを非アクティブにする
            if (allStopped) gameObject.SetActive(false);
        }

        /// <summary>全てのパーティクルをクリアして再スタートさせる</summary>
        public void RestartParticles()
        {
            foreach (var particle in _particles)
            {
                particle.Clear();
                particle.Play();
            }
        }
    }
}