using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Unit",menuName ="Units")]
[System.Serializable]
public class UnitState : ScriptableObject{
    public int d_strength;//기본스탯 : 힘
    public int d_intelligence;//기본스탯 : 지능
    public int d_speed;//기본스탯 : 공격속도
    public int d_defence;//기본스탯 : 방어력
    public int defaultHP;//기본체력

    public int strength;//레벨당스탯 : 힘
    public int intelligence;//레벨당스탯 : 지능
    public int speed;//레벨당스탯 : 공격속도
    public int defence;//레벨당스탯 : 방어력
    public int maxHp;//레벨당스탯 : HP

    public float maxStemina;

    public int faction;//같은팀 여부
    public int UnitEntry;

    public string UnitName;
    public int[] spellEntryList = new int[5];
    public Sprite idleMotion;
    public Sprite[] spellMotion;
    public Color frameColor = new Color(1.0f, 1.0f, 1.0f);

    public int Strength(int level)
    {
        return d_strength + (level * strength);
    }
    public int Intelligence(int level)
    {
        return d_intelligence + (level * intelligence);
    }
    public int Speed(int level)
    {
        return d_speed + (level * speed);
    }
    public int Defence(int level)
    {
        return d_defence + (level * defence);
    }
    public int MaxHP(int level)
    {
        return defaultHP + (level * maxHp);
    }
    //public int startPos;//던전시작시 포지션 디폴트 -1(어디에도 없음)
}
