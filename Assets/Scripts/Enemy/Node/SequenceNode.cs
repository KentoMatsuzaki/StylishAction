using Enum;
using System.Collections.Generic;
using System.Linq;

namespace Enemy.Nodes
{
    /// <summary>シーケンスノード</summary>
    public class SequenceNode : BaseNode
    {
        /// <summary>子ノードのリスト</summary>
        readonly List<BaseNode> _nodeList = new List<BaseNode>();

        /// <summary>子ノードを追加する</summary>
        public void AddNode(BaseNode node)
        {
            _nodeList.Add(node);
        }

        /// <summary>ノードの評価結果を返す</summary>
        /// <returns>Success = 成功, Failure = 失敗, Running = 実行中</returns>
        public override EnemyEnum.NodeStatus Execute()
        {
            foreach (var status in _nodeList.Select(node => node.Execute()))
            {
                if (status != EnemyEnum.NodeStatus.Success) return status;
            }
            // 全ての子ノードの評価結果が成功の場合のみ、成功の評価結果を返す
            return EnemyEnum.NodeStatus.Success;
        }
    }
}