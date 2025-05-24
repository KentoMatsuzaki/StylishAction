using UnityEngine;

namespace SO.Enemy
{
    /// <summary>敵のステータス情報を保持するスクリプタブルオブジェクト</summary>
    [CreateAssetMenu(fileName = "EnemyStats", menuName = "ScriptableObjects/EnemyStats")]
    public class EnemyStats : ScriptableObject
    {
        /// <summary>最大体力</summary>
        public float maxHp;
        
        /// <summary>移動速度</summary>
        public float moveSpeed;
        
        /// <summary>回転速度</summary>
        public float rotateSpeed;

        /// <summary>ノックバック速度</summary>
        public float knockBackSpeed;

        /// <summary>最大ヒット数</summary>
        public int maxHitCount;
    }
}