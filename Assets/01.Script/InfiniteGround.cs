using Unity.VisualScripting;
using UnityEngine;

public class InfiniteGround : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float tileSize = 20f;

    private readonly Transform[] tiles = new Transform[9];
    private Vector2 origin;
    private Vector2Int currentCell;
    private bool hasCell;

    private void Awake()
    {
        if(tileSize <= 0f || transform.childCount != tiles.Length)
        {
            Debug.LogError("바닥 크기는 양수, 자식 바닥이 9장 필요", this);

            enabled = false;
            return;
        }

        origin = new Vector2(transform.position.x, transform.position.y);

        for(int i = 0; i < tiles.Length; i++)
        {
            tiles[i] = transform.GetChild(i);
        }
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        Vector2Int nextCell = new Vector2Int(
            Mathf.FloorToInt((target.position.x - origin.x) / tileSize + 0.5f),
            Mathf.FloorToInt((target.position.y - origin.y) / tileSize + 0.5f)
        );
    

    //플레이어가 다른 구역으로 넘어갈 때만 재배치
        if(hasCell && nextCell == currentCell)
            return;

        currentCell = nextCell;
        hasCell = true;

        int index = 0;

        for (int y = -1; y <= 1; y++)
        { 
            for (int x = -1; x <= 1; x++) 
            { 
                Transform Tile = tiles[index++];

                Tile.position = new Vector3(
                    origin.x + (currentCell.x + x) * tileSize,
                    origin.y + (currentCell.y + y) * tileSize,
                    Tile.position.z
                    );
            }
        }
    }

}
