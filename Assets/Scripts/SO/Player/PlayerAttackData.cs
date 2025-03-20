using Enum.Player;
using UnityEngine;

namespace SO.Player
{
    /// <summary>プレイヤーの攻撃情報を保持するスクリプタブルオブジェクト</summary>
    [CreateAssetMenu(fileName = "PlayerAttackData", menuName = "ScriptableObjects/CreatePlayerAttackAsset")]
    public class PlayerAttackData : ScriptableObject
    {
        /// <summary>攻撃の種類</summary>
        public PlayerEnum.PlayerAttackType type;

        /// <summary>与えるダメージ量</summary>
        public float damageAmount;
    }
}