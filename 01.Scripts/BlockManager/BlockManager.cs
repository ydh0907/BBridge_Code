using System.Collections.Generic;
using UnityEngine;

public enum BlockType
{
    Road,
    Wood,
    Steel,
    Rope
}

public class BlockManager : MonoBehaviour
{
    public static BlockManager Instance { get; private set; }

    public List<BlockType> typeList = new List<BlockType>();
    public List<ABlock> blockList = new List<ABlock>();
    public List<Transform> blockParent = new List<Transform>();

    public Dictionary<BlockType, ABlock> blocks = new();
    public Dictionary<BlockType, Transform> parents = new();
    public Dictionary<BlockType, List<ABlock>> currentInstanceBlock = new();
    public bool isBreak = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Is Multiple Manager!");
            Destroy(this);
        }
        Instance = this;

        for (int i = 0; i < typeList.Count; i++)
        {
            if (i >= blockList.Count || i >= blockParent.Count) break;

            blocks.Add(typeList[i], blockList[i]);
            parents.Add(typeList[i], blockParent[i]);
            currentInstanceBlock[typeList[i]] = new();
        }
    }

    public ABlock MakeBlock(Point start, Point end, BlockType type, bool active = false)
    {
        ABlock block = Instantiate(blocks[type], parents[type]);
        block.Init();

        Vector3 direction = (end.transform.position - start.transform.position).normalized;
        Vector3 position = (start.transform.position + end.transform.position) * 0.5f;
        Quaternion rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

        block.transform.SetPositionAndRotation(position, rotation);
        block.type = type;

        start.Connect(block);
        end.Connect(block);
        block.Connect();
        block.Active(active);

        currentInstanceBlock[type].Add(block);
        
        MoneyManager.Instance.IncMoney(block.cost);

        return block;
    }

    public void DeleteBlock(BlockType type, ABlock block)
    {
        currentInstanceBlock[type].Remove(block);
        foreach (Point point in block.connectedPoints)
            point.Disconnect(block);

        // bond
        MoneyManager.Instance.DecMoney(block.cost);

        Destroy(block.gameObject);
    }

    public void DeleteAllBlock()
    {
        foreach (BlockType type in currentInstanceBlock.Keys)
        {
            List<ABlock> list = currentInstanceBlock[type];
            while (list.Count > 0)
                DeleteBlock(type, list[0]);
            list.Clear();
        }
    }

    public void ActiveAll(bool active)
    {
        isBreak = false;
        foreach (List<ABlock> list in currentInstanceBlock.Values)
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].Active(active);
            }
        }
    }
}
