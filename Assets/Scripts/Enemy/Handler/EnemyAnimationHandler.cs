using System.Threading;
using Const;
using Cysharp.Threading.Tasks;
using Enum.Enemy;
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
        /// <param name="type">スキルの種類</param>
        public void TriggerAttack(EnemyEnum.EnemyAttackType type)
        {
            _animator.SetTrigger(type.ToString());
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