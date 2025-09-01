using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryView : UiView
{
    [Header("Inventory Elements")] [SerializeField]
    private SoulInformation SoulItemPlaceHolder;

    [SerializeField] private Text Description;
    [SerializeField] private Text Name;
    [SerializeField] private Image Avatar;
    [SerializeField] private Button UseButton;
    [SerializeField] private Button DestroyButton;
    [SerializeField] private Selectable SelectOnDisable;

    private RectTransform _contentParent;
    private GameObject _currentSelectedGameObject;
    private SoulInformation _currentSoulInformation;

    private List<SoulInformation> _soulItems = new List<SoulInformation>();

    public override void Awake()
    {
        base.Awake();
        _contentParent = (RectTransform)SoulItemPlaceHolder.transform.parent;
        InitializeInventoryItems();
    }

    private void InitializeInventoryItems()
    {
        for (int i = 0, j = SoulController.Instance.Souls.Count; i < j; i++)
        {
            SoulInformation newSoul = Instantiate(SoulItemPlaceHolder.gameObject, _contentParent).GetComponent<SoulInformation>();
            newSoul.SetSoulItem(SoulController.Instance.Souls[i], () => SoulItem_OnClick(newSoul));
            _soulItems.Add(newSoul);
        }

        SoulItemPlaceHolder.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        SelectFirstSoul();
    }

    private void SelectFirstSoul()
    {
        if(_soulItems.Count == 0)
        {
            ClearSoulInformation();
            return;
        }
        FixSoulItemsList();
        _soulItems[0].SelectMe();
        OnChildSelectableSelected(_soulItems[0].GetComponent<Selectable>());
        _currentSoulInformation = _soulItems[0];
        _currentSelectedGameObject = _soulItems[0].gameObject;
        SetupSoulInformation(_soulItems[0].soulItem);
    }

    private void FixSoulItemsList()
    {
        for(int i = _soulItems.Count - 1; i >= 0; i--)
        {
            if(_soulItems[i] == null)
            {
                _soulItems.RemoveAt(i);
            }
        }
        _soulItems.Sort((x, y) => x.transform.GetSiblingIndex() - y.transform.GetSiblingIndex());
    }

    private void ClearSoulInformation()
    {
        Description.text = "";
        Name.text = "";
        Avatar.sprite = null;
        SetupUseButton(false);
        SetupDestroyButton(false);
        _currentSelectedGameObject = null;
        _currentSoulInformation = null;
    }

    public void SoulItem_OnClick(SoulInformation soulInformation)
    {
        _currentSoulInformation = soulInformation;
        _currentSelectedGameObject = soulInformation.gameObject;
        SetupSoulInformation(soulInformation.soulItem);
    }

    private void SetupSoulInformation(SoulItem soulItem)
    {
        Description.text = soulItem.Description;
        Name.text = soulItem.Name;
        Avatar.sprite = soulItem.Avatar;
        SetupUseButton(soulItem.CanBeUsed);
        SetupDestroyButton(soulItem.CanBeDestroyed);
    }

    public override void OnChildSelectableSelected(Selectable selectable)
    {
        SelectOnEnable = selectable;
    }

    public override void DisableView()
    {
        base.DisableView();
        if(_contentParent.childCount > 0)
        {
            SelectOnEnable = _contentParent.GetChild(0).GetComponent<Selectable>();
        }
        if (SelectOnDisable != null) SelectOnDisable.Select();
    }

    private void SelectElement(int index)
    {

    }

    private void CantUseCurrentSoul()
    {
        PopUpInformation popUpInfo = new PopUpInformation { DisableOnConfirm = true, UseOneButton = true, Header = "CAN'T USE", Message = "THIS SOUL CANNOT BE USED IN THIS LOCALIZATION" };
        GUIController.Instance.ShowPopUpMessage(popUpInfo);
    }

    private void UseCurrentSoul(bool canUse)
    {
        if (!canUse)
        {
            CantUseCurrentSoul();
        }
        else
        {
            //USE SOUL
            GameControlller.Instance.score += _currentSoulInformation.soulItem.ScoreOnUse;
            Destroy(_currentSelectedGameObject);
            SelectFirstSoul();
        }
    }

    private void DestroyCurrentSoul()
    {
        Destroy(_currentSelectedGameObject);
        SelectFirstSoul();
    }

    private void SetupUseButton(bool active)
    {
        UseButton.onClick.RemoveAllListeners();
        if (active)
        {
            bool isInCorrectLocalization = GameControlller.Instance.IsCurrentLocalization(_currentSoulInformation.soulItem.UsableInLocalization);
            UseButton.interactable = isInCorrectLocalization;
            PopUpInformation popUpInfo = new PopUpInformation
            {
                DisableOnConfirm = isInCorrectLocalization,
                UseOneButton = false,
                Header = "USE ITEM",
                Message = "Are you sure you want to USE: " + _currentSoulInformation.soulItem.Name + " ?",
                Confirm_OnClick = () => UseCurrentSoul(isInCorrectLocalization)
            };
            UseButton.onClick.AddListener(() => GUIController.Instance.ShowPopUpMessage(popUpInfo));
        }
        UseButton.gameObject.SetActive(active);
    }

    private void SetupDestroyButton(bool active)
    {
        DestroyButton.onClick.RemoveAllListeners();
        if (active)
        {
            PopUpInformation popUpInfo = new PopUpInformation
            {
                DisableOnConfirm = true,
                UseOneButton = false,
                Header = "DESTROY ITEM",
                Message = "Are you sure you want to DESTROY: " + Name.text + " ?",
                Confirm_OnClick = () => DestroyCurrentSoul()
            };
            DestroyButton.onClick.AddListener(() => GUIController.Instance.ShowPopUpMessage(popUpInfo));
        }

        DestroyButton.gameObject.SetActive(active);
    }
}