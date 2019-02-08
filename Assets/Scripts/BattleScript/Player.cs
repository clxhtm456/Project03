using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Player : Unit {
    virtual public string ScriptName
    {
        get { return state.UnitName; }
    }
    
    // Update is called once per frame
    override protected void Update () {
        base.Update();
	}
    protected override void Start()
    {
        base.Start();
    }

    override public void CalcSpellCoolTime(Spell _spell)
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
