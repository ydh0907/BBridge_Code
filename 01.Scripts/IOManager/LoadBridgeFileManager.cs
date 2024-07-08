using System;
using System.IO;
using UnityEngine;

public class LoadBridgeFileManager : MonoBehaviour
{
    public static LoadBridgeFileManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
            return;
        Instance = this;
    }

    public BridgeData Load(StageFileInfo info)
    {
        string path = $"{Application.persistentDataPath}/{info.theme}/{info.stage}/{info.name}.txt";
        BridgeData data = new BridgeData() { info = info };

        byte[] buffer = new byte[12];

        try
        {
            if (!File.Exists(path))
            {
                Debug.LogError($"There is no file on {path}");
                BridgeLoader.Instance.DeleteSavedBridge(info);
                return data;
            }

            using (FileStream fs = File.OpenRead(path))
            {
                int length = 0;
                fs.Read(buffer, 0, 4);
                length = BitConverter.ToInt32(buffer, 0);
                data.pointDatas = new PointData[length];
                for (int i = 0; i < length; ++i)
                {
                    PointData point = new PointData();

                    fs.Read(buffer, 0, 12);
                    point.id = BitConverter.ToInt32(buffer, 0);
                    point.position.x = BitConverter.ToSingle(buffer, 4);
                    point.position.y = BitConverter.ToSingle(buffer, 8);

                    data.pointDatas[i] = point;
                }
                fs.Read(buffer, 0, 4);
                length = BitConverter.ToInt32(buffer, 0);
                data.blockDatas = new BlockData[length];
                for (int i = 0; i < length; ++i)
                {
                    BlockData block = new BlockData();

                    fs.Read(buffer, 0, 12);
                    block.type = (BlockType)BitConverter.ToInt32(buffer, 0);
                    block.point1 = BitConverter.ToInt32(buffer, 4);
                    block.point2 = BitConverter.ToInt32(buffer, 8);

                    data.blockDatas[i] = block;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            data.info.stage = -1;
        }

        return data;
    }
}
