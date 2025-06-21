using System.Threading;
using Cysharp.Threading.Tasks;
using Definitions.Enum;

namespace Enemy.AsyncNode
{
    /// <summary>非同期ノードの基底クラス</summary>
    public abstract class BaseAsyncNode 
    {
        /// <summary>ノードの評価結果を返す</summary>
        /// <returns>Success = 成功, Failure = 失敗, Running = 実行中</returns>
        public abstract UniTask<InGameEnums.EnemyNodeStatus> ExecuteAsync(CancellationToken token);
    }
}