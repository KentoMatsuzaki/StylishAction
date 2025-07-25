namespace Definitions.Enum
{
    /// <summary>
    /// ゲームプレイ中の処理や演出に関する列挙型を定義するクラス
    /// </summary>
    public static class InGameEnums
    {
        //-------------------------------------------------------------------------------
        // プレイヤーに関する列挙型
        //-------------------------------------------------------------------------------
        
        /// <summary>プレイヤーの状態の種類</summary>
        public enum PlayerStateType
        {
            Idle,    // 待機状態
            Move,    // 移動状態
            Roll,    // 回避状態
            Parry,   // パリィ状態
            Guard,   // 防御状態
            AttackN, // 通常攻撃
            AttackS, // 特殊攻撃
            AttackE, // EX攻撃
            Damage,  // 被弾状態
            Dead     // 死亡状態
        }

        /// <summary>プレイヤーの攻撃の種類</summary>
        public enum PlayerAttackType
        {
            Normal,  // 通常攻撃
            Special, // 特殊攻撃
            Extra    // EX攻撃
        }
        
        //-------------------------------------------------------------------------------
        //　敵に関する列挙型
        //-------------------------------------------------------------------------------

        /// <summary>敵の攻撃の種類</summary>
        public enum EnemyAttackType
        {
            Scythe,
            Meteor,
            Seraph,
            Eclipse,
            Vortex,
            Photon,
            Explosion,
            Waterfall
        }
        
        /// <summary>BehaviourTreeの評価結果</summary>
        public enum EnemyNodeStatus
        {
            Success, 
            Failure,
            Running  
        }
        
        //-------------------------------------------------------------------------------
        // パーティクルに関する列挙型
        //-------------------------------------------------------------------------------

        /// <summary>パーティクルの種類</summary>
        public enum ParticleType
        {
            None = 0,
            
            // プレイヤーの攻撃に関するパーティクル（1~10）
            AtkN01 = 1,
            AtkN02 = 2,
            AtkN03 = 3,
            AtkN04 = 4,
            AtkN05 = 5,
            AtkS01 = 6,
            AtkS02 = 7,
            AtkS03 = 8,
            AtkS04 = 9,
            AtkE01 = 10,
            
            // 敵の攻撃に関するパーティクル
            Scythe = 11,
            Meteor = 12,
            Seraph1 = 13,
            Seraph2 = 14,
            Eclipse = 15,
            Explosion = 16,
            WaterFall1 = 17,
            WaterFall2 = 18,
            Vortex = 19,
            Photon = 20,
            
            
            // プレイヤーの汎用パーティクル
            Parry = 21,
            Guard = 22,
            Parried = 23,
            Hit = 24,
            
            // 敵の汎用パーティクル
        }
    }
}