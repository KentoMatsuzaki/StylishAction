using Const;
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

        /// <summary>移動のパラメーターを設定する</summary>
        /// <param name="moveInput">移動の入力値</param>
        public void SetMoveParameter(Vector2 moveInput)
        {
            _animator.SetFloat(PlayerConst.MoveInputX, moveInput.x);
            _animator.SetFloat(PlayerConst.MoveInputY, moveInput.y);
        }

        /// <summary>移動のフラグを有効化する</summary>
        public void EnableMove()
        {
            _animator.SetBool(PlayerConst.IsMoving, true);
        }

        /// <summary>移動のフラグを無効化する</summary>
        public void DisableMove()
        {
            _animator.SetBool(PlayerConst.IsMoving, false);
        }
        
        //-------------------------------------------------------------------------------
        // スプリントに関する処理
        //-------------------------------------------------------------------------------

        /// <summary>スプリントのフラグを有効化する</summary>
        public void EnableSprint()
        {
            _animator.SetBool(PlayerConst.IsSprinting, true);
        }

        /// <summary>スプリントのフラグを無効化する</summary>
        public void DisableSprint()
        {
            _animator.SetBool(PlayerConst.IsSprinting, false);
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
        // 通常攻撃に関する処理
        //-------------------------------------------------------------------------------

        /// <summary>通常攻撃のトリガーを有効化する</summary>
        public void TriggerNormalAttack()
        {
            // 通常攻撃アニメーションをトリガーする
            _animator.SetTrigger(PlayerConst.AttackNormalTrigger);
        }
        
        //-------------------------------------------------------------------------------
        // 特殊攻撃に関する処理
        //-------------------------------------------------------------------------------

        /// <summary>特殊攻撃のトリガーを有効化する</summary>
        public void TriggerSpecialAttack()
        {
            // 特殊攻撃アニメーションをトリガーする
            _animator.SetTrigger(PlayerConst.AttackSpecialTrigger);
        }
        
        //-------------------------------------------------------------------------------
        // 必殺攻撃に関する処理
        //-------------------------------------------------------------------------------

        /// <summary>必殺攻撃のトリガーを有効化する</summary>
        public void TriggerExtraAttack()
        {
            // 必殺攻撃アニメーションをトリガーする
            _animator.SetTrigger(PlayerConst.AttackExtraTrigger);
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