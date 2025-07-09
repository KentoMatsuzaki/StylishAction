using System;
using Cysharp.Threading.Tasks;
using Definitions.Const;
using DG.Tweening;
using TMPro;
using UI;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    /// <summary>ゲーム全体のUIを管理するクラス</summary>
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Slider enemyHp;
        [SerializeField] private Slider playerHp;
        [SerializeField] private Slider playerSp;
        [SerializeField] private Image playerEpGauge;
        [SerializeField] private Image playerEpImage;

        [SerializeField] private Image dashIcon;
        [SerializeField] private Image rollIcon;
        [SerializeField] private Image atkNIcon;
        [SerializeField] private Image atkSIcon;
        [SerializeField] private Image atkEIcon;
        [SerializeField] private Image parryIcon;
        [SerializeField] private Image guardIcon;

        [SerializeField] private Canvas titleCanvas;
        [SerializeField] private Canvas dynamicCanvas;
        [SerializeField] private Canvas staticCanvas;
        
        [SerializeField] private Image clearPanel;
        [SerializeField] private Image gameOverPanel;
        [SerializeField] private TextMeshProUGUI clearText;
        [SerializeField] private Text gameOverText;

        [SerializeField] private DamageTextController damageTextPrefab;
        
        private readonly Color _colorActive = Color.white;
        private readonly Color _colorInactive = Color.black;
        private readonly Color _colorReady = Color.red;
        
        private Tween _gaugePulseTween;
        private Tween _iconPulseTween;
        
        public static UIManager Instance;

        //-------------------------------------------------------------------------------
        // 初期化に関する処理
        //-------------------------------------------------------------------------------

        private void Awake()
        {
            if (Instance == null) Instance = this; else Destroy(gameObject);
        }

        public void Initialize()
        {
            BindEnemyStats();
            BindPlayerStats();
        }

        private void BindEnemyStats()
        {
            GameManager.Instance.Enemy.EnemyHp.Subscribe(currentHp =>
            {
                enemyHp.value = currentHp / GameManager.Instance.Enemy.MaxHp;

                if (currentHp <= 0)
                {
                    ShowClearUI();
                }
                
            }).AddTo(gameObject);
        }

        private void BindPlayerStats()
        {
            GameManager.Instance.Player.PlayerHp.Subscribe(currentHp =>
            {
                playerHp.value = currentHp / GameManager.Instance.Player.MaxHp;

                if (currentHp <= 0)
                {
                    ShowGameOverUI();
                }
                
            }).AddTo(gameObject);
            
            GameManager.Instance.Player.PlayerSp.Subscribe(currentSp =>
            {
                playerSp.value = currentSp / GameManager.Instance.Player.MaxSp;
                
                dashIcon.color = currentSp >= InGameConsts.PlayerDashSpCost ? _colorActive : _colorInactive;
                rollIcon.color = currentSp >= InGameConsts.PlayerRollSpCost ? _colorActive : _colorInactive;
                atkSIcon.color = currentSp >= InGameConsts.PlayerAtkSSpCost ? _colorActive : _colorInactive;
                parryIcon.color = currentSp >= InGameConsts.PlayerParrySpCost ? _colorActive : _colorInactive;
                guardIcon.color = currentSp >= InGameConsts.PlayerGuardSpCost ? _colorActive : _colorInactive;
                
            }).AddTo(gameObject);
            
            GameManager.Instance.Player.PlayerEp.Subscribe(currentEp =>
            {
                var isAtkEReady = currentEp >= GameManager.Instance.Player.MaxEp;
                
                playerEpGauge.fillAmount = currentEp / GameManager.Instance.Player.MaxEp;
                playerEpImage.color = isAtkEReady ? _colorReady :_colorActive;
                atkEIcon.color = isAtkEReady ? _colorActive : _colorInactive;

                if (isAtkEReady && _gaugePulseTween == null)
                {
                    _gaugePulseTween = playerEpImage.transform.DOScale(1.5f, 0.5f).SetLoops(-1, LoopType.Yoyo)
                        .SetEase(Ease.InOutSine);
                }
                else if (!isAtkEReady && _gaugePulseTween != null)
                {
                    _gaugePulseTween.Kill();
                    _gaugePulseTween = null;
                    playerEpImage.transform.localScale = Vector3.one;
                }
                
                if (isAtkEReady && _iconPulseTween == null)
                {
                    _iconPulseTween = atkEIcon.transform.DOScale(1.25f, 0.5f).SetLoops(-1, LoopType.Yoyo)
                        .SetEase(Ease.InOutSine);
                }
                else if (!isAtkEReady && _iconPulseTween != null)
                {
                    _iconPulseTween.Kill();
                    _iconPulseTween = null;
                    atkEIcon.transform.localScale = Vector3.one;
                }

            }).AddTo(gameObject);
        }
        
        //-------------------------------------------------------------------------------
        // UI管理に関する処理
        //-------------------------------------------------------------------------------

        /// <summary>メインシーンのUIを表示する</summary>
        public void ShowMainUI()
        {
            titleCanvas.gameObject.SetActive(false);
            dynamicCanvas.gameObject.SetActive(true);
            staticCanvas.gameObject.SetActive(true);
        }

        /// <summary>ゲームクリア時のUIを表示する</summary>
        private void ShowClearUI()
        {
            dynamicCanvas.gameObject.SetActive(false);
            staticCanvas.gameObject.SetActive(false);
            
            clearPanel.gameObject.SetActive(true);
            clearPanel.DOFade(0.4f, 1.0f);
            
            clearText.gameObject.SetActive(true);
            clearText.DOFade(1.0f, 1.0f);
            
            GameManager.Instance.OnGameClear();
        }

        /// <summary>ゲームオーバー時のUIを表示する</summary>
        public void ShowGameOverUI()
        {
            dynamicCanvas.gameObject.SetActive(false);
            staticCanvas.gameObject.SetActive(false);
            
            gameOverPanel.gameObject.SetActive(true);
            gameOverPanel.DOFade(0.4f, 1.0f);
            
            gameOverText.gameObject.SetActive(true);
            gameOverText.DOFade(1.0f, 1.0f);

            GameManager.Instance.OnGameOver();
        }
        
        //-------------------------------------------------------------------------------
        // 動的UIの処理
        //-------------------------------------------------------------------------------

        /// <summary>
        /// ダメージUIを生成して表示する
        /// </summary>
        /// <param name="worldPos">ダメージUIを生成するワールド座標</param>
        /// <param name="damage">表示するダメージ量</param>
        /// <param name="startColor">ダメージUIの最初の色</param>
        /// <param name="endColor">ダメージUIの最後の色</param>
        public void ShowDamageUI(Vector3 worldPos, float damage, Color startColor, Color endColor)
        {
            var screenPos = GameManager.Instance.MainCamera.WorldToScreenPoint(worldPos);
            var damageText = Instantiate(damageTextPrefab, screenPos, Quaternion.identity, transform);
            damageText.Initialize(damage, startColor, endColor);
        }
    }
}