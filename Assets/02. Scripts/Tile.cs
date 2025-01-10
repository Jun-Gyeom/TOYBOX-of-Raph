using UnityEngine;

public enum TileType
{
    VOID,   // 절벽 타일 
    BASE,   // 기본 타일
    SPIKE,  // 가시 타일
    FALL,   // 낙석 타일
}

public class Tile : MonoBehaviour
{
    public bool PlayerMoveAble { get; private set; }    // 이동 가능한 타일인지 여부 
    public TileType Type { get; private set; }          // 타일 타입 

    private int x;
    private int y;
    private Animator anim;

    public void Init(int x, int y)
    {
        this.x = x;
        this.y = y;
        Type = TileType.BASE;
    }

    public void SetTileType(TileType type)
    {
        this.Type = type;

        switch (type)
        {
            case TileType.VOID:
                break;
            case TileType.BASE:

                break;
            case TileType.SPIKE:

                break;
            case TileType.FALL:

                break;
            default:
                break;
        }

        // 타일 애니메이션 재생 
        SetAnimInt((int)type);
        SetAnimTrigger();
    }

    public void PlayerHit()
    {
        // 플레이어 위치 가져오기 

        // 현재 타일 위치 가져오기 

        // 체력 감소 
    }

    public void SetAnimTrigger(string param = "SHOT")
    {
        anim.SetTrigger(param);
    }

    public void SetAnimInt(int param, string name = "TYPE")
    {
        anim.SetFloat(name, param);
    }
}
