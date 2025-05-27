using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Particle
{
    /// <summary>ライフタイム経過後に破棄するパーティクルを制御するクラス</summary>
    public class LastingParticle : ParticleBase
    {
        /// <summary>パーティクルのライフタイム</summary>
        [SerializeField] private float lifeTime;

        /// <summary>パーティクルを有効化する</summary>
        public override void Activate()
        {
            CustomActivation();
        }

        /// <summary>ライフタイム経過後にオブジェクトを破棄する</summary>
        private async void CustomActivation()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(lifeTime));
            Destroy(gameObject);
        }
    }
}