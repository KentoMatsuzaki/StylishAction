using System.Threading;
using Cysharp.Threading.Tasks;
using Enemy.AsyncNode;

namespace Enemy.AI
{
    /// <summary>敵AIのインターフェース</summary>
    public interface IEnemyAI
    {
        /// <summary>ビヘイビアツリーを実行する</summary>
        UniTask ExecuteBehaviourTree(CancellationToken token);

        /// <summary>ビヘイビアツリーを構築する</summary>
        AsyncSelectorNode ConstructBehaviourTree();
        
        /// <summary>攻撃シーケンスを構築する</summary>
        AsyncSequenceNode ConstructAttackSequence();
    }
}