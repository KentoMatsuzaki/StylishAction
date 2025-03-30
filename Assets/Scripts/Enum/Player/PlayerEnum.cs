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
            Attack, // 攻撃
            Parry,  // パリィ
            PostParry, // パリィ後の硬直
            Damage, // 被ダメージ
            Dead    // 死亡
        }

        /// <summary>パーティクルの種類</summary>
        public enum PlayerParticleType
        {
            Attack1,
            Attack2,
            Attack3,
            Parry
        }
    }
}