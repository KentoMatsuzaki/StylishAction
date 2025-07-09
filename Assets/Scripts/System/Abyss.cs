using Definitions.Const;
using Managers;
using UnityEngine;

public class Abyss : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(InGameConsts.PlayerGameObjectTag))
        {
            UIManager.Instance.ShowGameOverUI();
        }
    }
}
