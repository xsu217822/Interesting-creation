using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{
    [Header("平面移动设置")]
    public float acceleration = 20f;
    public float drag = 5f;
    public float maxMoveSpeed = 10f;

    [Header("缩放设置")]
    public float zoomForce = 40f;
    public float zoomDrag = 8f;
    public float reboundStrength = 20f;
    public float minY = 5f;
    public float maxY = 40f;

    [Header("Tilemap 渐隐控制")]
    public Tilemap tilemapGrid;
    public float tileFadeStartY = 10f;
    public float tileFadeEndY = 25f;


    private Vector3 moveVelocity = Vector3.zero;
    private float zoomVelocity = 0f;

    private Vector3 zoomTargetDirection;
    private Vector3 lastMousePos;

    void Update()
    {
        HandleMovementInput();
        HandleZoomInput();
        HandleMiddleMousePan();

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        float currentY = transform.position.y;

        // 缩放方向 = 鼠标点向相机方向
        Vector3? mouseWorld = GetMouseWorldPositionOnPlane(Input.mousePosition);
        if (mouseWorld.HasValue)
        {
            zoomTargetDirection = (mouseWorld.Value - transform.position).normalized;
        }

        // 自动回弹逻辑
        if (Mathf.Approximately(scroll, 0f))
        {
            if (currentY < minY)
            {
                float overshoot = minY - currentY;
                zoomVelocity = Mathf.Lerp(zoomVelocity, -reboundStrength * overshoot, Time.deltaTime * zoomDrag);
            }
            else if (currentY > maxY)
            {
                float overshoot = currentY - maxY;
                zoomVelocity = Mathf.Lerp(zoomVelocity, reboundStrength * overshoot, Time.deltaTime * zoomDrag);
            }
        }

        // 应用缩放
        transform.position += zoomTargetDirection * zoomVelocity * Time.deltaTime;

        // 缩放速度惯性
        zoomVelocity = Mathf.Lerp(zoomVelocity, 0f, zoomDrag * Time.deltaTime);

        // 应用平面滑动
        transform.position += moveVelocity * Time.deltaTime;

        // 更新相机倾斜角度
        UpdateCameraTilt();

        // 更新 Tilemap 渐隐效果
        UpdateTilemapTransparency();

    }

    // 处理平面移动输入
    void HandleMovementInput()
    {
        Vector3 input = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            0f,
            Input.GetAxisRaw("Vertical")
        ).normalized;

        moveVelocity += input * acceleration * Time.deltaTime;
        moveVelocity = Vector3.Lerp(moveVelocity, Vector3.zero, drag * Time.deltaTime);
        moveVelocity = Vector3.ClampMagnitude(moveVelocity, maxMoveSpeed);
    }

    // 处理缩放输入
    void HandleZoomInput()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        float currentY = transform.position.y;

        if (scroll > 0f)
        {
            float intensity = Mathf.Clamp01((currentY - minY) / 5f);
            zoomVelocity += zoomForce * intensity;
        }

        if (scroll < 0f)
        {
            float intensity = Mathf.Clamp01((maxY - currentY) / 5f);
            zoomVelocity -= zoomForce * intensity;
        }
    }

    // 处理中键平移
    void HandleMiddleMousePan()
    {
        if (Input.GetMouseButtonDown(2))
        {
            lastMousePos = Input.mousePosition;
        }

        if (Input.GetMouseButton(2))
        {
            Vector3? currentWorld = GetMouseWorldPositionOnPlane(Input.mousePosition);
            Vector3? lastWorld = GetMouseWorldPositionOnPlane(lastMousePos);

            if (currentWorld.HasValue && lastWorld.HasValue)
            {
                Vector3 delta = lastWorld.Value - currentWorld.Value; // 注意方向
                transform.position += delta;
            }

            lastMousePos = Input.mousePosition;
        }
    }

    /// <summary>
    /// 获取给定屏幕坐标在 Y=0 平面上的世界坐标
    /// </summary>
    Vector3? GetMouseWorldPositionOnPlane(Vector3 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        Plane plane = new Plane(Vector3.up, Vector3.zero);

        if (plane.Raycast(ray, out float enter))
        {
            return ray.GetPoint(enter);
        }

        return null;
    }

    // 更新相机倾斜角度
    void UpdateCameraTilt()
    {
        float currentY = transform.position.y;
        float t = Mathf.InverseLerp(minY, maxY, currentY); // 0~1

        float tiltAngle = Mathf.Lerp(30f, 60f, t); // 近时 60，远时 30

        Vector3 rotation = transform.eulerAngles;
        rotation.x = tiltAngle;
        transform.eulerAngles = rotation;
    }

    // 更新 Tilemap 渐隐效果
    void UpdateTilemapTransparency()
    {
        if (tilemapGrid == null) return;

        float y = transform.position.y;
        float t = Mathf.InverseLerp(tileFadeStartY, tileFadeEndY, y); // y 从低到高 → 0 到 1

        float alpha = Mathf.Clamp01(t); // 低高度透明度 = 0，高度越高 → 1

        var renderer = tilemapGrid.GetComponent<TilemapRenderer>();
        if (renderer != null && renderer.material != null)
        {
            Color c = renderer.material.color;
            c.a = alpha;
            renderer.material.color = c;
        }
    }

}
