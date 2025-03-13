using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Enum.Enemy;

namespace Enemy.AsyncNode
{
    /// <summary>非同期アクションノード</summary>
    public class AsyncActionNode : BaseAsyncNode
    {
        /// <summary>非同期アクション</summary>
        private readonly Func<CancellationToken, UniTask<EnemyEnum.NodeStatus>> _action;

        /// <summary>コンストラクター</summary>
        public AsyncActionNode(Func<CancellationToken, UniTask<EnemyEnum.NodeStatus>> action)
        {
            _action = action;
        }

        /// <summary>ノードの評価結果を返す</summary>
        /// <returns>Success = 成功, Failure = 失敗, Running = 実行中</returns>
        public override UniTask<EnemyEnum.NodeStatus> ExecuteAsync(CancellationToken token)
        {
            return _action(token);
        }
    }
}