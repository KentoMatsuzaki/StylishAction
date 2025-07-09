using Definitions.Enum;
using UnityEngine;

namespace Definitions.Data
{
    /// <summary>
    /// プレイヤーの攻撃パラメーター（攻撃の種類,ダメージ量など）を保持するスクリプタブルオブジェクト
    /// </summary>
    [CreateAssetMenu(fileName = "PlayerAttackStats", menuName = "ScriptableObjects/Player/AttackStats", order = 1)]
    public class PlayerAttackStats : ScriptableObject
    {
        /// <summary>攻撃の種類</summary>
        public InGameEnums.PlayerAttackType attackType;
        
        /// <summary>攻撃で与えるダメージ量</summary>
        public float damage;
    }
}