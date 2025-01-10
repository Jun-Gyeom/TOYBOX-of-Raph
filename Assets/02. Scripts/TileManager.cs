using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : Singleton<TileManager>
{
    public List<List<Tile>> GameMap { get; private set; }

    [SerializeField] private int sizeX = 10;
    [SerializeField] private int sizeY = 5;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Vector3 tileoffset;

    protected override void Awake()
    {
        Init();
    }

    private void Init()
    {
        GameMap = new List<List<Tile>>();
        // �� �������� 
        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                GameMap.Add(new List<Tile>());
                if (Instantiate(tilePrefab).TryGetComponent(out Tile tile))
                {
                    GameMap[y].Add(tile);
                }
                GameMap[y][x].Init(x, y);
                GameMap[y][x].transform.position = new Vector3(x, -y, 0) + tileoffset;
            }
        }
    }

    // Ÿ�� �Ӽ� ���� ( �ܼ� )
    public void SetTileType(int x, int y, TileType type, float startupTime, float holdingTime)
    {
        // 1. Ÿ�� �ִϸ����� �ӵ� ���� (����) 
        // 2. �ִϸ��̼� Ŭ������ �̺�Ʈ Ʈ���� ȣ�� 
        // 3. �̺�Ʈ Ʈ���� �Լ����� Ÿ�� Ȧ�� �ڷ�ƾ ȣ�� 

        GameMap[y][x].SetTileType(type, startupTime, holdingTime);
    }

    // Ÿ�� �Ӽ� ���� ( ���� )
    public void SetTileType(List<Vector2> pos, TileType type, float startupTime, float holdingTime)
    {
        for (int i = 0; i < pos.Count; i++)
        {
            GameMap[(int)pos[i].y][(int)pos[i].x].SetTileType(type, startupTime, holdingTime);
        }
    }

    // �÷��̾� �̵� ��� üũ 
    public Vector2 CheckMoveAble(Vector2 playerPos, Vector2 targetPos)
    {

        return new Vector2();
    }

    public Vector3 GetTileObejctPosition(Vector2 pos)
    {
        return GameMap[(int)pos.y][(int)pos.x].transform.position;
    }
}
