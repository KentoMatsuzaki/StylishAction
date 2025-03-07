using System;
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

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }
        
        //-------------------------------------------------------------------------------
        // 汎用処理
        //-------------------------------------------------------------------------------

        /// <summary>アニメーションを再生する</summary>
        public void PlayAnimation(string stateName)
        {
            _animator.Play(stateName);
        }
        
        //-------------------------------------------------------------------------------
        // 攻撃アニメーションに関する処理
        //-------------------------------------------------------------------------------
        
        /// <summary>攻撃アニメーションをトリガーする</summary>
        /// <param name="skillNumber">攻撃スキルの番号（1~6）</param>
        public void TriggerAttack(int? skillNumber)
        {
            // スキル番号が未割り当ての場合は処理を抜ける
            if (!skillNumber.HasValue) return; 
            string triggerName = $"Skill{skillNumber.Value}Trigger";
            _animator.SetTrigger(triggerName);
        }
        
        /// <summary>アニメーションの再生終了を待つ</summary>
        public async UniTask WaitForAnimationEnd(CancellationToken token)
        {
            var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

            while (stateInfo.normalizedTime < 1.0f)
            {
                await UniTask.Yield(PlayerLoopTiming.Update, token);
                stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            }
        }
    }
}