using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BridgeMakeManager : MonoBehaviour
{
    public static BridgeMakeManager Instance { get; private set; }

    public Camera buildingCam;
    public LayerMask NoBuilding;
    public LayerMask pointLayer;
    public LayerMask blockLayer;
    public BlockType currentType = BlockType.Road;

    private bool isUI => EventSystem.current.IsPointerOverGameObject();

    #region Right Var

    #endregion

    #region Left Var
    private Point firstPoint;
    private Point secondPoint;
    private Vector3 startPos;
    private Vector3 endPos;
    private bool isStartWithUI = false;
    #endregion

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Is Multiple Manager!");
            Destroy(this);
        }
        Instance = this;
    }

    private void Update()
    {
        if (GameManager.Instance && !GameManager.Instance.isSimulate)
        {
            DetectLeftClick();
            DetectRightClick();
        }
    }

    public void ChangeType(int type)
    {
        currentType = (BlockType)type;
    }

    public void ChangeType(BlockType type)
    {
        currentType = type;
    }

    #region Right Click
    private void DetectRightClick()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RightMouseDown();
        }
    }
    private void RightMouseDown()
    {
        Vector3 worldPos = buildingCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = buildingCam.transform.forward;

        bool isHit = Physics.Raycast(worldPos, direction, out RaycastHit hit, 100, blockLayer);
        if (isHit)
        {
            ABlock block = hit.transform.GetComponentInParent<ABlock>();
            BlockManager.Instance.DeleteBlock(block.type, block);
        }
    }
    #endregion

    #region Left Click
    private void DetectLeftClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LeftMouseDown();
        }
        else if (Input.GetMouseButton(0))
        {
            LeftMouseStay();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            LeftMouseUp();
        }
    }
    private void LeftMouseDown()
    {
        if (isUI)
        {
            isStartWithUI = true;
            return;
        }
        firstPoint = null;
        secondPoint = null;

        Vector3 worldPos = buildingCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = buildingCam.transform.forward;

        bool isHit = Physics.Raycast(worldPos, direction, out RaycastHit hit, 100, pointLayer);
        if (isHit)
        {
            firstPoint = hit.transform.GetComponent<Point>();
            startPos = hit.transform.position;
        }
        else
        {
            startPos = new Vector3(worldPos.x, worldPos.y, 0);
        }
    }
    private void LeftMouseStay()
    {

    }
    private void LeftMouseUp()
    {
        if (isStartWithUI)
        {
            isStartWithUI = false;
            return;
        }

        Vector3 worldPos = buildingCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = buildingCam.transform.forward;

        bool isHit = Physics.Raycast(worldPos, direction, out RaycastHit hit, 100, pointLayer);
        if (isHit)
        {
            secondPoint = hit.transform.GetComponent<Point>();
            endPos = hit.transform.position;
        }
        else
        {
            endPos = new Vector3(worldPos.x, worldPos.y, 0);
        }

        if (Vector2.Distance(startPos, endPos) < 0.1f) return;

        if (!CanBuild(startPos, endPos, firstPoint, secondPoint))
            return;

        if (firstPoint == null)
            firstPoint = PointManager.Instance.MakePoint(startPos);
        if (secondPoint == null)
            secondPoint = PointManager.Instance.MakePoint(endPos);

        BlockManager.Instance.MakeBlock(firstPoint, secondPoint, currentType);
    }
    #endregion

    public bool CanBuild(Vector3 start, Vector3 end, Point first, Point second)
    {
        start.z = -10;
        bool able = true;

        if (able)
        {
            Vector3 position = start;
            Vector3 direction = end - start;
            able = !Physics.Raycast(position, direction, direction.magnitude, NoBuilding);
        }

        // check same place
        if (able)
        {
            if (first && second)
                foreach (List<ABlock> blocks in BlockManager.Instance.currentInstanceBlock.Values)
                    foreach (ABlock block in blocks)
                        if (block.connectedPoints.Contains(first) && block.connectedPoints.Contains(second))
                        {
                            BlockManager.Instance.DeleteBlock(block.type, block);
                            break;
                        }
        }

        return able;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(startPos, 0.25f);
        Gizmos.DrawWireSphere(endPos, 0.25f);
        Gizmos.color = Color.white;
    }
#endif
}
