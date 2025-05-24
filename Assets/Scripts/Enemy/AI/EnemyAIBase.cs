using Cysharp.Threading.Tasks;
using Player;
using SO.Enemy;
using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using Enemy.AsyncNode;
using Enemy.Handler;

namespace Enemy.AI
{
    /// <summary>敵のAI挙動を制御する抽象クラス</summary>
    public abstract class EnemyAIBase : MonoBehaviour
    {
        /// <summary>プレイヤー</summary>
        protected PlayerController Player;
        
        /// <summary>ステータス情報</summary>
        protected EnemyStats Stats;

        /// <summary>攻撃情報のリスト</summary>
        protected List<EnemyAttackStats> AttackStatsList;

        /// <summary>現在の攻撃情報</summary>
        protected EnemyAttackStats CurrentAttackStats;
        
        /// <summary>ビヘイビアツリーの最上位ノード</summary>
        protected BaseAsyncNode RootNode;

        /// <summary>ビヘイビアツリーのキャンセルトークン</summary>
        protected CancellationTokenSource Cts;

        /// <summary>攻撃の制御クラス</summary>
        protected EnemyAttackHandler AttackHandler;

        //-------------------------------------------------------------------------------
        // ビヘイビアツリーに関する処理
        //-------------------------------------------------------------------------------
        
        /// <summary>ビヘイビアツリーを構築する</summary>
        protected abstract void BuildTree();

        /// <summary>ビヘイビアツリーの評価を実行する</summary>
        protected abstract UniTask Tick();
        
        /// <summary>ビヘイビアツリーの評価を開始する</summary>
        protected abstract void StartBehaviour();
        
        //-------------------------------------------------------------------------------
        // 攻撃に関する処理
        //-------------------------------------------------------------------------------

        /// <summary>攻撃の結果を適用する</summary>
        public abstract void ApplyAttack();
    }
}