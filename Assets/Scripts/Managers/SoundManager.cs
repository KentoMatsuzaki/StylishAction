using System.Collections.Generic;
using Definitions.Enum;
using UnityEngine;

namespace Managers
{
    /// <summary>ゲーム全体の音声を管理するクラス</summary>
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private AudioSource bgm;
        [SerializeField] private AudioSource se;

        [SerializeField] private AudioClip bgmSound;
        [SerializeField] private AudioClip buttonSound;
        [SerializeField] private AudioClip clearSound;
        [SerializeField] private AudioClip gameOverSound;
        [SerializeField] private AudioClip atkNSound;
        [SerializeField] private AudioClip atkSSound1;
        [SerializeField] private AudioClip atkSSound2;
        [SerializeField] private AudioClip parrySound;
        [SerializeField] private AudioClip guardHitSound;
        [SerializeField] private AudioClip playerHitSound;
        [SerializeField] private AudioClip enemyHitSound;
        [SerializeField] private AudioClip rollingSound;
        [SerializeField] private AudioClip scythe;
        [SerializeField] private AudioClip meteor;
        [SerializeField] private AudioClip seraph;
        [SerializeField] private AudioClip vortex;
        [SerializeField] private AudioClip photon;
        [SerializeField] private AudioClip explosion;
        [SerializeField] private AudioClip waterfall;
        
        
        public static SoundManager Instance;

        private static Dictionary<OutGameEnums.SoundType, AudioClip> _soundMap;
        
        //-------------------------------------------------------------------------------
        // 初期化に関する処理
        //-------------------------------------------------------------------------------

        private void Awake()
        {
            if (Instance == null) Instance = this; else Destroy(gameObject);

            _soundMap = new()
            {
                { OutGameEnums.SoundType.BGM, bgmSound },
                { OutGameEnums.SoundType.Button, buttonSound },
                { OutGameEnums.SoundType.Clear, clearSound },
                { OutGameEnums.SoundType.GameOver, gameOverSound },
                { OutGameEnums.SoundType.AttackN, atkNSound },
                { OutGameEnums.SoundType.AttackS1, atkSSound1 },
                { OutGameEnums.SoundType.AttackS2, atkSSound2 },
                { OutGameEnums.SoundType.Parry, parrySound },
                { OutGameEnums.SoundType.GuardHit, guardHitSound },
                { OutGameEnums.SoundType.PlayerHit, playerHitSound },
                { OutGameEnums.SoundType.EnemyHit, enemyHitSound },
                { OutGameEnums.SoundType.Rolling, rollingSound },
                { OutGameEnums.SoundType.Scythe, scythe },
                { OutGameEnums.SoundType.Meteor, meteor },
                { OutGameEnums.SoundType.Seraph, seraph },
                { OutGameEnums.SoundType.Vortex, vortex },
                { OutGameEnums.SoundType.Photon, photon },
                { OutGameEnums.SoundType.Explosion, explosion },
                { OutGameEnums.SoundType.Waterfall, waterfall }
            };
        }
        
        //-------------------------------------------------------------------------------
        // サウンド制御に関する処理
        //-------------------------------------------------------------------------------

        public void PlayBGM()
        {
            bgm.Play();
        }

        public void StopBGM()
        {
            bgm.Stop();
        }

        public void PlaySe(OutGameEnums.SoundType soundType)
        {
            se.PlayOneShot(_soundMap[soundType]);
        }
    }
}