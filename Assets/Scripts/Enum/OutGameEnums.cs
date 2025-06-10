namespace Enum
{
    /// <summary>UIに関する列挙型を保持するクラス</summary>
    public static class OutGameEnums
    {
        public enum ButtonType
        {
            None,
            Start,
            Settings,
            Quit,
            Restart,
            Title,
        }
        
        public enum SoundType
        {
            Bgm,
            Clear,
            GameOver,
            Button,
            AttackNormal,
            AttackSpecial,
            AttackExtra,
            Parry,
            Guard,
            Dodge,
            Scythe,
            Meteor,
            Seraph,
            Explosion,
            Vortex,
            Waterfall,
            Photon
        }
    }
}