using Enum;
using UnityEngine;

namespace SO.Player
{
    /// <summary>プレイヤーの攻撃情報を保持するスクリプタブルオブジェクト</summary>
    [CreateAssetMenu(fileName = "PlayerAttackStats", menuName = "ScriptableObjects/PlayerAttackStats")]
    public class PlayerAttackStats : ScriptableObject
    {
        /// <summary>攻撃の種類</summary>
        public PlayerEnums.AttackType attackType;

        /// <summary>ダメージ量</summary>
        public float attackDamage;

        /// <summary>攻撃時に敵の方向へ回転する速度</summary>
        public float attackRotationSpeed;
    }
}