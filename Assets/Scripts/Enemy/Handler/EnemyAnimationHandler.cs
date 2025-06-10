using System.Threading;
using Const;
using Cysharp.Threading.Tasks;
using Enum;
using UnityEngine;

namespace Enemy.Handler
{
    /// <summary>敵のアニメーションを制御するクラス</summary>
    public class EnemyAnimationHandler : MonoBehaviour
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
        // 登場に関する処理
        //-------------------------------------------------------------------------------

        /// <summary>登場のアニメーションを再生する</summary>
        public void PlayBornAnimation()
        {
            _animator.Play(EnemyConst.BornState);
        }
        
        //-------------------------------------------------------------------------------
        // 静止に関する処理
        //-------------------------------------------------------------------------------

        /// <summary>静止のアニメーションを再生する</summary>
        public void PlayIdleAnimation()
        {
            _animator.Play(EnemyConst.IdleState);
        }
        
        //-------------------------------------------------------------------------------
        // 移動に関する処理
        //-------------------------------------------------------------------------------
        
        /// <summary>移動のフラグを有効化する</summary>
        public void EnableMove()
        {
            _animator.SetBool(EnemyConst.IsMoving, true);
        }

        /// <summary>移動のフラグを無効化する</summary>
        public void DisableMove()
        {
            _animator.SetBool(EnemyConst.IsMoving, false);
        }
        
        //-------------------------------------------------------------------------------
        // 攻撃に関する処理
        //-------------------------------------------------------------------------------
        
        /// <summary>攻撃のトリガーを有効化する</summary>
        /// <param name="attackType">攻撃の種類</param>
        public void TriggerAttack(EnemyEnums.AttackType attackType)
        {
            _animator.SetTrigger(attackType.ToString());
        }
        
        //-------------------------------------------------------------------------------
        // スタンに関する処理
        //-------------------------------------------------------------------------------

        /// <summary>スタンのアニメーションを再生する</summary>
        public void PlayStunAnimation()
        {
            _animator.Play(EnemyConst.StunState);
        }

        /// <summary>スタン復帰のアニメーションを再生する</summary>
        public void PlayRecoverAnimation()
        {
            _animator.Play(EnemyConst.RecoverState);
        }
        
        //-------------------------------------------------------------------------------
        // 被弾に関する処理
        //-------------------------------------------------------------------------------

        /// <summary>被弾のアニメーションを再生する</summary>
        public void PlayHitAnimation()
        {
            _animator.Play(EnemyConst.HitState);
        }

        /// <summary>攻撃アニメーションを再生しているか</summary>
        public bool IsPlayingAttackAnimation()
        {
            var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            return !(stateInfo.IsName(EnemyConst.IdleState) || stateInfo.IsName(EnemyConst.MoveState));
        }
        
        //-------------------------------------------------------------------------------
        // 死亡に関する処理
        //-------------------------------------------------------------------------------

        /// <summary>死亡のアニメーションを再生する</summary>
        public void PlayDieAnimation()
        {
            _animator.Play(EnemyConst.DieState);
        }
        
        //-------------------------------------------------------------------------------
        // 非同期処理
        //-------------------------------------------------------------------------------
        
        /// <summary>アニメーションの再生完了を待つ</summary>
        public async UniTask WaitUntilAnimationComplete()
        {
            // 現在のステート情報を取得する
            var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            // アニメーションを再生している場合
            while (stateInfo.normalizedTime < 1.0f || _animator.IsInTransition(0))
            {
                // 次のフレームまで待機する
                await UniTask.Yield(PlayerLoopTiming.Update);
                // 現在のステート情報を更新する
                stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            }
        }
    }
}