using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour {
    public Land[] AllyPosistion;
    public Land[] EnemyPosistion;
    public MonsterWave[] MonsterWaveList;
    public GameObject monsterSpawnPoint;
    public GameObject AllySpawnPoint;

    public List<GameObject> recentUnitList = new List<GameObject>();

    

    private int waveCount = 0;
    [System.Serializable]
    public struct MonsterWave
    {
        public GameObject[] monsterList;
    }
    // Use this for initialization
    void WaveStart()//약간의 시간차후 몬스터들의 등장애니메이션후 새로운 몬스터들 등장
    {
        for(int i = 0; i < EnemyPosistion.Length && i <MonsterWaveList[waveCount].monsterList.Length; i++)//맵의 몬스터 배치수만큼 몬스터 배치가능 그이상은 목록에있어도 무시됨
        {
            if (MonsterWaveList[waveCount].monsterList[i] == null)//배치보다 몬스터가 더 적은경우 남은빈칸은 무시됨 포지션을 위해 빈칸을 남겨둔경우 다음칸으로이동
                continue;
            Unit monster = Instantiate(MonsterWaveList[waveCount].monsterList[i]).GetComponent<Unit>();
            Vector3 Pos = Vector3.zero;
            Vector3 Scale = monster.transform.localScale;
            monster.transform.SetParent(monsterSpawnPoint.transform);
            monster.transform.localPosition = Pos;
            monster.transform.localScale = Scale;
            monster.MoveLand(EnemyPosistion[i]);
            monster.Faction = 1;
            monster.playable = false;
            monster.diedEvent += ()=> { CheckMonsterWave(monster); };
            recentUnitList.Add(monster.gameObject);
        }
    }
    void CheckMonsterWave(Unit _temp)
    {
        Debug.Log("유닛파괴");
        recentUnitList.Remove(_temp.gameObject);
        if(recentUnitList.Count <= 0)
        {
            Debug.Log("다음웨이브시작");
            //다음웨이브시작
        }
    }
    void Start ()
    {
        //아군유닛 소환
        for (int i = 0; i < SaveListManager.instance.playerdata.playerList.Count; i++)
        {
            if (SaveListManager.instance.playerdata.playerList[i].startPos == -1)//세이브파일 조회후 유닛이없는 배열은 패스 아니면 유닛 생성
                continue;
            Unit player = Instantiate(Resources.Load("Player\\" + SaveListManager.instance.playerdata.playerList[i].UnitName) as GameObject).GetComponent<Unit>();
            player.GetComponent<Player>().SetState(SaveListManager.instance.playerdata.playerList[i]);
            Vector3 Pos = Vector3.zero;
            Vector3 Scale = player.transform.localScale;
            player.transform.SetParent(AllySpawnPoint.transform);
            player.transform.localPosition = Pos;
            player.transform.localScale = Scale;
            player.MoveLand(AllyPosistion[SaveListManager.instance.playerdata.playerList[i].startPos]);
            player.Faction = 0;
            player.playable = true;
        }
        WaveStart();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
