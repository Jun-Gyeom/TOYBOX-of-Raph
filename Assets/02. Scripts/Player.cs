using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;
using static UnityEngine.GraphicsBuffer;

public class Player : MonoBehaviour
{
    // Is Ability Able
    public Ability PlayerAbility { get; set; } = new Ability();
    private bool isAbility;

    [SerializeField] private float immortalDuration = 1f;
    [SerializeField] private float immortalCoolDown = 10f;
    private bool isImmortalCoolDown;

    //SingleTon
    TileManager tileManager;

    //Component
    Transform tf;
    Animator anim;

    //Move
    [Header("MOVE")]
    [SerializeField] Vector2 StartPos;
    [SerializeField] float MoveSpeed;
    [SerializeField] float DashSpeed;
    [SerializeField] int MaxDashStack = 3;
    [SerializeField] public int DashStack { get; private set; }
    [SerializeField] float ForcingMoveSpeed;

    public Vector2 CurrtyPos { get; private set; }

    Vector2 NextPos;
    Vector2 BeforePos;
    Vector3 TargetPos;
    float MoveDelay;
    bool IsMove;
    bool IsFalling;
    bool MoveAble = true;
    int DashDistance = 2;
    float DashChargingDelay = 1;
    bool DashCharging = false;
    float h, v;

    //HP 
    [Header("HP")]
    [SerializeField] int CurrtyHP;
    [SerializeField] int MaxHP;
    int DieHP =0;

    //Invincible
    [Header("INVINCIBLE")]
    [SerializeField] float HitInvincibleTime;
    bool OnInvincible;

    Animator Anim;
    #region Start
    private void Start()
    {
        Init_GetComponent();
        Init_StartSet();
    }

    private void Init_GetComponent()
    {
        tileManager = TileManager.Instance;
        tf = transform;
        anim = GetComponent<Animator>();
    }

    private void Init_StartSet()
    {
        DirectForcingMove(StartPos);
        DashStack = MaxDashStack;
        CurrtyPos = StartPos;
        CurrtyHP = MaxHP;
        DieHP = 0;
    }
    #endregion
    #region Update
    private void Update()
    {
        InputKey();
        //Moving();
    }
    #endregion
    #region Move
    void InputKey()
    {
        bool DashOn = false;
        if (Input.GetKeyDown(KeyCode.X))
        {
            Immortal(immortalDuration);
        }

        if(Input.GetKey(KeyCode.Z) && DashStack > 0)
        {
            DashOn = true;  
        }
        if (Input.anyKeyDown)
        {

            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");


            if (h == 0 && v == 0)
                return;

            Vector2 pos = Vector2.zero;

            if (h != 0)
            {
                AnimFloatSet("H", Mathf.Clamp(h, -1, 1));
            }
            AnimFloatSet("V", Mathf.Clamp(v * -1, -1, 1));
            if (h > 0)
                pos = Vector2.right;
            else if (h < 0)
                pos = Vector2.left;
            if (v > 0)
                pos = Vector2.down;
            else if (v < 0)
                pos = Vector2.up;


            if (!DashOn)
                MoveStart(pos, MoveSpeed);
            else
                DashStart(pos,DashSpeed);

        }
    }
    void MoveStart(Vector2 pos,float MoveSpeed)
    {
        if(pos == Vector2.zero || IsMove || !MoveAble) return;
        if(MovePositionGet(pos))
        {
            TargetPos = tileManager.GetTileObejctPosition(NextPos);
            IsMove = true;
            StartCoroutine("MoveToTarget",MoveSpeed);
        }
    }

    private IEnumerator MoveToTarget(float MoveSpeed)
    {
        while (Vector3.Distance(tf.position, TargetPos) > 0.1f) // 목표 지점 근처까지 이동
        {
            // 현재 위치에서 목표 위치로 일정 속도로 이동
            tf.position = Vector3.MoveTowards(transform.position, TargetPos, MoveSpeed * Time.deltaTime);
            
            yield return null; // 다음 프레임까지 대기
        }
        tf.position = TargetPos;
        MoveStop();
    }

    void Moving()
    {
        if(IsMove)
        {
            tf.position = Vector3.MoveTowards(tf.position, TargetPos, MoveSpeed * Time.deltaTime);
            float distance = Vector2.Distance(tf.position, TargetPos);
            if(distance <= 0.1f)
            {
                IsMove = false;
                MoveStop();
            }
        }
    } //삭제해야할것 같음

    void MoveStop()
    {
        IsMove = false;
        BeforePos = CurrtyPos;
        CurrtyPos = NextPos;
        AnimBoolSet("DASH", false);
        Interactor();
    }
    void DashStart(Vector2 pos, float MoveSpeed)
    {
        // 대쉬 권능 여부 
        if (!PlayerAbility.dashAble) return;

        pos *= DashDistance;
        if (pos == Vector2.zero || IsMove || !MoveAble || DashStack <= 0) return;
        if (MovePositionGet(pos))
        {
            TargetPos = tileManager.GetTileObejctPosition(NextPos);
            IsMove = true;
            DashStack--;
            UIManager.Instance.UpdateDashUI(DashStack);
            if(!DashCharging)
                StartCoroutine("DashChargDelay");
            AnimBoolSet("DASH", true);
            StartCoroutine("MoveToTarget", MoveSpeed);
        }
    }
    private IEnumerator DashChargDelay()
    {
        DashCharging = true;
        yield return new WaitForSeconds(DashChargingDelay);
        while (DashStack < MaxDashStack) 
        {
            DashStack++;
            UIManager.Instance.UpdateDashUI(DashStack);
            yield return new WaitForSeconds(1);
        }
        DashCharging = false;
    }

    //강제 이동 (비타겟팅 - 탐색함)
    public bool ForcingMove()
    {
        List<Vector2> guide = new List<Vector2>() {Vector2.up,Vector2.right,Vector2.down,Vector2.left };
        for (int i = 0; i < guide.Count; i++)
        {
            Vector2 newPos = guide[i];
            if (newPos.y < 0 || newPos.x < 0 || newPos.x > 9 || newPos.y > 4)
                continue;
            if (ForcingMove(newPos))
                return true;
        }
        
        return false;
    }
    //강제 이동 (타겟팅)
    public bool ForcingMove(Vector2 pos)
    {
        if (pos == Vector2.zero)
            return false;
        if (MovePositionGet(pos))
        {
            TargetPos = tileManager.GetTileObejctPosition(NextPos);
            IsMove = true;
            StartCoroutine("MoveToTarget", ForcingMoveSpeed);
            return true;
        }
        return false;
    }
    public void DirectionForcingMove()
    {
        List<Vector2> guide = new List<Vector2>() { Vector2.up, Vector2.right, Vector2.down, Vector2.left };
        Debug.Log("Forcing");
        for (int j = 1; j <= 2; j++)
        {
            for (int i = 0; i < guide.Count; i++)
            {
                Vector2 newPos = guide[i] * j;
                if (newPos.y < 0 || newPos.x < 0 || newPos.x > 9 || newPos.y > 4)
                    continue;
                if (MovePositionGet_OnlyTargetPosCheck(newPos))
                  {
                    DirectForcingMove(CurrtyPos + newPos);
                    return; 
                }
            }
        }
    }
    //즉시 강제 이동 
    public void DirectForcingMove(Vector2 pos)
    {
        tf.position = tileManager.GetTileObejctPosition(pos);
        CurrtyPos = pos;
    }

    bool MovePositionGet(Vector2 pos)
    {
        // 이동경로가 존재 하지 않을 경우 playerPos 반환하기 
        Vector2 newPos = tileManager.CheckMoveAble(CurrtyPos, CurrtyPos + pos);
        if (newPos == CurrtyPos)
            return false;
        else
        {
            NextPos = newPos;
            return true;
        }
    }
    bool MovePositionGet_OnlyTargetPosCheck(Vector2 pos)
    {
        Vector2 newPos = CurrtyPos + pos;
        if(tileManager.GameMap[(int)newPos.y][(int)newPos.x].PlayerMoveAble)
        {
            NextPos = newPos;
            return true;
        }
        return false; 
    }


    #endregion
    #region HP

    void Heal(int amount = 1)
    {
        CurrtyHP += amount;
        if(CurrtyHP > MaxHP)
            CurrtyHP = MaxHP;   
    }

    public void Damage(int amount = 1)
    {
        if (OnInvincible)
            return;
        CurrtyHP -= amount;
        UIManager.Instance.UpdateHealthUI(CurrtyHP);
        InvincibleStart(HitInvincibleTime, false);

        // 카메라 흔들림 
        GameManager.Instance.ShakeCamera(0.5f, 0.3f);

        if (CurrtyHP <= 0)
            Die();
    }

    void Die()
    {
        AnimTriggerSet("DIE");
        this.enabled = false;
    }

    public void DieEnd()
    {
        gameObject.SetActive(false);
    }
    #endregion

    #region Interact
    public void Interactor()
    {
        Tile tile = tileManager.GameMap[(int)CurrtyPos.y][(int)CurrtyPos.x];
        if(tile.LeaderTile != null && tile.LeaderTile.InteractionAble)
        {
            switch (tile.LeaderTile.Type)
            {
                case TileType.TADDYBEAR:
                    Damage();
                    DirectionForcingMove();
                    break;
                default:
                    break;
            }
        }
        if (!tile.InteractionAble)
            return;
        TileType type = tile.Type;
        switch (type)
        {
            case TileType.VOID:
                Falling();
                break;
            case TileType.BASE:
                //없음
                break;
            case TileType.SPIKE:
                Damage();
                break;
            case TileType.FALL:
                Damage();
                ForcingMove();
                break;
            case TileType.TADDYBEAR:
                Damage();
                DirectionForcingMove();
                break;
            default:
                break;
        }
    }

    // 무적 스킬 
    public void Immortal(float duration)
    {
        // 무적 권능 여부 + 쿨타임 
        if (!PlayerAbility.immortalAble || isImmortalCoolDown) return;

        isImmortalCoolDown = true;
        StartCoroutine(StartImmortalCoolDown());

        InvincibleStart(duration, true);
    }

    private IEnumerator StartImmortalCoolDown()
    {
        yield return new WaitForSeconds(immortalCoolDown);
        isImmortalCoolDown = false;
    }

    void Falling()
    {
        //스킬 무적만 리턴시킴
        if (OnInvincible && isAbility || IsFalling)
            return;
        IsFalling = true;
        AnimTriggerSet("FALL");
        MoveAble = false;
    }

    //Animation Event Method
    public void FallingEnd()
    {
        DirectForcingMove(BeforePos);
        Damage();
        IsFalling = false;
        MoveAble = true;
    }


    void InvincibleStart(float time, bool isAbility)
    {
        if (time == 0)
            return;
        OnInvincible = true;

        this.isAbility = isAbility;
        if (this.isAbility)
        {
            // 무적 권능 스킬 애니메이션
            AnimFloatSet("ONINVINCIBLE", 1);
        }
        else
        {
            // 피격 애니메이션 
            AnimBoolSet("HITINVINCIBLE", true);
        }

        //Layer 따로 둬서 애니메이션 실행
        StartCoroutine("InvincibleDelay", time);
    }
    IEnumerator InvincibleDelay(float time)
    {
        yield return new WaitForSeconds(time);
        InvincibleEnd();
    }
    void InvincibleEnd()
    {
        OnInvincible = false;

        if (this.isAbility)
        {
            // 무적 권능 스킬 애니메이션
            AnimFloatSet("ONINVINCIBLE", 0);

            this.isAbility = false; 
        }
        else
        {
            // 피격 애니메이션 
            AnimBoolSet("HITINVINCIBLE", false);
        }
    }
    #endregion
    #region Animation
    private void AnimFloatSet(string name,float value)
    {
        anim.SetFloat(name, value);
    }
    private void AnimBoolSet(string name,bool value)
    {
        anim.SetBool(name, value);
    }
    private void AnimTriggerSet(string name)
    {
        anim.SetTrigger(name);
    }
    private bool AnimStatCompare(string name)
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(name);
    }
    #endregion
}
