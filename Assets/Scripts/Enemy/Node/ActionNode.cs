using Enum;
using System;
using Enum.Enemy;

namespace Enemy.Nodes
{
    /// <summary>アクションノード</summary>
    public class ActionNode : BaseNode
    {
        /// <summary>アクション</summary>
        readonly Func<EnemyEnum.NodeStatus> _action;

        /// <summary>コンストラクター</summary>
        public ActionNode(Func<EnemyEnum.NodeStatus> action)
        {
            _action = action;
        }

        /// <summary>ノードの評価結果を返す</summary>
        /// <returns>Success = 成功, Failure = 失敗, Running = 実行中</returns>
        public override EnemyEnum.NodeStatus Execute()
        {
            return _action();
        }
    }
}