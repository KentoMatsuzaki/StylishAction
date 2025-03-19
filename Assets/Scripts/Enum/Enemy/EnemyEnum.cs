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

        public enum EnemySkillType
        {
            Scythe, // スキル1
            
        }
    }
}