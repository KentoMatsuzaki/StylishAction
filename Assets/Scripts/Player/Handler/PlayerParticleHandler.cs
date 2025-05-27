using Enum;
using Particle;
using UnityEngine;

namespace Player.Handler
{
    /// <summary>プレイヤーのパーティクルを制御するクラス</summary>
    public class PlayerParticleHandler : MonoBehaviour
    {
        /// <summary>攻撃パーティクルを有効化する</summary>
        /// <param name="attackType">パーティクルと紐づいている攻撃の種類</param>
        public void ActivateAttackParticle(ParticleEnums.ParticleAttackType attackType)
        {
            ParticleManager.Instance.ActivateAttackParticle(attackType);
        }

        /// <summary>攻撃パーティクルを無効化する</summary>
        /// <param name="attackType">パーティクルと紐づいている攻撃の種類</param>
        public void DeactivateAttackParticle(ParticleEnums.ParticleAttackType attackType)
        {
            ParticleManager.Instance.DeactivateAttackParticle(attackType);
        }
    }
}
