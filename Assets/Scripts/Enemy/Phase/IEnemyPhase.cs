using System.Threading;
using Cysharp.Threading.Tasks;

namespace Enemy.Phase
{
    /// <summary>敵のフェーズのインターフェース</summary>
    public interface IEnemyPhase
    {
        UniTask OnPhaseEnter(CancellationToken token);
    }
}
