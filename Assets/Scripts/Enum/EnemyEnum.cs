using UnityEngine;

namespace Enum
{
    /// <summary>敵に関する列挙型をまとめたクラス</summary>
    public class EnemyEnum : MonoBehaviour
    {
        public enum NodeStatus
        {
            Success, // 成功
            Failure, // 失敗
            Running  // 実行中
        }
    }
}