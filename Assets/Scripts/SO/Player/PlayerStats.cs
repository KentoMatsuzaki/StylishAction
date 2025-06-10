using UnityEngine;

namespace SO.Player
{
    /// <summary>プレイヤーのステータス情報を保持するスクリプタブルオブジェクト</summary>
    [CreateAssetMenu(fileName = "PlayerStats", menuName = "ScriptableObjects/PlayerStats")]
    public class PlayerStats : ScriptableObject
    {
        /// <summary>最大体力</summary>
        public float maxHp;

        /// <summary>最大スタミナ</summary>
        public float maxSp = 1;

        /// <summary>最大EXポイント</summary>
        public float maxEp = 20;

        /// <summary>スタミナ回復速度</summary>
        public float sPRegenRate;

        /// <summary>スプリントのスタミナ消費量</summary>
        public float sprintSpCost;
        
        /// <summary>回避のスタミナ消費量</summary>
        public float dodgeSpCost;
        
        /// <summary>パリィのスタミナ消費量</summary>
        public float parrySpCost;
        
        /// <summary>ガードのスタミナ消費量</summary>
        public float guardSpCost;

        /// <summary>特殊攻撃のスタミナ消費量</summary>
        public float attackSpecialSpCost;

        /// <summary>EX攻撃のEP消費量</summary>
        public float attackExtraEpCost;
        
        /// <summary>移動状態の移動速度</summary>
        [Range(0f, 100f)] 
        public float moveSpeed = 7.5f;

        /// <summary>スプリント状態の移動速度</summary>
        [Range(0f, 100f)] 
        public float sprintSpeed = 12.5f;

        /// <summary>特殊攻撃時に前方へ加える力</summary>
        [Range(0f, 100f)] 
        public float attackSpecial4Power = 10f;

        /// <summary>EX攻撃時に移動方向へ加える力</summary>
        [Range(0f, 100f)] 
        public float attackExtraSpeed = 10f;
    }
}