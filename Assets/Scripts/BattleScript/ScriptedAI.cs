using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedAI : Unit {
    [Range(0, 100)]
    public float rangeThreat = 10; //거리에 따른 어그로 보정
    protected List<ThreatList> aggroList = new List<ThreatList>();
    [System.Serializable]
    protected struct ThreatList
    {
        public Unit threatUnit;
        public float threatMount;
        public void AddThreatValue(float _temp)
        {
            threatMount += _temp;
        }
    };
    protected Unit MainTarget
    {
        get
        {
            if (aggroList.Count == 0)
                return null;
            //int max = 0;
            //for (int j = 0; j < aggroList.Count; j++)//중복검사
            //{
            //    if (aggroList[j].threatMount - aggroList[j].threatUnit.MyLand().DistLand(MyLand()) >=
            //        aggroList[max].threatMount - aggroList[max].threatUnit.MyLand().DistLand(MyLand()))
            //    {
            //        max = j;
            //    }
            //}
            //return aggroList[max].threatUnit;
            //어그로양이 높은상대를 따라감
            int max = 0;
            for (int j = 0; j < aggroList.Count; j++)//중복검사
            {
                if (aggroList[j].threatUnit.MyLand.DistLand(MyLand) <
                    aggroList[max].threatUnit.MyLand.DistLand(MyLand))
                {
                    max = j;
                }
            }
            return aggroList[max].threatUnit;
            //거리가 가까운 상대를 따라감
        }
    }
    public void ReCheckThreatList()
    {
        for (int i = 0; i < Land.allLand.Count; i++)
        {
            if (Land.allLand[i].LandUnit != null && Land.allLand[i].LandUnit.Faction != Faction)
            {
                if (SearchTheartList(Land.allLand[i].LandUnit) == -1)
                {
                    ThreatList temp = new ThreatList();
                    temp.threatUnit = Land.allLand[i].LandUnit;
                    temp.threatMount = 0.0f;
                    aggroList.Add(temp);
                }
            }
        }
    }
    public void DeleteThreatTarget(Unit _target)
    {
        for (int j = 0; j < aggroList.Count; j++)//중복검사
        {
            if (aggroList[j].threatUnit == _target)
                aggroList.Remove(aggroList[j]);
        }
    }
    protected int SearchTheartList(Unit _temp)
    {
        for (int j = 0; j < aggroList.Count; j++)//중복검사
        {
            if (aggroList[j].threatUnit == _temp)
                return j;
        }
        return -1;//검색결과없음
    }
    public void AddThreat(Unit _target,float _value)
    {
        int temp = SearchTheartList(_target);
        if (temp != -1)
        {
            aggroList[temp].AddThreatValue(_value);
        }else
        {
            ThreatList tempList = new ThreatList();
            tempList.threatUnit = _target;
            tempList.threatMount = _value;
            aggroList.Add(tempList);
        }
    }
    protected override void Start()
    {
        base.Start();
        ReCheckThreatList();
    }
    protected override void Update()
    {
        base.Update();
        
    }
    public override void CalcSpellCoolTime(Spell _spell)
    {
        base.CalcSpellCoolTime(_spell);
        switch(_spell.spellEntry)
        {
            case 1://이동스킬
                {
                    Unit taget = MainTarget;
                    if (!taget)
                        return;
                    if (_spell.CanReach(taget.MyLand))
                    {
                        _spell.SpellCast(taget.MyLand);
                    }
                    else
                    {
                        _spell.SpellCast(MyLand.AstarAlg(taget.MyLand));
                    }
                }
                break;
            default:
                {
                    Unit taget = MainTarget;
                    if (!taget)
                        return;
                    if (_spell.CanReach(taget.MyLand))
                    {
                        _spell.SpellCast(taget.MyLand);
                    }
                }
                break;
        }
    }
}
