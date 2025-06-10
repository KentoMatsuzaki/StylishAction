using System.Collections.Generic;
using Enum;
using UnityEngine;

namespace Sound
{
    /// <summary>サウンドを管理するクラス</summary>
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private AudioClip bgm;
        [SerializeField] private AudioClip clear;
        [SerializeField] private AudioClip gameOver;
        [SerializeField] private AudioClip button;
        [SerializeField] private AudioClip attackNormal;
        [SerializeField] private AudioClip attackSpecial;
        [SerializeField] private AudioClip attackExtra;
        [SerializeField] private AudioClip parry;
        [SerializeField] private AudioClip guard;
        [SerializeField] private AudioClip dodge;
        [SerializeField] private AudioClip scythe;
        [SerializeField] private AudioClip meteor;
        [SerializeField] private AudioClip seraph;
        [SerializeField] private AudioClip explosion;
        [SerializeField] private AudioClip vortex;
        [SerializeField] private AudioClip waterfall;
        [SerializeField] private AudioClip photon;
        
        public static SoundManager Instance;

        private AudioSource _source;
        private Dictionary<OutGameEnums.SoundType, AudioClip> _clipMap;
        
        //-------------------------------------------------------------------------------
        // 初期設定
        //-------------------------------------------------------------------------------

        private void Awake()
        {
            if (Instance == null) Instance = this; else Destroy(gameObject);
            
            _source = GetComponent<AudioSource>();
            
            _clipMap = new Dictionary<OutGameEnums.SoundType, AudioClip>
            {
                { OutGameEnums.SoundType.Bgm, bgm },
                { OutGameEnums.SoundType.Clear, clear },
                { OutGameEnums.SoundType.GameOver, gameOver },
                { OutGameEnums.SoundType.Button, button },
                { OutGameEnums.SoundType.AttackNormal, attackNormal },
                { OutGameEnums.SoundType.AttackSpecial, attackSpecial },
                { OutGameEnums.SoundType.AttackExtra, attackExtra },
                { OutGameEnums.SoundType.Parry, parry },
                { OutGameEnums.SoundType.Guard, guard },
                { OutGameEnums.SoundType.Dodge, dodge },
                { OutGameEnums.SoundType.Scythe, scythe },
                { OutGameEnums.SoundType.Meteor, meteor },
                { OutGameEnums.SoundType.Seraph, seraph },
                { OutGameEnums.SoundType.Explosion, explosion },
                { OutGameEnums.SoundType.Vortex, vortex },
                { OutGameEnums.SoundType.Waterfall, waterfall },
                { OutGameEnums.SoundType.Photon, photon }
            };
        }
        
        //-------------------------------------------------------------------------------
        // 音に関する処理
        //-------------------------------------------------------------------------------
        
        /// <summary>指定されたサウンドをループ再生する
        public void PlayLoopSound(OutGameEnums.SoundType type)
        {
            if (_clipMap.TryGetValue(type, out AudioClip clip) && clip != null)
            {
                _source.clip = clip;
                _source.loop = true;
                _source.Play();
            }
            else
            {
                Debug.LogWarning($"Audio clip for {type} not found");
            }
        }

        /// <summary>指定されたサウンドを一度だけ再生</summary>
        public void PlayOneShot(OutGameEnums.SoundType type)
        {
            if (_clipMap.TryGetValue(type, out AudioClip clip) && clip != null)
            {
                _source.PlayOneShot(clip);
            }
            else
            {
                Debug.LogWarning($"Audio clip for {type} not found");
            }
        }

        /// <summary>全ての音の再生を停止する</summary>
        public void StopAllSounds()
        {
            _source.Stop();
        }
    }
}