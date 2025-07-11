using System.Threading;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Definitions.Enum;

namespace Enemy.AsyncNode
{
    /// <summary>非同期セレクターノード</summary>
    public class AsyncSelectorNode : BaseAsyncNode
    {
        /// <summary>子ノードのリスト</summary>
        private readonly List<BaseAsyncNode> _nodeList = new List<BaseAsyncNode>();

        /// <summary>子ノードを追加する</summary>
        public void AddNode(BaseAsyncNode node)
        {
            _nodeList.Add(node);
        }

        /// <summary>ノードの評価結果を返す</summary>
        /// <returns>Success = 成功, Failure = 失敗, Running = 実行中</returns>
        public override async UniTask<InGameEnums.EnemyNodeStatus> ExecuteAsync(CancellationToken token)
        {
            // 全ての子ノードを評価する
            foreach (var node in _nodeList)
            {
                var status = await node.ExecuteAsync(token);
                if (status != InGameEnums.EnemyNodeStatus.Failure) return status;
            }
            // 全ての子ノードの評価結果が失敗の場合のみ、失敗の評価結果を返す
            return InGameEnums.EnemyNodeStatus.Failure;
        }
    }
}