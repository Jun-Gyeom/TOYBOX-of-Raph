using System;
using System.Collections;
using UnityEngine;

public enum TileType
{
    VOID,   // 절벽 타일 
    BASE,   // 기본 타일
    SPIKE,  // 가시 타일
    FALL,   // 낙석 타일
    TADDYBEAR, // 곰인형 타일
}

public class Tile : MonoBehaviour
{
    public bool _PlayerMoveAble;
    public bool PlayerMoveAble { get { return LeaderTile == null ? _PlayerMoveAble : LeaderTile._PlayerMoveAble; } private set { _PlayerMoveAble = value; } }    // 이동 가능한 타일인지 여부 
    public TileType Type { get; private set; }          // 타일 타입 

    public Tile LeaderTile;

    private Vector2 TilePos;
    private Animator anim;
    private float startupTime;
    private float holdingTime;
    private bool TypeChangeLock;

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
        if (TypeChangeLock)
            return;
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
            case TileType.TADDYBEAR:
                PlayerMoveAble = true;
                break;
            default:
                break;
        }
        //이전 상태의 TileTypeHolding 코루틴 취소
        StopCoroutine("TileTypeHolding");

        // 타일 애니메이션 재생 속도 조절
        if (startupTime == 0) startupTime = 0.0001f;
        SetAnimInt((1f /startupTime), "STARTUP_SPEED");

        // 타일 애니메이션 재생 
        SetAnimInt((float)type);
        SetAnimTrigger();
    }

    public void SetTileType(TileType type,float holdingTime)
    {
        TypeChangeLock = true;
        this.Type = type; 
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
            case TileType.TADDYBEAR:
                PlayerMoveAble = true;
                break;
            default:
                break;
        }
        //이전 상태의 TileTypeHolding 코루틴 취소
        StopCoroutine("TileTypeHolding");


        // 타일 애니메이션 재생 
        SetAnimInt((float)type);
        SetAnimTrigger();

        StartCoroutine("LockDuration", holdingTime);
    }
    IEnumerator LockDuration(float holdingTime)
    {
        yield return new WaitForSeconds(holdingTime);
        TypeChangeLock = false;
    }

    #region Event
    //낙석 - 데미지 부여 이벤트 
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
    //타일 타입 지속시간 설정 이벤트
    public void StartTileHold()
    {
        StartCoroutine("TileTypeHolding");
    }

    // 타일 변경 홀딩 코루틴 
    private IEnumerator TileTypeHolding()
    {
        yield return new WaitForSeconds(holdingTime);
        SetAnimTrigger("END");
    }

    //타일 타입 Base타입으로 변경 이벤트
    public void SetBaseTile()
    {
        SetTileType(TileType.BASE, 0, 0);
    }

    //Player 해당 타일 이동 가능 유무 비활성화 이벤트
    public void SetPlayerMoveDisable()
    {
        PlayerMoveAble = false; 
    }
    public void SetPlayerMoveAble()
    {
        PlayerMoveAble = true;
    }

    public void SetInteractionAble()
    {
        InteractionAble = true;
        if(GameManager.Instance.Player != null) 
            GameManager.Instance.Player.Interactor();
    }
    
    public void SetIteractionDisAble()
    {
        InteractionAble = false;
    }
    public void SetTaddyBearChildNode()
    {
        TileManager tilemanager = TileManager.Instance;
        Vector2 detectPos = TilePos + new Vector2(-1,-1);
        int x = (int)detectPos.x;

        for (int i=0;i<3;i++)
        {
            detectPos.x = x;
            for(int j=0;j<3;j++)
            {
                if (detectPos == TilePos)
                    continue;
                tilemanager.GameMap[(int)detectPos.y][(int)detectPos.x].LeaderTile = this;
                detectPos.x++;
            }
            detectPos.y++;
        }

    }

    public void ResetTaddyBearChildNode()
    {
        TileManager tilemanager = TileManager.Instance;
        Vector2 detectPos = TilePos + new Vector2(-1, -1);
        int x = (int)detectPos.x;

        for (int i = 0; i < 3; i++)
        {
            detectPos.x = x;
            for (int j = 0; j < 3; j++)
            {
                if (detectPos == TilePos)
                    continue;
                tilemanager.GameMap[(int)detectPos.y][(int)detectPos.x].LeaderTile = null;
                detectPos.x++;
            }
            detectPos.y++;
        }
    }
    #endregion




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
