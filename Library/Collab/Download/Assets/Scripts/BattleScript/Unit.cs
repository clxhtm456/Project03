using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Unit :MonoBehaviour{
    ProgressBar uiHpbar;
    ProgressBar uiSteminabar;
    public Color frameColor = new Color(1.0f, 1.0f, 1.0f);
    public UnitState state = new UnitState();
    public GameObject UnitModel;
    public bool playable;//플레이어유닛인지여부
    
    public Spell[] spellList = new Spell[5];
    public delegate void DiedEvent();
    public DiedEvent diedEvent;
    public Land MyLand()
    {
        return gameObject.transform.parent.GetComponent<Land>();
    }
    public void SetState(UnitState _temp)
    {
        state = _temp;
    }
    IEnumerator MoveCorutine()
    {
        yield return null;
        float middlePoint = Vector3.Distance(Vector3.zero, transform.localPosition);
        while (Vector3.Distance(Vector3.zero, transform.localPosition) > 0.1f)
        {
            if(Vector3.Distance(Vector3.zero, transform.localPosition) < middlePoint)
            {
                transform.Translate(Vector3.up * Time.deltaTime * 3.0f);
            }else
            {
                transform.Translate(Vector3.down * Time.deltaTime * 3.0f);
            }
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, Time.deltaTime*5.0f);
            yield return null;
        }
    }
    IEnumerator AttackAnim(Land _dest)
    {
        yield return null;
        Vector3 startPos = transform.position;
        float timer = 0.0f;
        while (timer < 0.2f)
        {
            transform.position = Vector3.MoveTowards(transform.position, _dest.transform.position, Time.deltaTime * 3.0f);
            //transform.Translate(Vector3.up * Time.deltaTime * 3.0f);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.position = startPos;
    }
    public void StartAttackAnim(Land _dest)
    {
        StartCoroutine(AttackAnim(_dest));
    }
    public void MoveLand(Land _dest)
    {
        gameObject.transform.SetParent(_dest.transform);
        //gameObject.transform.localPosition = Vector3.zero;
        StartCoroutine(MoveCorutine());
    }
    virtual public Unit SummonUnit(GameObject _object, Land _Pos)
    {
        Unit monster;
        if (monster = Instantiate(_object).GetComponent<Unit>())
        {
            Vector3 pos = Vector3.zero;
            Vector3 Scale = monster.transform.localScale;
            monster.transform.SetParent(_Pos.transform);
            monster.transform.localPosition = pos;
            monster.transform.localScale = Scale;
            monster.Faction = Faction;
            monster.playable = false;
        }
        return null;
    }
    virtual public int Strength
    {
        get { return state.strength; }//각종 버프나 아이템에 의해 계산이 끝난 힘스탯
    }
    virtual public int Intelligence
    {
        get { return state.intelligence; }//각종 버프나 아이템에 의해 계산이 끝난 지능스탯
    }
    virtual public int Speed
    {
        get { return state.speed; }//각종 버프나 아이템에 의해 계산이 끝난 공격속도스탯
    }
    virtual public int Defence
    {
        get { return state.defence; }//각종 버프나 아이템에 의해 계산이 끝난 방어력스탯
    }
    public int Faction
    {
        get{return state.faction; }
        set { state.faction = value; }
    }
    public void Attack()
    {

    }
    // Use this for initialization
    virtual protected void Start () {
        if(UnitModel)
        {
            GameObject model = Instantiate(UnitModel);
            model.transform.SetParent(transform);
            model.transform.localPosition = new Vector3(0, 0, 0);
            model.transform.localScale = Vector3.one*0.1f;
        }
        if (uiHpbar = EffectManager.instance.MakeUIEffect("HPBar").GetComponent<ProgressBar>())
        {
            uiHpbar.GetComponent<UIFollower>().target = gameObject;
            uiHpbar.GetComponent<UIFollower>().height = -0.7f;
            uiHpbar.maxValue = state.maxHp;
            uiHpbar.RecentValue = state.recentHp;
            uiHpbar.Stop = true;
        }
        if (uiSteminabar = EffectManager.instance.MakeUIEffect("MovementBar").GetComponent<ProgressBar>())
        {
            uiSteminabar.GetComponent<UIFollower>().target = gameObject;
            uiSteminabar.GetComponent<UIFollower>().height = -0.8f;
            uiSteminabar.maxValue = state.maxStemina;
            uiSteminabar.RecentValue = 0.0f;
            uiSteminabar.Stop = true;
        }
        Debug.Log(transform.name + "스펠추가");
        for (int i = 0; i < state.spellEntryList.Length; i++)
        {
            Spell spell = Instantiate(SpellManager.instance.SpellResearch(state.spellEntryList[i])).GetComponent<Spell>();
            spellList[i] = spell;
            spell.spellCaster = this;
        }
    }
    public void DealDamage(Unit _target,int _value)
    {
        _target.TakeDamage(_value);

        //단일 어그로 증가
        ScriptedAI targetAI;
        if(targetAI = _target.gameObject.GetComponent<ScriptedAI>())
        {
            if (targetAI.Faction != Faction)
                targetAI.AddThreat(this,_value * 1.0f);
        }
    }
    protected void TakeDamage(int _value)
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        DamageFont font =  EffectManager.instance.MakeUIEffect("DamageFont").GetComponent<DamageFont>();
        font.transform.position = pos;
        font.SetText(_value.ToString());
        EffectManager.instance.MakeEffect("CFX4 Hit B (Orange)",transform.position,Quaternion.identity);
        state.recentHp -= _value;
        if (uiHpbar)
        {
            uiHpbar.RecentValue = state.recentHp;
        }
        if (state.recentHp <=0)
        {
            state.recentHp = 0;
            DeadEvent();
        }
    }
    public void SetStemina(float _value)
    {

        state.recentStemina += _value;
        if (state.recentStemina > state.maxStemina)
            state.recentStemina = state.maxStemina;
        if (state.recentStemina < 0)
            state.recentStemina = 0;
        uiSteminabar.RecentValue = state.recentStemina;
    }
    protected void DeadEvent()
    {
        state.died = true;
        for (int i = 0; i < spellList.Length; i++)
        {
            if(spellList[i] != null)
                Destroy(spellList[i].gameObject);//자신의 스펠 삭제
        }
        for (int i = 0; i < Land.allLand.Count; i++)
        {
            if (Land.allLand[i].LandUnit != null)
            {
                ScriptedAI targetAI = Land.allLand[i].LandUnit.gameObject.GetComponent<ScriptedAI>();
                if (targetAI)
                {
                    targetAI.DeleteThreatTarget(this);
                }
            }
        }
        diedEvent();//죽었을때 이벤트발생(몬스터같은경우 던전스테이지갱신등)
        Destroy(gameObject);
    }
    public void HealDamage(Unit _target, int _value)
    {
        _target.RestoreDamage(_value);
        //힐 광역어그로 생성
        for (int i = 0; i < Land.allLand.Count; i++)
        {
            if (Land.allLand[i].LandUnit != null)
            {
                ScriptedAI targetAI;
                if (targetAI = _target.gameObject.GetComponent<ScriptedAI>())
                {
                    if(targetAI.Faction != Faction)
                        targetAI.AddThreat(this, _value * 1.0f);
                }
            }
        }
    }
    protected void RestoreDamage(int _value)
    {
        if (state.died)//죽엇을경우 힐을 받을수없음
            return;
        state.recentHp += _value;
        if (state.recentHp > state.maxHp)
            state.recentHp = state.maxHp;
        if (uiHpbar)
        {
            uiHpbar.RecentValue = state.recentHp;
        }
        if (state.recentHp >= state.maxHp)
        {
            state.recentHp = state.maxHp;
        }
    }

    // Update is called once per frame
    virtual protected void Update () {
        if (state.died)//죽은자는 말이없음
            return;
        

    }
    virtual public void CreateUnit()
    {

    }
    virtual public void CalcSpellCoolTime(Spell _spell)
    {
        if (state.died)//죽은자는 말이없음
            return;
    }
}
