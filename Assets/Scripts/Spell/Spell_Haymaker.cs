using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_Haymaker : Spell {

    public override bool SpellEffect(params Land[] _target)
    {
        spellCaster.StartSpellAnim();
        Land temp = Land.LandbyPos(1, _target[0].Ypos);
        int count = 0;
        AudioManager.instance.PlayEffect("char_en_swineprince_enrage");
        for (int i = 0; temp != null && count<10; temp = temp.NearLand(1, 0), count++)
        {
            EffectManager.instance.MakeEffect("CFX_Hit_C White", temp.transform.position);
            if (temp == null || temp.LandUnit == null)
                continue;
            if (temp.LandUnit.Faction != spellCaster.Faction)
                spellCaster.DealDamage(temp.LandUnit, spellDamage());//지정한 한줄 전체에 데미지
            
        }
        return true;
    }
    public override bool CanReach(Land _dest)
    {
        return true;
    }
}
