using System;
using Enum;

namespace Particle
{
    /// <summary>敵のパーティクルを制御するクラス</summary>
    public class EnemyParticleController : ParticleControllerBase
    {
        /// <summary>敵のパーティクルの種類</summary>
        public ParticleEnums.EnemyParticleType particleType;

        /// <summary>パーティクルを有効化する</summary>
        public override void Activate()
        {
            base.Activate();
            ApplyCustomBehaviour();
        }

        /// <summary>パーティクルの固有処理</summary>
        private void ApplyCustomBehaviour()
        {
            switch (particleType)
            {
                case ParticleEnums.EnemyParticleType.Meteor :
                    //transform.position = GameManager.Instance.Player.transform.position;
                    break;
            }
        }
    }
}