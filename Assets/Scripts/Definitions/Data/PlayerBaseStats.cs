using UnityEngine;

namespace Definitions.Data
{
    /// <summary>
    /// プレイヤーの基本パラメーター（HP,SP,EP,移動速度など）を保持するスクリプタブルオブジェクト
    /// </summary>
    [CreateAssetMenu(fileName = "PlayerBaseStats", menuName = "ScriptableObjects/Player/BaseStats", order = 0)]
    public class PlayerBaseStats : ScriptableObject
    {
        public float maxHp; // 最大HP
        public float maxSp; // 最大SP
        public float maxEp; // 最大EP
        public float moveForce; // 移動時に加える力の大きさ（非ロックオン時）
        public float strafeForce; // 移動時に加える力の大きさ（ロックオン時）
        public float atkEForce; // EX攻撃時に加える力の大きさ
    }
}