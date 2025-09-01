using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable))]
public class SelectOnEnable : MonoBehaviour
{
    [SerializeField] private Selectable selectOnEnable;

    private void OnEnable()
    {
        if (selectOnEnable != null)
        {
            selectOnEnable.Select();
        }
    }
}
