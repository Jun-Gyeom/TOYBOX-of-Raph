using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail : MonoBehaviour
{
    [SerializeField] public HV hv;
    private bool isShoted;
    private float speed; 
    private Rigidbody2D rb;

    private void Awake()
    {
        TryGetComponent(out rb);
    }

    private void FixedUpdate()
    {
        if (isShoted)
        {
            switch (hv)
            {
                case HV.LEFT:
                    rb.velocity = new Vector2(-speed * Time.fixedDeltaTime, 0f);
                    break;
                case HV.RIGHT:
                    rb.velocity = new Vector2(speed * Time.fixedDeltaTime, 0f);
                    break;
                case HV.UP:
                    rb.velocity = new Vector2(0f, speed * Time.fixedDeltaTime);
                    break;
                case HV.DOWN:
                    rb.velocity = new Vector2(0f, -speed * Time.fixedDeltaTime);
                    break;
                default:
                    break;
            }
        }
    }
    public void Shot(float speed)
    {
        isShoted = true;
        this.speed = speed;
        Invoke("Delete", 10f);
    }

    private void Delete()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.Player.Damage();
        }
    }
}
