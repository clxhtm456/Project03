using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_NonTarget : Spell {
    protected override void LateUpdate()
    {
    }
    override public void SpellActive()
    {
        Active = true;
    }
    override public void SpellDeActive()
    {
        Active = false;
    }

    public override bool SpellCast()
    {
        bool trigger1 = SpellAlarm();
        bool trigger2 = SpellParticle();
        int Count = Land.allLand.Count;
        Land[] allList = new Land[Count];
        int i = 0;
        foreach (Land l in Land.allLand)
        {
            if(CanReach(l))
            {
                allList[i] = l;
                i++;
            }
        }
        Land[] targetList;
        if (i > 0)
            targetList = new Land[i];
        else
            targetList = null;
        for(int l = 0; l< targetList.Length;l++)
        {
            targetList[l] = allList[l];
        }

        bool trigger3 = SpellEffect(targetList);
        if (trigger1 && trigger2 && trigger3)
        {
            spellCaster.SetStemina(-1.0f * steminaCost);
            spellReady = false;
            return true;
        }
        else
            return false;
    }

}
