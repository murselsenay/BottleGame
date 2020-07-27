using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleCarrier : MonoBehaviour
{
    Transform targetTransform;
    public Vector3 startPosition;
    float t;
    bool canChange = true;
    internal bool canChange2 = false;
    public float speed;

    public static BottleCarrier instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        targetTransform = GameObject.FindGameObjectWithTag("Center").transform;
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        if (LevelManager.instance != null)
        {
            if (LevelManager.instance.bottlesColoringDone() && canChange)
            {

                LevelManager.instance.ConveyorBelt.GetComponent<Animator>().SetTrigger("Working");
                startPosition = GameObject.FindGameObjectWithTag("Center").transform.position;
                targetTransform = GameObject.FindGameObjectWithTag("Left").transform;
                t = 0;
                canChange = false;
                LevelManager.instance.InvokeCallPetri();
                Invoke("GoSecondStep", 2f);

            }
            if (LevelManager.instance.bottlesFillingDone() && canChange2)
            {

                LevelManager.instance.ConveyorBelt.GetComponent<Animator>().SetTrigger("Working");
                startPosition = GameObject.FindGameObjectWithTag("Center").transform.position;
                targetTransform = GameObject.FindGameObjectWithTag("Left").transform;
                t = 0;
                canChange2 = false;
                Invoke("GoThirdStep", 2f);

            }
            Move(startPosition, targetTransform.position);
        }






    }
    private void Move(Vector3 startPos, Vector3 target)
    {
        t += Time.deltaTime / speed;
        transform.position = Vector3.Lerp(startPos, target, t);
    }
    public void GoSecondStep()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(false);
        }
        startPosition = GameObject.FindGameObjectWithTag("Right").transform.position;
        transform.position = startPosition;
        targetTransform = GameObject.FindGameObjectWithTag("Center").transform;
        t = 0;
    }

    public void GoThirdStep()
    {
        LevelManager.instance.DestroyPetri();
        LevelManager.instance.CallShelves();
        LevelManager.instance.ConveyorBelt.GetComponent<Animator>().SetTrigger("Working");
        LevelManager.instance.canCheckPill = false;
        startPosition = GameObject.FindGameObjectWithTag("Right").transform.position;
        transform.position = startPosition;
        targetTransform = GameObject.FindGameObjectWithTag("Center").transform;
        t = 0;
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            gameObject.transform.GetChild(i).GetComponent<CapsuleCollider>().radius = 0.03f;
            gameObject.transform.GetChild(i).GetComponent<MoveBottle>().enabled = true;
            for (int j = 4; j < gameObject.transform.GetChild(i).childCount; j++)
            {
                gameObject.transform.GetChild(i).transform.GetChild(j).gameObject.GetComponent<Rigidbody>().isKinematic = true;
                gameObject.transform.GetChild(i).transform.GetChild(j).gameObject.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            }
        }
    }
}
