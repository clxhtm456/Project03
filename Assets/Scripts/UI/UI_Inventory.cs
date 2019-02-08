using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : UIBase {
    private int Selected_Icon = -1;
    public Text itemName;
    public Text itemRarety;
    public Text itemOption;
    public Text itemContext;

    public RectTransform ContextPanel;
    public RectTransform Cursor;
    public Button[] recentInvenPanel;
    private Button[] inventoryPanel;
    private GameObject[] tempItem = new GameObject[2];
    protected void ChangeSelected(int _temp)
    {
        
        if (_temp == -1)
        {
            ContextPanel.gameObject.SetActive(false);
            Cursor.gameObject.SetActive(false);
        }
        else
        {
            ContextPanel.gameObject.SetActive(true);
            Cursor.gameObject.SetActive(true);
            if (_temp >= 100)//장착중인 아이템 선택
            {
                Cursor.transform.position = recentInvenPanel[_temp - 100].transform.position;
            }
            else if (_temp >= 0)//인벤토리선택
                Cursor.transform.position = inventoryPanel[_temp].transform.position;
        }
        Selected_Icon = _temp;
        ResetPanel();
    }
    protected override void Awake() 
    {
        base.Awake();
        UI_PlayerState.characterChange += ResetPanel;
        UI_PlayerState.closePlayerState += CloseUI;
        inventoryPanel = transform.GetComponentInChildren<ContentSizeFitter>().gameObject.GetComponentsInChildren<Button>();
        ItemReorder();

        for(int i = 0; i < inventoryPanel.Length;i++)
        {
            int temp = i;
            inventoryPanel[i].onClick.AddListener(()=> { ChangeSelected(temp); });
        }
        for (int i = 0; i < recentInvenPanel.Length; i++)
        {
            int temp = i;
            recentInvenPanel[i].onClick.AddListener(() => { ChangeSelected(temp + 100); });
        }

    }
    protected void PlayerItemReset()
    {
        for (int ownNumber = 0; ownNumber < 2; ownNumber++)
        {
            if (tempItem[ownNumber] != null)//기존 아이템 정리
            {
                EffectManager.instance.DeleteEffect(tempItem[ownNumber].gameObject);
                tempItem[ownNumber] = null;
            }
            if (SaveListManager.instance.playerdata.playerList[UI_PlayerState.recentCharacter].itemEntryList.Length <= ownNumber)
                continue;
            int item = SaveListManager.instance.playerdata.playerList[UI_PlayerState.recentCharacter].itemEntryList[ownNumber];

            if (item == 0)
                continue;
            tempItem[ownNumber] = EffectManager.instance.MakeUIEffect("ItemIcon");
            tempItem[ownNumber].GetComponent<Image>().sprite = ItemManager.instance.ItemResearch(item).itemIconSprite();

            if (tempItem[ownNumber] != null)
            {
                tempItem[ownNumber].transform.SetParent(recentInvenPanel[ownNumber].transform);
                tempItem[ownNumber].transform.localPosition = Vector3.zero;
                tempItem[ownNumber].transform.localScale = Vector3.one;
            }
        }
    }
    public void ItemReorder()//스펠 재정렬 스펠 사용시 또는 새로운 스펠 추가시 작동
    {
        int invenCount = SaveListManager.instance.playerdata.itemList.Length;
        for (int i = 0; i < invenCount; i++)
        {
            if (SaveListManager.instance.playerdata.itemList[i] != 0)
            {
                for (int j = 0; j < i; j++)
                {
                    if (SaveListManager.instance.playerdata.itemList[j] == 0)
                    {
                        SaveListManager.instance.playerdata.itemList[j] = SaveListManager.instance.playerdata.itemList[i];
                        SaveListManager.instance.playerdata.itemList[i] = 0;
                    }
                }
            }
        }
    }
    protected void InventoryItemReset()
    {
        int itemCount = SaveListManager.instance.playerdata.itemList.Length;
        for (int saveSlot = 0, invenSlot = 0; saveSlot < itemCount && invenSlot < inventoryPanel.Length; saveSlot++)
        {
            int item = SaveListManager.instance.playerdata.itemList[saveSlot];
            if (item == 0)
            {
                if (inventoryPanel[invenSlot].transform.childCount > 0)
                {
                    EffectManager.instance.DeleteEffect(inventoryPanel[invenSlot].transform.GetChild(0).gameObject);
                    invenSlot++;
                }
                continue;
            }
            //Debug.Log(inventoryPanel[invenSlot].transform.childCount);
            if (inventoryPanel[invenSlot].transform.childCount == 0)
            {
                GameObject itemIcon = EffectManager.instance.MakeUIEffect("ItemIcon");
                itemIcon.GetComponent<Image>().sprite = ItemManager.instance.ItemResearch(item).itemIconSprite();

                if (itemIcon != null)
                {
                    itemIcon.transform.SetParent(inventoryPanel[invenSlot].transform);
                    itemIcon.transform.localPosition = Vector3.zero;
                    itemIcon.transform.localScale = Vector3.one;
                }
            }
            invenSlot++;
        }
        ItemReorder();
    }
    public int AddItemtoInventory(Item _item)
    {
        ItemReorder();
        int invenCount = 0;
        int maxCount = SaveListManager.instance.playerdata.itemList.Length;
        while (SaveListManager.instance.playerdata.itemList[invenCount] != 0 && invenCount < maxCount)
            invenCount++;

        if (invenCount == maxCount)//인벤토리가 가득참
            return -1;
        SaveListManager.instance.playerdata.itemList[invenCount] = _item.itemEntry;
        return invenCount;
    }
    public int AddItemtoInventory(int _itemEntry)
    {
        ItemReorder();
        int invenCount = 0;
        int maxCount = SaveListManager.instance.playerdata.itemList.Length;
        while (SaveListManager.instance.playerdata.itemList[invenCount] != 0 && invenCount < maxCount)
            invenCount++;
        if (invenCount == maxCount)//인벤토리가 가득참
            return -1;
        SaveListManager.instance.playerdata.itemList[invenCount] = _itemEntry;
        return invenCount;
    }
    public void DelItemtoInventory(int _itemEntry)
    {
        ItemReorder();
        int invenCount = 0;
        int maxCount = SaveListManager.instance.playerdata.itemList.Length;
        while (SaveListManager.instance.playerdata.itemList[invenCount] != _itemEntry && invenCount < maxCount)
            invenCount++;

        if (invenCount == maxCount)//삭제할 아이템이없음
            return;
        SaveListManager.instance.playerdata.itemList[invenCount] = 0;
        ItemReorder();
    }
    public void changeEquipState()
    {
        if (Selected_Icon >= 100)//장착중인 아이템 선택
        {
            int itemEntry = SaveListManager.instance.playerdata.playerList[UI_PlayerState.recentCharacter].itemEntryList[Selected_Icon - 100];

            if (itemEntry == 0)//불가능하겠지만 선택한 아이템이 null인 경우 종료
            {
                return;
            }
            SaveListManager.instance.playerdata.playerList[UI_PlayerState.recentCharacter].itemEntryList[Selected_Icon - 100] = 0;
            ChangeSelected( AddItemtoInventory(itemEntry));
        }
        else if (Selected_Icon >= 0)//인벤토리선택
        {
            int itemEntry = SaveListManager.instance.playerdata.itemList[Selected_Icon];
            if (itemEntry == 0)//불가능하겠지만 선택한 아이템이 null인 경우 종료
            {
                return;
            }
            int itemInven1 = SaveListManager.instance.playerdata.playerList[UI_PlayerState.recentCharacter].itemEntryList[0];
            int itemInven2 = SaveListManager.instance.playerdata.playerList[UI_PlayerState.recentCharacter].itemEntryList[1];
            if (itemInven1 != 0 
                && itemInven2 == 0)
            {
                SaveListManager.instance.playerdata.playerList[UI_PlayerState.recentCharacter].itemEntryList[1] = itemEntry;
                DelItemtoInventory(itemEntry);
                ChangeSelected(101);
            }
            else
            {
                if(itemInven1 != 0)//1번슬롯에 아이템이 이미 있는 경우 아이템 해제
                {
                    SaveListManager.instance.playerdata.playerList[UI_PlayerState.recentCharacter].itemEntryList[0] = 0;
                    AddItemtoInventory(itemInven1);
                }
                DelItemtoInventory(itemEntry);
                SaveListManager.instance.playerdata.playerList[UI_PlayerState.recentCharacter].itemEntryList[0] = itemEntry;
                ChangeSelected(100);
            }
        }
        ResetPanel();
    }
    public override void ResetPanel()
    {
        if (gameObject.activeInHierarchy == false)
            return;
        PlayerItemReset();
        InventoryItemReset();
        if (Selected_Icon >= 100)//장착중인 아이템 선택
        {
            if (SaveListManager.instance.playerdata.playerList[UI_PlayerState.recentCharacter].itemEntryList.Length <= Selected_Icon-100)
            {
                ChangeSelected(-1);
                return;
            }
            int item = SaveListManager.instance.playerdata.playerList[UI_PlayerState.recentCharacter].itemEntryList[Selected_Icon - 100];

            if (item == 0)
            {
                ChangeSelected(-1);
                return;
            }
            Item tempItem = ItemManager.instance.ItemResearch(item);

            itemName.text = tempItem.itemName;
            //itemRarety.text = tempItem.itemRarety;
            itemOption.text = tempItem.itemOption;
            itemContext.text = tempItem.itemContext;
            
        }
        else if(Selected_Icon >= 0)//인벤토리선택
        {
            if (SaveListManager.instance.playerdata.itemList.Length <= Selected_Icon)
            {
                ChangeSelected(-1);
                return;
            }
            int item = SaveListManager.instance.playerdata.itemList[Selected_Icon];

            if (item == 0)
            {
                ChangeSelected(-1);
                return;
            }
            Item tempItem = ItemManager.instance.ItemResearch(item);

            itemName.text = tempItem.itemName;
            //itemRarety.text = tempItem.itemRarety;
            itemOption.text = tempItem.itemOption;
            itemContext.text = tempItem.itemContext;
        }
        
    }
}
