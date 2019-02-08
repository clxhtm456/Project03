using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Welber : ScriptedAI {

    public override void CalcSpellCoolTime(Spell _spell)
    {
        base.CalcSpellCoolTime(_spell);
        switch (_spell.spellEntry)
        {
            case 1://이동스킬
                {
                    Unit taget = MainTarget;
                    if (!taget)
                        return;
                    if (_spell.CanReach(taget.MyLand))
                    {
                        _spell.SpellCast(taget.MyLand);
                    }
                    else
                    {
                        _spell.SpellCast(MyLand.AstarAlg(taget.MyLand));
                    }
                }
                break;
            default:
                {
                    Unit taget = MainTarget;
                    if (!taget)
                        return;
                    if (_spell.CanReach(taget.MyLand))
                    {
                        _spell.SpellCast(taget.MyLand);
                    }
                }
                break;
        }
    }
}
