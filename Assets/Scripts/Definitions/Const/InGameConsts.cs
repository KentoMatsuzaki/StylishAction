using JetBrains.Annotations;

namespace Definitions.Const
{
    /// <summary>
    /// ゲームプレイ中の処理や演出に関する定数を定義するクラス
    /// </summary>
    public static class InGameConsts
    {
        //-------------------------------------------------------------------------------
        // プレイヤーのアニメーションステート
        //-------------------------------------------------------------------------------
        
        public const string PlayerIdleAnimState = "Idle";         // 待機
        public const string PlayerMoveInAnimState = "MoveIn";     // 移動開始
        public const string PlayerMoveLoopAnimState = "MoveLoop"; // 移動ループ
        public const string PlayerDashAnimState = "Dash";         // ダッシュ
        public const string PlayerRollAnimState = "Roll";         // 回避
        public const string PlayerParryAnimState = "Parry";       // パリィ
        public const string PlayerGuardAnimState = "Guard";       // 防御
        public const string PlayerAttackNAnimState = "AttackN";   // 通常攻撃
        public const string PlayerAttackSAnimState = "AttackS";   // 特殊攻撃
        public const string PlayerAttackEAnimState = "AttackE";   // EX攻撃
        public const string PlayerDamageAnimState = "Damage";     // 被弾
        public const string PlayerDeadAnimState = "Dead";         // 死亡
    }
}