using System;
using System.IO;
using UnityEngine;

public class SaveBridgeFileManager : MonoBehaviour
{
    public static SaveBridgeFileManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
            return;
        Instance = this;
    }

    public bool Save(BridgeData data)
    {
        string dir = $"{Application.persistentDataPath}/{data.info.theme}/{data.info.stage}";
        string path = $"{Application.persistentDataPath}/{data.info.theme}/{data.info.stage}/{data.info.name}.txt";

        try
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            File.Create(path).Close();

            using (FileStream fs = File.OpenWrite(path))
            {
                fs.Write(BitConverter.GetBytes(data.pointDatas.Length));
                for (int i = 0; i < data.pointDatas.Length; i++)
                {
                    PointData point = data.pointDatas[i];
                    fs.Write(BitConverter.GetBytes(point.id));
                    fs.Write(BitConverter.GetBytes(point.position.x));
                    fs.Write(BitConverter.GetBytes(point.position.y));
                }
                fs.Write(BitConverter.GetBytes(data.blockDatas.Length));
                for (int i = 0; i < data.blockDatas.Length; i++)
                {
                    BlockData block = data.blockDatas[i];
                    fs.Write(BitConverter.GetBytes((int)block.type));
                    fs.Write(BitConverter.GetBytes(block.point1));
                    fs.Write(BitConverter.GetBytes(block.point2));
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
            return false;
        }
        return true;
    }
}
