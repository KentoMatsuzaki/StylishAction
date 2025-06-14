using System;
using UnityEngine;
using UniRx;

namespace Extensions
{
    /// <summary>
    /// Animatorに対するUniRxベースの拡張メソッドを提供するクラス
    /// アニメーションステートの監視や、再生状態のリアクティブな検出に利用できる
    /// </summary>
    public static class AnimatorRxExtensions
    {
        /// <summary>
        /// 指定したステートのnormalizedTimeを監視する
        /// ループアニメーションはnormalizedTimeをnormalizedTime % 1f に補正する
        /// </summary>
        /// <param name="animator">監視対象のAnimator</param>
        /// <param name="stateName">監視対象のステート名</param>
        /// <param name="loop">ループアニメーションの場合はtrue</param>
        public static IObservable<float> ObserveNormalizedTime(this Animator animator, string stateName, bool loop = true)
        {
            return Observable.EveryUpdate()
                .Select(_ => animator.GetCurrentAnimatorStateInfo(0))
                .Where(info => info.IsName(stateName))
                .Select(info => loop ? info.normalizedTime % 1f : info.normalizedTime);
        }
    }
}