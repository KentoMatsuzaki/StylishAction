namespace Enum
{
    /// <summary>インゲームの列挙型をまとめたクラス</summary>
    public static class InGameEnum
    {
        //-------------------------------------------------------------------------------
        // プレイヤーに関する列挙型
        //-------------------------------------------------------------------------------

        /// <summary>プレイヤーの状態</summary>
        public enum PlayerState
        {
            Idle,   // 静止
            Move,   // 移動
            Attack, // 攻撃
            Parry,  // パリィ
            PostParry, // パリィ後の硬直
            Damage, // 被ダメージ
            Dead    // 死亡
        }

        /// <summary>プレイヤーの攻撃の種類</summary>
        public enum PlayerAttackType
        {
            Iai,
            Hien,
            Shiden,
        }
        
        //-------------------------------------------------------------------------------
        // 敵に関する列挙型
        //-------------------------------------------------------------------------------

        /// <summary>敵ノードの評価結果</summary>
        public enum EnemyNodeStatus
        {
            Success, 
            Failure, 
            Running  
        }

        /// <summary>敵の攻撃の種類</summary>
        public enum EnemyAttackType
        {
            Scythe,
            Meteor,
            Seraphic,
        }
        
        //-------------------------------------------------------------------------------
        // パーティクルに関する列挙型
        //-------------------------------------------------------------------------------

        /// <summary>パーティクルのライフタイムの種類</summary>
        public enum ParticleLifeTimeType
        {
            OneShot, // 場に残らない
            Lasting  // 場に残る
        }
        
        /// <summary>プレイヤーのパーティクルの種類</summary>
        public enum PlayerParticleType
        {
            Iai,
            Hien,
            Shiden,
            Parry
        }
        
        /// <summary>敵のパーティクルの種類</summary>
        public enum EnemyParticleType
        {
            Scythe,
            Meteor,
            Seraphic,
        }
    }
}