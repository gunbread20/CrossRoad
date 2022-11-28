using System.Collections.Generic;
using UnityEngine;

public class Grass : Floor
{

    [SerializeField]
    PoolObjectType objectType;

    List<GameObject> trees = new List<GameObject>();

    float[] floorPosArr = new float[] { -15, -14, -13, -12, -11, 11, 12, 13 };

    public override void Generate()
    {
        CreateTrees();
    }

    public override void Reset()
    {
        ReturnTrees();

        ObjectPool.Instance.ReturnObject(objectType, gameObject);
    }

    void CreateTrees()
    {
        int random = Random.Range(3, 6);

        random += floorPosArr.Length;

        for (int i = 0; i < random; i++)
        {
            GameObject tree = ObjectPool.Instance.GetObject((PoolObjectType)Random.Range(3, 6));

            tree.transform.SetParent(transform, true);
            tree.transform.position = i < floorPosArr.Length ? new Vector3(floorPosArr[i], 0, transform.position.z) : GetRandomTreePos();

            trees.Add(tree);
        }
    }

    void ReturnTrees()
    {
        for(int i = 0; i < trees.Count; i++)
        {
            switch (trees[i].tag)
            {
                case "Tree_Type0":
                    ObjectPool.Instance.ReturnObject(PoolObjectType.Tree_Type0, trees[i]);
                    break;
                case "Tree_Type1":
                    ObjectPool.Instance.ReturnObject(PoolObjectType.Tree_Type1, trees[i]);
                    break;
                case "Tree_Type2":
                    ObjectPool.Instance.ReturnObject(PoolObjectType.Tree_Type2, trees[i]);
                    break;
                default:
                    break;
            }
        }

        trees.Clear();
    }

    Vector3 GetRandomTreePos()
    {
        Vector3 pos;

        while (true)
        {
            pos = new Vector3(Random.Range(-10, 11), 0, transform.position.z);

            if (pos.x == 0 && pos.z == 0)
                continue;

            if (isOverlap(pos))
                continue;

            return pos;
        }
    }

    bool isOverlap(Vector3 pos)
    {
        for (int i = 0; i < trees.Count; i++)
            if (Mathf.Approximately(trees[i].transform.position.x, pos.x))
                return true;

        return false;
    }

    public override void CreateCoin()
    {
        coins.Add(ObjectPool.Instance.GetObject(
            PoolObjectType.Coin));

        coins[coins.Count - 1].transform.SetParent(transform, true);
        coins[coins.Count - 1].transform.position = GetRandomTreePos();
    }

    void ReturnCoins()
    {
       for(int i = 0; i < coins.Count; i++)
        {
            ObjectPool.Instance.ReturnObject(
                PoolObjectType.Coin, coins[i]);
        }

        coins.Clear();
    }
}