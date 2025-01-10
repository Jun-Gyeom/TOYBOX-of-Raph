using UnityEngine;

public enum TileType
{
    VOID,   // ���� Ÿ�� 
    BASE,   // �⺻ Ÿ��
    SPIKE,  // ���� Ÿ��
    FALL,   // ���� Ÿ��
}

public class Tile : MonoBehaviour
{
    public bool PlayerMoveAble { get; private set; }    // �̵� ������ Ÿ������ ���� 
    public TileType Type { get; private set; }          // Ÿ�� Ÿ�� 

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

        // Ÿ�� �ִϸ��̼� ��� 
        SetAnimInt((int)type);
        SetAnimTrigger();
    }

    public void PlayerHit()
    {
        // �÷��̾� ��ġ �������� 

        // ���� Ÿ�� ��ġ �������� 

        // ü�� ���� 
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
