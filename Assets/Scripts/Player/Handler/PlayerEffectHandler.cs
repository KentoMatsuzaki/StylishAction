using UnityEngine;
using System.Collections.Generic;
using Effect;

namespace Player.Handler
{
    /// <summary>プレイヤーのエフェクトを制御するクラス</summary>
    public class PlayerEffectHandler : MonoBehaviour
    {
        [Header("攻撃エフェクトの生成位置"), SerializeField] 
        private List<Transform> attackEffectPoints;

        /// <summary>攻撃エフェクトを生成する</summary>
        /// <param name="attackNumber">攻撃の番号(1~)</param>
        public void InstantiateAttackEffect(int attackNumber)
        {
            Quaternion fixedRotation = transform.rotation * Quaternion.Euler(0, 180, 0);
            
            EffectManager.Instance.InstantiatePlayerAttackEffect
                (attackNumber, attackEffectPoints[attackNumber - 1].position, fixedRotation);
        }
    }
}
