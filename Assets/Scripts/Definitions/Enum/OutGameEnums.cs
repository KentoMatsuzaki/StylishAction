namespace Definitions.Enum
{
    /// <summary>
    /// ゲームプレイ外の要素に関する列挙型を定義するクラス
    /// </summary>
    public class OutGameEnums
    {
        //-------------------------------------------------------------------------------
        // サウンドに関する列挙型
        //-------------------------------------------------------------------------------

        public enum SoundType
        {
            BGM,
            Button,
            Clear,
            GameOver,
            AttackN,
            AttackS1,
            AttackS2,
            Parry,
            GuardHit,
            PlayerHit,
            EnemyHit,
            Rolling,
            Scythe,
            Meteor,
            Seraph,
            Vortex,
            Photon,
            Explosion,
            Waterfall
        }
        
        //-------------------------------------------------------------------------------
        // UIに関する列挙型
        //-------------------------------------------------------------------------------

        public enum ButtonType
        {
            Start,
            Quit,
        }
    }
}