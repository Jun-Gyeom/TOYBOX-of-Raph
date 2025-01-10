using System.Collections;
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

        // 타일 애니메이션 재생 속도 조절
        SetAnimInt((1f /startupTime), "STARTUP_SPEED");

        // 타일 애니메이션 재생 
        SetAnimInt((float)type);
        SetAnimTrigger();
    }

    #region Event
    public void PlayerHit()
    {
        // 플레이어 위치 가져오기 ( from GameManger ) 
        Vector2 playerPos = new Vector2();// = GameManager.Instance.

        // 현재 타일 위치 가져오기 

        // 위치가 겹치면 체력 감소 
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

    // 타일 변경 홀딩 코루틴 
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
