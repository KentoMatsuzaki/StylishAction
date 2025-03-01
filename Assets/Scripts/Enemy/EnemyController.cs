using System.Threading;
using Cysharp.Threading.Tasks;
using Enemy.AI;
using Enemy.Handler;
using Player;
using UnityEngine;
using UnityEngine.Animations;

namespace Enemy
{
    /// <summary>敵を制御するクラス</summary>
    public class EnemyController : MonoBehaviour
    {
        private Phase1AI _phase1AI;
        
        private CancellationTokenSource _cts;
        private IEnemyAI _bt;
        
        private EnemyAnimationHandler _animationHandler;

        [Header("スキル1の攻撃クラス"), SerializeField] private EnemyAttackHandler skill1AttackHandler;
        [Header("スキル2の攻撃クラス"), SerializeField] private EnemyAttackHandler skill2AttackHandler;

        private void Start()
        {
            _animationHandler = GetComponent<EnemyAnimationHandler>();
            _phase1AI = new Phase1AI(_animationHandler, skill1AttackHandler, skill2AttackHandler);
        }

        private void BeginBehaviourTree()
        {
            // キャンセルトークンを作成する
            _cts = new CancellationTokenSource();
            // ビヘイビアツリーを実行する
            _bt.ExecuteBehaviourTree(_cts.Token).Forget();
        }
        
        //-------------------------------------------------------------------------------
        // パリィに関する処理
        //-------------------------------------------------------------------------------

        public void OnHitPlayer(PlayerController player)
        {
            // プレイヤーがパリィ状態である場合
            OnParried();
            // プレイヤーがパリィ状態でない場合
            // player.TakeDamage
        }

        private void OnParried()
        {
            _cts?.Cancel();
        }

        private async void Stun()
        {
            // アニメーションを再生する
            
            
            // スタンしている間は待機する
            
            
            // ビヘイビアツリーを再開する
            BeginBehaviourTree();
        }
    }
}