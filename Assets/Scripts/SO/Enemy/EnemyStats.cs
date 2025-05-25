using UnityEngine;
using UnityEngine.Serialization;

namespace SO.Enemy
{
    /// <summary>敵のステータス情報を保持するスクリプタブルオブジェクト</summary>
    [CreateAssetMenu(fileName = "EnemyStats", menuName = "ScriptableObjects/EnemyStats")]
    public class EnemyStats : ScriptableObject
    {
        /// <summary>最大体力値</summary>
        public float maxHp;
        
        /// <summary>移動速度</summary>
        public float moveSpeed;
        
        /// <summary>回転速度</summary>
        public float rotateSpeed;

        /// <summary>スタン時間</summary>
        public float stunDuration;

        /// <summary>ノックバック時に加える力の大きさ</summary>
        public float knockBackPower;

        /// <summary>最大靭性値</summary>
        public int maxPoise;
    }
}