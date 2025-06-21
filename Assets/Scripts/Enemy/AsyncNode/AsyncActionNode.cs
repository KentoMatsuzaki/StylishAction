using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Definitions.Enum;

namespace Enemy.AsyncNode
{
    /// <summary>非同期アクションノード</summary>
    public class AsyncActionNode : BaseAsyncNode
    {
        /// <summary>非同期アクション</summary>
        private readonly Func<CancellationToken, UniTask<InGameEnums.EnemyNodeStatus>> _action;

        /// <summary>コンストラクター</summary>
        public AsyncActionNode(Func<CancellationToken, UniTask<InGameEnums.EnemyNodeStatus>> action)
        {
            _action = action;
        }

        /// <summary>ノードの評価結果を返す</summary>
        /// <returns>Success = 成功, Failure = 失敗, Running = 実行中</returns>
        public override UniTask<InGameEnums.EnemyNodeStatus> ExecuteAsync(CancellationToken token)
        {
            return _action(token);
        }
    }
}