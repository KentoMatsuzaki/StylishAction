using Enum.Player;

namespace Effect
{
    /// <summary>プレイヤーの攻撃パーティクルを制御するクラス</summary>
    public class PlayerAttackParticleController : ParticleControllerBase
    {
        public PlayerEnum.PlayerAttackType type;
    }
}