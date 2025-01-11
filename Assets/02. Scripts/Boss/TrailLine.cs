using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrailLine : MonoBehaviour
{
    [SerializeField] Transform pivot;
    public void Init(Vector3 startPos,HV type)
    {
        float offset = 2.2f;
        
        switch (type)
        {
            case HV.LEFT:
                pivot.rotation = Quaternion.Euler(0,0,-90);
                startPos.x += offset;
                break;
            case HV.RIGHT:
                pivot.rotation = Quaternion.Euler(0, 0,90);
                startPos.x -= offset;
                break;
            case HV.DOWN:
                pivot.rotation = Quaternion.Euler(0, 0, 0);
                startPos.y += offset;
                break;
            default:
                break;
        }
        transform.position = startPos;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Trail"))
        {
            GetComponent<Animator>().SetTrigger("DESTORY");
            Destroy(this, 0.3f);
        }
    }
}
