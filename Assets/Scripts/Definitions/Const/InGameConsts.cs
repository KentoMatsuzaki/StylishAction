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
        public const string PlayerMoveInAnimState = "MoveIn";     // 移動（開始）
        public const string PlayerMoveLoopAnimState = "MoveLoop"; // 移動（ループ）
        public const string PlayerDashAnimState = "Dash";         // ダッシュ
        public const string PlayerRollAnimState = "Roll";         // 回避
        public const string PlayerParryAnimState = "Parry";       // パリィ
        public const string PlayerGuardAnimState = "Guard";       // 防御
        public const string PlayerAttackNAnimState1 = "Atk_N_01"; // 通常攻撃（1段目）
        public const string PlayerAttackNAnimState2 = "Atk_N_02"; // 通常攻撃（1段目）
        public const string PlayerAttackNAnimState3 = "Atk_N_03"; // 通常攻撃（1段目）
        public const string PlayerAttackNAnimState4 = "Atk_N_04"; // 通常攻撃（1段目）
        public const string PlayerAttackNAnimState5 = "Atk_N_05"; // 通常攻撃（1段目）
        public const string PlayerAttackSAnimState1 = "Atk_S_01"; // 特殊攻撃（1段目）
        public const string PlayerAttackSAnimState2 = "Atk_S_02"; // 特殊攻撃（2段目）
        public const string PlayerAttackSAnimState3 = "Atk_S_03"; // 特殊攻撃（3段目）
        public const string PlayerAttackSAnimState4 = "Atk_S_04"; // 特殊攻撃（4段目）
        public const string PlayerAttackEAnimState = "Atk_E";     // EX攻撃
        public const string PlayerDamageAnimState = "Damage";     // 被弾
        public const string PlayerDeadAnimState = "Dead";         // 死亡
        
        //-------------------------------------------------------------------------------
        // Death（敵）のアニメーションステート
        //-------------------------------------------------------------------------------
        
        public const string DeathIdleAnimState = "Idle";
        public const string DeathMoveAnimState = "Move";
        public const string DeathScytheAnimState = "Scythe";
        public const string DeathMeteorAnimState = "Meteor";
        public const string DeathSeraphAnimState = "Seraph";
        public const string DeathEclipseAnimState = "Eclipse";
        public const string DeathVortexAnimState = "Vortex";
        public const string DeathPhotonAnimState = "Photon";
        public const string DeathExplosionAnimState = "Explosion";
        public const string DeathWaterfallAnimState = "Waterfall";
    }
}