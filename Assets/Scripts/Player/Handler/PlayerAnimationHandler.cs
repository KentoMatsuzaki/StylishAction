using Const;
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
        // 攻撃に関する処理
        //-------------------------------------------------------------------------------

        /// <summary>対応する攻撃アニメーションをトリガーする</summary>
        /// <param name="attackNumber">トリガーする攻撃の番号</param>
        public void TriggerAttack(int attackNumber)
        {
            _animator.SetTrigger($"Attack {attackNumber} Trigger");
        }
        
        //-------------------------------------------------------------------------------
        // ダッシュに関する処理
        //-------------------------------------------------------------------------------
        
        /// <summary>ダッシュアニメーションをトリガーする</summary>
        public void TriggerDash()
        {
            _animator.SetTrigger(InGameConst.PlayerDashTrigger);
        }
        
        //-------------------------------------------------------------------------------
        // パリィに関する処理
        //-------------------------------------------------------------------------------

        /// <summary>パリィアニメーションをトリガーする</summary>
        public void TriggerParry()
        {
            _animator.SetTrigger(InGameConst.PlayerParryTrigger);
        }
    }
}