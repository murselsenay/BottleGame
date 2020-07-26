using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    private Vector3 mOffset;
    private float mZCoord;
    Vector3 startPosition;
    bool canTurn = true;
    Rigidbody rb;
    bool canDrag = false;
    public GameObject targetShelf;
    public static Move instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        rb = gameObject.GetComponent<Rigidbody>();
        startPosition = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

       
        if (canDrag)
        {

            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -3.6f, 4.2f), Mathf.Clamp(transform.position.y, 0.15f, 1.5f), Mathf.Clamp(transform.position.z, 2.9f, 3f));
        }

    }
    private void OnMouseDown()
    {
        if (gameObject.GetComponent<Move>().enabled == true)
        {
            startPosition = gameObject.transform.position;
            canDrag = true;
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
            mOffset = gameObject.transform.position - GetMouseWorldPos();
        }
    }
    private void OnMouseUp()
    {
        if (gameObject.GetComponent<Move>().enabled == true)
        {
            canDrag = false;
            rb.isKinematic = true;
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

            Debug.Log(targetShelf);
            if (targetShelf != null)
            {
                gameObject.transform.position = targetShelf.transform.position;
                LevelManager.instance.bottleCount -= 1;
                if (targetShelf.GetComponent<Renderer>().material.color == gameObject.transform.GetChild(0).GetComponent<Renderer>().material.color)
                {
                    LevelManager.instance.score += 5;
                    LevelManager.instance.UpdateScoreText();
                }
                else
                {
                    LevelManager.instance.score -= 3;
                    LevelManager.instance.UpdateScoreText();
                }
                Destroy(targetShelf);
            }
            else if (targetShelf == null)
            {
                gameObject.transform.position = startPosition;
            }
        }
    }
    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mZCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);

    }
    private void OnMouseDrag()
    {
        if (canDrag && gameObject.GetComponent<Move>().enabled==true)
        {
            transform.position = GetMouseWorldPos() + mOffset;

        }

    }
 
    private void OnTriggerEnter(Collider other)
    {
      
        if (other.gameObject.tag == "Shelf")
        {
            
            Debug.Log(other.gameObject.transform.childCount.ToString());
            if (other.gameObject.transform.childCount!=1)
            {
                targetShelf = other.gameObject.transform.GetChild(1).gameObject;
             
            }
          
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Shelf")
        {
              if (other.gameObject.transform.childCount!=0)
            {
                targetShelf = null;
            }
        
        }
    }
}
