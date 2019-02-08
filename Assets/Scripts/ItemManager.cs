using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager> {

    public List<Item> itemDBList = new List<Item>();

    // Use this for initialization
    override public void Awake()
    {
        base.Awake();
        //spellscript 시작구문 추가

        //
        //LoadingSpellTable();
        //ShowLogSpellList();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public Item ItemResearch(int _entry)
    {
        for (int i = 0; i < itemDBList.Count; i++)
        {
            if (itemDBList[i].itemEntry == _entry)
            {
                return itemDBList[i];
            }
        }
        return null;
    }
}
