using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Berserker : Player {
    override public string ScriptName
    {
        get { return "Berserker"; }
    }
    //override public void AddPlayerScript()
    //{
    //    CharacterManager.instance.CharacterScriptList.Add(this);
    //}

    //override public int Strength
    //{
    //    get {
    //        int tempStrength = (int)((state.level * 5)*( 1.0f-(state.recentHp *1.0f / state.maxHp)));//레벨*5의 힘스탯이 현재체력양에 반비례해서 추가됨
    //        return base.Strength + tempStrength;
    //    }
    //}
}
