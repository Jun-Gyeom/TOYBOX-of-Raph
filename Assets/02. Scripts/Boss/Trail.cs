using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail : MonoBehaviour
{
    [SerializeField] public HV hv;
    private bool isShoted;
    private float speed; 

    private Rigidbody2D rb;
    private SpriteRenderer childSprite;

    private void Awake()
    {
        TryGetComponent(out rb);
        childSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (isShoted)
        {
            rb.velocity = transform.right * -1 * speed * Time.fixedDeltaTime;
        }
    }
    public void Shot(float speed,HV hv)
    {
        isShoted = true;
        this.speed = speed;
        this.hv = hv;
        RotatTrail();
        Invoke("Delete", 10f);
    }
    private void RotatTrail()
    {
        switch (hv)
        {
            case HV.LEFT:
                //그대로 
                break;
            case HV.RIGHT:
                transform.rotation = Quaternion.Euler(0, 0, 180);
                childSprite.flipY = true;
                break;
            case HV.UP:
                transform.rotation = Quaternion.Euler(0, 0, -90);
                break;
            case HV.DOWN:
                transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            default:
                break;
        }
    }
    private void Delete()
    {
        Destroy(gameObject);
    }

    //변경 이유 - 열차가 지나갈 때 겹쳐있으면 지속적으로 피해를 줘야함
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.Player.Damage();
        }
    }
}
