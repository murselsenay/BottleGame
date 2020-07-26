using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillMove : MonoBehaviour
{
    private Vector3 mOffset;
    private float mZCoord;

    private float maxYLimit, minYLimit;
    bool canTurn = true;
    Rigidbody rb;
    bool canDrag = true;

    // Start is called before the first frame update
    void Start()
    {
        maxYLimit = 5f;
        minYLimit = -5f;
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        if (LevelManager.instance.bottlesColoringDone() && canTurn)
        {
            Invoke("TurnGravityOn", 0.3f);
            canTurn = false;
        }
        if (canDrag)
        {
          
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -3.6f, 4.2f), Mathf.Clamp(transform.position.y, minYLimit, maxYLimit), Mathf.Clamp(transform.position.z, -1f, 5f));
        }
      
    }
    private void OnMouseDown()
    {
        if (gameObject.tag=="YellowPill" || gameObject.tag=="PinkPill")
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (gameObject.tag == "GreenPill")
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        mOffset = gameObject.transform.position - GetMouseWorldPos();
        maxYLimit = 1.3f;
        minYLimit = 1.1f;
    }
    private void OnMouseUp()
    {
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        maxYLimit = 5f;
        minYLimit = -5f;
    }
    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mZCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);

    }
    private void OnMouseDrag()
    {
        if (canDrag)
        {
            transform.position = GetMouseWorldPos() + mOffset;

        }

    }
    private void TurnGravityOn()
    {

        rb.useGravity = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="PillDetector")
        {
            gameObject.transform.position = other.gameObject.transform.position;
            gameObject.transform.SetParent(other.gameObject.transform.parent);
            canDrag = false;
        }
        if (other.gameObject.tag=="PillRecover")
        {
            gameObject.transform.position = LevelManager.instance.pillSpawnPoint.position;
        }
       
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "BottleBottom")
        {
            rb.isKinematic = true;
        }
    }
}
