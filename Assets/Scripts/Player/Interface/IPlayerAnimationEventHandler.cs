namespace Player.Interface
{
    /// <summary>本体からモデルへアニメーションイベントを仲介する役割を持つインターフェース</summary>
    public interface IPlayerAnimationEventHandler
    {
        /// <summary>静止アニメーションのアニメーションイベント</summary>
        void SwitchStateToIdle();
        
        /// <summary>特殊攻撃アニメーション4のアニメーションイベント</summary>
        void ApplyAttackSpecial4Force();
    }
}