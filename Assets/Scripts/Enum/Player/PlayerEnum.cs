namespace Enum.Player
{
    /// <summary>プレイヤーに関する列挙型をまとめたクラス</summary>
    public static class PlayerEnum 
    {
        /// <summary>プレイヤーの状態</summary>
        public enum EPlayerState
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
    }
}