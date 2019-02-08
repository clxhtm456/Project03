using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : Singleton<SpellManager> {
    public List<Spell> spellDBList = new List<Spell>();
    private List<Spell> spellScriptList = new List<Spell>();
    public List<Dictionary<string, object>> spellTable;

    // Use this for initialization
    override public void Awake () {
        base.Awake();
        //spellscript 시작구문 추가

        //
        //LoadingSpellTable();
        //ShowLogSpellList();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public Spell SpellResearch(int _entry)
    {
        for(int i = 0; i < spellDBList.Count;i++)
        {
            if (spellDBList[i].spellEntry == _entry)
            {
                return spellDBList[i];
            }
        }
        return null;
    }

    //public void LoadingSpellTable()
    //{
    //    try
    //    {
    //        spellTable = CSVReader.Read("SpellTable");
    //        Debug.Log(spellTable.Count+"개 테이블읽는중");
    //        for (int i = 0; i < spellTable.Count; i++)
    //        {
    //            Spell tempSpell = new Spell();
    //            tempSpell.spellEntry = int.Parse(spellTable[i]["Spell_Entry"].ToString());
    //            tempSpell.spellName = spellTable[i]["Spell_Name"].ToString();
    //            tempSpell.spellDistance = int.Parse(spellTable[i]["Distance"].ToString());
    //            tempSpell.spellType = int.Parse(spellTable[i]["Spell_Type"].ToString());
    //            float.TryParse(spellTable[i]["Value_1"].ToString(), out tempSpell.value1);
    //            float.TryParse(spellTable[i]["Value_2"].ToString(), out tempSpell.value2);
    //            float.TryParse(spellTable[i]["Value_3"].ToString(), out tempSpell.value3);
    //            float.TryParse(spellTable[i]["Value_4"].ToString(), out tempSpell.value4);
    //            float.TryParse(spellTable[i]["CoolTime"].ToString(), out tempSpell.coolTime);
    //            tempSpell.spellScript = spellTable[i]["Spell_Script"].ToString();
    //            spellDBList.Add(tempSpell);
    //        }
    //    }
    //    catch
    //    {
    //        Debug.Log("SpellTable 읽기실패");
    //    }
    //}
    //public void ShowLogSpellList()
    //{
    //    for (int i = 0; i < spellDBList.Count; i++)
    //    {
    //        Debug.Log(
    //        spellDBList[i].spellEntry+
    //        spellDBList[i].spellName+
    //        spellDBList[i].spellDistance+
    //        spellDBList[i].spellType+
    //        spellDBList[i].value1+
    //        spellDBList[i].value2+
    //        spellDBList[i].value3+
    //        spellDBList[i].value4+
    //        spellDBList[i].coolTime+
    //        spellDBList[i].spellScript
    //        );
    //    }
    //}
}
