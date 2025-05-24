using System;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Enemy.AsyncNode;
using Enemy.Handler;
using Enum;
using Player;

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
            RootNode.AddNode(BuildAttackSequence());
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
            // 攻撃シーケンスを作成する
            var attackSequence = new AsyncSequenceNode();
            // 攻撃アクションを追加する
            attackSequence.AddNode(BuildAttackAction());
            // 攻撃シーケンスを返す
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
                    // 移動アニメーションのフラグを無効化する
                    AnimationHandler.DisableMove();
                    // 攻撃アニメーションのトリガーを有効化する
                    AnimationHandler.TriggerAttack(CurrentAttackStats.attackType);
                    // 攻撃アニメーションの再生完了を待つ
                    await AnimationHandler.WaitUntilAnimationComplete(token);
                    // 攻撃のクールタイムを待機する
                    await UniTask.Delay(TimeSpan.FromSeconds(CurrentAttackStats.attackCooldown), cancellationToken: token);
                    // 攻撃情報をリセットする
                    CurrentAttackStats = null;
                    // 評価結果を返す
                    return EnemyEnum.NodeStatus.Success;
                }

                // 移動アニメーションのフラグを有効化する
                AnimationHandler.EnableMove();
                // プレイヤーの方向へ回転させる
                AttackHandler.RotateTowardsPlayer(Player, Stats.rotateSpeed);
                    
                // プレイヤーが攻撃の最小有効射程よりも近くにいる場合
                if (AttackHandler.IsPlayerTooClose(Player, CurrentAttackStats))
                {
                    // プレイヤーの逆方向へ移動させる
                    AttackHandler.MoveAwayFromPlayer(Player, Stats.moveSpeed);
                }
                    
                // プレイヤーが攻撃の最大有効射程よりも遠くにいる場合
                if (AttackHandler.IsPlayerTooFar(Player, CurrentAttackStats))
                {
                    // プレイヤーの方向へ移動させる
                    AttackHandler.MoveTowardsPlayer(Player, Stats.moveSpeed);
                }
                    
                // 評価結果を返す
                return EnemyEnum.NodeStatus.Running;
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
        
        //-------------------------------------------------------------------------------
        // 攻撃の結果を適用する処理
        //-------------------------------------------------------------------------------
        
        /// <summary>攻撃の結果を適用する</summary>
        public override void ApplyAttack(PlayerController player)
        {
            // プレイヤー側のダメージを適用する処理を呼ぶ
            player.ApplyDamage(this, CurrentAttackStats);
        }
        
        //-------------------------------------------------------------------------------
        // パリィの結果を適用する処理
        //-------------------------------------------------------------------------------

        /// <summary>パリィの結果を適用する</summary>
        public override void ApplyParry()
        {
            // ビヘイビアツリーの評価をキャンセルする
            Cts.Cancel();
            // 
        }
    }
}