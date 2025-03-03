using Cysharp.Threading.Tasks;
using Enemy.AsyncNode;

namespace Enemy.AI
{
    /// <summary>敵AIの基底クラス</summary>
    public abstract class EnemyAIBase
    {
        /// <summary>ビヘイビアツリーを開始する</summary>
        public abstract void BeginBehaviourTree();

        /// <summary>ビヘイビアツリーを実行する</summary>
        protected abstract UniTask ExecuteBehaviourTree();

        /// <summary>ビヘイビアツリーを構築する</summary>
        protected abstract AsyncSelectorNode ConstructBehaviourTree();
        
        /// <summary>攻撃シーケンスを構築する</summary>
        protected abstract AsyncSequenceNode ConstructAttackSequence();
    }
}