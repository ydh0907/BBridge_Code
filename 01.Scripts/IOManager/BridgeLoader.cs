using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BridgeLoader : MonoBehaviour
{
    public static BridgeLoader Instance { get; private set; }

    public int currentStage => StageManager.Instance.stage;

    private void Awake()
    {
        if (Instance != null)
            return;
        Instance = this;
    }

    public BridgeData BridgeToData(int theme, int stage, string name)
    {
        StageFileInfo fileInfo = new StageFileInfo() { theme = theme, stage = stage, name = name };
        PointData[] pointDatas = new PointData[PointManager.Instance.currentInstancePoint.Count];

        int blockCount = 0;
        foreach (BlockType type in Enum.GetValues(typeof(BlockType)))
            blockCount += BlockManager.Instance.currentInstanceBlock[type].Count;
        BlockData[] blockDatas = new BlockData[blockCount];

        for (int i = 0; i < pointDatas.Length; i++)
        {
            Point point = PointManager.Instance.currentInstancePoint[i];
            pointDatas[i] = new PointData();
            pointDatas[i].id = i;
            pointDatas[i].position = point.transform.position;
            point.tempID = i;
        }

        int offset = 0;
        foreach (BlockType type in Enum.GetValues(typeof(BlockType)))
        {
            int count = BlockManager.Instance.currentInstanceBlock[type].Count;
            for (int i = 0; i < count; i++)
            {
                ABlock block = BlockManager.Instance.currentInstanceBlock[type][i];
                blockDatas[offset + i] = new BlockData();
                blockDatas[offset + i].type = block.type;
                blockDatas[offset + i].point1 = block.connectedPoints.Count > 0 ? block.connectedPoints[0].tempID : -1;
                blockDatas[offset + i].point2 = block.connectedPoints.Count > 1 ? block.connectedPoints[1].tempID : -1;
            }
            offset += count;
        }

        BridgeData data = new BridgeData { info = fileInfo, pointDatas = pointDatas, blockDatas = blockDatas };
        return data;
    }

    public void DataToBridge(BridgeData data)
    {
        GameManager.Instance.GameReset(false);
        PointManager.Instance.DeleteAllPoint();
        BlockManager.Instance.DeleteAllBlock();

        PointData[] points = data.pointDatas;
        BlockData[] blocks = data.blockDatas;
        Dictionary<int, Point> shortCut = new Dictionary<int, Point>();

        List<Point> stagedPoint = StageManager.Instance.currentStage.stagedPoint;
        for (int i = 0; i < stagedPoint.Count; i++)
        {
            stagedPoint[i].connectedHinge.Clear();
            shortCut.Add(stagedPoint[i].tempID, stagedPoint[i]);
        }
        for (int i = 0; i < points.Length; i++)
        {
            Point point = PointManager.Instance.MakePoint(points[i].position, points[i].id);
            shortCut.Add(points[i].id, point);
        }
        for (int i = 0; i < blocks.Length; i++)
        {
            BlockData block = blocks[i];
            BlockManager.Instance.MakeBlock(shortCut[block.point1], shortCut[block.point2], block.type);
        }
    }

    public void SaveBridge(BridgeData data)
    {
        SaveBridgeFileManager.Instance.Save(data);
        SavedStageInfo.WriteStageInfo(data.info);
    }

    public BridgeData LoadBridge(StageFileInfo info)
    {
        return LoadBridgeFileManager.Instance.Load(info);
    }

    public void DeleteSavedBridge(int theme, int stage, string name)
    {
        string path = $"{Application.persistentDataPath}/{theme}/{stage}/{name}.txt";
        if (File.Exists(path))
            File.Delete(path);
        SavedStageInfo.RemoveStageInfo(new(theme, stage, name));
    }

    public void DeleteSavedBridge(StageFileInfo info)
    {
        string path = $"{Application.persistentDataPath}/{info.theme}/{info.stage}/{info.name}.txt";
        if (File.Exists(path))
            File.Delete(path);
        SavedStageInfo.RemoveStageInfo(info);
    }
}
