using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_BloodThirst : Spell {

    public override bool SpellEffect(params Land[] _target)
    {
        if (!CanReach(_target[0]) || _target[0] == spellCaster.MyLand)
            return false;
        base.SpellEffect(_target);
        spellCaster.StartAttackAnim(_target[0]);
        spellCaster.HealDamage(spellCaster, spellDamage());
        return true;
    }
    public override bool CanReach(Land _dest)
    {
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
