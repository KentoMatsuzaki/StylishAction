using Enum;
using Particle;
using UnityEngine;

namespace Player.Handler
{
    /// <summary>プレイヤーのパーティクルを制御するクラス</summary>
    public class PlayerParticleHandler : MonoBehaviour
    {
        /// <summary>パーティクルを有効化する</summary>
        /// <param name="particleType">パーティクルの種類</param>
        public void ActivateParticle(ParticleEnums.PlayerParticleType particleType)
        {
            ParticleManager.Instance.ActivatePlayerParticle(particleType);
        }

        /// <summary>パーティクルを無効化する</summary>
        /// <param name="particleType">パーティクルの種類</param>
        public void DeactivateParticle(ParticleEnums.PlayerParticleType particleType)
        {
            ParticleManager.Instance.DeactivatePlayerParticle(particleType);
        }
    }
}
