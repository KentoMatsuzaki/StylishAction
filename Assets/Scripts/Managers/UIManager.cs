using DG.Tweening;
using TMPro;
using UI;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;
using Definitions.Const;

namespace Managers
{
    /// <summary>ゲーム全体のUIを管理するクラス</summary>
    public class UIManager : MonoBehaviour
    {
        [Header("プレイヤーの回避アクション")]
        
        [SerializeField] private Image rollCoolDownOverlayImage; // クールタイムのオーバーレイ
        [SerializeField] private Image rollCoolDownGaugeImage;   // クールタイムのゲージ
        [SerializeField] private Text rollCoolDownText;          // クールタイムのカウント
        
        [Header("プレイヤーのパリィアクション")]
        
        [SerializeField] private Image parryCoolDownOverlayImage; // クールタイムのオーバーレイ
        [SerializeField] private Image parryCoolDownGaugeImage;   // クールタイムのゲージ
        [SerializeField] private Text parryCoolDownText;          // クールタイムのカウント
        
        [Header("プレイヤーの防御アクション")]
        
        [SerializeField] private Image guardCoolDownOverlayImage; // クールタイムのオーバーレイ
        [SerializeField] private Image guardCoolDownGaugeImage;   // クールタイムのゲージ
        [SerializeField] private Text guardCoolDownText;          // クールタイムのカウント
        
        [Header("プレイヤーの特殊攻撃アクション")]
        [SerializeField] private Image atkSCoolDownOverlayImage; // クールタイムのオーバーレイ
        [SerializeField] private Image atkSCoolDownGaugeImage;   // クールタイムのゲージ
        [SerializeField] private Text atkSCoolDownText;          // クールタイムのカウント
        
        [SerializeField] private Slider enemyHp;
        [SerializeField] private Slider playerHp;
        [SerializeField] private Image playerEpGauge;
        [SerializeField] private Image playerEpImage;
        
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
        // 初期化処理
        //-------------------------------------------------------------------------------

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        public void Initialize()
        {
            BindEnemyStats();
            BindPlayerStats();
            BindPlayerRollCoolDown();
            BindPlayerParryCoolDown();
            BindPlayerGuardCoolDown();
            BindPlayerAtkSCoolDown();
        }
        
        //-------------------------------------------------------------------------------
        // UIのバインド処理
        //-------------------------------------------------------------------------------

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

        /// <summary>
        /// プレイヤーの回避アクションのクールタイムをUIに反映させる
        /// </summary>
        private void BindPlayerRollCoolDown()
        {
            GameManager.Instance.Player.PlayerRollCoolDown
                .Subscribe(coolDownTime => 
                {
                    var isCoolDown = coolDownTime > 0;

                    // クールタイム中は各UI要素を表示する
                    rollCoolDownOverlayImage.enabled = isCoolDown; // クールタイムのオーバーレイ
                    rollCoolDownGaugeImage.enabled = isCoolDown;   // クールタイムのゲージ
                    rollCoolDownText.enabled = isCoolDown;         // クールタイムのカウント

                    if (isCoolDown)
                    {
                        // クールタイムの秒数を更新
                        rollCoolDownText.text = coolDownTime.ToString("F1", CultureInfo.InvariantCulture);
                        // クールタイムのゲージを更新
                        rollCoolDownGaugeImage.fillAmount = coolDownTime / InGameConsts.PlayerRollCoolDown;
                    }
                })
                .AddTo(this);
        }
        
        /// <summary>
        /// プレイヤーのパリィアクションのクールタイムをUIに反映させる
        /// </summary>
        private void BindPlayerParryCoolDown()
        {
            GameManager.Instance.Player.PlayerParryCoolDown
                .Subscribe(coolDownTime => 
                {
                    var isCoolDown = coolDownTime > 0;

                    // クールタイム中は各UI要素を表示する
                    parryCoolDownOverlayImage.enabled = isCoolDown; // クールタイムのオーバーレイ
                    parryCoolDownGaugeImage.enabled = isCoolDown;   // クールタイムのゲージ
                    parryCoolDownText.enabled = isCoolDown;         // クールタイムのカウント

                    if (isCoolDown)
                    {
                        // クールタイムの秒数を更新
                        parryCoolDownText.text = coolDownTime.ToString("F1", CultureInfo.InvariantCulture);
                        // クールタイムのゲージを更新
                        parryCoolDownGaugeImage.fillAmount = coolDownTime / InGameConsts.PlayerParryCoolDown;
                    }
                })
                .AddTo(this);
        }
        
        /// <summary>
        /// プレイヤーの防御アクションのクールタイムをUIに反映させる
        /// </summary>
        private void BindPlayerGuardCoolDown()
        {
            GameManager.Instance.Player.PlayerGuardCoolDown
                .Subscribe(coolDownTime => 
                {
                    var isCoolDown = coolDownTime > 0;

                    // クールタイム中は各UI要素を表示する
                    guardCoolDownOverlayImage.enabled = isCoolDown; // クールタイムのオーバーレイ
                    guardCoolDownGaugeImage.enabled = isCoolDown;   // クールタイムのゲージ
                    guardCoolDownText.enabled = isCoolDown;         // クールタイムのカウント

                    if (isCoolDown)
                    {
                        // クールタイムの秒数を更新
                        guardCoolDownText.text = coolDownTime.ToString("F1", CultureInfo.InvariantCulture);
                        // クールタイムのゲージを更新
                        guardCoolDownGaugeImage.fillAmount = coolDownTime / InGameConsts.PlayerGuardCoolDown;
                    }
                })
                .AddTo(this);
        }
        
        /// <summary>
        /// プレイヤーの特殊攻撃アクションのクールタイムをUIに反映させる
        /// </summary>
        private void BindPlayerAtkSCoolDown()
        {
            GameManager.Instance.Player.PlayerAtkSCoolDown
                .Subscribe(coolDownTime => 
                {
                    var isCoolDown = coolDownTime > 0;

                    // クールタイム中は各UI要素を表示する
                    atkSCoolDownOverlayImage.enabled = isCoolDown; // クールタイムのオーバーレイ
                    atkSCoolDownGaugeImage.enabled = isCoolDown;   // クールタイムのゲージ
                    atkSCoolDownText.enabled = isCoolDown;         // クールタイムのカウント

                    if (isCoolDown)
                    {
                        // クールタイムの秒数を更新
                        atkSCoolDownText.text = coolDownTime.ToString("F1", CultureInfo.InvariantCulture);
                        // クールタイムのゲージを更新
                        atkSCoolDownGaugeImage.fillAmount = coolDownTime / InGameConsts.PlayerGuardCoolDown;
                    }
                })
                .AddTo(this);
        }
        
        //-------------------------------------------------------------------------------
        // UI全体の処理
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
        // ダメージUIの処理
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