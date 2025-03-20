using System;
using System.Collections.Generic;
using System.Threading;
using Const;
using Enemy.AsyncNode;
using Cysharp.Threading.Tasks;
using Data.Enemy;
using Enemy.Handler;
using Enum.Enemy;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy.AI
{
    /// <summary>敵のAIを制御するクラス</summary>
    public class Phase1AI : EnemyAIBase
    {
        [Header("敵のステータス"), SerializeField] private EnemyStatusData statusData;
        [Header("スキルのリスト"), SerializeField] private List<EnemySkillData> skillDataList;
        
        /// <summary>キャンセルトークン</summary>
        private CancellationTokenSource _cts;
        /// <summary>メインノード</summary>
        private BaseAsyncNode _mainNode;
        /// <summary>アニメーション</summary>
        private EnemyAnimationHandler _animationHandler;
        /// <summary>プレイヤー</summary>
        private PlayerController _player;
        /// <summary>現在の体力</summary>
        private float _currentHp;
        /// <summary>現在のヒットカウント</summary>
        private int _currentHitCount;
        private Rigidbody _rb;

        /// <summary>次に使用するスキルのデータ</summary>
        private EnemySkillData _nextSkillData;
        
        //-------------------------------------------------------------------------------
        // 初期設定
        //-------------------------------------------------------------------------------

        private void Awake()
        {
            _animationHandler = GetComponent<EnemyAnimationHandler>();
            _rb = GetComponent<Rigidbody>();
        }

        public override void Initialize()
        {
            SetStatus();
            ConstructBehaviourTree();
        }

        public override void SetPlayer(PlayerController player)
        {
            _player = player;
        }

        protected override void SetStatus()
        {
            _currentHp = statusData.phase1MaxHp;
        }

        //-------------------------------------------------------------------------------
        // ビヘイビアツリーに関する処理
        //-------------------------------------------------------------------------------

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
        protected override void ConstructBehaviourTree()
        {
            var mainNode = new AsyncSelectorNode();
            mainNode.AddNode(ConstructAttackSequence());
            _mainNode = mainNode;
        }

        //-------------------------------------------------------------------------------
        // 攻撃シーケンスに関する処理
        //-------------------------------------------------------------------------------

        /// <summary>攻撃シーケンスを構築する</summary>
        protected override AsyncSequenceNode ConstructAttackSequence()
        {
            var attackAction = new AsyncActionNode(async (token) =>
            {
                // 次の攻撃を設定する
                SetNextSkillData();
                
                // プレイヤーが有効射程・角度内に存在する場合
                if (IsPlayerInAttackRange() && IsPlayerInAttackAngle())
                { 
                    // 移動フラグを設定する
                    _animationHandler.SetMoveFlag(false);
                    // 攻撃アニメーションをトリガーする
                    _animationHandler.TriggerAttack(_nextSkillData.type);
                    // 攻撃アニメーションの再生終了を待機する
                    await _animationHandler.WaitForAnimationEnd(token);
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
                    _animationHandler.SetMoveFlag(true);
                    
                    // プレイヤーの逆方向へ移動する
                    MoveAwayFromPlayer();
                }
                
                // プレイヤーがスキルの最大射程の外側にいる場合
                else
                {
                    // 移動フラグを設定する
                    _animationHandler.SetMoveFlag(true);
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

        /// <summary>次のスキルのデータをランダムに設定する</summary>
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

        /// <summary>プレイヤーがスキルの攻撃角度内に存在するか</summary>
        private bool IsPlayerInAttackAngle()
        {
            var angleToPlayer = Vector3.Angle(transform.forward, GetHorizontalDirectionToPlayer());
            return _nextSkillData.maxAttackAngle >= angleToPlayer;
        }

        /// <summary>高低差を無視してプレイヤーへの距離を求める</summary>
        private float GetHorizontalDistanceToPlayer()
        {
            return Vector3.Distance(new Vector3(_player.transform.position.x, 0, _player.transform.position.z), 
                new Vector3(transform.position.x, 0, transform.position.z));
        }

        /// <summary>高低差を無視してプレイヤーへの方向を求める</summary>
        private Vector3 GetHorizontalDirectionToPlayer()
        {
            return Vector3.Normalize(new Vector3(_player.transform.position.x, 0, _player.transform.position.z) - 
                new Vector3(transform.position.x, 0, transform.position.z));
        }
        
        /// <summary>プレイヤーの方向へ移動する</summary>
        private void MoveToPlayer()
        {
            transform.Translate(GetHorizontalDirectionToPlayer() * Time.deltaTime * statusData.moveSpeed, Space.World);
        }

        /// <summary>プレイヤーの方向へ回転する</summary>
        private void RotateToPlayer()
        {
            var desiredRotation = Quaternion.LookRotation(GetHorizontalDirectionToPlayer());
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime * statusData.rotateSpeed);
        }

        /// <summary>プレイヤーの逆方向へ移動する</summary>
        private void MoveAwayFromPlayer()
        {
            transform.Translate(-GetHorizontalDirectionToPlayer() * Time.deltaTime * statusData.moveSpeed * 3, Space.World);
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
        // ダメージに関する処理
        //-------------------------------------------------------------------------------

        /// <summary>被ダメージ時の処理</summary>
        public override void OnHit(float damage, Vector3 hitPosition)
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
                _currentHp = Mathf.Max(_currentHp - damage, 0);
                // ヒットカウントを増加させる
                _currentHitCount++;
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
            _rb.velocity = Vector3.zero;
            // ノックバックさせる
            _rb.AddForce(knockBackDirection * statusData.knockBackSpeed, ForceMode.Impulse);
        }

        /// <summary>怯み判定</summary>
        private bool IsFlinched()
        {
            if (_currentHitCount >= statusData.maxHitCount)
            {
                // 現在のヒットカウントをリセットする
                _currentHitCount = 0; return true;
            }
            return false;
        }

        /// <summary>怯み時の処理</summary>
        private void OnFlinch()
        {
            // ビヘイビアツリーをキャンセルする
            _cts?.Cancel();
            // 怯みアニメーションを再生する
            _animationHandler.PlayAnimation(InGameConst.EnemyHitAnimation);
        }

        /// <summary>死亡判定</summary>
        private bool IsDied()
        {
            return _currentHp <= 0;
        }

        /// <summary>死亡時の処理</summary>
        private void OnDie()
        {
            _cts?.Cancel();
            _animationHandler.PlayAnimation(InGameConst.EnemyDieAnimation);
        }
    }
}