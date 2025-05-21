namespace Enum.Player
{
    /// <summary>プレイヤーに関する列挙型をまとめたクラス</summary>
    public static class PlayerEnum 
    {
        /// <summary>プレイヤーの状態</summary>
        public enum EPlayerState
        {
            Idle,   // 静止
            Move,   // 移動
            Sprint, // スプリント
            Dodge,  // 回避
            Attack, // 攻撃
            Parry,  // パリィ
            PostParry, // パリィ後の硬直
            Damage, // 被ダメージ
            Dead    // 死亡
        }
    }
}