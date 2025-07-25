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
        public const string PlayerMoveFreeInAnimState = "Move_Free_In";     // 移動開始（非ロックオン）
        public const string PlayerMoveFreeLoopAnimState = "Move_Free_Loop"; // 移動ループ（非ロックオン）
        public const string PlayerMoveLockOnLoopAnimState = "Move_LockOn_Loop"; // 移動ループ（ロックオン）
        public const string PlayerRollAnimState = "Roll";         // ローリング
        public const string PlayerSlideAnimState = "Slide";       // スライド 
        public const string PlayerParryAnimState = "Parry";       // パリィ
        public const string PlayerGuardAnimState = "Guard";       // 防御
        public const string PlayerGuardHitAnimState = "GuardHit"; // 防御時の被弾
        public const string PlayerAttackNAnimState1 = "Atk_N_01"; // 通常攻撃（1段目）
        public const string PlayerAttackNAnimState2 = "Atk_N_02"; // 通常攻撃（2段目）
        public const string PlayerAttackNAnimState3 = "Atk_N_03"; // 通常攻撃（3段目）
        public const string PlayerAttackNAnimState4 = "Atk_N_04"; // 通常攻撃（4段目）
        public const string PlayerAttackNAnimState5 = "Atk_N_05"; // 通常攻撃（5段目）
        public const string PlayerAttackSAnimState1 = "Atk_S_01"; // 特殊攻撃（1段目）
        public const string PlayerAttackSAnimState2 = "Atk_S_02"; // 特殊攻撃（2段目）
        public const string PlayerAttackSAnimState3 = "Atk_S_03"; // 特殊攻撃（3段目）
        public const string PlayerAttackEAnimState = "Atk_E";     // EX攻撃
        public const string PlayerDamageAnimState = "Damage";     // 被弾
        public const string PlayerDeadAnimState = "Dead";         // 死亡
        
        //-------------------------------------------------------------------------------
        // プレイヤーのアニメーションパラメーター
        //-------------------------------------------------------------------------------

        public const string PlayerMoveInputX = "MoveX"; // 入力方向のX成分
        public const string PlayerMoveInputY = "MoveY"; // 入力方向のY成分
        
        //-------------------------------------------------------------------------------
        // Death（敵）のアニメーションステート
        //-------------------------------------------------------------------------------
        
        public const string DeathIdleAnimState = "Idle";
        public const string DeathMoveAnimState = "Move";
        public const string DeathParriedAnimState = "Parried";
        public const string DeathRecoveryAnimState = "Recovery";
        public const string DeathDieAnimState = "Die";
        public const string DeathScytheAnimState = "Scythe";
        public const string DeathMeteorAnimState = "Meteor";
        public const string DeathSeraphAnimState = "Seraph";
        public const string DeathEclipseAnimState = "Eclipse";
        public const string DeathVortexAnimState = "Vortex";
        public const string DeathPhotonAnimState = "Photon";
        public const string DeathExplosionAnimState = "Explosion";
        public const string DeathWaterfallAnimState = "Waterfall";
        
        //-------------------------------------------------------------------------------
        // GameObjectのタグ名
        //-------------------------------------------------------------------------------
        
        public const string PlayerGameObjectTag = "Player";
        public const string EnemyGameObjectTag = "Enemy";
        
        //-------------------------------------------------------------------------------
        // プレイヤーのステータス
        //-------------------------------------------------------------------------------

        public const float PlayerSpRegenerateRate = 0.05f;
        public const float PlayerDashSpCost = 0.001f;
        public const float PlayerRollSpCost = 0.125f;
        public const float PlayerParrySpCost = 0.25f;
        public const float PlayerGuardSpCost = 0.005f;
        public const float PlayerAtkSSpCost = 0.25f;
    }
}