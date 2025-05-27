namespace Particle
{
    /// <summary>再生完了後に非アクティブにするパーティクルを制御するクラス</summary>
    public class OneShotParticle : ParticleBase
    {
        /// <summary>パーティクルを有効化する</summary>
        public override void Activate()
        {
            // パーティクルを再生している場合
            if (gameObject.activeSelf)
            {
                // 全てのパーティクルを再再生する
                ReplayAllParticles();
            }
            // パーティクルを再生していない場合
            else
            {
                // 固有の有効化処理を呼ぶ
                CustomActivation();
            }
            // 全てのパーティクルの再生完了後にオブジェクトを無効化する
            SetInactiveAfterAllParticlesStop();
        }

        /// <summary>パーティクルを有効化する固有処理</summary>
        private void CustomActivation()
        {
            gameObject.SetActive(true);
            PlayAllParticles();
        }
    }
}