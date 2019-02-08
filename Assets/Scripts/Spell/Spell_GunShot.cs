using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_GunShot : Spell {

    public override bool SpellEffect(params Land[] _target)
    {
        if (!CanReach(_target[0]) || _target[0] == spellCaster.MyLand)
            return false;
        Land temp = spellCaster.MyLand;
        int count = 0;
        while (temp != _target[0] && count < 5)
        {
            count++;
            if (temp.Xpos > _target[0].Xpos)
                temp = temp.NearLand(-1, 0);
            else if (temp.Xpos < _target[0].Xpos)
                temp = temp.NearLand(1, 0);
            if (temp.LandUnit != null && temp.LandUnit.Faction != spellCaster.Faction)
                break;
        }
        if (temp.LandUnit != null)
        {
            spellCaster.DealDamage(temp.LandUnit, (int)(value1));
        }
        spellCaster.StartSpellAnim(2);
        return true;
    }
    public override bool CanReach(Land _dest)
    {
        if ( _dest.Ypos - spellCaster.MyLand.Ypos == 0)
        {
            return true;
        }
        else
            return false;
    }
}
