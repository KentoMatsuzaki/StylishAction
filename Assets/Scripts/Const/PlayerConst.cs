namespace Const
{
    /// <summary>プレイヤーに関する定数を保持するクラス</summary>
    public static class PlayerConst
    {
        //-------------------------------------------------------------------------------
        // タグ
        //-------------------------------------------------------------------------------
        
        public const string GameObjectTag = "Player"; // プレイヤーのGameObjectに割り当てるタグ
        
        //-------------------------------------------------------------------------------
        // アニメーションステート
        //-------------------------------------------------------------------------------

        public const string DodgeState = "Dodge"; // 回避
        public const string LightHitState = "Light Hit"; // 被ダメージ（軽）
        public const string HeavyHitState = "Heavy Hit"; // 被ダメージ（重）
        public const string DieState = "Die"; // 死亡
        
        //-------------------------------------------------------------------------------
        // アニメーションフラグ
        //-------------------------------------------------------------------------------
        
        public const string IsMoving = "Is Moving"; // 移動
        public const string IsSprinting = "Is Sprinting"; // スプリント
        public const string IsExtraAttacking = "Is ExtraAttacking"; // EX攻撃
        
        //-------------------------------------------------------------------------------
        // アニメーショントリガー
        //-------------------------------------------------------------------------------
        
        public const string AttackNormalTrigger = "Attack Normal"; // 通常攻撃
        public const string AttackSpecialTrigger = "Attack Special"; // 特殊攻撃
        public const string ParryTrigger = "Parry"; // パリィ
        
        //-------------------------------------------------------------------------------
        // アニメーションパラメーター
        //-------------------------------------------------------------------------------
        
        public const string MoveInputX = "Move Input X"; // 移動の入力値
        public const string MoveInputY = "Move Input Y"; // 移動の入力値
    }
}