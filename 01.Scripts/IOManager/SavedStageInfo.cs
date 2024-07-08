using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class SavedStageInfo
{
    public static List<StageFileInfo> savedStages = new();

    private static string path = $"{Application.persistentDataPath}/stageinfo.txt";

    public static bool WriteStageInfo(StageFileInfo info)
    {
        if (!File.Exists(path))
        {
            File.Create(path).Close();
        }

        try
        {
            using (StreamWriter sw = new StreamWriter(path, true, Encoding.UTF8))
            {
                sw.WriteLine($"{info.theme} {info.stage} {info.name}");
            }
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            return false;
        }
    }

    public static bool ReadAllStageInfo()
    {
        if (!File.Exists(path))
        {
            File.Create(path).Close();
        }

        try
        {
            savedStages.Clear();
            List<string> stages = new();
            using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
            {
                while (!sr.EndOfStream)
                    stages.Add(sr.ReadLine());
            }
            for (int i = 0; i < stages.Count; i++)
            {
                string[] infomation = stages[i].Split(' ');
                int theme = int.Parse(infomation[0]);
                int stage = int.Parse(infomation[1]);
                string name = infomation[2];
                savedStages.Add(new() { theme = theme, stage = stage, name = name });
            }
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            return false;
        }
    }

    public static bool RemoveStageInfo(StageFileInfo info)
    {
        if (!File.Exists(path))
        {
            File.Create(path).Close();
        }

        try
        {
            string target = $"{info.theme} {info.stage} {info.name}";
            List<string> stages = new();
            using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
            {
                while (!sr.EndOfStream)
                    stages.Add(sr.ReadLine());
            }
            if (stages.Remove(target))
            {
                File.Create(path).Close();
                using (StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8))
                {
                    for (int i = 0; i < stages.Count; i++)
                        sw.WriteLine(stages[i]);
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            return false;
        }
    }
}

[Serializable]
public struct StageFileInfo
{
    public int theme;
    public int stage;
    public string name;

    public StageFileInfo(int theme, int stage, string name)
    {
        this.theme = theme;
        this.stage = stage;
        this.name = name;
    }
}

[Serializable]
public struct BridgeData
{
    public StageFileInfo info;
    public PointData[] pointDatas;
    public BlockData[] blockDatas;
}

[Serializable]
public struct PointData
{
    public int id;
    public Vector2 position;
}

[Serializable]
public struct BlockData
{
    public BlockType type;
    public int point1;
    public int point2;
}