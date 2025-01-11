using System;
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

    private Vector2 TilePos;
    private Animator anim;
    private float startupTime;
    private float holdingTime;

    public bool InteractionAble = false;
    public void Init(int x, int y)
    {
        TilePos.x = x;
        TilePos.y = y;
        TryGetComponent(out anim);
        SetTileType(TileType.BASE, 0, 0);
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
        //���� ������ TileTypeHolding �ڷ�ƾ ���
        StopCoroutine("TileTypeHolding");

        // Ÿ�� �ִϸ��̼� ��� �ӵ� ����
        SetAnimInt((1f /startupTime), "STARTUP_SPEED");

        // Ÿ�� �ִϸ��̼� ��� 
        SetAnimInt((float)type);
        SetAnimTrigger();
    }

    #region Event
    //���� - ������ �ο� �̺�Ʈ 
    public void PlayerHit()
    {
        Player player = GameManager.Instance.Player;
        Vector2 playerPos = player.CurrtyPos;

        if (PlayerPosCompare())
        {
            player.Damage();
        }
        return;
    }
    private bool PlayerPosCompare()
    {
        Vector2 playerPos = GameManager.Instance.Player.CurrtyPos;
        if(playerPos == TilePos)
            return true;
        return false;
    }
    //Ÿ�� Ÿ�� ���ӽð� ���� �̺�Ʈ
    public void StartTileHold()
    {
        StartCoroutine("TileTypeHolding");
    }

    //Ÿ�� Ÿ�� BaseŸ������ ���� �̺�Ʈ
    public void SetBaseTile()
    {
        SetTileType(TileType.BASE, 0, 0);
    }

    //Player �ش� Ÿ�� �̵� ���� ���� ��Ȱ��ȭ �̺�Ʈ
    public void SetPlayerMoveDisable()
    {
        PlayerMoveAble = false; 
    }
    public void SetPlayerMoveAble()
    {
        PlayerMoveAble = true;
    }
    #endregion

    public void SetInteractionAble()
    {
        InteractionAble = true;
        GameManager.Instance.Player.Interactor();
    }

    public void SetIteractionDisAble()
    {
        InteractionAble = false;
    }

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
        Debug.Log(param);
        anim.SetFloat(name, param);
    }
}
