using Const;
using Enum;
using UnityEngine;

namespace Player.Handler
{
    /// <summary>プレイヤーのアニメーションを制御するクラス</summary>
    public class PlayerAnimationHandler : MonoBehaviour
    {
        private Animator _animator;
        
        //-------------------------------------------------------------------------------
        // 初期設定
        //-------------------------------------------------------------------------------

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }
        
        //-------------------------------------------------------------------------------
        // 移動に関する処理
        //-------------------------------------------------------------------------------

        /// <summary>移動のフラグを切り替える</summary>
        public void ToggleMove()
        {
            _animator.SetBool(PlayerConst.IsMoving, !_animator.GetBool(PlayerConst.IsMoving));
        }

        /// <summary>移動のパラメーターを設定する</summary>
        /// <param name="moveInput">移動の入力値</param>
        public void SetMoveParameter(Vector2 moveInput)
        {
            _animator.SetFloat(PlayerConst.MoveInputX, moveInput.x);
            _animator.SetFloat(PlayerConst.MoveInputY, moveInput.y);
        }
        
        //-------------------------------------------------------------------------------
        // スプリントに関する処理
        //-------------------------------------------------------------------------------
        
        /// <summary>スプリントのフラグを切り替える</summary>
        public void ToggleSprint()
        {
            _animator.SetBool(PlayerConst.IsSprinting, !_animator.GetBool(PlayerConst.IsSprinting));
        }
        
        //-------------------------------------------------------------------------------
        // 回避に関する処理
        //-------------------------------------------------------------------------------
        
        /// <summary>回避のアニメーションを再生する</summary>
        public void PlayDodgeAnimation()
        {
            _animator.Play(PlayerConst.DodgeState);
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
        
        //-------------------------------------------------------------------------------
        // 汎用処理
        //-------------------------------------------------------------------------------

        /// <summary>AnimatorのRootMotionを有効化する</summary>
        public void EnableRootMotion()
        {
            _animator.applyRootMotion = true;
        }

        /// <summary>AnimatorのRootMotionを無効化する</summary>
        public void DisableRootMotion()
        {
            _animator.applyRootMotion = false;
        }
    }
}