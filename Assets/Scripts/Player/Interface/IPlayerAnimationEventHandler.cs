namespace Player.Interface
{
    /// <summary>本体からモデルへアニメーションイベントを仲介する役割を持つインターフェース</summary>
    public interface IPlayerAnimationEventHandler
    {
        /// <summary>静止アニメーションのアニメーションイベント</summary>
        void SwitchStateToIdle();

        public void SwitchStateToTransition();
        
        /// <summary>特殊攻撃アニメーション4のアニメーションイベント</summary>
        void ApplyAttackSpecial4Force();

        /// <summary>右手の攻撃の当たり判定を持続時間だけ有効化する</summary>
        void EnableRightAttackColliderForDuration();

        /// <summary>左手の攻撃の当たり判定を持続時間だけ有効化する</summary>
        void EnableLeftAttackColliderForDuration();


        void ActivateAttackNormal1Particle();

        void ActivateAttackNormal2Particle();

        void ActivateAttackNormal3Particle();

        void ActivateAttackNormal4Particle();

        void ActivateAttackNormal5Particle();

        void ActivateAttackSpecial1Particle();

        void ActivateAttackSpecial2Particle();

        void ActivateAttackSpecial3Particle();
    }
}