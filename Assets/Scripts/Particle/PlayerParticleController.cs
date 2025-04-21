using Enum;

namespace Particle
{
    /// <summary>プレイヤーのパーティクルを制御するクラス</summary>
    public class PlayerParticleController : ParticleControllerBase
    {
        /// <summary>プレイヤーのパーティクルの種類</summary>
        public InGameEnum.PlayerParticleType playerParticleType;
    }
}