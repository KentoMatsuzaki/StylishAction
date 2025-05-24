using Enum;
using UnityEngine;

namespace SO.Player
{
    /// <summary>プレイヤーの攻撃情報を保持するスクリプタブルオブジェクト</summary>
    [CreateAssetMenu(fileName = "PlayerAttackStats", menuName = "ScriptableObjects/PlayerAttackStats")]
    public class PlayerAttackStats : ScriptableObject
    {
        /// <summary>攻撃の種類</summary>
        public InGameEnum.PlayerAttackType type;

        /// <summary>ダメージ量</summary>
        public float attackDamage;
    }
}