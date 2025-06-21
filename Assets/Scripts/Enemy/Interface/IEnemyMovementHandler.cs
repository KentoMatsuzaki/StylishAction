namespace Enemy.Interface
{
    /// <summary>敵の移動制御に関するインターフェース</summary>
    public interface IEnemyMovementHandler
    {
        /// <summary>プレイヤーの方向に回転させる</summary>
        /// <param name="rotateSpeed">回転速度</param>
        void RotateTowardsPlayer(float rotateSpeed);
        
        /// <summary>プレイヤーの方向に力を加えて移動させる</summary>
        /// <param name="moveForce">移動時に加える力の大きさ</param>
        void MoveTowardsPlayer(float moveForce);
        
        /// <summary>プレイヤーの逆方向に力を加えて移動させる</summary>
        /// /// <param name="moveForce">移動時に加える力の大きさ</param>
        void MoveAwayFromPlayer(float moveForce);
    }
}