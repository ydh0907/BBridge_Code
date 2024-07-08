using System.Collections.Generic;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    public static PointManager Instance;

    public Point pointPrefab;
    public Transform pointParent;

    public List<Point> currentInstancePoint = new List<Point>();

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Is Multiple Manager!");
            Destroy(this);
        }
        Instance = this;
    }

    public Point MakePoint(Vector3 pos, int id = 0, bool isFixed = true)
    {
        Point point = Instantiate(pointPrefab, pointParent);
        point.transform.SetPositionAndRotation(pos, Quaternion.identity);
        point.tempID = id;
        point.rigid.isKinematic = isFixed;

        currentInstancePoint.Add(point);

        return point;
    }

    public void DeletePoint(Point point)
    {
        if (!currentInstancePoint.Contains(point)) return;

        currentInstancePoint.Remove(point);
        Destroy(point.gameObject);
    }

    public void DeleteAllPoint()
    {
        while (currentInstancePoint.Count > 0)
            DeletePoint(currentInstancePoint[0]);
        currentInstancePoint.Clear();
    }

    public void ActiveAll(bool active)
    {
        for (int i = 0; i < currentInstancePoint.Count; i++)
            currentInstancePoint[i].Active(active);
    }
}
