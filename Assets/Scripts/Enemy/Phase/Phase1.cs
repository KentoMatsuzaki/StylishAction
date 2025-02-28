using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Enemy.Phase
{
    /// <summary>敵の一つ目のフェーズ</summary>
    public class Phase1 : IEnemyPhase
    {
        private readonly Func<CancellationToken, UniTask> _phaseEnterAction;

        public Phase1(Func<CancellationToken, UniTask> phaseEnterAction)
        {
            _phaseEnterAction = phaseEnterAction;
        }
        
        public async UniTask OnPhaseEnter(CancellationToken token)
        {
            await _phaseEnterAction(token);
        }
    }
}