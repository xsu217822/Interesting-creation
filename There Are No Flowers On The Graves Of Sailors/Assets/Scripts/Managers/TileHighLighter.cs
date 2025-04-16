using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileHighLighter : MonoBehaviour
{
    [Header("��ͼ�����ͼ��")]
    public Tilemap baseTilemap;              // ��ͼͼ��
    public Tilemap highlightTilemap;         // ����ͼ�㣨������ͣ + ѡ����ʾ��

    [Header("������ʹ�õ� Tile")]
    public TileBase highlightTile;           // �����ͣ���ӵ� tile
    public TileBase selectedTile;            // �����ѡ�е� tile

    private Vector3Int? selectedCell = null;      // ��ǰѡ�еĸ���
    private Vector3Int? lastHoverCell = null;     // ��һ֡��ͣ����

    void Update()
    {
        // ͨ�������� Y=0 ƽ���󽻣��õ����������������ָ��
        Vector3 mouseWorldPos = GetMouseWorldPositionOnPlane(0f);  // Tilemap ���ڵ� Y ����
        Vector3Int cellPos = baseTilemap.WorldToCell(mouseWorldPos);

        // ���������߼�����������ƶ����¸���ʱ��
        if (cellPos != lastHoverCell)
        {
            // �����һ�����Ӹ�����ǰ�᲻�Ǳ�ѡ�еģ�
            if (lastHoverCell.HasValue && lastHoverCell != selectedCell)
                highlightTilemap.SetTile(lastHoverCell.Value, null);

            // ��������ͣ���ӵĸ�����ǰ�᲻�ǵ�ǰѡ�еģ�
            if (cellPos != selectedCell)
                highlightTilemap.SetTile(cellPos, highlightTile);

            lastHoverCell = cellPos;
        }

        // �����ѡ�и���
        if (Input.GetMouseButtonDown(0))
        {
            // ���֮ǰ��ѡ�и���
            if (selectedCell.HasValue)
                highlightTilemap.SetTile(selectedCell.Value, null);

            // ���õ�ǰ����Ϊѡ�и���
            highlightTilemap.SetTile(cellPos, selectedTile);
            selectedCell = cellPos;
        }
    }

    /// <summary>
    /// ��ȡ�����ָ��Yƽ���ϵ��������꣨����͸���������
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
