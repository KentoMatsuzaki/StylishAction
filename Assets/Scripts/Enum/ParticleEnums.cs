namespace Enum
{
    /// <summary>パーティクルに関する列挙型を保持するクラス</summary>
    public static class ParticleEnums
    {
        /// <summary>パーティクルが紐づいている攻撃の種類</summary>
        public enum ParticleAttackType
        {
            // プレイヤーの攻撃（1~100）
            None = 0,
            AttackNormal1 = 1,
            AttackNormal2 = 2,
            AttackNormal3 = 3,
            AttackNormal4 = 4,
            AttackNormal5 = 5,
            AttackSpecial1 = 6,
            AttackSpecial2 = 7,
            AttackSpecial3 = 8,
            AttackExtra = 9,
            
            // 敵の攻撃（101~200）
            Scythe = 101,
            Meteor = 102,
            Seraph = 103,
        }
    }
}