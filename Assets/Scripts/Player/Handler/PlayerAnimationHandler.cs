using Const;
using Enum;
using UnityEngine;

namespace Player.Handler
{
    /// <summary>プレイヤーのアニメーションを制御するクラス</summary>
    public class PlayerAnimationHandler : MonoBehaviour
    {
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }
        
        //-------------------------------------------------------------------------------
        // 移動に関する処理
        //-------------------------------------------------------------------------------

        /// <summary>移動フラグを設定する</summary>
        public void SetMoveFlag(bool value)
        {
            if (_animator.GetBool(PlayerConst.IsMoving) != value)
            {
                _animator.SetBool(PlayerConst.IsMoving, value);
            }
        }
        
        //-------------------------------------------------------------------------------
        // ダッシュに関する処理
        //-------------------------------------------------------------------------------
        
        /// <summary>ダッシュアニメーションをトリガーする</summary>
        public void TriggerDash()
        {
            // ダッシュアニメーションを再生中は処理を抜ける
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Dash")) return;
            //_animator.SetTrigger(PlayerConst.PlayerDashTrigger);
        }
        
        //-------------------------------------------------------------------------------
        // 攻撃に関する処理
        //-------------------------------------------------------------------------------

        /// <summary>対応する攻撃アニメーションをトリガーする</summary>
        public void TriggerAttack(InGameEnum.PlayerAttackType type)
        {
            // 攻撃アニメーションを再生中は処理を抜ける
            if (_animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) return;
            _animator.SetTrigger($"{type.ToString()} Trigger");
        }
        
        //-------------------------------------------------------------------------------
        // パリィに関する処理
        //-------------------------------------------------------------------------------

        /// <summary>パリィアニメーションをトリガーする</summary>
        public void TriggerParry()
        {
            // パリィアニメーションを再生中は処理を抜ける
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Parry")) return; 
            _animator.SetTrigger(PlayerConst.ParryTrigger);
        }
        
        //-------------------------------------------------------------------------------
        // 被ダメージに関する処理
        //-------------------------------------------------------------------------------

        /// <summary>被ダメージアニメーションを再生する</summary>
        public void PlayHitAnimation()
        {
            _animator.Play(PlayerConst.HeavyHitState);
        }
        
        //-------------------------------------------------------------------------------
        // 死亡に関する処理
        //-------------------------------------------------------------------------------

        /// <summary>死亡アニメーションを再生する</summary>
        public void PlayDieAnimation()
        {
            _animator.Play(PlayerConst.DieState);
        }
    }
}