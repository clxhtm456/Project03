using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Land : MonoBehaviour {
    [SerializeField]
    private int xPos = 0;
    [SerializeField]
    private int yPos = 0;
    [HideInInspector]
    public float gDist;//A*알고리즘을 위함
    [HideInInspector]
    public float fDist;//A*알고리즘을 위함
    [HideInInspector]
    public float hDist;//A*알고리즘을 위함
    private Land nextLand;//A*알고리즘을 위함
    [HideInInspector]
    public List<Land> pathFind = new List<Land>();
    [HideInInspector]
    public Land[] nearLand = new Land[8];
    [HideInInspector]
    static public List<Land> allLand = new List<Land>();
    static public Land LandbyPos(int _xPos, int _yPos)
    {
        for(int i = 0; i < allLand.Count;i++)
        {
            if (allLand[i].Xpos == _xPos && allLand[i].Ypos == _yPos)
                return allLand[i];
        }
        return null;
    }
    Unit landUnit = null;
    public Unit LandUnit
    {
        get
        {
            if (landUnit != null &&  transform.childCount > 0 && transform.GetChild(0) == landUnit.gameObject)
                return landUnit;
            else if (transform.childCount > 0)
            {
                landUnit = GetComponentInChildren<Unit>();
                return landUnit;
            }
            else
            {
                landUnit = null;
                return null;
            }
        }
    }
    public Land NearLand(int _xPos, int _yPos)
    {
        var destX = Xpos + _xPos;
        var destY = Ypos + _yPos;
        return LandbyPos(destX,destY);
    }
    public void LandActive()
    {
        GetComponentInChildren<SpriteRenderer>().color = new Color(0.16f,1.0f,0.07f,0.3f);
    }
    public void LandDeActive()
    {
        GetComponentInChildren<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.06f,0.3f);
    }
    public void LandNormalize()
    {
        GetComponentInChildren<SpriteRenderer>().color = new Color(1.0f,1.0f,1.0f,0.3f);
    }
    public int Xpos
    {
        get { return xPos; }
    }
    public int Ypos
    {
        get { return yPos; }
    }
    void SearchNearLand()//주변배열 최대 8개 저장
    {
        Land[] totalLand = FindObjectsOfType<Land>();
        int count = 0;
        foreach(Land temp in totalLand)
        {
            if(temp != this && DistLand(temp) <= 1)
            {
                nearLand[count] = temp;
                count++;
            }
        }
    }
    public float DistLand(Land _dest)
    {
        if (_dest == null)
            return 0;
        return Mathf.Sqrt(Mathf.Pow((_dest.Xpos - Xpos), 2) + Mathf.Pow((_dest.Ypos - Ypos), 2));
    }
    private void AddNearTile(Land _temp, Land _destination, List<Land> _landList,List<Land> _closeList)
    {
        for (int i = 0; i<_temp.nearLand.Length; i++)
        {
            if (_temp.nearLand[i] == null)
                break;
            if (_closeList.Contains(_temp.nearLand[i]))
                continue;
            if (_temp.nearLand[i] != _destination && _temp.nearLand[i].GetComponentsInChildren<Unit>().Length > 0)//위치에 유닛이 있는경우 갈수없는블록
                continue;
            if (!_landList.Contains(_temp.nearLand[i]))
            {
                _temp.nearLand[i].gDist = _temp.gDist + _temp.DistLand(_temp.nearLand[i]);
                _temp.nearLand[i].hDist = _temp.nearLand[i].DistLand(_destination);
                _temp.nearLand[i].fDist = _temp.nearLand[i].gDist + _temp.nearLand[i].hDist;

                _temp.nearLand[i].nextLand = _temp;
                _landList.Add(_temp.nearLand[i]);
            }else if(_temp.nearLand[i].gDist > _temp.gDist+1)
            {
                _temp.nearLand[i].gDist = _temp.gDist + _temp.DistLand(_temp.nearLand[i]);
                _temp.nearLand[i].fDist = _temp.nearLand[i].gDist + _temp.nearLand[i].hDist;
                _temp.nearLand[i].nextLand = _temp;
            }
        }
    }
    public Land AstarAlg(Land _dest)//A*알고리즘으로 최단경로찾기
    {
        if (_dest == this)
            return this;
        List<Land> landList = new List<Land>();
        List<Land> closeList = new List<Land>();

        landList.Add(this);
        gDist = 0.0f;
        fDist = 0.0f;
        while (landList.Count > 0)
        {
            Land temp = landList[0];

            for (int i = 0; i<landList.Count; i++)
            {
                if (landList[i].fDist < temp.fDist)
                    temp = landList[i];
            }
            if (temp == _dest)
            {
                break;
            }
            landList.Remove(temp);
            closeList.Add(temp);
            AddNearTile(temp, _dest, landList, closeList);
        }
        FindPath(_dest);
        return pathFind[0];
    }

    private void FindPath(Land _dest)
    {
        Land tile = _dest;

        while (tile != null)
        {
            pathFind.Add(tile);

            if (tile.nextLand.Equals(this))
                break;

            tile = tile.nextLand;
        }
        pathFind.Reverse();
    }
    // Use this for initialization
    private void Awake()
    {
        
    }
    void Start () {
        SearchNearLand();
        allLand.Add(this);
    }
    private void OnDestroy()
    {
        allLand.Remove(this);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
