namespace Enum.Enemy
{
    /// <summary>敵に関する列挙型をまとめたクラス</summary>
    public static class EnemyEnum 
    {
        /// <summary>ノードの評価結果</summary>
        public enum NodeStatus
        {
            Success, // 成功
            Failure, // 失敗
            Running  // 実行中
        }

        /// <summary>敵の攻撃の種類</summary>
        public enum EnemyAttackType
        {
            Scythe, // 鎌
            Meteor, // 隕石
            Feather, // 羽
        }
    }
}