using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MathPlus;

public class SpellCard : MonoBehaviour {
    public GameObject parentUI;
    public Vector3 SpellCardPos;
    private DragDropObject dragDropObject;

    public Spell spell;

    public Text cardName;
    public Text cardContext;
    public Image cardFrame;
    public Image cardIcon;


    public GameObject detail;
    static private GameObject crosshair;

    public Vector3 DragPos
    {
        get
        {
            return dragDropObject.defaultPosition;
        }
        set
        {
            dragDropObject.defaultPosition = value;
        }
    }
    private void Awake()
    {
        dragDropObject = GetComponent<DragDropObject>();
        cardName = transform.Find("Name").GetComponent<Text>();
        cardContext = transform.Find("Context").GetComponent<Text>();
        cardFrame = transform.Find("Frame").GetComponent<Image>();
        cardIcon = transform.Find("Icon").GetComponent<Image>();
    }

    public void ShowSpellDetail()
    {
        if (detail != null)
            return;
        detail = EffectManager.instance.MakeUIEffect("SpellDetail", new Vector3(-414.0f, 96.2f, 0.0f), Quaternion.identity);
        detail.transform.Find("Name").GetComponent<Text>().text = spell.spellName;
        detail.transform.Find("Context").GetComponent<Text>().text = spell.spellContext;
        detail.transform.Find("Frame").GetComponent<Image>().color = spell.spellCaster.frameColor;
        if (!spell.spellIcon.Equals(""))
            detail.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("Icon\\" + spell.spellIcon);
    }

    public void UnShowSpellDetail()
    {
        if (detail == null)
            return;
        EffectManager.instance.DeleteEffect(detail);
        detail = null;
    }
    public bool SpellDrag
    {
        get
        {
            return dragDropObject.isDrag;
        }
    }
    
    public void DumpCard()
    {
        UnShowSpellDetail();
        SpellCardActive(false);
        EffectManager.instance.DeleteEffect(gameObject);
        SpellTableInterface.Spellcheck();
    }
    public void SpellCardActive(bool _value)//스펠카드를 드래그했을때
    {
        if(_value)
        {
            spell.SpellActive();
        }
        else
        {
            spell.SpellDeActive();
        }
    }
    public void SpellCast()
    {
        if (!spell.SpellCast())
        {
            Debug.Log("스킬발동실패");
            UnShowSpellDetail();
            SpellCardActive(false);
            CardReturn();
        }
        else
        {
            UnShowSpellDetail();
            SpellCardActive(false);
            EffectManager.instance.DeleteEffect(gameObject);
            SpellTableInterface.Spellcheck();
        }
    }
    // Use this for initialization
    void Start () {

    }
	public void ChoiceSpellTarget()
    {
        if(spell.spellType == 2 ||
            spell.spellType == 3 ||
            spell.spellType == 5 )
        {
            //광역기술이므로 타겟이 필요없음 그대로 시전
        }else
        {
            
        }
    }
    public bool CardVisable
    {
        set
        {
            Vector3 Pos = transform.localScale;
            if (value)
            {
                Pos.x = 1.0f;
            }
            else
            {
                Pos.x = 0.0f;
            }
            transform.localScale = Pos;
        }
        get
        {
            Vector3 Pos = transform.localScale;
            if (Pos.x == 1.0f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    void CardReturn()
    {
        transform.SetParent(parentUI.transform);
        DragPos = SpellCardPos;
        transform.localPosition = SpellCardPos;
    }
	// Update is called once per frame
	void Update () {
        if (transform.localPosition.y > 90.0f)//카드밖으로 커서가 나갔을경우
        {
            CardVisable = false;
            UnShowSpellDetail();
            SpellCardActive(true);
        }
        else//커서안일 경우
        {
            if(CardVisable == false)
            {
                SpellCardActive(false);
            }
            else if(dragDropObject.isDrag)
            {
                ShowSpellDetail();
            }else
                UnShowSpellDetail();
            CardVisable = true;
        }

        if (transform.parent != parentUI.transform)
        {
            if(transform.parent.CompareTag("SpellZone"))
                SpellCast();
            else if(transform.parent.CompareTag("SpellDump"))
            {
                DumpCard();
            }
        }
    }
}
