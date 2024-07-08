using UnityEngine;

[CreateAssetMenu(menuName = "SO/Stage/Info")]
public class StageInfoSO : ScriptableObject
{
    public string stageName;
    public int targetMoney = 10000;
    public Stage map;
}
