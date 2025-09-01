using UnityEngine;
using UnityEngine.UI;

public class PauseView : UiView
{
    [SerializeField] private Selectable SelectOnClose;

    public override void DisableView()
    {
        base.DisableView();
        if (SelectOnClose) SelectOnClose.Select();
    }
}
