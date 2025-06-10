using DG.Tweening;
using Enemy.AI;
using Player;
using SO.Player;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>UIを管理するクラス</summary>
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private DeathAI deathAI;
        [SerializeField] private Slider deathHpSlider;
        [SerializeField] private PlayerController player;
        [SerializeField] private Slider playerHpSlider;
        [SerializeField] private Slider playerSpSlider;
        [SerializeField] private PlayerStats playerStats;
        [SerializeField] private Image sprintIcon;
        [SerializeField] private Image dodgeIcon;
        [SerializeField] private Image attackNormalIcon;
        [SerializeField] private Image attackSpecialIcon;
        [SerializeField] private Image attackExtraIcon;
        [SerializeField] private Image parryIcon;
        [SerializeField] private Image guardIcon;
        [SerializeField] private Image playerEpGauge;
        [SerializeField] private Image playerEpGaugeImage;
        [SerializeField] private Canvas titleCanvas;
        [SerializeField] private Canvas dynamicCanvas;
        [SerializeField] private Canvas staticCanvas;
        [SerializeField] private Image clearPanel;
        [SerializeField] private Image gameOverPanel;
        [SerializeField] private TextMeshProUGUI clearText;
        [SerializeField] private Text gameOverText;
        
        private readonly Color _activeColor = Color.white;
        private readonly Color _inactiveColor = Color.black;
        private readonly Color _filledColor = Color.red;
        private Tween _gaugePulseTween;
        private Tween _iconPulseTween;
        
        public static UIManager Instance;
        
        //-------------------------------------------------------------------------------
        // 初期設定
        //-------------------------------------------------------------------------------

        private void Awake()
        {
            if (Instance == null) Instance = this; else Destroy(gameObject);
        }

        private void Start()
        {
            BindDeathHpSlider();
            BindPlayerHpSlider();
            BindPlayerSpSlider();
            BindPlayerEpGauge();
        }
        
        //-------------------------------------------------------------------------------
        // UIに関する処理
        //-------------------------------------------------------------------------------

        /// <summary>DeathエネミーのHPスライダーをHPと紐づけて、HP変化時に自動でUIを更新する</summary>
        private void BindDeathHpSlider()
        {
            deathAI.CurrentHp.Subscribe(currentHp =>
                {
                    deathHpSlider.value = currentHp / deathAI.MaxHp;

                    if (currentHp <= 0)
                    {
                        
                    }
                })
                .AddTo(this);
        }

        /// <summary>プレイヤーのHPスライダーをHPと紐づけて、HP変化時に自動でUIを更新する</summary>
        private void BindPlayerHpSlider()
        {
            player.CurrentHp.Subscribe(currentHp =>
            {
                playerHpSlider.value = currentHp / player.MaxHp;
            })
            .AddTo(this);
        }

        /// <summary>プレイヤーのSPスライダーをSPと紐づけて、SP変化時に自動でUIを更新する</summary>
        private void BindPlayerSpSlider()
        {
            player.CurrentSp.Subscribe(currentSp =>
            {
                playerSpSlider.value = currentSp / player.MaxSp;
                sprintIcon.color = currentSp >= playerStats.sprintSpCost ? _activeColor : _inactiveColor;
                dodgeIcon.color = currentSp >= playerStats.dodgeSpCost ? _activeColor : _inactiveColor;
                attackSpecialIcon.color = currentSp >= playerStats.attackSpecialSpCost ? _activeColor : _inactiveColor;
                parryIcon.color = currentSp >= playerStats.parrySpCost ? _activeColor : _inactiveColor;
                guardIcon.color = currentSp >= playerStats.guardSpCost ? _activeColor : _inactiveColor; 
            })
            .AddTo(this);
        }

        /// <summary>プレイヤーのEPゲージをEPと紐づけて、EP変化時に自動でUIを更新する</summary>
        private void BindPlayerEpGauge()
        {
            player.CurrentEp.Subscribe(currentEp =>
            {
                playerEpGauge.fillAmount = currentEp / playerStats.maxEp;
                playerEpGaugeImage.color = currentEp >= playerStats.maxEp ? _filledColor : _activeColor;
                attackExtraIcon.color = currentEp >= playerStats.maxEp ? _filledColor : _inactiveColor;
                
                var isFilled = currentEp >= playerStats.maxEp;
                
                if (isFilled && _gaugePulseTween == null)
                {
                    _gaugePulseTween = playerEpGaugeImage.transform.DOScale(1.5f, 0.5f).SetLoops(-1, LoopType.Yoyo)
                        .SetEase(Ease.InOutSine);
                }
                else if (!isFilled && _gaugePulseTween != null)
                {
                    _gaugePulseTween.Kill();
                    _gaugePulseTween = null;
                    playerEpGaugeImage.transform.localScale = Vector3.one;
                }

                if (isFilled && _iconPulseTween == null)
                {
                    _iconPulseTween = attackExtraIcon.transform.DOScale(1.25f, 0.5f).SetLoops(-1, LoopType.Yoyo)
                        .SetEase(Ease.InOutSine);
                }
                else if (!isFilled && _iconPulseTween != null)
                {
                    _iconPulseTween.Kill();
                    _iconPulseTween = null;
                    attackExtraIcon.transform.localScale = Vector3.one;
                }
            })
            .AddTo(this);
        }
        
        //-------------------------------------------------------------------------------
        // ゲームの進行に関する処理
        //-------------------------------------------------------------------------------

        /// <summary>タイトルシーンのUiを非表示にする</summary>
        public void HideTitleUI()
        {
            titleCanvas.gameObject.SetActive(false);
        }

        /// <summary>オーバーレイUIを非表示にする</summary>
        public void HideOverlayUI()
        {
            gameOverPanel.gameObject.SetActive(false);
            gameOverText.gameObject.SetActive(false);
            clearPanel.gameObject.SetActive(false);
            clearText.gameObject.SetActive(false);
        }
        
        /// <summary>メインシーンのUIを表示する</summary>
        public void ShowMainUI()
        {
            dynamicCanvas.gameObject.SetActive(true);
            staticCanvas.gameObject.SetActive(true);
        }

        /// <summary>メインシーンのUIを非表示にする</summary>
        public void HideMainUI()
        {
            dynamicCanvas.gameObject.SetActive(false);
            staticCanvas.gameObject.SetActive(false);
        }

        /// <summary>ゲームオーバー時のUIを表示する</summary>
        public void ShowGameOverUI()
        {
            gameOverPanel.color = new Color(255f, 0f, 0f, 0f);
            gameOverPanel.gameObject.SetActive(true);
            gameOverPanel.DOFade(0.4f, 1.0f);
            
            gameOverText.color = new Color(0f, 0f, 0f, 0f);
            gameOverText.gameObject.SetActive(true);
            gameOverText.DOFade(1.0f, 1.0f);
        }

        /// <summary>ゲームクリア時のUIを表示する</summary>
        public void ShowGameClearUI()
        {
            clearPanel.color = new Color(255f, 255f, 255f, 0f);
            clearPanel.gameObject.SetActive(true);
            clearPanel.DOFade(0.4f, 1.0f);
            
            clearText.color = new Color32(190, 230, 200, 0);
            clearText.gameObject.SetActive(true);
            clearText.DOFade(1.0f, 1.0f);
        }
    }
}