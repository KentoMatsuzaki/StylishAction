using System.Globalization;
using TMPro;
using UnityEngine;
using DG.Tweening;

namespace UI
{
    /// <summary>
    /// ダメージ表記のテキストを制御するクラス
    /// </summary>
    public class DamageTextController : MonoBehaviour
    {
        [SerializeField] private float maxOffsetX = 0.5f;
        [SerializeField] private float maxOffsetY = 0.5f;
        
        private TextMeshProUGUI _text;

        private void Awake()
        {
            _text = GetComponentInChildren<TextMeshProUGUI>();
        }
        
        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Initialize(float damage, Color startColor, Color endColor)
        {
            _text.text = damage.ToString("F0", CultureInfo.InvariantCulture);
            _text.color = startColor;
            _text.transform.localScale = Vector3.one * 3f;
            
            var offsetX = Random.Range(-maxOffsetX, maxOffsetX);
            var offsetY = Random.Range(-maxOffsetY, maxOffsetY);
            
            _text.rectTransform.anchoredPosition += new Vector2(offsetX, offsetY);
            
            var seq = DOTween.Sequence();
            seq.Append(_text.transform.DOScale(1.0f, 0.25f)) 
                .Join(_text.DOColor(endColor, 0.5f))         
                .SetDelay(0.25f)
                .Append(_text.DOFade(0f, 0.5f))
                .AppendCallback(() => Destroy(gameObject));
        }
    }
}