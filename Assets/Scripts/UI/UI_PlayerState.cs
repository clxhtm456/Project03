using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerState : UIBase {
    public static int recentCharacter = 0;
    public delegate void CharacterChange();
    public static CharacterChange characterChange;
    public delegate void ClosePlayerState();
    public static ClosePlayerState closePlayerState;
    private List<GameObject> modelList = new List<GameObject>();
    GameObject recentCharacterModel;
    private Button[] mapstate;

    public GameObject mapObject;
    public GameObject characterObject;

    public Text unitClass;
    public Text unitStrength;
    public Text unitInteligence;
    public Text unitSpeed;
    public Text unitDefence;
    public Text unitLevel;
    public Image playerIllu;
    public UIItemState[] unitItem = new UIItemState[2];//케릭터 아이템
    public UIItemState[] unitspell = new UIItemState[5];//케릭터 스킬목록

    private GameObject[] tempItem = new GameObject[2];

    [System.Serializable]
    public struct UIItemState
    {
        public Image unitItemIcon;
        public Text unitItemName;
        public Text unitItemContext;
    }
    IEnumerator SwitchCharacterModel(bool up)
    {
        //GameObject lastModel = recentCharacterModel;
        //recentCharacterModel = 
        yield return null;
    }
    public override void CloseUI()
    {
        if(closePlayerState != null)
            closePlayerState();
        mapObject.SetActive(false);
        base.CloseUI();
    }
    void DrawCharacter(int _temp)
    {
        //기존위치의 케릭터 삭제
        if (SaveListManager.instance.playerdata.playerList[recentCharacter].unitPos != -1)
        {
            Transform[] childlist = mapstate[SaveListManager.instance.playerdata.playerList[recentCharacter].unitPos].GetComponentsInChildren<Transform>();
            Destroy(childlist[1].gameObject);
        }

        //새 케릭터 그리기
        SaveListManager.instance.playerdata.playerList[recentCharacter].unitPos = _temp;
        GameObject player = Instantiate(Resources.Load("PlayerModel\\UnitModel") as GameObject);
        Vector3 Pos = Vector3.zero;
        Pos.z = -10.0f;
        player.transform.SetParent(mapstate[SaveListManager.instance.playerdata.playerList[recentCharacter].unitPos].transform);
        player.transform.localPosition = Pos;
        player.transform.localScale = new Vector3(15.0f, 15.0f, 15.0f);
        player.GetComponent<SpriteRenderer>().sprite = CharacterManager.instance.ResearchUnit(SaveListManager.instance.playerdata.playerList[recentCharacter].unitEntry).idleMotion;
    }
    public void SetCharacterBatch(int _temp)
    {
        //현재 클릭된곳에 있던 유닛을 지움
        //현재 선택된 캐릭터를 배치에서 삭제함

        //현재 클릭한 버튼에 선택된 케릭터를 배치함
        //만약 현재 클릭한 버튼이 케릭터가 있던곳이었으면 케릭터를 배치하지않음
        for(int i = 0; i < SaveListManager.instance.playerdata.playerList.Count;i++)
        {
            if (SaveListManager.instance.playerdata.playerList[i].unitPos == _temp)
            {
                SaveListManager.instance.playerdata.playerList[i].unitPos = -1;
                Transform[] childlist = mapstate[_temp].GetComponentsInChildren<Transform>();
                Destroy(childlist[1].gameObject);
                if(i == recentCharacter)
                    return;
            }
        }
        DrawCharacter(_temp);
        SaveListManager.instance.GameSave(0);
    }
    public void OpenMapObject()
    {
        mapObject.SetActive(!mapObject.activeInHierarchy);
    }
    public void SelectCharacterUp()
    {
        if (SaveListManager.instance.playerdata.playerList.Count - 1 > recentCharacter)
            recentCharacter++;
        else
            recentCharacter = 0;
        characterChange();
    }
    public void SelectCharacterDown()
    {
        if (recentCharacter > 0)
            recentCharacter--;
        else
            recentCharacter = SaveListManager.instance.playerdata.playerList.Count - 1;
        characterChange();
    }
    public override void ResetPanel()
    {
        for(int i = 0; i < modelList.Count;i++)
        {
            if(modelList[i].activeInHierarchy == true)
            {
                if(i != recentCharacter)
                {
                    modelList[i].SetActive(false);
                    //모델전환애니메이션 삽입위치
                }
            }
        }
        modelList[recentCharacter].SetActive(true);
        //케릭터 스테이터스 갱신
        UnitState temp = CharacterManager.instance.ResearchUnit(SaveListManager.instance.playerdata.playerList[recentCharacter].unitEntry);
        Item tempItem1 = ItemManager.instance.ItemResearch(SaveListManager.instance.playerdata.playerList[recentCharacter].itemEntryList[0]);
        Item tempItem2 = ItemManager.instance.ItemResearch(SaveListManager.instance.playerdata.playerList[recentCharacter].itemEntryList[1]);
        int level = SaveListManager.instance.playerdata.playerList[recentCharacter].unitLevel;
        int[] itemValue = new int[4];
        if (tempItem1 != null)
        {
            itemValue[0] += tempItem1.value1;
            itemValue[1] += tempItem1.value2;
            itemValue[2] += tempItem1.value3;
            itemValue[3] += tempItem1.value4;
        }
        if (tempItem2 != null)
        {
            itemValue[0] += tempItem2.value1;
            itemValue[1] += tempItem2.value2;
            itemValue[2] += tempItem2.value3;
            itemValue[3] += tempItem2.value4;
        }
        unitClass.text = temp.UnitName;
        unitLevel.text = level.ToString();
        unitStrength.text = temp.Strength(level).ToString();
        if(itemValue[0] != 0)
        {
            unitStrength.text += "(";
            if (itemValue[0] > 0)
                unitStrength.text += "+";
            unitStrength.text+= itemValue[0] + ")";
        }
        unitInteligence.text = temp.Intelligence(level).ToString();
        if (itemValue[1] != 0)
        {
            unitInteligence.text += "(";
            if (itemValue[1] > 0)
                unitInteligence.text += "+";
            unitInteligence.text += itemValue[1] + ")";
        }
        unitSpeed.text = temp.Speed(level).ToString();
        if (itemValue[2] != 0)
        {
            unitSpeed.text += "(";
            if (itemValue[2] > 0)
                unitSpeed.text += "+";
            unitSpeed.text += itemValue[2] + ")";
        }
        unitDefence.text = temp.Defence(level).ToString();
        if (itemValue[3] != 0)
        {
            unitDefence.text += "(";
            if (itemValue[3] > 0)
                unitDefence.text += "+";
            unitDefence.text += itemValue[3] + ")";
        }
        //playerIllu.sprite = Resources.Load<Sprite>("Illu\\"+SaveListManager.instance.playerdata.playerList[recentCharacter].UnitName);
        int spellCount = temp.spellEntryList.Length;
        for (int i = 0; i<unitspell.Length;i++)
        {
            unitspell[i].unitItemIcon.gameObject.SetActive(false);
            if (i< spellCount)
            {
                Spell spell = SpellManager.instance.SpellResearch(temp.spellEntryList[i]);
                if (spell == null)
                    continue;
                if(spell.spellIcon != null)
                    unitspell[i].unitItemIcon.sprite = spell.spellIcon;
                unitspell[i].unitItemName.text = spell.spellName;
                unitspell[i].unitItemContext.text = spell.spellContext;
                unitspell[i].unitItemIcon.gameObject.SetActive(true);

            }
        }
        PlayerItemResetUI();
    }

    protected void PlayerItemResetUI()
    {
        for (int ownNumber = 0; ownNumber < 2; ownNumber++)
        {
            if (tempItem[ownNumber] != null)//기존 아이템 정리
            {
                EffectManager.instance.DeleteEffect(tempItem[ownNumber].gameObject);
                tempItem[ownNumber] = null;
            }
            if (SaveListManager.instance.playerdata.playerList[recentCharacter].itemEntryList.Length <= ownNumber)
                continue;
            int item = SaveListManager.instance.playerdata.playerList[recentCharacter].itemEntryList[ownNumber];

            if (item == 0)
                continue;
            tempItem[ownNumber] = EffectManager.instance.MakeUIEffect("ItemIcon");
            tempItem[ownNumber].GetComponent<Image>().sprite = ItemManager.instance.ItemResearch(item).itemIconSprite();

            if (tempItem[ownNumber] != null)
            {
                tempItem[ownNumber].transform.SetParent(unitItem[ownNumber].unitItemIcon.transform);
                tempItem[ownNumber].transform.localPosition = Vector3.zero;
                tempItem[ownNumber].transform.localScale = Vector3.one;
            }
        }
    }
    // Use this for initialization
    new void Awake () {
        base.Awake();
        mapstate = mapObject.GetComponentsInChildren<Button>();
        for (int i = 0; i < SaveListManager.instance.playerdata.playerList.Count; i++)
        {
            recentCharacterModel = Instantiate(Resources.Load("PlayerModel\\UnitModel") as GameObject);
            Vector3 Pos = Vector3.zero;
            recentCharacterModel.transform.SetParent(characterObject.transform);
            recentCharacterModel.transform.localPosition = Pos;
            recentCharacterModel.SetActive(false);
            recentCharacterModel.transform.localScale = new Vector3(38.0f, 38.0f, 38.0f);
            recentCharacterModel.GetComponent<SpriteRenderer>().sprite = CharacterManager.instance.ResearchUnit(SaveListManager.instance.playerdata.playerList[i].unitEntry).idleMotion;
            modelList.Add(recentCharacterModel);

            if (SaveListManager.instance.playerdata.playerList[i].unitPos == -1)//세이브파일 조회후 유닛이없는 배열은 패스 아니면 유닛 생성
                continue;
            GameObject player = Instantiate(Resources.Load("PlayerModel\\UnitModel") as GameObject);
            Pos = Vector3.zero;
            Pos.z = -10.0f;
            player.transform.SetParent(mapstate[SaveListManager.instance.playerdata.playerList[i].unitPos].transform);
            player.transform.localPosition = Pos;
            player.transform.localScale = new Vector3(15.0f,15.0f,15.0f);
            player.GetComponent<SpriteRenderer>().sprite = CharacterManager.instance.ResearchUnit(SaveListManager.instance.playerdata.playerList[i].unitEntry).idleMotion;

        }
        characterChange = ResetPanel;
        closePlayerState = null;
        ResetPanel();
    }
	
	// Update is called once per frame
	void Update () {
	}
}
