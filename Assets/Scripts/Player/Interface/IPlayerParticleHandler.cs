using UnityEngine;

namespace Player.Interface
{
    /// <summary>プレイヤーのパーティクル制御に関するインターフェース</summary>
    public interface IPlayerParticleHandler
    {
        /// <summary>EX攻撃01のパーティクルを再生する</summary>
        void PlayAtkE01Particle();
        
        /// <summary>EX攻撃01のパーティクルを停止する</summary>
        void StopAtkE01Particle();
        
        /// <summary>敵の攻撃をパリィした際のパーティクルを再生する</summary>
        void PlayParryParticle();
        
        /// <summary>防御のパーティクルを再生する</summary>
        void PlayGuardParticle();
        
        /// <summary>防御のパーティクルを停止する</summary>
        void StopGuardParticle();
        
        /// <summary>攻撃命中時のパーティクルを再生する</summary>
        void PlayHitParticle(Vector3 position);
    }
}