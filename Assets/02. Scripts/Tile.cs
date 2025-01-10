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
        TryGetComponent(out anim);
        SetTileType(TileType.FALL, 5, 100);
    }

    public void SetTileType(TileType type, float startupTime, float holdingTime)
    {
        this.Type = type;
        this.startupTime = startupTime;
        this.holdingTime = holdingTime;

        switch (type)
        {
            case TileType.VOID:
                PlayerMoveAble = true;
                break;
            case TileType.BASE:
                PlayerMoveAble = true;
                break;
            case TileType.SPIKE:
                PlayerMoveAble = true;
                break;
            case TileType.FALL:
                PlayerMoveAble = true;
                break;
            default:
                break;
        }

        // Ÿ�� �ִϸ��̼� ��� �ӵ� ����
        SetAnimInt((1f /startupTime), "STARTUP_SPEED");

        // Ÿ�� �ִϸ��̼� ��� 
        SetAnimInt((float)type);
        SetAnimTrigger();
    }

    #region Event
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

    public void SetBaseTile()
    {
        SetTileType(TileType.BASE, 0, 0);
    }

    public void SetPlayerMoveDisable()
    {
        PlayerMoveAble = false; 
    }
    #endregion

    // Ÿ�� ���� Ȧ�� �ڷ�ƾ 
    private IEnumerator TileTypeHolding()
    {
        yield return new WaitForSeconds(holdingTime);

        SetAnimTrigger("END");
    }

    public void SetAnimTrigger(string param = "SHOT")
    {
        if (anim == null)
        {
            Debug.LogWarning($"{gameObject.name} Animator :: NULL");
            return;
        }
        anim.SetTrigger(param);
    }

    public void SetAnimInt(float param, string name = "TYPE")
    {
        if (anim == null)
        {
            Debug.LogWarning($"{gameObject.name} Animator :: NULL");
            return;
        }
        anim.SetFloat(name, param);
    }
}
