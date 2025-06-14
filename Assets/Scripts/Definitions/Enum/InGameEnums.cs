namespace Definitions.Enum
{
    /// <summary>
    /// ゲームプレイ中の処理や演出に関する列挙型を定義するクラス
    /// </summary>
    public static class InGameEnums
    {
        //-------------------------------------------------------------------------------
        // プレイヤーに関する項目
        //-------------------------------------------------------------------------------
        
        /// <summary>プレイヤーの状態の種類</summary>
        public enum PlayerStateType
        {
            Idle,    // 待機状態
            Move,    // 移動状態
            Dash,    // ダッシュ状態
            Roll,    // 回避状態
            Parry,   // パリィ状態
            Guard,   // 防御状態
            AttackN, // 通常攻撃
            AttackS, // 特殊攻撃
            AttackE, // EX攻撃
            Damage,  // 被弾状態
            Dead     // 死亡状態
        }
    }
}