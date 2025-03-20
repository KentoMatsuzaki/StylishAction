namespace Enum.Player
{
    /// <summary>プレイヤーに関する列挙型をまとめたクラス</summary>
    public static class PlayerEnum 
    {
        /// <summary>プレイヤーの状態</summary>
        public enum PlayerState
        {
            Idle,   // 静止
            Move,   // 移動
            Dash,   // ダッシュ
            Attack, // 攻撃
            Parry,  // パリィ
            Stun,   // スタン
            Dead    // 死亡
        }

        /// <summary>攻撃の種類</summary>
        public enum PlayerAttackType
        {
            Attack1,
            Attack2,
            Attack3
        }
    }
}