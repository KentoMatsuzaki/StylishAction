namespace Enum
{
    /// <summary>パーティクルに関する列挙型を保持するクラス</summary>
    public static class ParticleEnums
    {
        /// <summary>プレイヤーのパーティクルの種類</summary>
        public enum PlayerParticleType
        {
            AttackSpecialRight,
            AttackSpecialLeft,
            AttackExtra
        }

        /// <summary>敵のパーティクルの種類</summary>
        public enum EnemyParticleType
        {
            Meteor,
            Seraphic,
        }
    }
}