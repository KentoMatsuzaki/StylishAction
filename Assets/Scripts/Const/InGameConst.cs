namespace Const
{
    /// <summary>インゲームに関する定数をまとめたクラス</summary>
    public static class InGameConst
    {
        //-------------------------------------------------------------------------------
        // タグ
        //-------------------------------------------------------------------------------
        
        public const string PlayerTag = "Player"; // プレイヤーのタグ
        public const string EnemyTag = "Enemy"; // 敵のタグ
        
        //-------------------------------------------------------------------------------
        // アニメーションステート
        //-------------------------------------------------------------------------------

        public const string PlayerHitAnimation = "Hit"; // プレイヤーの被ダメージ
        public const string PlayerDieAnimation = "Die"; // プレイヤーの死亡
        public const string EnemyStunAnimation = "Stun"; // 敵のスタン
        public const string EnemyHitAnimation = "Hit"; // 敵の被ダメージ
        public const string EnemyDieAnimation = "Die"; // 敵の死亡
        
        //-------------------------------------------------------------------------------
        // アニメーションフラグ
        //-------------------------------------------------------------------------------

        public const string EnemyMoveFlag = "Move Flag"; // 敵の移動
        public const string PlayerMoveFlag = "Move Flag"; // プレイヤーの移動
        
        //-------------------------------------------------------------------------------
        // アニメーショントリガー
        //-------------------------------------------------------------------------------
        
        public const string PlayerParryTrigger = "Parry Trigger"; // プレイヤーのパリィ
        public const string PlayerDashTrigger = "Dash Trigger"; // プレイヤーのダッシュ
    }
}