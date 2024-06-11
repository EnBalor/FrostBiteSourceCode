using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class Craft
{
    public string craftName;
    public Sprite craftImage;
    public string craftDescription;
    public GameObject go_Prefab;
    public GameObject go_PreviewPrefab;
}
public class CraftManual : MonoBehaviour
{
    private bool isActivated = false;
    private bool isPreviewActivated = false;

    private int tabNumber = 0;
    private int page = 1;
    private int selectedSlotNumber;
    private Craft[] craft_SelectedTab;

    [SerializeField]
    private GameObject go_BaseUI;

    [SerializeField]
    private Craft[] craft_fire;
    [SerializeField]
    private Craft[] craft_build;

    private GameObject go_Preview;
    private GameObject go_Prefab;

    [SerializeField]
    private Transform tf_Player;
    [SerializeField]
    private GameObject[] go_Slots;
    [SerializeField]
    private Image[] image_Slot;
    [SerializeField]
    private Text[] text_SlotName;
    [SerializeField]
    private Text[] text_SlotDescription;

    private RaycastHit hitInfo;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private float range;

    void Start()
    {
        tabNumber = 0;
        page = 1;
        TabSlotSetting(craft_fire);
    }

    public void TabSetting(int _tabNumber)
    {
        tabNumber = _tabNumber;
        page = 1;

        switch(tabNumber)
        {
            case 0:
                TabSlotSetting(craft_fire);
                break;
            case 1:
                TabSlotSetting(craft_build);
                break;
        }
    }

    private void ClearSlot()
    {
        for(int i = 0; i < go_Slots.Length; i++)
        {
            image_Slot[i].sprite = null;
            text_SlotName[i].text = "";
            text_SlotDescription[i].text = "";
            go_Slots[i].SetActive(false);
        }
    }    

    private void TabSlotSetting(Craft[] _craft_tab)
    {
        ClearSlot();
        craft_SelectedTab = _craft_tab;
        int startSlotNumber = (page - 1) * go_Slots.Length;
        for(int i = startSlotNumber; i < craft_SelectedTab.Length; i++)
        {
            if(i == page * go_Slots.Length)
            {
                break;
            }
            go_Slots[i - startSlotNumber].SetActive(true);

            image_Slot[i - startSlotNumber].sprite = craft_SelectedTab[i].craftImage;
            text_SlotName[i - startSlotNumber].text = craft_SelectedTab[i].craftName;
            text_SlotDescription[i - startSlotNumber].text = craft_SelectedTab[i].craftDescription;
        }
    }
    public void SlotClick(int _slotNumber)
    {
        selectedSlotNumber = _slotNumber + (page - 1) * go_Slots.Length;
        go_Preview = Instantiate(craft_SelectedTab[selectedSlotNumber].go_PreviewPrefab, tf_Player.position + tf_Player.forward, Quaternion.identity);
        go_Prefab = craft_SelectedTab[selectedSlotNumber].go_Prefab;
        isPreviewActivated = true;
        go_BaseUI.SetActive(false);
        CharacterManager.Instance.Player.controller.ToggleCursor();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B) && !isPreviewActivated)
        {
            ToggleWindow();
        }
        if (isPreviewActivated)
        {
            PreviewPositionUpdate();
        }
        if (Input.GetButtonDown("Fire1"))
        {
            Build();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cancel();
        }
    }

    private void Build()
    {
        if (isPreviewActivated && go_Preview.GetComponent<PreviewObject>().isBuildable())
        {
            Instantiate(go_Prefab, go_Preview.transform.position, go_Preview.transform.rotation);
            Destroy(go_Preview);
            isActivated = false;
            isPreviewActivated = false;
            go_Preview = null;
            go_Prefab = null;
        }
    }

    private void PreviewPositionUpdate()
    {
        if (Physics.Raycast(tf_Player.position, tf_Player.forward, out hitInfo, range, layerMask))
        {
            if (hitInfo.transform != null)
            {
                Vector3 _location = hitInfo.point;

                if (Input.GetKeyDown(KeyCode.Q))
                {
                    go_Preview.transform.Rotate(0f, -90f, 0f);
                }
                else if (Input.GetKeyDown(KeyCode.R))
                {
                    go_Preview.transform.Rotate(0f, 90f, 0f);
                }

                _location.Set(Mathf.Round(_location.x), Mathf.Round(_location.y / 0.1f) * 0.1f, Mathf.Round(_location.z));
                go_Preview.transform.position = _location;
            }
        }

    }

    public void ToggleWindow()
    {
        if (!isActivated)
        {
            OpenWindow();
        }
        else
        {
            CloseWindow();
        }
    }
    private void OpenWindow()
    {
        isActivated = true;
        go_BaseUI.SetActive(true);
        CharacterManager.Instance.Player.controller.ToggleCursor();
    }
    private void CloseWindow()
    {
        isActivated = false;
        go_BaseUI.SetActive(false);
        CharacterManager.Instance.Player.controller.ToggleCursor();

    }

    private void Cancel()
    {
        if (isPreviewActivated)
        {
            Destroy(go_Preview);
        }
        isActivated = false;
        isPreviewActivated = false;
        go_Preview = null;
        go_Prefab = null;
        go_BaseUI.SetActive(false);

        CharacterManager.Instance.Player.controller.ToggleCursor();
    }

    private void ToggleCursor(bool activate)
    {
        Cursor.lockState = activate ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = activate;

    }
}
