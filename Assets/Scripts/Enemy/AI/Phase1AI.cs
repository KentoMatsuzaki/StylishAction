using System;
using System.Collections.Generic;
using System.Threading;
using Const;
using Enemy.AsyncNode;
using Cysharp.Threading.Tasks;
using Enum.Enemy;
using Player;
using SO.Enemy;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy.AI
{
    /// <summary>敵のAIを制御するクラス</summary>
    public class Phase1AI : EnemyAIBase
    {
        [Header("スキルのリスト"), SerializeField] private List<EnemySkillData> skillDataList;

        /// <summary>次に使用するスキルのデータ</summary>
        private EnemySkillData _nextSkillData;
        
        //-------------------------------------------------------------------------------
        // フェーズ切り替え時の処理
        //-------------------------------------------------------------------------------

        //-------------------------------------------------------------------------------
        // ビヘイビアツリーの構築・実行・開始
        //-------------------------------------------------------------------------------

        /// <summary>ビヘイビアツリーを構築する</summary>
        public override void ConstructBehaviourTree()
        {
            var mainNode = new AsyncSelectorNode();
            mainNode.AddNode(ConstructAttackSequence());
            MainNode = mainNode;
        }
        
        /// <summary>ビヘイビアツリーを実行する</summary>
        protected override async UniTask ExecuteBehaviourTree()
        {
            try
            {
                while (!Cts.Token.IsCancellationRequested)
                {
                    await MainNode.ExecuteAsync(Cts.Token);
                    await UniTask.Yield(PlayerLoopTiming.Update, Cts.Token);
                }
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Behaviour Tree Canceled");
            }
        }
        
        /// <summary>ビヘイビアツリーを開始する</summary>
        public override void BeginBehaviourTree()
        {
            Cts?.Cancel();
            Cts = new CancellationTokenSource();
            ExecuteBehaviourTree().Forget();
        }
        
        //-------------------------------------------------------------------------------
        // 攻撃シーケンスに関する処理
        //-------------------------------------------------------------------------------

        /// <summary>攻撃シーケンスを構築する</summary>
        private AsyncSequenceNode ConstructAttackSequence()
        {
            var attackAction = new AsyncActionNode(async (token) =>
            {
                // 次の攻撃を設定する
                SetNextSkillData();
                
                // プレイヤーが有効射程・角度内に存在する場合
                if (IsPlayerInAttackRange() && IsPlayerInAttackAngle())
                { 
                    // 移動フラグを設定する
                    AnimationHandler.SetMoveFlag(false);
                    // 攻撃アニメーションをトリガーする
                    AnimationHandler.TriggerAttack(_nextSkillData.type);
                    // 攻撃アニメーションの再生終了を待機する
                    await AnimationHandler.WaitForAnimationEnd(token);
                    // 攻撃のクールタイム
                    await UniTask.Delay(1000);
                    // スキルのデータをリセットする
                    _nextSkillData = null;
                    // ノードの評価結果を返す
                    return EnemyEnum.NodeStatus.Success;
                }

                // プレイヤーがスキルの最小射程の内側にいる場合
                if (!IsPlayerBeyondMinAttackRange())
                {
                    // 移動フラグを設定する
                    AnimationHandler.SetMoveFlag(true);
                    // プレイヤーの逆方向へ移動する
                    MoveAwayFromPlayer();
                }
                
                // プレイヤーがスキルの最大射程の外側にいる場合
                else
                {
                    // 移動フラグを設定する
                    AnimationHandler.SetMoveFlag(true);
                    // プレイヤーの方向へ回転する
                    RotateToPlayer();
                    // プレイヤーの方向へ移動する
                    MoveToPlayer();
                }
                
                // ノードの評価結果を返す
                return EnemyEnum.NodeStatus.Running;
            });
            
            var attackSequence = new AsyncSequenceNode();
            attackSequence.AddNode(attackAction);
            return attackSequence;
        }

        /// <summary>次に使用するスキルのデータをランダムに設定する</summary>
        private void SetNextSkillData()
        {
            // スキルのデータが既に設定されている場合は処理を抜ける
            if (_nextSkillData != null) return;
            // スキルのデータをランダムに取得する
            _nextSkillData = skillDataList[Random.Range(0, skillDataList.Count)];
        }

        /// <summary>プレイヤーがスキルの有効射程内に存在するか</summary>
        private bool IsPlayerInAttackRange()
        {
            return IsPlayerBeyondMinAttackRange() && IsPlayerInMaxAttackRange();
        }

        /// <summary>プレイヤーとの距離がスキルの最小射程よりも大きいかどうか</summary>
        private bool IsPlayerBeyondMinAttackRange()
        {
            var distance = GetHorizontalDistanceToPlayer();
            return _nextSkillData.minAttackRange <= distance;
        }

        /// <summary>プレイヤーとの距離がスキルの最大射程よりも小さいかどうか</summary>
        private bool IsPlayerInMaxAttackRange()
        {
            var distance = GetHorizontalDistanceToPlayer();
            return distance <= _nextSkillData.maxAttackRange;
        }

        /// <summary>プレイヤーがスキルの有効角度内に存在するか</summary>
        private bool IsPlayerInAttackAngle()
        {
            var angleToPlayer = Vector3.Angle(transform.forward, GetHorizontalDirectionToPlayer());
            return _nextSkillData.maxAttackAngle >= angleToPlayer;
        }

        /// <summary>高低差を無視してプレイヤーへの距離を求める</summary>
        private float GetHorizontalDistanceToPlayer()
        {
            return Vector3.Distance(new Vector3(Player.transform.position.x, 0, Player.transform.position.z), 
                new Vector3(transform.position.x, 0, transform.position.z));
        }

        /// <summary>高低差を無視してプレイヤーへの方向を求める</summary>
        private Vector3 GetHorizontalDirectionToPlayer()
        {
            return Vector3.Normalize(new Vector3(Player.transform.position.x, 0, Player.transform.position.z) - 
                new Vector3(transform.position.x, 0, transform.position.z));
        }
        
        /// <summary>プレイヤーの方向へ移動する</summary>
        private void MoveToPlayer()
        {
            transform.Translate(GetHorizontalDirectionToPlayer() * Time.deltaTime * StatusData.moveSpeed, Space.World);
        }

        /// <summary>プレイヤーの方向へ回転する</summary>
        private void RotateToPlayer()
        {
            var desiredRotation = Quaternion.LookRotation(GetHorizontalDirectionToPlayer());
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime * StatusData.rotateSpeed);
        }

        /// <summary>プレイヤーの逆方向へ移動する</summary>
        private void MoveAwayFromPlayer()
        {
            transform.Translate(-GetHorizontalDirectionToPlayer() * Time.deltaTime * StatusData.moveSpeed * 3, Space.World);
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
            //EnemySkillDatabase.Instance.GetSkillData(_nextSkillNumber).damageAmount;
        }

        /// <summary>プレイヤーにパリィされた時の処理</summary>
        private void OnParried()
        {
            // ビヘイビアツリーをキャンセルする
            Cts?.Cancel();
            // スタン処理を呼ぶ
            Stun();
        }

        /// <summary>敵のスタン時の処理</summary>
        private async void Stun()
        {
            // アニメーションを再生する
            AnimationHandler.PlayAnimation(InGameConst.EnemyStunAnimation);
            // スタンしている間は待機する
            await UniTask.Delay(2000);
            // ビヘイビアツリーを再開する
            BeginBehaviourTree();
        }
        
        //-------------------------------------------------------------------------------
        // ダメージに関する処理
        //-------------------------------------------------------------------------------

        /// <summary>被ダメージ時の処理</summary>
        public override void OnHitByPlayer(float damage, Vector3 hitPosition)
        {
            // ダメージ処理
            TakeDamage(damage);
            
            // 死亡処理
            if (IsDied())
            {
                OnDie(); return;
            }
            
            // 怯み処理
            if (IsFlinched())
            {
                OnFlinch();
            }
            
            // ノックバック
            KnockBack(hitPosition);
        }

        /// <summary>ダメージ処理</summary>
        private void TakeDamage(float damage)
        {
            if (damage > 0)
            {
                // ダメージを適用する
                CurrentHp = Mathf.Max(CurrentHp - damage, 0);
                // ヒットカウントを増加させる
                CurrentHitCount++;
            }
        }

        /// <summary>ノックバック処理</summary>
        /// <param name="hitPosition">プレイヤーの攻撃が命中した座標</param>
        private void KnockBack(Vector3 hitPosition)
        {
            // ノックバックさせる方向を求める
            var knockBackDirection = (transform.position - hitPosition).normalized;
            // Y座標を無視する
            knockBackDirection.y = 0;
            // 速度をリセットする
            Rigidbody.velocity = Vector3.zero;
            // ノックバックさせる
            Rigidbody.AddForce(knockBackDirection * StatusData.knockBackSpeed, ForceMode.Impulse);
        }

        /// <summary>怯み判定</summary>
        private bool IsFlinched()
        {
            if (CurrentHitCount >= StatusData.maxHitCount)
            {
                // 現在のヒットカウントをリセットする
                CurrentHitCount = 0; return true;
            }
            return false;
        }

        /// <summary>怯み時の処理</summary>
        private void OnFlinch()
        {
            // ビヘイビアツリーをキャンセルする
            Cts?.Cancel();
            // 怯みアニメーションを再生する
            AnimationHandler.PlayAnimation(InGameConst.EnemyHitAnimation);
        }

        /// <summary>死亡判定</summary>
        private bool IsDied()
        {
            return CurrentHp <= 0;
        }

        /// <summary>死亡時の処理</summary>
        private void OnDie()
        {
            Cts?.Cancel();
            AnimationHandler.PlayAnimation(InGameConst.EnemyDieAnimation);
        }
    }
}