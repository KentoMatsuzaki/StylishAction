using UnityEngine;

namespace Data.Enemy
{
    /// <summary>敵のステータス情報を保持するスクリプタブルオブジェクト</summary>
    [CreateAssetMenu(fileName = "EnemyStatusData", menuName = "ScriptableObjects/CreateEnemyStatusAsset")]
    public class EnemyStatusData : ScriptableObject
    {
        /// <summary>移動速度</summary>
        public float moveSpeed;
        
        /// <summary>回転速度</summary>
        public float rotateSpeed;
    }
}