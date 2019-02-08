using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonManager : MonoBehaviour {
    private bool dungeonSwitch = true;

    public Land[] AllyPosistion;
    public Land[] EnemyPosistion;
    public MonsterWave[] MonsterWaveList;
    public GameObject monsterSpawnPoint;
    public GameObject AllySpawnPoint;

    public ItemReward[] itemRewardList;

    public UIBase clearUI;
    public UIBase defeatUI;

    private List<GameObject> recentUnitList = new List<GameObject>();
    private List<GameObject> AllyUnitList = new List<GameObject>();


    public string bgmName;
    private int waveCount = 0;
    [System.Serializable]
    public struct MonsterWave
    {
        public GameObject[] monsterList;
    }
    [System.Serializable]
    public struct ItemReward
    {
        public string itemEntry;
        [Range(0.0f,1.0f)]
        public float itemRate;
    }
    // Use this for initialization
    void WaveStart()//약간의 시간차후 몬스터들의 등장애니메이션후 새로운 몬스터들 등장
    {
        for(int i = 0; i < EnemyPosistion.Length && i <MonsterWaveList[waveCount].monsterList.Length; i++)//맵의 몬스터 배치수만큼 몬스터 배치가능 그이상은 목록에있어도 무시됨
        {
            if (MonsterWaveList[waveCount].monsterList[i] == null)//배치보다 몬스터가 더 적은경우 남은빈칸은 무시됨 포지션을 위해 빈칸을 남겨둔경우 다음칸으로이동
                continue;
            Unit monster = Instantiate(MonsterWaveList[waveCount].monsterList[i].GetComponent<Unit>());
            Vector3 Pos = new Vector3(0.0f, 0.0f, -0.5f);
            Vector3 Scale = monster.transform.localScale;
            monster.transform.SetParent(monsterSpawnPoint.transform);
            monster.transform.localPosition = Pos;
            monster.transform.localScale = Scale;
            monster.MoveLand(EnemyPosistion[i]);
            monster.Faction = 1;
            monster.playable = false;
            monster.CharacterSpriteTurnRight(false);
            monster.diedEvent += ()=> { CheckMonsterWave(monster); };
            recentUnitList.Add(monster.gameObject);
        }
    }
    void CheckMonsterWave(Unit _temp)
    {
        recentUnitList.Remove(_temp.gameObject);
        CheckWaveCount();
    }
    void CheckAllyWave(Unit _temp)
    {
        AllyUnitList.Remove(_temp.gameObject);
        CheckWaveCount();
    }
    void CheckWaveCount()
    {
        if (!dungeonSwitch)
            return;
        if (recentUnitList.Count <= 0)
        {
            if(MonsterWaveList.Length <= waveCount)
            {
                //던전클리어
                if(clearUI)
                    clearUI.OpenUI();
                dungeonSwitch = false;
                Debug.Log("던전클리어");
            }
            else
            {
                Debug.Log("다음웨이브시작");
                //다음웨이브시작
                WaveStart();
                waveCount++;
            }
        }else if(AllyUnitList.Count <= 0)
        {
            dungeonSwitch = false;
            if (defeatUI)
                defeatUI.OpenUI();
            Debug.Log("던전실패 마을로 돌아가기");
        }
    }
    public void ReturnMainMenu()
    {
        EffectManager.instance.ResetAllList();
        SceneManager.LoadScene("MainScene");
    }
    void Start ()
    {
        Time.timeScale = 1;
        Debug.Log("타임" + Time.timeScale);
        dungeonSwitch = true;
        AudioManager.instance.PlayBGM(bgmName);
        //아군유닛 소환
        for (int i = 0; i < SaveListManager.instance.playerdata.playerList.Count; i++)
        {
            if (SaveListManager.instance.playerdata.playerList[i].unitPos == -1)//세이브파일 조회후 유닛이없는 배열은 패스 아니면 유닛 생성
                continue;
            Unit player = Instantiate(Resources.Load("PlayerModel\\UnitFrame") as GameObject).GetComponent<Unit>();
            player.SetState(CharacterManager.instance.ResearchUnit(SaveListManager.instance.playerdata.playerList[i].unitEntry));
            Vector3 Pos = new Vector3(0.0f,0.0f,-0.5f);
            Vector3 Scale = player.transform.localScale;
            player.transform.SetParent(AllySpawnPoint.transform);
            player.transform.localPosition = Pos;
            player.transform.localScale = Scale;
            for(int j = 0; j< 2; j++)
            {
                player.itemList[j] = ItemManager.instance.ItemResearch(SaveListManager.instance.playerdata.playerList[i].itemEntryList[j]);
            }
            player.MoveLand(AllyPosistion[SaveListManager.instance.playerdata.playerList[i].unitPos]);
            player.Faction = 0;
            player.diedEvent += () => { CheckAllyWave(player); };
            player.playable = true;
            AllyUnitList.Add(player.gameObject);
        }
        CheckWaveCount();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
