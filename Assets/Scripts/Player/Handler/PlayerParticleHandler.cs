using Enum;
using Particle;
using UnityEngine;

namespace Player.Handler
{
    /// <summary>プレイヤーのパーティクル処理を仲介するクラス</summary>
    public class PlayerParticleHandler : MonoBehaviour
    {
        /// <summary>パーティクルを有効化する</summary>
        /// <param name="particleType">パーティクルの種類</param>
        public void ActivateParticle(string particleType)
        {
            if (System.Enum.TryParse<InGameEnum.PlayerParticleType>(particleType, out var type))
            {
                ParticleManager.Instance.ActivatePlayerParticle(type);
            }
            else
            {
                Debug.LogWarning($"Particle not found : {particleType}");
            }
        }
    }
}
