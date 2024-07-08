using System;
using System.IO;
using UnityEngine;

public class ClearInfoManager
{
    public static bool SaveClearInfo(int theme, int stage, bool clear, bool money, bool isBreak)
    {
        string dir = $"{Application.persistentDataPath}/{theme}/{stage}";
        string path = $"{Application.persistentDataPath}/{theme}/{stage}/clearInfo.txt";

        byte[] buffer = new byte[3];
        Array.Copy(BitConverter.GetBytes(clear), 0, buffer, 0, 1);
        Array.Copy(BitConverter.GetBytes(money), 0, buffer, 1, 1);
        Array.Copy(BitConverter.GetBytes(isBreak), 0, buffer, 2, 1);
        try
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            File.Create(path).Close();
            using (FileStream fs = File.OpenWrite(path))
            {
                fs.Write(buffer, 0, 3);
            }
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            return false;
        }
    }

    public static (bool, bool, bool, bool) ReadClearInfo(int theme, int stage)
    {
        string path = $"{Application.persistentDataPath}/{theme}/{stage}/clearInfo.txt";
        if (!File.Exists(path))
            return (true, false, false, false);

        byte[] buffer = new byte[3];
        try
        {
            using (FileStream fs = File.OpenRead(path))
            {
                fs.Read(buffer, 0, 3);
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            return (false, false, false, false);
        }
        bool clear = BitConverter.ToBoolean(buffer, 0);
        bool money = BitConverter.ToBoolean(buffer, 1);
        bool isBreak = BitConverter.ToBoolean(buffer, 2);
        return (true, clear, money, isBreak);
    }
}
