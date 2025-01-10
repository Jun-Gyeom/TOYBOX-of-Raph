using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Player : MonoBehaviour
{
    //SingleTon
    TileManager tileManager;

    //Component
    Transform tf;

    //Move
    public Vector2 CurrtyPos { get; private set; }
    Vector2 NextPos;
    Vector3 TargetPos;
    float MoveDelay;
    bool IsMove;
    bool MoveAble = true;
    [SerializeField] float MoveSpeed;
    int DashDistance;
    float h, v;

    //HP 
    int CurrtyHP;
    int MaxHP;
    int DieHP =0;

    Animator Anim;
    #region Start
    private void Start()
    {
        Init_GetComponent();
    }
    private void Init_GetComponent()
    {
        tileManager = TileManager.Instance;
        tf = transform;
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
        if (Input.anyKeyDown)
        {
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");
            Debug.Log("GetKey");
            if (h == 0 && v == 0)
                return;

            Vector2 pos = Vector2.zero;

            if (h > 0)
                pos = Vector2.right;
            else if (h < 0)
                pos = Vector2.left;
            if (v > 0)
                pos = Vector2.down;
            else if (v < 0)
                pos = Vector2.up;

            MoveStart(pos);

        }
    }
    void MoveStart(Vector2 pos)
    {
        if(pos == Vector2.zero || IsMove || !MoveAble) return;
        if(MovePositionGet(pos))
        {
            TargetPos = tileManager.GetTileObejctPosition(NextPos);
            IsMove = true;
            StartCoroutine("MoveToTarget");
        }
    }

    private IEnumerator MoveToTarget()
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
        CurrtyPos = NextPos;
    }
    void Dash(Vector2 pos)
    {

    }

    void ForcingMove(Vector2 pos)
    {

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


    #endregion
    #region HP

    bool Heal(int amount)
    {
        return false;
    }

    bool Damage(int amount)
    {
        return false;
    }

    void Die()
    {

    }
    #endregion

    #region Interact

    private void Interactor(TileType type)
    {
        switch (type)
        {
            case TileType.VOID:
                Falling();
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
    }

    void Falling()
    {

    }

    void InvincibleStart(float time)
    {

    }
    IEnumerator InvincibleDelay(float time)
    {
        yield return new WaitForSeconds(time);
    }
    void InvincibleEnd()
    {

    }
    #endregion

}
