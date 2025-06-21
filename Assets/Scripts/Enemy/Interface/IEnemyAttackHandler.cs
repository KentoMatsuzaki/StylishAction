using Definitions.Data;

namespace Enemy.Interface
{
    /// <summary>敵の攻撃制御に関するインターフェース</summary>
    public interface IEnemyAttackHandler
    {
        /// <summary>現在の攻撃パラメーター</summary>
        EnemyAttackStats CurrentAttackStats { get; }

        /// <summary>プレイヤーを攻撃できるか</summary>
        bool CanAttackPlayer();

        /// <summary>プレイヤーが最小有効射程よりも近い位置にいるか</summary>
        bool IsPlayerTooClose();

        /// <summary>プレイヤーが最大有効射程よりも遠い位置にいるか</summary>
        bool IsPlayerTooFar();

        /// <summary>プレイヤーが有効角度内にいるか</summary>
        bool IsPlayerInAngle();

        /// <summary>現在の攻撃パラメーターを設定する</summary>
        void SetAttackStats();
    }
}