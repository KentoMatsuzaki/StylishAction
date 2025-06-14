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
        
    }
}