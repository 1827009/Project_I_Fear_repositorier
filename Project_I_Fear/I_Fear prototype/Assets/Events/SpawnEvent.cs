using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEvent : MonoBehaviour
{
    [SerializeField, TooltipAttribute("出現させる位置をまとめた親オブジェクト")] GameObject spawnPoints;
    [SerializeField, TooltipAttribute("出現させるプレハブ、個数")] List<SpawnData> spawnPrefab;
    GameObject player = null;
    [SerializeField, TooltipAttribute("巡回ルート")]  GameObject[] movePoint = null;
    [SerializeField, TooltipAttribute("カメラ")] new Clear2 camera;
    [SerializeField, TooltipAttribute("太陽")]  new GameObject light;
    [SerializeField, TooltipAttribute("鈴での挙動")]  int ver;

    Monster_Script monster;
    int x = 0;
    int count;
    bool RsponeCount = false;

    // Start is called before the first frame update
    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player");
        RandomSpawn();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (monster.Rstart)
        {
            if (8 < movePoint.Length)
            {
                for (int h = 0; h < 2; h++)
                {
                    RandomSpawn();
                }
            }
            else
            {
                RandomSpawn();
            }
        }
       
    }

    void RandomSpawn()
    {
        // 出現位置取得
        List<Vector3> points = new List<Vector3>();
        for (int i = 0; i < spawnPoints.transform.childCount; i++)
        {
            points.Add(spawnPoints.transform.GetChild(i).localPosition + spawnPoints.transform.position);
        }

        // 出現
        foreach(SpawnData data in spawnPrefab){
            List<GameObject> instance = new List<GameObject>();
            if (data.prefab.gameObject.tag == "Monster" || !RsponeCount)
            {
                for (int i = 0; i < data.count; i++)
                {
                    instance.Add(Instantiate(data.prefab));
                    if (instance[i].gameObject.tag == "Monster")
                    {
                        
                        monster = instance[i].GetComponent<Monster_Script>();
                        monster.player = player;
                        monster.camera = camera;
                        monster.light = light;
                        monster.BellMove = ver;
                        ver = 0;
                        switch (movePoint.Length)
                        {
                            case 8:
                                MoveS1();
                                break;
                            case 13:
                                MoveS2();
                                break;

                        }
                        

                    }
                    if(instance[i].gameObject.tag == "Bell")
                    {
                        RsponeCount = true;
                    }

                    if (points.Count > 0)
                    {
                        int p;
                        if (instance[i].gameObject.tag == "Bell")
                        {
                             p = Random.Range(0, points.Count);
                        }
                        else
                        {
                             p = count;
                            switch (count)
                            {
                                case 0:
                                    count = 7;
                                    break;
                                case 7:
                                    count = 0;
                                    break;

                            }
                        }
                        instance[i].transform.position = points[p];
                        points.RemoveAt(p);
                    }
                    else
                    {
                        instance[i].transform.position = points[Random.Range(0, spawnPoints.transform.childCount)];
                    }
                }
            }
        }

    }

    private void MoveS1()
    {
        switch (x)
        {
            case 0:
                monster.movePoint[0] = movePoint[0];
                monster.movePoint[1] = movePoint[1];
                monster.movePoint[2] = movePoint[2];
                monster.movePoint[3] = movePoint[3];
                x = 1;
                break;
            case 1:
                monster.movePoint[0] = movePoint[4];
                monster.movePoint[1] = movePoint[5];
                monster.movePoint[2] = movePoint[6];
                monster.movePoint[3] = movePoint[7];
                x = 0;
                break;
        }
    }
    private void MoveS2()
    {
        switch (x)
        {
            case 0:
                monster.movePoint[0] = movePoint[0];
                monster.movePoint[1] = movePoint[1];
                monster.movePoint[2] = movePoint[2];
                monster.movePoint[3] = movePoint[3];
                monster.movePoint[4] = movePoint[4];
                monster.movePoint[5] = movePoint[5];
                monster.movePoint[6] = movePoint[2];
                monster.movePoint[7] = movePoint[3];
                monster.movePoint[8] = movePoint[0];

                monster.escapePoint[0] = movePoint[1];
                monster.escapePoint[1] = movePoint[2];
                monster.escapePoint[2] = movePoint[5];
                monster.escapePoint[3] = movePoint[6];
                monster.escapePoint[4] = movePoint[11];
                monster.escapePoint[5] = movePoint[12];
                x = 1;
                break;
            case 1:
                monster.movePoint[0] = movePoint[11];
                monster.movePoint[1] = movePoint[6];
                monster.movePoint[2] = movePoint[7];
                monster.movePoint[3] = movePoint[8];
                monster.movePoint[4] = movePoint[9];
                monster.movePoint[5] = movePoint[10];
                monster.movePoint[6] = movePoint[7];
                monster.movePoint[7] = movePoint[8];
                monster.movePoint[8] = movePoint[11];

                monster.escapePoint[0]= movePoint[1];
                monster.escapePoint[1] = movePoint[2];
                monster.escapePoint[2] = movePoint[5];
                monster.escapePoint[3] = movePoint[6];
                monster.escapePoint[4] = movePoint[11];
                monster.escapePoint[5] = movePoint[12];
                x = 0;
                break;
        }
    }
}

[System.Serializable]
public class SpawnData
{
    [SerializeField, TooltipAttribute("出現させるプレハブ")] public GameObject prefab;
    [SerializeField, TooltipAttribute("出現させる数")] public int count;
}