using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellTableInterface : MonoBehaviour {
    bool globalCool = false;
    public Image globalCoolPanel;
    public float spellCardSpeed = 3.0f;
    public SpellCard[] spellList = new SpellCard[6];
    public delegate void SpellCheck();
    static public SpellCheck Spellcheck;
    Vector3[] spellPosition = {
        new Vector3(334.8f,0.0f,0.0f),
        new Vector3(200.5f,0.0f,0.0f),
        new Vector3(66.4f,0.0f,0.0f),
        new Vector3(-67.9f,0.0f,0.0f),
        new Vector3(-202.2f,0.0f,0.0f),
        new Vector3(-336.2f,0.0f,0.0f),
        new Vector3(-470.0f,0.0f,0.0f)
    };
    int spellCount = 0;
    IEnumerator GlobalCoolDown()
    {
        yield return null;
        globalCool = true;
        globalCoolPanel.fillAmount = 1.0f;
        float timer = 0.5f;
        while(timer > 0.0f)
        {
            globalCoolPanel.fillAmount = timer * 2.0f;
            timer -= Time.deltaTime;
            yield return null;
        }
        globalCoolPanel.fillAmount = 0.0f;
        globalCool = false;
    }
    void StartGlobalCoolDown()
    {
        StartCoroutine(GlobalCoolDown());
    }
    public void SpellAdd(Unit _caster,Spell _spell)
    {
        SpellReorder();//버그방지를 위한 스펠 재정렬
        spellCount = SizeOfSpellCard();
        if (spellCount >= 6)//스펠테이블이 가득차있는경우 스펠이 추가될수없음
            return;
        Vector3 createPos = Vector3.zero;

        if (spellCount > 0 && spellList[spellCount - 1].transform.localPosition.x < -336.2f)
        {
            createPos = new Vector2(spellList[spellCount - 1].transform.localPosition.x - 134.3f, 0.0f);
        }
        else
            createPos = spellPosition[6];
        spellList[spellCount] = EffectManager.instance.MakeUIEffect("SpellCard").GetComponent<SpellCard>();
        spellList[spellCount].transform.SetParent(transform);
        spellList[spellCount].SpellCardPos = createPos;
        spellList[spellCount].transform.localPosition = createPos;
        spellList[spellCount].parentUI = gameObject;
        _spell.spellCardList.Add(spellList[spellCount]);
        SpellCard tempCard = spellList[spellCount].GetComponent<SpellCard>();
        tempCard.spell = _spell;
        tempCard.spell.spellCaster = _caster;
        tempCard.cardName.text = _spell.spellName;
        tempCard.cardContext.text = _spell.spellContext;
        tempCard.cardFrame.color = _caster.state.frameColor;
        if (_spell.spellIcon!= null)
            tempCard.cardIcon.sprite = _spell.spellIcon;
        spellCount++;
    }
    int SizeOfSpellCard()
    {
        for (int i = 5; i >= 0; i--)
        {
            if (spellList[i] != null)
                return i+1;
        }
        return 0;
    }
	// Use this for initialization
	void Start () {
        Spellcheck = new SpellCheck(SpellReorder);
        Spellcheck += StartGlobalCoolDown;
    }
	
	// Update is called once per frame
	void Update () {
        MoveSpellCard();

    }
    public void SpellReorder()//스펠 재정렬 스펠 사용시 또는 새로운 스펠 추가시 작동
    {
        for (int i = 0; i < 6; i++)
        {
            if (spellList[i] && spellList[i].gameObject.activeInHierarchy == false)
            {
                spellList[i] = null;
            }
        }
        for (int i = 0; i < 6;i++)
        {
            if (spellList[i] != null)
            {
                for (int j = 0; j < i; j++)
                {
                    if (spellList[j] == null)
                    {
                        spellList[j] = spellList[i];
                        spellList[i] = null;
                    }
                }
            }
        }
    }
    void MoveSpellCard()//스펠카드이동
    {
        for(int i = 0; i < spellList.Length;i++)
        {
            if (spellList[i] == null)//스펠이동이 다 끝나면 리턴
                return;
            if (spellList[i].SpellCardPos.x < spellPosition[i].x)
            {
                spellList[i].SpellCardPos += (Vector3.right * Time.deltaTime * spellCardSpeed);
                spellList[i].DragPos = spellList[i].SpellCardPos;
                if (spellList[i].SpellDrag == false)
                {
                    spellList[i].transform.localPosition = spellList[i].SpellCardPos;
                }
            }
        }
    }
}
