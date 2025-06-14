namespace Enum
{
    /// <summary>パーティクルに関する列挙型を保持するクラス</summary>
    public static class ParticleEnums
    {
        /// <summary>パーティクルの種類</summary>
        public enum ParticleType
        {
            // プレイヤーの攻撃（1~30）
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
            
            // 敵の攻撃（31~100）
            Scythe = 31,
            Meteor = 32,
            Seraph1 = 33,
            Seraph2 = 34,
            Eclipse = 35,
            Explosion = 36,
            WaterFall1 = 37,
            WaterFall2 = 38,
            Vortex1 = 39,
            Vortex2 = 40,
            Photon = 41,
            
            // 汎用（101~130）
            Parry = 101,
            Guard = 102,
            Stun = 103,
            Hit = 104,
        }
    }
}