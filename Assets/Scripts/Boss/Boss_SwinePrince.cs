using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_SwinePrince : ScriptedAI {
    public Transform idleMotion;
    public Transform attackMotion;
    public GameObject WelberPrefab;
    Unit Welber;
    public GameObject arm;
    float attackSpeed = 0;
    float choiceTargetTimer = 5;
    float attackTimer = 50;
    GameObject rayEffect;

    Land attackTarget;

    IEnumerator SpellAnim(int _temp, float _timer = 0.2f)
    {
        yield return null;
        float timer = 0.0f;
        SwitchMotionIdle(false);
        while (timer < 0.2f)
        {
            //transform.Translate(Vector3.up * Time.deltaTime * 3.0f);
            timer += Time.deltaTime;
            yield return null;
        }
        SwitchMotionIdle(true);
    }
    public void SwitchMotionIdle(bool _temp)
    {
        if(_temp)
        {
            idleMotion.gameObject.SetActive(true);
            attackMotion.gameObject.SetActive(false);
        }else
        {
            idleMotion.gameObject.SetActive(false);
            attackMotion.gameObject.SetActive(true);
        }
    }
    protected override void Start()
    {
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

        ReCheckThreatList();

        Welber = Instantiate(WelberPrefab.GetComponent<Unit>());
        Vector3 Pos = new Vector3(0.0f, 0.0f, -0.5f);
        Vector3 Scale = transform.localScale;
        Welber.transform.SetParent(MyLand.transform);
        Welber.transform.localPosition = Pos;
        Welber.transform.localScale = Scale;
        Welber.MoveLand(MyLand.NearLand(0,1));
        Welber.Faction = 1;
        Welber.playable = false;
        Welber.CharacterSpriteTurnRight(false);
    }

    protected override void Update()
    {
        base.Update();
        SetStemina(attackSpeed * 50 * Time.deltaTime);
        if (Welber.died != true)
        {
            if (choiceTargetTimer < Time.deltaTime)
            {
                rayEffect = EffectManager.instance.MakeEffect("Ray_Target", MainTarget.transform.position);
                attackTarget = MainTarget.MyLand;
                attackSpeed = 1;
                AudioManager.instance.PlayEffect("char_en_piglet_squeal");
                Welber.UnitSpeakText("저놈을 끝장내십쇼");
                Welber.StartSpellAnim(0,2.0f);
                choiceTargetTimer = 9999;
            }
            else
                choiceTargetTimer -= Time.deltaTime;
        }else
        {
            if(MainTarget)
                attackTarget = MainTarget.MyLand;
            attackSpeed = 10;
            Debug.Log("광폭화");
        }
        if(recentStemina >= state.maxStemina)
        {
            SetSteminaZero();
            if (rayEffect != null)
                EffectManager.instance.DeleteEffect(rayEffect);
            spellList[0].SpellCast(attackTarget);
            Debug.Log("공격");
            attackSpeed = 0;
            choiceTargetTimer = 5;
        }
        

    }
    public override void CalcSpellCoolTime(Spell _spell)
    {
    }
}
