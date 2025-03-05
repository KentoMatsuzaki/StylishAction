using System;
using System.Threading;
using Const;
using Enemy.AsyncNode;
using Cysharp.Threading.Tasks;
using Enemy.Handler;
using Enum;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy.AI
{
    /// <summary>敵のAIを制御するクラス</summary>
    public class Phase1AIBase : EnemyAIBase
    {
        /// <summary>キャンセルトークン</summary>
        private CancellationTokenSource _cts;
        /// <summary>メインノード</summary>
        private BaseAsyncNode _mainNode;
        /// <summary>アニメーション</summary>
        private readonly EnemyAnimationHandler _animationHandler;

        /// <summary>コンストラクタ</summary>
        public Phase1AIBase(EnemyAnimationHandler animationHandler)
        {
            _animationHandler = animationHandler;
        }

        /// <summary>ビヘイビアツリーを開始する</summary>
        public override void BeginBehaviourTree()
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            ExecuteBehaviourTree().Forget();
        }

        /// <summary>ビヘイビアツリーを実行する</summary>
        protected override async UniTask ExecuteBehaviourTree()
        {
            try
            {
                while (!_cts.Token.IsCancellationRequested)
                {
                    await _mainNode.ExecuteAsync(_cts.Token);
                    await UniTask.Yield(PlayerLoopTiming.Update, _cts.Token);
                }
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Behaviour Tree Canceled");
            }
        }

        /// <summary>ビヘイビアツリーを構築する</summary>
        protected override AsyncSelectorNode ConstructBehaviourTree()
        {
            return new AsyncSelectorNode();
        }

        //-------------------------------------------------------------------------------
        // 攻撃に関する処理
        //-------------------------------------------------------------------------------

        /// <summary>攻撃シーケンスを構築する</summary>
        protected override AsyncSequenceNode ConstructAttackSequence()
        {
            var attackAction = new AsyncActionNode(async (token) =>
            {
                // 攻撃アニメーションのトリガーを設定する
                _animationHandler.TriggerAttack(Random.Range(1,3));
                
                // 攻撃アニメーションの再生終了を待機する
                await _animationHandler.WaitForAnimationEnd(token);

                // ノードの評価結果を返す
                return EnemyEnum.NodeStatus.Success;
            });
            
            var attackSequence = new AsyncSequenceNode();
            return attackSequence;
        }
        
        //-------------------------------------------------------------------------------
        // パリィに関する処理
        //-------------------------------------------------------------------------------

        /// <summary>プレイヤーに攻撃が命中した時の処理</summary>
        public override void OnHitPlayer(PlayerController player)
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
        
    }
}