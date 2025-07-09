namespace Player.Interface
{
    /// <summary>プレイヤーの攻撃制御に関するインターフェース</summary>
    public interface IPlayerAttackHandler
    {
        /// <summary>敵の方向へ即座に回転させる</summary>
        void RotateTowardsEnemyInstantly();
    }
}