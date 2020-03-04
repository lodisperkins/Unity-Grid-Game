using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekBehaviour : MonoBehaviour
{

    [SerializeField]
    private Vector3 target;
    [SerializeField]
    private Vector3 velocity;
    [SerializeField]
    private int max_speed;
    private Rigidbody body;
    public bool isTemporary;
    public float captureRange;
    // Use this for initialization
    void Start()
    {
        body = GetComponent<Rigidbody>();
        isTemporary = true;
    }
    void seek()
    {

        Vector3 seekforce = target - transform.position;
        seekforce = (seekforce.normalized * max_speed) - velocity;
        if (velocity.magnitude > max_speed)
        {
            velocity = velocity.normalized * max_speed;
        }
        transform.position += seekforce * Time.deltaTime;
    }
    private void TemporarySeek()
    {
        Vector3 seekforce = target - transform.position;
        seekforce = (seekforce.normalized * max_speed) - velocity;
        if (velocity.magnitude > max_speed)
        {
            velocity = velocity.normalized * max_speed;
        }
        transform.position += seekforce * Time.deltaTime;
        float distance = Vector3.Distance(transform.position, target);
        if (distance <= captureRange && isTemporary)
        {
            Destroy(GetComponent<SeekBehaviour>());
        }
    }
    public void Init(Vector3 targetVal, Vector3 velocityVal, int speedVal, float rangeVal = 0,bool temporary = false)
    {
        target = targetVal;
        velocity = velocityVal;
        max_speed = speedVal;
        captureRange = rangeVal;
        isTemporary = temporary;
    }
    // Update is called once per frame
    void Update()
    {
        if(!isTemporary)
        {
            seek();
            return;
        }
        TemporarySeek();
    }
}