using System;
using Enum;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using Sound;

namespace UI
{
    public class ButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
    {
        [SerializeField] private Sprite hoverImage; 
        [SerializeField] private Sprite normalImage;
        [SerializeField] private Sprite pressedImage;
        [SerializeField] private OutGameEnums.ButtonType type;
        
        private Image _currentImage;
        private Button _button;

        private void Awake()
        {
            _currentImage = GetComponent<Image>();
            _currentImage.sprite = normalImage;
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _currentImage.sprite = hoverImage;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _currentImage.sprite = normalImage;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _currentImage.sprite = normalImage;   
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _currentImage.sprite = pressedImage;
        }

        public void OnClick()
        {
            SoundManager.Instance.PlayOneShot(OutGameEnums.SoundType.Button);
            
            switch (type)
            {
                case OutGameEnums.ButtonType.Start:
                    UIManager.Instance.HideTitleUI(); 
                    GameManager.Instance.StartGameAsync().Forget(); break;
                
                case OutGameEnums.ButtonType.Quit:
                    GameManager.Instance.QuitGame(); break;
                
                case OutGameEnums.ButtonType.Restart:
                    UIManager.Instance.HideOverlayUI();
                    GameManager.Instance.RestartGameAsync().Forget(); break;
            }
        }
    }
}