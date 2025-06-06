using System;
using Enemy.AI;
using Enum;

namespace Particle
{
    /// <summary>永続的に再生され続けるパーティクルを制御するクラス</summary>
    public class PermanentParticle : ParticleBase
    {
        public override void Activate()
        {
            // ゲームオブジェクトを有効化する
            gameObject.SetActive(true);
            // 全てのパーティクルを再生する
            PlayAllParticles();
        }

        private void Update()
        {
            switch (type)
            {
                case ParticleEnums.ParticleType.Vortex1:
                    var vortexTarget = GameManager.Instance.Player.transform.position;
                    vortexTarget.y = 0;
                    transform.position = vortexTarget; break;
            }
        }
    }
}