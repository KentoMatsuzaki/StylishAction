using System.Threading;
using Const;
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
        
        //-------------------------------------------------------------------------------
        // 移動に関する処理
        //-------------------------------------------------------------------------------

        /// <summary>移動フラグを設定する</summary>
        public void SetMoveFlag(bool value)
        {
            if (_animator.GetBool(InGameConst.EnemyMoveFlag) != value)
            {
                _animator.SetBool(InGameConst.EnemyMoveFlag, value);
            }
        }
        
        //-------------------------------------------------------------------------------
        // 攻撃に関する処理
        //-------------------------------------------------------------------------------
        
        /// <summary>攻撃アニメーションをトリガーする</summary>
        /// <param name="skillNumber">攻撃スキルの番号（1~6）</param>
        public void TriggerAttack(int? skillNumber)
        {
            // スキル番号が未割り当ての場合は処理を抜ける
            if (!skillNumber.HasValue) return; 
            _animator.SetTrigger($"Skill {skillNumber.Value} Trigger");
        }
        
        /// <summary>アニメーションの再生終了を待つ</summary>
        public async UniTask WaitForAnimationEnd(CancellationToken token)
        {
            var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

            while (stateInfo.normalizedTime < 1.0f || _animator.IsInTransition(0) || stateInfo.normalizedTime > 1.1f)
            {
                await UniTask.Yield(PlayerLoopTiming.Update, token);
                stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            }
        }
    }
}