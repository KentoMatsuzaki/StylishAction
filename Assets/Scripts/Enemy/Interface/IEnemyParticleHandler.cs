using UnityEngine;

namespace Enemy.Interface
{
    /// <summary>敵のパーティクル制御に関するインターフェース</summary>
    public interface IEnemyParticleHandler
    {
        /// <summary>被パリィ時のパーティクルを再生する</summary>
        void PlayOnParriedParticle();
        
        /// <summary>被パリィ時のパーティクルを停止する</summary>
        void StopOnParriedParticle();

        /// <summary>攻撃ヒット時のパーティクルを再生する</summary>
        void PlayHitParticle(Vector3 position);
    }
}