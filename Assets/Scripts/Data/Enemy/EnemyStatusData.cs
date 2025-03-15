using UnityEngine;

namespace Data.Enemy
{
    /// <summary>敵のステータス情報を保持するスクリプタブルオブジェクト</summary>
    [CreateAssetMenu(fileName = "EnemyStatusData", menuName = "ScriptableObjects/CreateEnemyStatusAsset")]
    public class EnemyStatusData : ScriptableObject
    {
        /// <summary>フェーズ1の最大体力</summary>
        public float phase1MaxHp;
        
        /// <summary>フェーズ2の最大体力</summary>
        public float phase2MaxHp;
        
        /// <summary>フェーズ3の最大体力</summary>
        public float phase3MaxHp;
        
        /// <summary>移動速度</summary>
        public float moveSpeed;
        
        /// <summary>回転速度</summary>
        public float rotateSpeed;

        /// <summary>ノックバック速度</summary>
        public float knockbackSpeed;
    }
}