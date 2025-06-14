namespace Const
{
    /// <summary>敵に関する定数を保持するクラス</summary>
    public static class EnemyConst
    {
        //-------------------------------------------------------------------------------
        // パラメーター
        //-------------------------------------------------------------------------------

        public const float InitialPositionX = 0f;
        public const float InitialPositionY = 0f;
        public const float InitialPositionZ = 0f;
        public const float InitialRotationX = 0f;
        public const float InitialRotationY = 0f;
        public const float InitialRotationZ = 0f;
        
        //-------------------------------------------------------------------------------
        // タグ
        //-------------------------------------------------------------------------------
        
        public const string GameObjectTag = "Enemy"; // 敵のGameObjectに割り当てるタグ
        
        //-------------------------------------------------------------------------------
        // アニメーションステート
        //-------------------------------------------------------------------------------
        
        public const string BornState = "Born"; // 登場
        public const string IdleState = "Idle"; // 静止
        public const string MoveState = "Move"; // 移動
        public const string HitState = "Hit"; // 被弾
        public const string StunState = "Stun"; // スタン
        public const string RecoverState = "Recover"; // スタン復帰
        public const string DieState = "Die"; // 死亡
        
        //-------------------------------------------------------------------------------
        // アニメーションフラグ
        //-------------------------------------------------------------------------------

        public const string IsMoving = "Is Moving"; // 移動
    }
}