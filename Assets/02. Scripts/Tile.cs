using System.Collections;
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
    private float startupTime;
    private float holdingTime;

    public void Init(int x, int y)
    {
        this.x = x;
        this.y = y;
        Type = TileType.BASE;
    }

    public void SetTileType(TileType type, float startupTime, float holdingTime)
    {
        this.Type = type;
        this.startupTime = startupTime;
        this.holdingTime = holdingTime;

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

        // Ÿ�� �ִϸ��̼� ��� �ӵ� ����
        SetAnimInt((1f /startupTime), "SPEED");

        // Ÿ�� �ִϸ��̼� ��� 
        SetAnimInt((float)type);
        SetAnimTrigger();
    }

    public void PlayerHit()
    {
        // �÷��̾� ��ġ �������� ( from GameManger ) 
        Vector2 playerPos = new Vector2();// = GameManager.Instance.

        // ���� Ÿ�� ��ġ �������� 

        // ��ġ�� ��ġ�� ü�� ���� 
        if (playerPos.x == this.x && playerPos.y == this.y)
        {

        }
    }

    public void StartTileHold()
    {
        StartCoroutine(TileTypeHolding());
    }

    // Ÿ�� ���� Ȧ�� �ڷ�ƾ 
    private IEnumerator TileTypeHolding()
    {
        yield return new WaitForSeconds(holdingTime);

        SetTileType(TileType.BASE, 0, 0);
    }

    public void SetAnimTrigger(string param = "SHOT")
    {
        anim.SetTrigger(param);
    }

    public void SetAnimInt(float param, string name = "TYPE")
    {
        anim.SetFloat(name, param);
    }
}
