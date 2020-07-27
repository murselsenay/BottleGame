using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveColourTag : MonoBehaviour
{
    public GameObject colorTag;
    private Vector3 mOffset;

    private float mZCoord;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (LevelManager.instance != null)
        {
            if (LevelManager.instance.bottlesColoringDone())
            {
                Destroy(gameObject);
            }
        }



    }
    private void OnMouseDown()
    {
        Instantiate(colorTag, transform.position, Quaternion.identity);
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        mOffset = gameObject.transform.position - GetMouseWorldPos();
    }
    private void OnMouseUp()
    {
        if (gameObject != null)
        {
            Destroy(gameObject);
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
        transform.position = GetMouseWorldPos() + mOffset;

    }
}
