using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]

public class Spell : MonoBehaviour{
    public int spellEntry;
    public string spellName;
    public int spellDistance;
    public int spellType;

    public float value1;
    public float value2;
    public float value3;
    public float value4;

    public float steminaCost;

    protected float spelltimer;
    public float coolTime;

    public string spellScript;
    public string spellContext;
    public string spellIcon;

    public Land spellTarget;
    public Unit spellCaster;
    public List<SpellCard> spellCardList = new List<SpellCard>();

    public bool spellReady = false;

    //[HideInInspector]
    protected ArrowRenderer arrow;
    protected GameObject crosshair;
    protected bool active = false;

    protected bool Active
    {
        set { active = value;
            AnalysisRange(value);
        }
        get { return active; }
    }

    public void AnalysisRange(bool _active)
    {
        switch (_active)
        {
            case true:
                {
                    for (int i = 0; i < Land.allLand.Count; i++)
                    {
                        if (CanReach(Land.allLand[i]))
                        {
                            Land.allLand[i].LandActive();
                        }
                        else
                            Land.allLand[i].LandDeActive();

                    }
                }
                break;
            case false:
                {
                    for (int i = 0; i < Land.allLand.Count; i++)
                    {
                        Land.allLand[i].LandNormalize();
                    }
                }
                break;
        }
    }
    virtual public void SpellActive()
    {
        Active = true;
        DrawParabola();
    }
    virtual public void SpellDeActive()
    {
        Active = false;
        DeleteParabola();
    }

    virtual public float CoolTime
    {
        get { return coolTime; }
        set { coolTime = value; }
    }
    virtual public bool SpellAlarm()//스킬발동시 스킬이름이 뜨는 효과
    {
        GameObject nameeffect =  EffectManager.instance.MakeUIEffect("SpellNameEffect",new Vector3(0,243,0),Quaternion.identity);
        nameeffect.GetComponentInChildren<Text>().text = spellName;
        return true;
    }
    virtual public bool SpellParticle()//스펠 이펙트효과 발동
    {
        return true;
    }
    public bool SpellCast(Land _target)//스펠발동
    {
        bool trigger1 = SpellAlarm();
        bool trigger2 = SpellParticle();
        bool trigger3 = SpellEffect(_target);
        if (trigger1 && trigger2 && trigger3)
        {
            spellCaster.SetStemina(-1.0f * steminaCost);
            spellReady = false;
            return true;
        }
        else
            return false;
    }
    virtual public bool SpellCast()//스펠발동
    {
        if (spellTarget == null)
            return false;
        if (!CanReach(spellTarget))
            return false;
        bool trigger1 = SpellAlarm();
        bool trigger2 = SpellParticle();
        bool trigger3 = SpellEffect(spellTarget);
        if (trigger1 && trigger2 && trigger3)
        {
            spellCaster.SetStemina(-1.0f * steminaCost);
            spellReady = false;
            return true;
        }
        else
            return false;
    }
    protected int spellDamage()
    {
        int damage = (int)(spellCaster.Strength * value1 +
                            spellCaster.Intelligence * value2 +
                            spellCaster.Speed * value3 +
                            spellCaster.Defence * value4);
        return damage;
    }
    virtual public bool SpellEffect(Land _targetLand)//스펠효과 정의
    {
        if (_targetLand.LandUnit == null)
            return true;
        switch (spellType)
        {
            case 0://단일 물리공격
            case 1://단일 마법공격
                {
                    spellCaster.DealDamage(_targetLand.LandUnit, spellDamage());
                }
                break;
            case 2://광역 물리공격
            case 3://광역 마법공격
                {
                    //광역시전
                }
                break;
            case 4://단일 치유기술
                {
                    spellCaster.HealDamage(_targetLand.LandUnit, spellDamage());
                }
                break;
            case 5://광역 치유기술
                {
                }
                break;
            case 6://그외
                {
                }
                break;
        }
        return true;
    }
    virtual public string ScriptName
    {
        get { return null; }
    }
    virtual protected void Update()
    {
        if (!spellReady)
        {
            spelltimer += Time.deltaTime;
            if (spelltimer >= CoolTime)
            {
                spellReady = true;
                spellCaster.CalcSpellCoolTime(this);
                spelltimer = 0;
            }
        }
        if (Active)
        {
            SpellTargeting();
        }
    }
    protected void SpellTargeting()
    {
        if (arrow != null)
        {
            arrow.SetPositions(spellCaster.transform.position, CursorStalker.GameCursor.Target.transform.position);
        }
        if (CursorStalker.GameCursor.Target != CursorStalker.GameCursor.gameObject)
        {
            spellTarget = CursorStalker.GameCursor.Target.GetComponent<Land>();
        }
        else
        {
            spellTarget = null;
        }
    }
    virtual protected void LateUpdate()
    {
        if (Active && CursorStalker.GameCursor.Target.tag == "Unit")
        {
            if (crosshair == null)
                crosshair = EffectManager.instance.MakeEffect("CrossHair");
            else
            {
                crosshair.transform.position = CursorStalker.GameCursor.Target.transform.position;
            }
        }
        else
        {
            if (crosshair != null)
            {
                EffectManager.instance.DeleteEffect(crosshair);
                crosshair = null;
            }
        }
    }

    virtual protected void Start()
    {

    }
    private void OnDestroy()
    {
        if(spellCardList.Count > 0)
        {
            for(int i = 0; i < spellCardList.Count;i++)
            {
                spellCardList[i].DumpCard();
            }
        }
    }
    virtual public bool CanReach(Land _dest)
    {
        if (_dest == null)
            return false;
        else
            return true;
    }


    protected void DeleteParabola()
    {
        if (arrow)
        {
            EffectManager.instance.DeleteEffect(arrow.gameObject);
            arrow = null;
        }
    }
    protected void DrawParabola()
    {
        if (arrow == null)
        {
            Vector3 pos = spellCaster.transform.position;
            arrow = EffectManager.instance.MakeEffect("ArrowRenderer", pos, Quaternion.identity).GetComponent<ArrowRenderer>();
            CursorStalker.GameCursor.track = arrow.GetComponent<TrackerArrow>();
        }
    }

}
