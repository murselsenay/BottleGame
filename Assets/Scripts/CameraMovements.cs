using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovements : MonoBehaviour
{
 
    public Vector3 startPosition;
    float t;
    public float speed;
    bool canMove = true;
    // Start is called before the first frame update
    void Start()
    {
        
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.instance != null)
        {
  if (LevelManager.instance.bottlesColoringDone() && canMove)
        {

            Invoke("Move", 2f);

        }
        if (LevelManager.instance.bottlesFillingDone())
        {
            canMove = false;
            Invoke("MoveForShelves", 2f);
            

        }
        }
      

    }
    private void Move()
    {
        t = 0;
        t += Time.deltaTime / speed;
        transform.position = Vector3.Lerp(startPosition, new Vector3(0f,2.770f,-0.816f), t);
        Quaternion currentRotation = transform.rotation;
        Quaternion wantedRotation = Quaternion.Euler(45, 0, 0);
        transform.rotation = Quaternion.RotateTowards(currentRotation, wantedRotation, Time.deltaTime * 50f);
    }
    private void MoveForShelves()
    {
        t = 0;
        t += Time.deltaTime / speed;
        transform.position = Vector3.Lerp(startPosition, new Vector3(0f, 1.25f, -2.5f), t);
        Quaternion currentRotation = transform.rotation;
        Quaternion wantedRotation = Quaternion.Euler(0, 0, 0);
        transform.rotation = Quaternion.RotateTowards(currentRotation, wantedRotation, Time.deltaTime * 50f);
    }
}
