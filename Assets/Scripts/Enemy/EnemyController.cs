using System.Threading;
using Const;
using Cysharp.Threading.Tasks;
using Enemy.AI;
using Enemy.Handler;
using Player;
using UnityEngine;

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

        /// <summary>プレイヤーに攻撃が命中した時の処理</summary>
        public void OnHitPlayer(PlayerController player)
        {
            // プレイヤーがパリィ状態である場合
            OnParried();
            // プレイヤーがパリィ状態でない場合
            // player.TakeDamage
        }

        /// <summary>プレイヤーにパリィされた時の処理</summary>
        private void OnParried()
        {
            // ビヘイビアツリーをキャンセルする
            _cts?.Cancel();
            // スタン処理を呼ぶ
            Stun();
        }

        /// <summary>敵のスタン時の処理</summary>
        private async void Stun()
        {
            // アニメーションを再生する
            _animationHandler.PlayAnimation(InGameConst.EnemyStunAnimation);
            // スタンしている間は待機する
            await UniTask.Delay(2000);
            // ビヘイビアツリーを再開する
            BeginBehaviourTree();
        }
        
        //-------------------------------------------------------------------------------
        // 攻撃コライダーに関する処理
        //-------------------------------------------------------------------------------

        public void EnableSkill1AttackCollider() => skill1AttackHandler.EnableAttackCollider();
        public void DisableSkill1AttackCollider() => skill1AttackHandler.DisableAttackCollider();
        public void EnableSkill2AttackCollider() => skill2AttackHandler.EnableAttackCollider();
        public void DisableSkill2AttackCollider() => skill2AttackHandler.DisableAttackCollider();
    }
}