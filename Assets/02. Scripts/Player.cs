using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Vector2 CurrtyPos;
    Vector2 NextPos;
    float MoveDelay;
    bool IsMove;
    float MoveSpeed;
    int DashDistance;

    //HP 
    int CurrtyHP;
    int MaxHP;
    int DieHP =0;

    Animator Anim;

    void Move(Vector2 pos)
    {

    }
    void Dash(Vector2 pos)
    {

    }
    void MovePositionGet(Vector2 pos)
    {

    }

    void MoveAbleCheck(Vector2 pos)
    {

    }
    void ForcingMove(Vector2 pos)
    {

    }
    void Falling()
    {

    }
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

}
