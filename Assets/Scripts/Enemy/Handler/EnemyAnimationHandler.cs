using System.Threading;
using Cysharp.Threading.Tasks;
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

        /// <summary>トリガーを有効化し、対応するアニメーションを再生する</summary>
        public void SetTrigger(string triggerName)
        {
            _animator.SetTrigger(triggerName);
        }
        
        //-------------------------------------------------------------------------------
        // 攻撃アニメーションに関する処理
        //-------------------------------------------------------------------------------

        public async UniTask WaitForAnimationEnd(string stateName, CancellationToken token)
        {
            var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

            while (stateInfo.IsName(stateName) && stateInfo.normalizedTime < 1.0f)
            {
                await UniTask.Yield(PlayerLoopTiming.Update, token);
                stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            }
        }
    }
}