using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    private new Rigidbody rigidbody;
    public Vector3 firstPos;
    public Quaternion firstRotation;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.isKinematic = true;
        firstPos =transform.position;
        firstRotation=transform.rotation;
    }

    // Update is called once per frame
    public void Reset()
    {
        rigidbody.isKinematic = true;
        transform.position = firstPos;
        transform.rotation = firstRotation;
    }

    public void Hit(Vector3 hitDirection,Vector3 hitPos)
    {
        rigidbody.isKinematic = false;
        float distance= Vector3.Distance(transform.position, hitPos);
        float effectRatio = Mathf.Clamp01(1 / distance) * ((distance > 1) ? 0f:1f) ;

        rigidbody.AddForce(hitDirection * effectRatio, ForceMode.Impulse);

    }
}
