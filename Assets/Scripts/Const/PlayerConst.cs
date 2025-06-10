using System.Numerics;

namespace Const
{
    /// <summary>プレイヤーに関する定数を保持するクラス</summary>
    public static class PlayerConst
    {
        //-------------------------------------------------------------------------------
        // パラメーター
        //-------------------------------------------------------------------------------

        public const float InitialPositionX = 0f;
        public const float InitialPositionY = 0f;
        public const float InitialPositionZ = 15f;
        public const float InitialRotationX = 0f;
        public const float InitialRotationY = 180f;
        public const float InitialRotationZ = 0f;
        
        //-------------------------------------------------------------------------------
        // タグ
        //-------------------------------------------------------------------------------
        
        public const string GameObjectTag = "Player"; // プレイヤーのGameObjectに割り当てるタグ
        
        //-------------------------------------------------------------------------------
        // アニメーションステート
        //-------------------------------------------------------------------------------

        public const string IdleState = "Idle"; // 静止
        public const string DodgeState = "Dodge"; // 回避
        public const string ParryState = "Parry"; // パリィ
        public const string LightHitState = "Light Hit"; // 軽い攻撃の被弾
        public const string HeavyHitState = "Heavy Hit"; // 重い攻撃の被弾
        public const string GuardHitState = "Guard Hit"; // ガード状態の被弾
        public const string DieState = "Die"; // 死亡
        
        //-------------------------------------------------------------------------------
        // アニメーションフラグ
        //-------------------------------------------------------------------------------
        
        public const string IsMoving = "Is Moving"; // 移動
        public const string IsSprinting = "Is Sprinting"; // スプリント
        public const string IsExtraAttacking = "Is ExtraAttacking"; // EX攻撃
        public const string IsGuarding = "Is Guarding"; // 防御
        
        //-------------------------------------------------------------------------------
        // アニメーショントリガー
        //-------------------------------------------------------------------------------
        
        public const string AttackNormalTrigger = "Attack Normal"; // 通常攻撃
        public const string AttackSpecialTrigger = "Attack Special"; // 特殊攻撃
        
        //-------------------------------------------------------------------------------
        // アニメーションパラメーター
        //-------------------------------------------------------------------------------
        
        public const string MoveInputX = "Move Input X"; // 移動の入力値
        public const string MoveInputY = "Move Input Y"; // 移動の入力値
    }
}