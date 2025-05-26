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

        /// <summary>特殊攻撃アニメーション4から呼ばれる</summary>
        public void ApplyAttackSpecial4Force()
        {
            _animationEventHandler.ApplyAttackSpecial4Force();
        }
        
        /// <summary>右手の攻撃の当たり判定を持続時間だけ有効化する</summary>
        public void EnableRightAttackColliderForDuration()
        {
            _animationEventHandler.EnableRightAttackColliderForDuration();
        }

        /// <summary>左手の攻撃の当たり判定を持続時間だけ有効化する</summary>
        public void EnableLeftAttackColliderForDuration()
        {
            _animationEventHandler.EnableLeftAttackColliderForDuration();
        }
    }
}