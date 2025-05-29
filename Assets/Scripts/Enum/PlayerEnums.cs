namespace Enum
{
    /// <summary>プレイヤーに関する列挙型を保持するクラス</summary>
    public static class PlayerEnums
    {
        //-------------------------------------------------------------------------------
        // 状態に関する列挙型
        //-------------------------------------------------------------------------------
        
        /// <summary>プレイヤーの状態</summary>
        public enum PlayerState
        {
            Idle,          // 静止
            Move,          // 移動
            Sprint,        // スプリント
            Dodge,         // 回避
            AttackNormal,  // 通常攻撃
            AttackSpecial, // 特殊攻撃
            AttackExtra,   // EX攻撃
            Transition,    // 遷移
            Parry,         // パリィ
            Guard,         // 防御
            Damage,        // 被弾
            Death          // 死亡
        }
        
        //-------------------------------------------------------------------------------
        // 攻撃に関する列挙型
        //-------------------------------------------------------------------------------

        /// <summary>攻撃の種類</summary>
        public enum AttackType
        {
            AttackNormal,
            AttackSpecial,
            AttackExtra
        }
    }
}