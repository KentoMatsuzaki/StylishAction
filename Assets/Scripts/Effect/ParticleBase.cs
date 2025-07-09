using Definitions.Enum;
using UnityEngine;

namespace Effect
{
    /// <summary>
    /// パーティクルの挙動を制御する基底クラス
    /// 共通の制御ロジックを提供し、各種パーティクルクラスが継承して実装する
    /// </summary>
    public abstract class ParticleBase : MonoBehaviour
    {
        [SerializeField] private InGameEnums.ParticleType particleType;
        public InGameEnums.ParticleType ParticleType => particleType;
        
        protected ParticleSystem[] Particles;
        
        //-------------------------------------------------------------------------------
        // 初期化に関する処理
        //-------------------------------------------------------------------------------
        
        protected virtual void Awake()
        {
            Particles = GetComponentsInChildren<ParticleSystem>();
        }
        
        //-------------------------------------------------------------------------------
        // パーティクルの挙動に関する処理
        //-------------------------------------------------------------------------------
        
        /// <summary>
        /// プレハブを有効化し、パーティクルを再生する
        /// </summary>
        public abstract void Play();

        /// <summary>
        /// パーティクルを停止させ、プレハブを無効化する
        /// </summary>
        public void Stop()
        {
            foreach (var particle in Particles)
            {
                particle.Stop();
                particle.Clear();
            }
            
            gameObject.SetActive(false);
        }
    }
}