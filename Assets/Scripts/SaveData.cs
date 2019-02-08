using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SaveData
{
    public bool newGame = true;
    public List<UnitData> playerList = new List<UnitData>();//전체 플레이어 리스트 0~3까지는 실제전투원
    public int[] itemList = new int[25];
    public int playerMoney;//게임머니
    public int lionCockes;//강화재료(캐쉬템)
    public int index;//세이브 슬롯
    [System.Serializable]
    public class UnitData
    {
        public int unitEntry;
        public int unitLevel;
        public int unitPos;
        public int[] itemEntryList = new int[2];
    }
    
}
