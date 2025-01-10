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
        // 맵 동적생성 
        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                GameMap.Add(new List<Tile>());
                if (Instantiate(tilePrefab).TryGetComponent(out Tile tile))
                {
                    GameMap[y].Add(tile);
                    tile.transform.parent = transform;
                }
                GameMap[y][x].Init(x, y);
                GameMap[y][x].transform.position = new Vector3(x, -y, 0) + tileoffset;
            }
        }
    }

    // 타일 속성 변경 ( 단수 )
    public void SetTileType(int x, int y, TileType type, float startupTime, float holdingTime)
    {
        // 1. 타일 애니메이터 속도 변경 (선딜) 
        // 2. 애니메이션 클립에서 이벤트 트리거 호출 
        // 3. 이벤트 트리거 함수에서 타일 홀딩 코루틴 호출 

        GameMap[y][x].SetTileType(type, startupTime, holdingTime);
    }

    // 타일 속성 변경 ( 복수 )
    public void SetTileType(List<Vector2> pos, TileType type, float startupTime, float holdingTime)
    {
        for (int i = 0; i < pos.Count; i++)
        {
            GameMap[(int)pos[i].y][(int)pos[i].x].SetTileType(type, startupTime, holdingTime);
        }
    }

    // 플레이어 이동 경로 체크 
    public Vector2 CheckMoveAble(Vector2 playerPos, Vector2 targetPos)
    {
        if (targetPos.x == -1 || targetPos.x >= sizeX || targetPos.y == -1 || targetPos.y >= sizeY)
            return playerPos;
        //Up Down 
        if (playerPos.x == targetPos.x)
        {
            // - = Down / + = Up
            int target_y = (int)(playerPos.y - targetPos.y);
            int currty_y = (int)playerPos.y;
            int offset = 0;
            if (target_y < 0)
                offset = 1;
            else
                offset = -1;


            for(int i = 0;i < Mathf.Abs(target_y);i++)
            {
                if (!GameMap[currty_y + (i * offset)][(int)playerPos.x].PlayerMoveAble)
                    return playerPos;
                return targetPos;
            }
        }
        else
        {
            // - = Right / + = Left
            int target_x = (int)(playerPos.x - targetPos.x);
            int currty_x = (int)playerPos.x;
            int offset = 0;
            if (target_x < 0)
                offset = 1;
            else
                offset = -1;


            for (int i = 0; i < Mathf.Abs(target_x); i++)
            {
                if (!GameMap[(int)playerPos.y][currty_x + (i * offset)].PlayerMoveAble)
                    return playerPos;
                return targetPos;
            }
        }
        return new Vector2();
    }

    public Vector3 GetTileObejctPosition(Vector2 pos)
    {
        return GameMap[(int)pos.y][(int)pos.x].transform.position;
    }
}
