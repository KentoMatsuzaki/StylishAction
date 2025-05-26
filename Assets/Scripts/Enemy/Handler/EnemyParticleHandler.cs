using Enum;
using Particle;
using UnityEngine;

namespace Enemy.Handler
{
    /// <summary>敵のパーティクルを制御するクラス</summary>
    public class EnemyParticleHandler : MonoBehaviour
    {
        /// <summary>パーティクルを有効化する</summary>
        /// <param name="particleType">パーティクルの種類</param>
        public void ActivateParticle(ParticleEnums.EnemyParticleType particleType)
        {
            ParticleManager.Instance.ActivateEnemyParticle(particleType);
        }

        /// <summary>パーティクルを無効化する</summary>
        /// <param name="particleType">パーティクルの種類</param>
        public void DeactivateParticle(ParticleEnums.EnemyParticleType particleType)
        {
            ParticleManager.Instance.DeactivateEnemyParticle(particleType);
        }
    }
}