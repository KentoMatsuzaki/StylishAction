using Effect;
using Enum.Enemy;
using UnityEngine;

namespace Enemy.Handler
{
    /// <summary>敵のエフェクトを制御するクラス</summary>
    public class EnemyEffectHandler : MonoBehaviour
    {
        /// <summary>攻撃エフェクトを有効化する</summary>
        /// <param name="attackName">攻撃の名前</param>
        public void ActivateAttackEffect(string attackName)
        {
            if (System.Enum.TryParse<EnemyEnum.EnemyAttackType>(attackName, out var type))
            {
                EffectManager.Instance.ActivateEnemyAttackEffect(type);
            }
            else
            {
                Debug.LogWarning($"Invalid Attack Name : {attackName}");
            }
        }
    }
}