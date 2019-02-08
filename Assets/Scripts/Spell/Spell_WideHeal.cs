using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_WideHeal : Spell_NonTarget {
    public override bool SpellEffect(params Land[] _target)
    {
        spellCaster.StartSpellAnim(1);
        for (int i = 0; i < _target.Length; i++)
        {
            if (_target[i] == null || _target[i].LandUnit == null)
                continue;
            if (_target[i].LandUnit.Faction == spellCaster.Faction)
                spellCaster.HealDamage(_target[i].LandUnit, spellDamage());
        }
        return true;
    }

    public override bool CanReach(Land _dest)
    {
        if (_dest.DistLand(spellCaster.MyLand) < 2.2)
        {
            return true;
        }
        else
            return false;
    }
}
