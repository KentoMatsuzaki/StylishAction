using System;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Enemy.AsyncNode;
using Enemy.Handler;

namespace Enemy.AI
{
    /// <summary>DeathのAI挙動を制御するクラス</summary>
    public class DeathAI : EnemyAIBase
    {
        //-------------------------------------------------------------------------------
        // 初期設定
        //-------------------------------------------------------------------------------

        private void Start()
        {
            AttackHandler = GetComponent<EnemyAttackHandler>();
        }

        //-------------------------------------------------------------------------------
        // ビヘイビアツリーに関する処理
        //-------------------------------------------------------------------------------

        /// <summary>ビヘイビアツリーを構築する</summary>
        protected override void BuildTree()
        {
            // 最上位ノードを作成する
            RootNode = new AsyncSelectorNode();
            // 最上位ノードに攻撃シーケンス追加する
        }

        /// <summary>ビヘイビアツリーの評価を実行する</summary>
        protected override async UniTask Tick()
        {
            try
            {
                while (!Cts.Token.IsCancellationRequested)
                {
                    // 最上位ノードを評価する
                    await RootNode.TickAsync(Cts.Token);
                    // 次のフレームまで待機する
                    await UniTask.Yield(PlayerLoopTiming.Update, Cts.Token);
                }
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Death AI : Tick loop was canceled");
            }
        }

        /// <summary>ビヘイビアツリーの評価を開始する</summary>
        protected override void StartBehaviour()
        {
            // 既存のキャンセルトークンがあればキャンセルする
            Cts?.Cancel();
            // 新しいキャンセルトークンを作成する
            Cts = new CancellationTokenSource();
            // ビヘイビアツリーの評価を開始する
            Tick().Forget();
        }
        
        //-------------------------------------------------------------------------------
        // 攻撃シーケンスに関する処理
        //-------------------------------------------------------------------------------

        /// <summary>攻撃シーケンスを構築する</summary>
        private AsyncSequenceNode BuildAttackSequence()
        {
            var attackSequence = new AsyncSequenceNode();
            return attackSequence;
        }

        /// <summary>攻撃アクションを構築する</summary>
        private AsyncActionNode BuildAttackAction()
        {
            return new AsyncActionNode(async (token) =>
            {
                // 攻撃情報を選択する
                SelectCurrentAttackStats();

                // プレイヤーを攻撃できる場合
                if (AttackHandler.CanAttackPlayer(Player, CurrentAttackStats))
                {
                    
                }
            });
        }
        
        /// <summary>攻撃情報のリストからランダムに現在の攻撃情報を選択する</summary>
        private void SelectCurrentAttackStats()
        {
            // 攻撃情報が既に設定されている場合は処理を抜ける
            if (CurrentAttackStats != null) return;
            // 攻撃情報をランダムに選択する
            CurrentAttackStats = AttackStatsList[UnityEngine.Random.Range(0, AttackStatsList.Count)];
        }
    }
}