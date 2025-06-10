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
        // 静止に関する処理
        //-------------------------------------------------------------------------------

        /// <summary>静止のアニメーションを再生する</summary>
        public void PlayIdleAnimation()
        {
            _animator.Play(PlayerConst.IdleState);
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
        public void TriggerAttackNormal()
        {
            _animator.SetTrigger(PlayerConst.AttackNormalTrigger);
        }
        
        //-------------------------------------------------------------------------------
        // 特殊攻撃に関する処理
        //-------------------------------------------------------------------------------

        /// <summary>特殊攻撃のトリガーを有効化する</summary>
        public void TriggerAttackSpecial()
        {
            _animator.SetTrigger(PlayerConst.AttackSpecialTrigger);
        }
        
        //-------------------------------------------------------------------------------
        // EX攻撃に関する処理
        //-------------------------------------------------------------------------------

        /// <summary>EX攻撃のフラグを有効化する</summary>
        public void EnableAttackExtra()
        {
            _animator.SetBool(PlayerConst.IsExtraAttacking, true);
        }

        /// <summary>EX攻撃のフラグを無効化する</summary>
        public void DisableAttackExtra()
        {
            _animator.SetBool(PlayerConst.IsExtraAttacking, false);
        }
        
        //-------------------------------------------------------------------------------
        // パリィに関する処理
        //-------------------------------------------------------------------------------

        /// <summary>パリィのアニメーションを再生する</summary>
        public void PlayParryAnimation()
        {
            _animator.Play(PlayerConst.ParryState);
        }
        
        //-------------------------------------------------------------------------------
        // 防御に関する処理
        //-------------------------------------------------------------------------------

        /// <summary>防御のフラグを有効化する</summary>
        public void EnableGuard()
        {
            _animator.SetBool(PlayerConst.IsGuarding, true);
        }

        /// <summary>防御のフラグを無効化する</summary>
        public void DisableGuard()
        {
            _animator.SetBool(PlayerConst.IsGuarding, false);
        }
        
        //-------------------------------------------------------------------------------
        // 被弾に関する処理
        //-------------------------------------------------------------------------------

        /// <summary>重い攻撃を受けた際の被弾アニメーションを再生する</summary>
        public void PlayHeavyHitAnimation()
        {
            _animator.Play(PlayerConst.HeavyHitState);
        }

        /// <summary>防御状態の被弾アニメーションを再生する</summary>
        public void PlayGuardHitAnimation()
        {
            _animator.Play(PlayerConst.GuardHitState);
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

        /// <summary>AnimatorのRootMotionによる移動量をプレイヤー本体に適用する</summary>
        public void ApplyRootMotionToTransform()
        {
            Vector3 deltaPosition = _animator.deltaPosition;
            transform.position += deltaPosition;
        }
        
        /// <summary>モデルの位置にプレイヤー本体を同期させ、RootMotionによるズレを補正する</summary>
        public void SnapToModelPosition(Transform modelTransform)
        {
            // プレイヤー本体の位置をモデルの位置に同期させる
            transform.position = modelTransform.position;
            // モデルのローカル位置を初期化する
            modelTransform.localPosition = Vector3.zero;
        }
    }
}