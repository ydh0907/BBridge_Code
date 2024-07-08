using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Stage : MonoBehaviour
{
    public CinemachineVirtualCamera view3D;
    public Transform stagedPointTrm;
    public Transform stagedCarTrm;
    public Transform stagedFlagTrm;
    [HideInInspector] public List<Point> stagedPoint = new();
    [HideInInspector] public List<Car> stagedCar = new();
    [HideInInspector] public List<GoalFlag> stagedFlag = new();
    [SerializeField] public readonly float phaseTime = 5f;
    public List<UnityEvent> phaseList = new();
    private Coroutine currentExecution;

    private void Awake()
    {
        stagedPoint = stagedPointTrm.GetComponentsInChildren<Point>().ToList();
        stagedCar = stagedCarTrm.GetComponentsInChildren<Car>().ToList();
        stagedFlag = stagedFlagTrm.GetComponentsInChildren<GoalFlag>().ToList();

        for (int i = 0; i < stagedPoint.Count; i++)
            stagedPoint[i].tempID = -(i + 1);
        for (int i = 0; i < stagedFlag.Count; i++)
            stagedFlag[i].stage = this;
    }

    public void SetStart()
    {
        currentExecution = StartCoroutine(Execute());
    }

    public IEnumerator Execute()
    {
        WaitForSeconds wait = new WaitForSeconds(phaseTime);
        for (int i = 0; i < phaseList.Count; i++)
        {
            phaseList[i]?.Invoke();
            yield return wait;
        }
    }

    public void Goal()
    {
        bool all = true;
        for (int i = 0; i < stagedFlag.Count; i++)
            all &= stagedFlag[i].goal;

        if (all)
            GameManager.Instance.GameEnd();
    }

    public void Reset()
    {
        if (currentExecution != null)
            StopCoroutine(currentExecution);
        for (int i = 0; i < stagedCar.Count; i++)
            stagedCar[i].Reset();
        for (int i = 0; i < stagedFlag.Count; i++)
            stagedFlag[i].goal = false;
    }
}
