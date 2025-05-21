using UnityEngine;

namespace SO.Player
{
    /// <summary>プレイヤーのステータス情報を保持するスクリプタブルオブジェクト</summary>
    [CreateAssetMenu(fileName = "PlayerStats", menuName = "ScriptableObjects/PlayerStats")]
    public class PlayerStats : ScriptableObject
    {
        /// <summary>最大体力</summary>
        public float maxHp;
        
        /// <summary>移動状態の移動速度</summary>
        [Range(0f, 100f)] 
        public float moveSpeed = 7.5f;

        /// <summary>スプリント状態の移動速度</summary>
        [Range(0f, 100f)] 
        public float sprintSpeed = 12.5f;

        /// <summary>回避時に前方へ加える力</summary>
        [Range(0f, 100f)] 
        public float dodgePower = 10f;

        /// <summary>攻撃時のエイム補正速度</summary>
        public float attackAimAssistSpeed;
    }
}