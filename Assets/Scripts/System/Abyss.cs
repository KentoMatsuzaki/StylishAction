using Const;
using UI;
using UnityEngine;

public class Abyss : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PlayerConst.GameObjectTag))
        {
            // ゲームオーバーUIを表示する
            UIManager.Instance.ShowGameOverUI();
            // メインUIを非表示にする
            UIManager.Instance.HideMainUI();
        }
    }
}
