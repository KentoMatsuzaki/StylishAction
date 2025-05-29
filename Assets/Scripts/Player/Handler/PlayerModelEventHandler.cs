using Enum;
using Player.Interface;
using UnityEngine;

namespace Player.Handler
{
    /// <summary>モデルのアニメーションイベントを制御するクラス</summary>
    public class PlayerModelEventHandler : MonoBehaviour
    {
        /// <summary>アニメーションイベントを仲介するインターフェース</summary>
        private IPlayerAnimationEventHandler _animationEventHandler;

        private void Awake()
        {
            _animationEventHandler = GetComponentInParent<IPlayerAnimationEventHandler>();
        }

        /// <summary>静止アニメーションから呼ばれる</summary>
        public void SwitchStateToIdle()
        {
            _animationEventHandler.SwitchStateToIdle();
        }

        
        public void SwitchStateToTransition()
        {
            _animationEventHandler.SwitchStateToTransition();
        }

        /// <summary>特殊攻撃アニメーション4から呼ばれる</summary>
        public void ApplyAttackSpecial4Force()
        {
            _animationEventHandler.ApplyAttackSpecial4Force();
        }

        public void ActivateAttackNormal1Particle()
        {
            _animationEventHandler.ActivateAttackNormal1Particle();
        }

        public void ActivateAttackNormal2Particle()
        {
            _animationEventHandler.ActivateAttackNormal2Particle();
        }

        public void ActivateAttackNormal3Particle()
        {
            _animationEventHandler.ActivateAttackNormal3Particle();
        }

        public void ActivateAttackNormal4Particle()
        {
            _animationEventHandler.ActivateAttackNormal4Particle();
        }

        public void ActivateAttackNormal5Particle()
        {
            _animationEventHandler.ActivateAttackNormal5Particle();
        }

        public void ActivateAttackSpecial1Particle()
        {
            _animationEventHandler.ActivateAttackSpecial1Particle();
        }

        public void ActivateAttackSpecial2Particle()
        {
            _animationEventHandler.ActivateAttackSpecial2Particle();
        }

        public void ActivateAttackSpecial3Particle()
        {
            _animationEventHandler.ActivateAttackSpecial3Particle();
        }
    }
}