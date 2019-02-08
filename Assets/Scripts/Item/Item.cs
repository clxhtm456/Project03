using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
[System.Serializable]
public class Item : ScriptableObject {
    public int itemEntry;
    public Sprite itemIcon;
    public string itemName;
    public int itemRarety;

    public int value1;
    public int value2;
    public int value3;
    public int value4;

    public int sellingCost;

    public string itemContext;

    public Spell activeSpell;
    public Unit ownerUnit;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public string itemOption
    {
        get
        {
            string context = null;
            if (value1 != 0)
                context += "힘 +" + value1.ToString() + "\n";
            if (value2 != 0)
                context += "지능 +" + value2.ToString() + "\n";
            if (value3 != 0)
                context += "속도 +" + value3.ToString() + "\n";
            if (value4 != 0)
                context += "체력 +" + value4.ToString() + "\n";

            context += "\n판매가격 "+ "<color=#FFEE60>" + sellingCost.ToString() + "Gold"+"</color>"+"\n";
            return context;
        }
    }
    public Sprite itemIconSprite()
    {
        return itemIcon;
    }
}
