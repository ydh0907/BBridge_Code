using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Stage/Theme")]
public class ThemeSO : ScriptableObject
{
    public string themeName;
    public List<StageInfoSO> list;
}
