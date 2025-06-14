namespace Player.Interface
{
    /// <summary>プレイヤーのアニメーションを制御するインターフェース</summary>
    public interface IPlayerAnimationHandler
    {
        /// <summary>待機アニメーションを再生する</summary>
        void PlayIdleAnimation();
        
        /// <summary>移動アニメーションを再生する</summary>
        void PlayMoveAnimation();
        
        /// <summary>移動アニメーションを中止する</summary>
        void CancelMoveAnimation();
        
        /// <summary>ダッシュアニメーションを再生する</summary>
        void PlayDashAnimation();
        
        /// <summary>回避アニメーションを再生する</summary>
        void PlayRollAnimation();
        
        /// <summary>パリィアニメーションを再生する</summary>
        void PlayParryAnimation();
        
        /// <summary>防御アニメーションを再生する</summary>
        void PlayGuardAnimation();

        /// <summary>通常攻撃アニメーションを再生する</summary>
        void PlayAttackNAnimation();

        /// <summary>特殊攻撃アニメーションを再生する</summary>
        void PlayAttackSAnimation();

        /// <summary>EX攻撃アニメーションを再生する</summary>
        void PlayAttackEAnimation();
        
        /// <summary>被弾アニメーションを再生する</summary>
        void PlayDamageAnimation();

        /// <summary>死亡アニメーションを再生する</summary>
        void PlayDeadAnimation();
    }
}