using UnityEngine;

namespace Definitions.Data
{
    /// <summary>
    /// 敵の基本パラメーター（HP,移動速度など）を保持するスクリプタブルオブジェクト
    /// </summary>
    [CreateAssetMenu(fileName = "EnemyBaseStats", menuName = "ScriptableObjects/Enemy/BaseStats", order = 0)]
    public class EnemyBaseStats: ScriptableObject
    {
        public float maxHp; // 最大HP
        public float rotateSpeed; // 回転速度
        public float moveForce; // 移動時に加える力の大きさ
    }
}