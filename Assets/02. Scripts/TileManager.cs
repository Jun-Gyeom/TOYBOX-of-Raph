using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TileManager : Singleton<TileManager>
{
    public List<List<Tile>> GameMap { get; private set; }

    [SerializeField] private int sizeX = 10;
    [SerializeField] private int sizeY = 5;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Vector3 tileoffset;
    //���ǰ��� ����
    [SerializeField] private Vector2 ScappingSize;

    [Header("Debug")]
    [Tooltip("Editor�󿡼� �ٷ� Ȯ���� Ȱ��ȭ")]
    [SerializeField] private bool OnDrawTile;

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
                    tile.transform.parent = transform;
                }
                GameMap[y][x].Init(x, y);
                GameMap[y][x].transform.position = new Vector3(ScappingSize.x*x, ScappingSize.y *(-y), 0) + tileoffset;
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
        GameManager.Instance.Player.Interactor();
    }

    // Ÿ�� �Ӽ� ���� ( ���� )
    public void SetTileType(List<Vector2> pos, TileType type, float startupTime, float holdingTime)
    {
        for (int i = 0; i < pos.Count; i++)
        {
            GameMap[(int)pos[i].y][(int)pos[i].x].SetTileType(type, startupTime, holdingTime);
        }
        GameManager.Instance.Player.Interactor();
    }

    // �÷��̾� �̵� ��� üũ 
    public Vector2 CheckMoveAble(Vector2 playerPos, Vector2 targetPos)
    {

        //Up Down 
        if (playerPos.x == targetPos.x)
        {
            if (targetPos.y < 0)
                targetPos.y = 0;
            else if (targetPos.y >= sizeY)
                targetPos.y = sizeY - 1;
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
                Debug.Log(GameMap[currty_y + ((i + 1) * offset)][(int)playerPos.x].PlayerMoveAble + "_" + playerPos+"_" + targetPos);
                if (!GameMap[currty_y + ((i+1) * offset)][(int)playerPos.x].PlayerMoveAble)
                    return new Vector2(playerPos.x,currty_y +(i*offset));
            }
        }
        else
        {
            if (targetPos.x < 0)
                targetPos.x = 0;
            else if (targetPos.x >= sizeX)
                targetPos.x = sizeX - 1;
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
                if (!GameMap[(int)playerPos.y][currty_x + ((i+1) * offset)].PlayerMoveAble)
                    return new Vector2(currty_x +(i * offset),playerPos.y);
            }
        }
        return targetPos;
    }

    public Vector3 GetTileObejctPosition(Vector2 pos)
    {
        return GameMap[(int)pos.y][(int)pos.x].transform.position;
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (tilePrefab == null || !OnDrawTile) return;

        // Sprite ��������
        SpriteRenderer spriteRenderer = tilePrefab.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null || spriteRenderer.sprite == null) return;

        Sprite sprite = spriteRenderer.sprite;
        Texture2D texture = sprite.texture;

        // Ÿ�� ��ġ�� Sprite�� �׸���
        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                Vector3 tilePosition = new Vector3(ScappingSize.x * x, ScappingSize.y * (-y), 0) + tileoffset;

                // Sprite�� �簢������ Scene �信 �׸���
                DrawSprite(sprite, tilePosition, spriteRenderer.color);
            }
        }
    }

    private void DrawSprite(Sprite sprite, Vector3 position, Color color)
    {
        if (sprite == null) return;

        // Sprite�� ũ��� UV ��ǥ ���
        Vector2 spriteSize = sprite.bounds.size;
        Rect textureRect = sprite.textureRect;
        textureRect.x /= sprite.texture.width;
        textureRect.y /= sprite.texture.height;
        textureRect.width /= sprite.texture.width;
        textureRect.height /= sprite.texture.height;

        // Position�� Pixel Perfect�� ���� ó��
        position.x = Mathf.Round(position.x * 100f) / 100f;
        position.y = Mathf.Round(position.y * 100f) / 100f;

        // Sprite �׸���
        Material spriteMaterial = new Material(Shader.Find("Sprites/Default"));
        spriteMaterial.color = color;

        Graphics.DrawTexture(
            new Rect(position.x, position.y, spriteSize.x, spriteSize.y),
            sprite.texture,
            textureRect,
            0, 0, 0, 0,
            color,
            spriteMaterial
        );
    }
#endif
}

