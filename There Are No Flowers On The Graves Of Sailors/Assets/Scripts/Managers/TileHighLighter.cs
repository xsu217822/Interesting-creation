using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileHighLighter : MonoBehaviour
{
    [Header("地图与高亮图层")]
    public Tilemap baseTilemap;              // 地图图层
    public Tilemap highlightTilemap;         // 高亮图层（用于悬停 + 选中显示）

    [Header("高亮所使用的 Tile")]
    public TileBase highlightTile;           // 鼠标悬停格子的 tile
    public TileBase selectedTile;            // 鼠标点击选中的 tile

    private Vector3Int? selectedCell = null;      // 当前选中的格子
    private Vector3Int? lastHoverCell = null;     // 上一帧悬停格子

    void Update()
    {
        // 通过射线与 Y=0 平面求交，得到世界坐标下鼠标所指点
        Vector3 mouseWorldPos = GetMouseWorldPositionOnPlane(0f);  // Tilemap 所在的 Y 坐标
        Vector3Int cellPos = baseTilemap.WorldToCell(mouseWorldPos);

        // 高亮更新逻辑（仅当鼠标移动到新格子时）
        if (cellPos != lastHoverCell)
        {
            // 清除上一个格子高亮（前提不是被选中的）
            if (lastHoverCell.HasValue && lastHoverCell != selectedCell)
                highlightTilemap.SetTile(lastHoverCell.Value, null);

            // 设置新悬停格子的高亮（前提不是当前选中的）
            if (cellPos != selectedCell)
                highlightTilemap.SetTile(cellPos, highlightTile);

            lastHoverCell = cellPos;
        }

        // 鼠标点击选中格子
        if (Input.GetMouseButtonDown(0))
        {
            // 清除之前的选中高亮
            if (selectedCell.HasValue)
                highlightTilemap.SetTile(selectedCell.Value, null);

            // 设置当前格子为选中高亮
            highlightTilemap.SetTile(cellPos, selectedTile);
            selectedCell = cellPos;
        }
    }

    /// <summary>
    /// 获取鼠标在指定Y平面上的世界坐标（适配透视摄像机）
    /// </summary>
    Vector3 GetMouseWorldPositionOnPlane(float yLevel)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, new Vector3(0, yLevel, 0));

        if (plane.Raycast(ray, out float enter))
        {
            return ray.GetPoint(enter);
        }

        return Vector3.zero;
    }
}
