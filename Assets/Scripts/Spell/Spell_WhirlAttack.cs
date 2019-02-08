using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_WhirlAttack : Spell_NonTarget {
    // Use this for initialization
    
    public override bool SpellEffect(params Land[] _target)
    {
        spellCaster.StartSpellAnim();
        for (int i = 0; i < _target.Length; i++)
        {
            if (_target[i] == null || _target[i].LandUnit == null)
                continue;
            if (_target[i].LandUnit.Faction != spellCaster.Faction)
                spellCaster.DealDamage(_target[i].LandUnit, spellDamage());
        }
        return true;
    }
    
    public override bool CanReach(Land _dest)
    {
        if (spellCaster.MyLand == _dest)
            return true;
        if ((Mathf.Abs(_dest.Xpos - spellCaster.MyLand.Xpos) == 1 &&
            _dest.Ypos - spellCaster.MyLand.Ypos == 0) ||
            (Mathf.Abs(_dest.Ypos - spellCaster.MyLand.Ypos) == 1 &&
            _dest.Xpos - spellCaster.MyLand.Xpos == 0))
        {
            return true;
        }
        else
            return false;
    }
}
