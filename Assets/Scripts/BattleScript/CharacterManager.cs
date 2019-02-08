using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : Singleton<CharacterManager> {
    public List<UnitState> CharacterScriptList = new List<UnitState>();
    // Use this for initialization
    public UnitState ResearchUnit(int _entry)
    {
        foreach(UnitState unit in CharacterScriptList)
        {
            if (unit.UnitEntry == _entry)
                return unit;
        }
        return null;
    }
    //public void CharacterCreate()
    //{
    //    for (int i = 0; i < SaveListManager.instance.playerdata.playerList.Count; i++)
    //    {
    //        if (SaveListManager.instance.playerdata.playerList[i].UnitName == "")//세이브파일 조회후 유닛이없는 배열은 패스 아니면 유닛 생성
    //            continue;
    //        Unit player = Instantiate(Resources.Load("Player\\" + SaveListManager.instance.playerdata.playerList[i].UnitName) as GameObject).GetComponent<Unit>();
    //        player.GetComponent<Player>().SetState(SaveListManager.instance.playerdata.playerList[i]);
    //        Vector3 Pos = Vector3.zero;
    //        Vector3 Scale = player.transform.localScale;
    //        Scale.x *= -1.0f;
    //        player.transform.localPosition = Pos;
    //        player.transform.localScale = Scale;
    //        player.Faction = 0;
    //        player.tag = "Player";
    //        DontDestroyOnLoad(player.gameObject);
    //    }
    //}

    // Update is called once per frame
    void Update () {
		
	}
}
