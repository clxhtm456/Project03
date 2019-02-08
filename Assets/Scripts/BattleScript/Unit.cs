using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class Unit :MonoBehaviour{
    SpriteRenderer renderer;

    public int level;
    public ProgressBarObj uiHpbar;
    public ProgressBarObj uiSteminabar;
    public UnitState state;
    public bool playable;//플레이어유닛인지여부

    public Item[] itemList = new Item[2];
    public Spell[] spellList = new Spell[5];
    public delegate void DiedEvent();
    public DiedEvent diedEvent;
    public bool died;

    float idleTimer;
    bool idlebool;
    public int recentHP;
    public float recentStemina;
    public Land MyLand;

    public void SetState(UnitState _temp)
    {
        state = _temp;
    }
    void IdleAnim()
    {
        Vector3 size = transform.localScale;
        if(idlebool)
        {
            size.y += 0.02f*Time.deltaTime;
            idleTimer += Time.deltaTime;
            if (idleTimer > 1.0f)
                idlebool = false;
        }
        else
        {
            size.y -= 0.02f * Time.deltaTime;
            idleTimer -= Time.deltaTime;
            if (idleTimer < 0.0f)
                idlebool = true;
        }
        transform.localScale = size;
    }
    protected void Awake()
    {
        renderer = GetComponentInChildren<SpriteRenderer>();
    }
    public void CharacterSpriteTurnRight(bool _right)
    {
        Vector3 scale = renderer.transform.localScale;
        if (_right)
            scale.x = 1.0f;
        else
            scale.x = -1.0f;

        renderer.transform.localScale = scale;
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
        Vector3 startPos = MyLand.transform.position;
        if(state.spellMotion.Length > 0)
            renderer.sprite = state.spellMotion[0];
        float timer = 0.0f;
        while (timer < 0.2f)
        {
            transform.position = Vector3.MoveTowards(transform.position, _dest.transform.position, Time.deltaTime * 1.5f);
            //transform.Translate(Vector3.up * Time.deltaTime * 3.0f);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.position = startPos;
        renderer.sprite = state.idleMotion;
    }
    IEnumerator SpellAnim(int _temp, float _timer = 0.2f)
    {
        yield return null;
        if (state.spellMotion.Length > _temp)
            renderer.sprite = state.spellMotion[_temp];
        float timer = 0.0f;
        while (timer < _timer)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        renderer.sprite = state.idleMotion;
    }
    IEnumerator EffectCam(params Land[] _dest)
    {
        float startTime = Time.realtimeSinceStartup;
        EffectManager.GamePause(true);
        GameObject highlightEffect = EffectManager.instance.MakeEffect("EffectCamera");
        foreach (Land i in _dest)
        {
            if (i.LandUnit != null)
                i.LandUnit.CharacterHighLight(true);
        }
        CharacterHighLight(true);
        Vector3 camPos = highlightEffect.transform.position;
        float xPos = transform.position.x;
        for(int i = 0; i < _dest.Length;i++)
        {
            xPos += _dest[i].LandUnit.transform.position.x;
        }
        xPos /= _dest.Length + 1;

        float yPos = transform.position.y;
        for (int i = 0; i < _dest.Length; i++)
        {
            yPos += _dest[i].LandUnit.transform.position.y;
        }
        yPos /= _dest.Length + 1;

        highlightEffect.transform.position = new Vector3(xPos, yPos, transform.position.z);
        while (Time.realtimeSinceStartup - startTime < 0.5f)
        {
            yield return null;
        }
        EffectManager.instance.DeleteEffect(highlightEffect);
        CharacterHighLight(false);
        foreach (Land i in _dest)
        {
            if (i.LandUnit != null)
                i.LandUnit.CharacterHighLight(false);
        }
        EffectManager.GamePause(false);
    }
    public void CharacterHighLight(bool _temp)
    {
        if(_temp)
        {
            Transform[] trans = GetComponentsInChildren<Transform>();
            foreach(Transform t in trans)
            {
                t.gameObject.layer = 9;
            }
        }else
        {
            Transform[] trans = GetComponentsInChildren<Transform>();
            foreach (Transform t in trans)
            {
                t.gameObject.layer = 0;
            }
        }
    }
    public virtual void StartAttackAnim(Land _dest)
    {
        StartCoroutine(AttackAnim(_dest));
        //StartCoroutine(EffectCam(_dest));
    }
    public virtual void StartSpellAnim(int animCount = 0, float _timer = 0.2f)
    {
        StartCoroutine(SpellAnim(animCount, _timer));
        //StartCoroutine(EffectCam());
    }
    public virtual void MoveLand(Land _dest)
    {
        MyLand = _dest;
        gameObject.transform.SetParent(_dest.transform);
        //gameObject.transform.localPosition = Vector3.zero;
        StartCoroutine(MoveCorutine());
    }
    public void Knockback(Land _dest)
    {
        if (_dest == null || _dest == MyLand)
            return;
        int destright;
        int destup;
        if ((MyLand.Xpos - _dest.Xpos) < 0)
            destright = 1;
        else if ((MyLand.Xpos - _dest.Xpos) > 0)
            destright = -1;
        else
            destright = 0;
        if ((MyLand.Ypos - _dest.Ypos) < 0)
            destup = 1;
        else if ((MyLand.Ypos - _dest.Ypos) > 0)
            destup = -1;
        else
            destup = 0;


        if (_dest.LandUnit != null)
        {
            _dest.LandUnit.Knockback(_dest.NearLand(destright,destup));
        }
        if(_dest.LandUnit == null)
            MoveLand(_dest);
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
            diedEvent += monster.DeadEvent;//주인이 죽으면 소환수도 죽음
        }
        return null;
    }
    virtual public int Strength
    {
        get {
            int temp = 0;
            if (itemList[0] != null)
                temp += itemList[0].value1;
            if (itemList[1] != null)
                temp += itemList[1].value1;
            return state.d_strength + (level*state.strength) + temp; }//각종 버프나 아이템에 의해 계산이 끝난 힘스탯
    }
    virtual public int Intelligence
    {
        get {
            int temp = 0;
            if (itemList[0] != null)
                temp += itemList[0].value2;
            if (itemList[1] != null)
                temp += itemList[1].value2;
            return state.d_intelligence + (level * state.intelligence) + temp; }//각종 버프나 아이템에 의해 계산이 끝난 지능스탯
    }
    virtual public int Speed
    {
        get {
            int temp = 0;
            if (itemList[0] != null)
                temp += itemList[0].value3;
            if (itemList[1] != null)
                temp += itemList[1].value3;
            return state.d_speed +(level * state.speed) + temp; }//각종 버프나 아이템에 의해 계산이 끝난 공격속도스탯
    }
    virtual public int Defence
    {
        get {
            int temp = 0;
            if (itemList[0] != null)
                temp += itemList[0].value4;
            if (itemList[1] != null)
                temp += itemList[1].value4;
            return state.d_defence + (level * state.defence) + temp; }//각종 버프나 아이템에 의해 계산이 끝난 방어력스탯
    }
    virtual public int MaxHP
    {
        get { return state.defaultHP + +(level * state.maxHp); }//각종 버프나 아이템에 의해 계산이 끝난 방어력스탯
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
        if (state.idleMotion)
        {
            renderer.sprite = state.idleMotion;
        }
        uiHpbar.maxValue = MaxHP;
        uiHpbar.RecentValue = MaxHP;
        recentHP = MaxHP;
        uiHpbar.Stop = true;

        uiSteminabar.maxValue = state.maxStemina;
        uiSteminabar.RecentValue = 0.0f;
        recentStemina = 0.0f;
        uiSteminabar.Stop = true;

        Debug.Log(transform.name + "스펠추가");
        for (int i = 0; i < state.spellEntryList.Length; i++)
        {
            if (state.spellEntryList[i] == 0)
                break;
            Spell spell = Instantiate(SpellManager.instance.SpellResearch(state.spellEntryList[i])).GetComponent<Spell>();
            spellList[i] = spell;
            spell.spellCaster = this;
        }
    }
    public void DealDamage(Unit _target,int _value)
    {
        if (_target == null)
            return;
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
        EffectManager.instance.MakeEffect("CFX_Hit_C White", transform.position);
        recentHP -= _value;
        if (uiHpbar)
        {
            uiHpbar.RecentValue = recentHP;
        }
        if (recentHP <= 0)
        {
            recentHP = 0;
            DeadEvent();
        }
    }
    public void UnitSpeakText(string _text)
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        GameObject font = EffectManager.instance.MakeUIEffect("TextEffect");
        pos.y += 120.0f;
        font.transform.position = pos;
        
        font.GetComponentInChildren<Text>().text = _text;
    }
    public void SetStemina(float _value)
    {
        recentStemina += _value;
        if (recentStemina > state.maxStemina)
            recentStemina = state.maxStemina;
        if (recentStemina < 0)
            recentStemina = 0;
        uiSteminabar.RecentValue = recentStemina;
    }
    public void SetSteminaZero()
    {
        recentStemina = 0;
        uiSteminabar.RecentValue = recentStemina;
    }
    private void OnDestroy()
    {
        DeadEvent();
    }
    public void DeadEvent()
    {
        died = true;
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
        if(diedEvent != null)
            diedEvent();//죽었을때 이벤트발생(몬스터같은경우 던전스테이지갱신등)
        Destroy(gameObject);
    }
    public void HealDamage(Unit _target, int _value)
    {
        if (_target == null)
            return;
        _target.RestoreDamage(_value);
        //힐 광역어그로 생성
        for (int i = 0; i < Land.allLand.Count; i++)
        {
            if (Land.allLand[i].LandUnit != null)
            {
                ScriptedAI targetAI;
                if (targetAI = Land.allLand[i].gameObject.GetComponent<ScriptedAI>())
                {
                    if(targetAI.Faction != Faction)
                        targetAI.AddThreat(this, _value * 1.0f);
                }
            }
        }
    }
    protected void RestoreDamage(int _value)
    {
        if (died)//죽엇을경우 힐을 받을수없음
            return;
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        DamageFont font = EffectManager.instance.MakeUIEffect("HealFont").GetComponent<DamageFont>();
        font.transform.position = pos;
        font.SetText(_value.ToString());
        recentHP += _value;
        if (recentHP > MaxHP)
            recentHP = MaxHP;
        if (uiHpbar)
        {
            uiHpbar.RecentValue = recentHP;
        }
    }

    // Update is called once per frame
    virtual protected void Update () {
        if (died || EffectManager.IsGamePause())//죽은자는 말이없음
            return;
        IdleAnim();

    }
    virtual public void CreateUnit()
    {

    }
    virtual public void CalcSpellCoolTime(Spell _spell)
    {
        if (died)//죽은자는 말이없음
            return;
        if (playable == true)//내가 조종하는 유닛의 경우 스킬테이블에 스킬등록
            GameObject.Find("SpellTable").GetComponent<SpellTableInterface>().SpellAdd(this, _spell);
        else//아닐경우 별도의 행동을 취하지않음(추후 pvp패치를 위함)
        {
        }
    }
}
