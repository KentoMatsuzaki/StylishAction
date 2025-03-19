using UnityEngine;
using System.Collections.Generic;
using Effect;

namespace Enemy.Handler
{
    /// <summary>敵のエフェクトを制御するクラス</summary>
    public class EnemyEffectHandler : MonoBehaviour
    {
        [Header("攻撃エフェクト"), SerializeField] private List<GameObject> attackEffects;

        /// <summary>攻撃エフェクトを有効化する</summary>
        /// <param name="skillNumber">スキルの番号(1~)</param>
        public void ActivateAttackEffect(int skillNumber)
        {
            attackEffects[skillNumber - 2].SetActive(true);
        }
    }
}