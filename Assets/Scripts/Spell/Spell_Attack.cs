using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_Attack : Spell {
    GameObject startPoint;
    public override float CoolTime
    {
        get
        {
            int variable = Random.Range(0, spellCaster.Defence);
            int result = spellCaster.Speed - variable;
            if (result < 1)
                return 1;
            else
                return result;//기본공격스킬은 쿨타임이 아닌 공격속도에 영향을 받음
        }

        set
        {
            base.CoolTime = value;
        }
    }
    override protected void Start()
    {
    }
    protected override void Update()
    {
        if (EffectManager.IsGamePause())
            return;
        spellCaster.SetStemina(CoolTime*50*Time.deltaTime);
        if (spellCaster.playable == true)
        {
            if (CursorStalker.GameCursor.Target == spellCaster.MyLand.gameObject &&//현재 케릭터를 선택하고있으면서
            spellCaster.recentStemina == spellCaster.state.maxStemina && //쿨타임이 되었고
            Input.GetMouseButtonDown(0)//클릭버튼을 눌렀을시 스킬발동
            )//내 유닛이여야만 조종가능
            {
                Active = true;
                startPoint = CursorStalker.GameCursor.Target;
                DrawParabola();
            }
            if (Active)
            {
                SpellTargeting();
                if (Input.GetMouseButtonUp(0))
                {
                    Active = false;
                    DeleteParabola();
                    Land dest = CursorStalker.GameCursor.Target.GetComponent<Land>();
                    if (dest == null || !dest.CompareTag("Unit") || CursorStalker.GameCursor.Target == spellCaster.MyLand.gameObject)
                        return;
                    if (CanReach(dest))
                    {
                        SpellCast(dest);
                    }
                    else
                    {
                        dest = spellCaster.MyLand.AstarAlg(CursorStalker.GameCursor.Target.GetComponent<Land>());//A*알고리즘을 사용한 목적지
                        SpellCast(dest);
                    }
                }
            }
        }else
        {
            if(spellCaster.recentStemina == spellCaster.state.maxStemina)
            {
                spellReady = true;
                spellCaster.CalcSpellCoolTime(this);
            }
        }
        
    }
    override public bool SpellParticle()//스펠 이펙트효과 발동
    {
        return true;
    }
    public override bool SpellAlarm()
    {
        return true;
    }
    public override bool SpellEffect(params Land[] _target)
    {
        Unit target = _target[0].GetComponentInChildren<Unit>();
        int damage = (int)(spellCaster.Strength * value1);
        if (target == null)//해당위치에 유닛이없는경우
        {
            spellCaster.MoveLand(_target[0]);//해당위치로 유닛이동
        }else if (target && target.Faction != spellCaster.Faction)//유닛이 있으면서 자기편이 아닐경우(자기도 포함) 공격
        {
            spellCaster.DealDamage(target, damage);
            spellCaster.StartAttackAnim(target.MyLand);
        }
        else//아군유닛과 자리바꾸기
        {
            if(target.recentStemina == target.state.maxStemina)
            {
                for(int i = 0; i < target.spellList.Length;i++)
                {
                    if(target.spellList[i].spellEntry == spellEntry)
                    {
                        Land templand = spellCaster.MyLand;
                        spellCaster.MoveLand(_target[0]);
                        target.spellList[i].SpellCast(templand);
                        return true;
                    }
                }
            }
            return false;
        }
        return true;
    }
    public override bool CanReach(Land _dest)
    {
        if (spellCaster.MyLand == _dest)
            return true;
        //if ((Mathf.Abs(_dest.Xpos - spellCaster.MyLand().Xpos) == 1 &&
        //    _dest.Ypos - spellCaster.MyLand().Ypos == 0) ||
        //    (Mathf.Abs(_dest.Ypos - spellCaster.MyLand().Ypos) == 1 &&
        //    _dest.Xpos - spellCaster.MyLand().Xpos == 0))
        if(spellCaster.MyLand.DistLand(_dest) < 2)
        {
            if (spellCaster.MyLand.DistLand(_dest) > 1 &&
                _dest.LandUnit != null &&
                _dest.LandUnit.Faction != spellCaster.Faction)
            {
                return false;
            }else
                return true;
        }
        else
            return false;
    }
}
