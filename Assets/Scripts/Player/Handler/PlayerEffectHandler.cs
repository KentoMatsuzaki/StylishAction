using Effect;
using Enum.Player;
using UnityEngine;

namespace Player.Handler
{
    /// <summary>プレイヤーのエフェクトを制御するクラス</summary>
    public class PlayerEffectHandler : MonoBehaviour
    {
        /// <summary>攻撃エフェクトを有効化する</summary>
        /// <param name="attackName">攻撃の名前</param>
        public void ActivateAttackEffect(string attackName)
        {
            if (System.Enum.TryParse<PlayerEnum.PlayerParticleType>(attackName, out var type))
            {
                EffectManager.Instance.ActivatePlayerAttackEffect(type);
            }
            else
            {
                Debug.LogWarning($"Invalid Attack Name : {attackName}");
            }
        }

        /// <summary>パリィエフェクトを有効化する</summary>
        public void ActivateParryEffect()
        {
            EffectManager.Instance.ActivatePlayerParryEffect();
        }
    }
}
