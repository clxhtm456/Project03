using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_Heal : Spell {

    public override bool SpellEffect(params Land[] _target)
    {
        if (!CanReach(_target[0]) || _target[0].LandUnit == null)
            return false;
        spellCaster.HealDamage(_target[0].LandUnit,spellDamage());
        spellCaster.StartSpellAnim(1);
        return true;
    }
    public override bool CanReach(Land _dest)
    {
        if (_dest.DistLand(spellCaster.MyLand ) < 2.2)
        {
            return true;
        }
        else
            return false;
    }
}
