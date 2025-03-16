using UnityEngine;

namespace Data.Player
{
    /// <summary>プレイヤーのステータス情報を保持するスクリプタブルオブジェクト</summary>
    [CreateAssetMenu(fileName = "PlayerStatus", menuName = "ScriptableObjects/CreatePlayerStatusAsset")]
    public class PlayerStatusData : ScriptableObject
    {
        /// <summary>最大体力</summary>
        public float maxHp;
        
        /// <summary>移動速度</summary>
        public float moveSpeed;
        
        /// <summary>ダッシュ速度</summary>
        public float dashSpeed;
    }
}