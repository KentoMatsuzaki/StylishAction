using System.Threading;
using Enemy.AI;
using Enemy.Handler;
using UnityEngine;

namespace Enemy
{
    /// <summary>敵を制御するクラス</summary>
    public class EnemyController : MonoBehaviour
    {
        private Phase1AIBase _phase1AIBase;
        
        private CancellationTokenSource _cts;
        public EnemyAIBase Bt { get; private set; }
        
        /// <summary></summary>
        private EnemyAnimationHandler _animationHandler;

        private void Start()
        {
            _animationHandler = GetComponent<EnemyAnimationHandler>();
            _phase1AIBase = new Phase1AIBase(_animationHandler);
        }

        /// <summary>ビヘイビアツリーを開始する</summary>
        private void BeginBehaviourTree() => Bt.BeginBehaviourTree();
    }
}