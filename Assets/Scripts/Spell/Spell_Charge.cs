using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_Charge : Spell {
    public override bool SpellEffect(params Land[] _target)
    {
        if (!CanReach(_target[0]) || _target[0] == spellCaster.MyLand)
            return false;
        Land temp = spellCaster.MyLand;
        int count = 0;
        while (temp != _target[0] && count <5)
        {
            spellCaster.Knockback(temp);
            count++;
            if(temp.Xpos > _target[0].Xpos)
                temp = temp.NearLand(-1,0);
            else if (temp.Xpos < _target[0].Xpos)
                temp = temp.NearLand(1, 0);
        }

        if (temp.LandUnit != null)
        {
            if (temp.LandUnit.Faction != spellCaster.Faction)
                base.SpellEffect(temp);
        }
        spellCaster.Knockback(temp);


        spellCaster.StartAttackAnim(temp);
        return true;
    }
    public override bool CanReach(Land _dest)
    {
        if ((Mathf.Abs(_dest.Xpos - spellCaster.MyLand.Xpos) <= 2 &&
            _dest.Ypos - spellCaster.MyLand.Ypos == 0))
        {
            return true;
        }
        else
            return false;
    }
}
