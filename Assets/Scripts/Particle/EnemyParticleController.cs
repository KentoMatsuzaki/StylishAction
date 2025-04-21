using System;
using Enum;

namespace Particle
{
    /// <summary>敵のパーティクルを制御するクラス</summary>
    public class EnemyParticleController : ParticleControllerBase
    {
        /// <summary>敵のパーティクルの種類</summary>
        public InGameEnum.EnemyParticleType enemyParticleType;

        /// <summary>パーティクルを有効化する</summary>
        public override void Activate()
        {
            base.Activate();
            HandleCustomBehaviour();
        }

        /// <summary>パーティクルの固有処理</summary>
        private void HandleCustomBehaviour()
        {
            switch (enemyParticleType)
            {
                case InGameEnum.EnemyParticleType.Meteor :
                    transform.position = GameManager.Instance.Player.transform.position; break;
            }
        }
    }
}