using System;
using System.Threading;
using Const;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Enemy.AsyncNode;
using Enemy.Handler;
using Enum;
using Particle;
using Player;
using SO.Player;
using Sound;
using UI;
using UniRx;

namespace Enemy.AI
{
    /// <summary>DeathのAI挙動を制御するクラス</summary>
    public class DeathAI : EnemyAIBase
    {
        private readonly ReactiveProperty<float> _currentHp = new ();
        public IReadOnlyReactiveProperty<float> CurrentHp => _currentHp;
        
        public float MaxHp { get; private set; }
        
        //-------------------------------------------------------------------------------
        // 初期設定
        //-------------------------------------------------------------------------------

        public override async UniTask Initialize()
        {
            InitializeComponents();
            InitializeStats();
            InitializeTransform();
            AnimationHandler.PlayBornAnimation();
            await AnimationHandler.WaitUntilAnimationComplete();
            BuildTree();
        }

        private void InitializeComponents()
        {
            AttackHandler = GetComponent<EnemyAttackHandler>();
            AnimationHandler = GetComponent<EnemyAnimationHandler>();
            Rb = GetComponent<Rigidbody>();
        }

        private void InitializeStats()
        {
            _currentHp.Value = stats.maxHp;
            MaxHp = stats.maxHp;
        }

        private void InitializeTransform()
        {
            transform.position = new Vector3(EnemyConst.InitialPositionX, EnemyConst.InitialPositionY, EnemyConst.InitialPositionZ);
            transform.rotation = Quaternion.Euler(EnemyConst.InitialRotationX, EnemyConst.InitialRotationY, EnemyConst.InitialRotationZ);
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
        public override void StartBehaviour()
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
                if (AttackHandler.CanAttackPlayer(player, CurrentAttackStats))
                {
                    // 移動アニメーションのフラグを無効化する
                    AnimationHandler.DisableMove();
                    // 攻撃アニメーションのトリガーを有効化する
                    AnimationHandler.TriggerAttack(CurrentAttackStats.attackType);
                    // 攻撃のクールタイムを待機する
                    await UniTask.Delay(TimeSpan.FromSeconds(CurrentAttackStats.attackCooldown), cancellationToken: token);
                    // 攻撃情報をリセットする
                    CurrentAttackStats = null;
                    // 評価結果を返す
                    return EnemyEnums.NodeStatus.Success;
                }
                
                // 移動アニメーションのフラグを有効化する
                AnimationHandler.EnableMove();

                // プレイヤーが攻撃の有効角度内にいない場合
                if (!AttackHandler.IsPlayerInAttackAngle(player, CurrentAttackStats))
                {
                    // プレイヤーの方向へ回転させる
                    AttackHandler.RotateTowardsPlayer(player, stats.rotateSpeed);
                }
                    
                // プレイヤーが攻撃の最小有効射程よりも近くにいる場合
                if (AttackHandler.IsPlayerTooClose(player, CurrentAttackStats))
                {
                    // プレイヤーの逆方向へ移動させる
                    AttackHandler.MoveAwayFromPlayer(player, stats.moveSpeed);
                }
                    
                // プレイヤーが攻撃の最大有効射程よりも遠くにいる場合
                if (AttackHandler.IsPlayerTooFar(player, CurrentAttackStats))
                {
                    // プレイヤーの方向へ移動させる
                    AttackHandler.MoveTowardsPlayer(player, stats.moveSpeed);
                }
                    
                // 評価結果を返す
                return EnemyEnums.NodeStatus.Running;
            });
        }
        
        /// <summary>攻撃情報のリストからランダムに現在の攻撃情報を選択する</summary>
        private void SelectCurrentAttackStats()
        {
            // 攻撃情報が既に設定されている場合は処理を抜ける
            if (CurrentAttackStats != null) return;
            // 攻撃情報をランダムに選択する
            CurrentAttackStats = attackStatsList[UnityEngine.Random.Range(0, attackStatsList.Count)];
        }
        
        //-------------------------------------------------------------------------------
        // 攻撃の結果を適用する処理
        //-------------------------------------------------------------------------------
        
        /// <summary>攻撃の結果を適用する</summary>
        public override void ApplyAttack(PlayerController playerController, Vector3 hitPosition)
        {
            // プレイヤーのダメージを適用する処理を呼ぶ
            playerController.ApplyDamage(this, CurrentAttackStats, hitPosition);
        }
        
        //-------------------------------------------------------------------------------
        // パリィの結果を適用する処理
        //-------------------------------------------------------------------------------

        /// <summary>パリィの結果を適用する</summary>
        public override void ApplyParry()
        {
            // ビヘイビアツリーの評価をキャンセルする
            Cts?.Cancel();
            // 敵をスタンさせる
            ApplyStun();
        }

        /// <summary>スタン時の処理</summary>
        private async void ApplyStun()
        {
            // スタンのアニメーションを再生する
            AnimationHandler.PlayStunAnimation();
            // スタンのパーティクルを有効化する
            ParticleManager.Instance.ActivateParticle(ParticleEnums.ParticleType.Stun);
            // スタン時間だけ待機する
            await UniTask.Delay(TimeSpan.FromSeconds(stats.stunDuration));
            // 復帰のアニメーションを再生する
            AnimationHandler.PlayRecoverAnimation();
            // スタンのパーティクルを無効化する
            ParticleManager.Instance.DeactivateParticle(ParticleEnums.ParticleType.Stun);
            // 復帰アニメーションの再生完了を待機する
            await AnimationHandler.WaitUntilAnimationComplete();
            // ビヘイビアツリーの評価を開始する
            StartBehaviour();
        }
        
        //-------------------------------------------------------------------------------
        // 被弾時の処理
        //-------------------------------------------------------------------------------

        /// <summary>ダメージを適用する</summary>
        public override void ApplyDamage(PlayerAttackStats attackStats, Vector3 hitPosition)
        {
            // 被弾時のパーティクルを有効化する
            ParticleManager.Instance.ActivateHitParticle(hitPosition);
            // ダメージを反映する
            TakeDamage(attackStats.attackDamage);
            
            // 死亡している場合
            if (_currentHp.Value <= 0)
            {
                // 死亡処理を呼ぶ
                ApplyDeath();
            }
            
            // 死亡していない場合
            else
            {
                // 攻撃アニメーションを再生していない場合
                if (!AnimationHandler.IsPlayingAttackAnimation())
                {
                    // 被弾のアニメーションを再生する
                    AnimationHandler.PlayHitAnimation();
                }
                // ノックバックさせる
                ApplyKnockBack();
            }
        }

        /// <summary>ダメージを反映する</summary>
        private void TakeDamage(float attackDamage)
        {
            // ダメージが0以下の場合はログを出力して処理を抜ける
            if (attackDamage <= 0)
            {
                Debug.LogWarning($"Received non-positive damage value : {attackDamage}");
                return;
            }

            // 現在の体力からダメージ量を減少させる
            _currentHp.Value = Mathf.Max(0, _currentHp.Value - attackDamage);
            Debug.Log($"Received damage : {attackDamage}. Current HP : {_currentHp}");
        }

        /// <summary>死亡時の処理</summary>
        private void ApplyDeath()
        {
            // ビヘイビアツリーの評価をキャンセルする
            Cts?.Cancel();
            // 死亡アニメーションを再生する
            AnimationHandler.PlayDieAnimation();
            // ゲームクリア時のUIを表示する
            UIManager.Instance.ShowGameClearUI();
            // メインUIを非表示にする
            UIManager.Instance.HideMainUI();
            // サウンドを再生する
            SoundManager.Instance.PlayOneShot(OutGameEnums.SoundType.Clear);
        }

        /// <summary>ノックバックを適用する</summary>
        private void ApplyKnockBack()
        {
            // プレイヤーの方向へ回転させる
            AttackHandler.RotateTowardsPlayer(player, stats.rotateSpeed);
            // プレイヤーの逆方向へ力を加える
            Rb.AddForce(AttackHandler.GetFlatDirectionToPlayer(player) * stats.knockBackPower, ForceMode.Impulse);
        }
        
        //-------------------------------------------------------------------------------
        // アニメーションイベント
        //-------------------------------------------------------------------------------
        
        public void ActivateMeteorParticle()
        {
            ParticleManager.Instance.ActivateParticle(ParticleEnums.ParticleType.Meteor);
        }

        public void ActivateSeraph1Particle()
        {
            ParticleManager.Instance.ActivateParticle(ParticleEnums.ParticleType.Seraph1);
        }

        public void ActivateSeraph2Particle()
        {
            ParticleManager.Instance.ActivateParticle(ParticleEnums.ParticleType.Seraph2);
        }

        public void ActivateEclipseParticle()
        {
            ParticleManager.Instance.ActivateParticle(ParticleEnums.ParticleType.Eclipse);
        }

        public void ActivateExplosionParticle()
        {
            ParticleManager.Instance.ActivateParticle(ParticleEnums.ParticleType.Explosion);
        }

        public void ActivateWaterFall1Particle()
        {
            ParticleManager.Instance.ActivateParticle(ParticleEnums.ParticleType.WaterFall1);
        }
        
        public void ActivateWaterFall2Particle()
        {
            ParticleManager.Instance.ActivateParticle(ParticleEnums.ParticleType.WaterFall2);
        }

        public void ActivateVortexParticle1()
        {
            ParticleManager.Instance.ActivateParticle(ParticleEnums.ParticleType.Vortex1);
        }

        public void DeactivateVortexParticle1()
        {
            ParticleManager.Instance.DeactivateParticle(ParticleEnums.ParticleType.Vortex1);
        }
        
        public void ActivateVortexParticle2()
        {
            ParticleManager.Instance.ActivateParticle(ParticleEnums.ParticleType.Vortex2);
        }

        public void ActivatePhotonParticles()
        {
            ParticleManager.Instance.ActivatePhotonParticles();
        }

        public void PlayScytheSound()
        {
            SoundManager.Instance.PlayOneShot(OutGameEnums.SoundType.Scythe);
        }

        public void PlayMeteorSound()
        {
            SoundManager.Instance.PlayOneShot(OutGameEnums.SoundType.Meteor);
        }

        public void PlaySeraphSound()
        {
            SoundManager.Instance.PlayOneShot(OutGameEnums.SoundType.Seraph);
        }

        public void PlayVortexSound()
        {
            SoundManager.Instance.PlayOneShot(OutGameEnums.SoundType.Vortex);
        }

        public void PlayPhotonSound()
        {
            SoundManager.Instance.PlayOneShot(OutGameEnums.SoundType.Photon);
        }

        public void PlayExplosionSound()
        {
            SoundManager.Instance.PlayOneShot(OutGameEnums.SoundType.Explosion);
        }

        public void PlayWaterFallSound()
        {
            SoundManager.Instance.PlayOneShot(OutGameEnums.SoundType.Waterfall);
        }
        
        //-------------------------------------------------------------------------------
        // その他の処理
        //-------------------------------------------------------------------------------

        public override void ResetBehaviour()
        {
            Cts?.Cancel();
        }
    }
}