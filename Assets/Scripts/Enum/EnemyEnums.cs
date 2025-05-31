using UnityEngine;

namespace Enum
{
    /// <summary>敵に関する列挙型を保持するクラス</summary>
    public static class EnemyEnums
    {
        //-------------------------------------------------------------------------------
        // 攻撃に関する列挙型
        //-------------------------------------------------------------------------------

        /// <summary>攻撃の種類</summary>
        public enum AttackType
        {
            Scythe,
            Meteor,
            Seraph,
            Eclipse,
            Explosion,
            WaterFall,
            
        }
        
        //-------------------------------------------------------------------------------
        // AIに関する列挙型
        //-------------------------------------------------------------------------------

        /// <summary>ノードの評価結果</summary>
        public enum NodeStatus
        {
            Success, 
            Failure, 
            Running  
        }
    }
}