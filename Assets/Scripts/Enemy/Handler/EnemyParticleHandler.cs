using Enum;
using Particle;
using UnityEngine;

namespace Enemy.Handler
{
    /// <summary>敵のパーティクル処理を仲介するクラス</summary>
    public class EnemyParticleHandler : MonoBehaviour
    {
        /// <summary>パーティクルを有効化する</summary>
        /// <param name="particleType">パーティクルの種類</param>
        public void ActivateParticle(string particleType)
        {
            if (System.Enum.TryParse<InGameEnum.EnemyParticleType>(particleType, out var type))
            {
                ParticleManager.Instance.ActivateEnemyParticle(type);
            }
            else
            {
                Debug.LogWarning($"Particle not found : {particleType}");
            }
        }
    }
}