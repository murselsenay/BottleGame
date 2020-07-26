using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public GameObject bottleCarrier;
    public GameObject ConveyorBelt;
    public GameObject petri;
    public GameObject greenPill, yellowPill, pinkPill;
    public Transform pillSpawnPoint;
    public Transform petriPosition;
    public GameObject shelves;
    internal bool canCheckPill = true;

    private GameObject whichIsGreen;
    private GameObject whichIsPink;
    private GameObject whichIsYellow;



    internal int score = 0;
    public Text scoreText;
    public GameObject gameOverPanel;


    internal bool threeBottle = false;
    internal bool sixBottle = false;
    float offset;
    float offsetToNextBottle;
    internal int bottleCount = 1;

    internal int greenCapCount;
    internal int yellowCapCount;
    internal int pinkCapCount;

    GameObject instantiatedPetri;
    GameObject instantiatedWhichIsGreen;
    GameObject instantiatedWhichIsPink;
    GameObject instantiatedWhichIsYellow;

    public List<GameObject> bottles = new List<GameObject>();
    public List<Material> capMaterials = new List<Material>();


    internal int completeBottleCount = 0;
    internal int unColoredBottleCount;
    internal bool canStartGame = false;

    // Start is called before the first frame update
    void Start()
    {

        Time.timeScale = 0;


    }
    public void startGame()
    {
        instance = this;

        if (threeBottle)
        {
            bottleCount = 3;
            offset = -0.3f;
            offsetToNextBottle = 0.3f;

        }
        if (sixBottle)
        {
            bottleCount = 6;
            offset = -0.5f;
            offsetToNextBottle = 0.2f;
        }

        UpdateScoreText();
        getBottles();
        unColoredBottleCount = bottleCount;

        whichIsGreen = GameObject.FindGameObjectWithTag("GreenPill");
        whichIsPink = GameObject.FindGameObjectWithTag("PinkPill");
        whichIsYellow = GameObject.FindGameObjectWithTag("YellowPill");
        if (whichIsGreen != null)
        {
            instantiatedWhichIsGreen = Instantiate(whichIsGreen);
            instantiatedWhichIsGreen.transform.GetChild(1).GetComponent<Renderer>().material = whichIsGreen.transform.GetChild(0).GetComponent<Renderer>().material;
            instantiatedWhichIsGreen.SetActive(false);
        }
        if (whichIsPink != null)
        {
            instantiatedWhichIsPink = Instantiate(whichIsPink);
            instantiatedWhichIsPink.transform.GetChild(1).GetComponent<Renderer>().material = whichIsPink.transform.GetChild(0).GetComponent<Renderer>().material;
            instantiatedWhichIsPink.SetActive(false);
        }
        if (whichIsYellow != null)
        {
            instantiatedWhichIsYellow = Instantiate(whichIsYellow);
            instantiatedWhichIsYellow.transform.GetChild(1).GetComponent<Renderer>().material = whichIsYellow.transform.GetChild(0).GetComponent<Renderer>().material;
            instantiatedWhichIsYellow.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (bottleCount == 0)
        {
            GameObject instantiatedGameOverPanel=Instantiate(gameOverPanel);
            instantiatedGameOverPanel.transform.SetParent(GameObject.FindGameObjectWithTag("CameraCanvas").transform);
            instantiatedGameOverPanel.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
            instantiatedGameOverPanel.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
            instantiatedGameOverPanel.GetComponent<RectTransform>().localPosition = new Vector2(0, 0);
            GameObject.FindGameObjectWithTag("FinalScoreText").GetComponent<Text>().text = "Final Score: " + score.ToString();
            bottleCount = 1;
        }
    }

    public void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    public void CallPetri()
    {
        instantiatedPetri = Instantiate(petri, petriPosition.transform.position, Quaternion.identity);

        if (threeBottle)
        {
            for (int i = 0; i < 4; i++)
            {

                Instantiate(greenPill, pillSpawnPoint.position, Quaternion.identity);
                Instantiate(yellowPill, pillSpawnPoint.position, Quaternion.identity);
                Instantiate(pinkPill, pillSpawnPoint.position, Quaternion.identity);

            }
        }
        if (sixBottle)
        {
            for (int i = 0; i < greenCapCount * 4; i++)
            {
                Instantiate(greenPill, pillSpawnPoint.position, Quaternion.identity);
            }
            for (int i = 0; i < yellowCapCount * 4; i++)
            {
                Instantiate(yellowPill, pillSpawnPoint.position, Quaternion.identity);
            }
            for (int i = 0; i < pinkCapCount * 4; i++)
            {
                Instantiate(pinkPill, pillSpawnPoint.position, Quaternion.identity);
            }
        }

    }

    public void InvokeCallPetri()
    {
        Invoke("CallPetri", 2f);
    }
    public void DestroyPetri()
    {
        Destroy(instantiatedPetri);
    }
    public void CallShelves()
    {
        Instantiate(shelves);
        if (instantiatedWhichIsGreen != null)
        {
            instantiatedWhichIsGreen.SetActive(true);
            instantiatedWhichIsGreen.transform.position = shelves.transform.GetChild(0).transform.GetChild(0).transform.position;
        }
        if (instantiatedWhichIsPink != null)
        {
            instantiatedWhichIsPink.SetActive(true);
            instantiatedWhichIsPink.transform.position = shelves.transform.GetChild(1).transform.GetChild(0).transform.position;
        }
        if (instantiatedWhichIsYellow != null)
        {
            instantiatedWhichIsYellow.SetActive(true);
            instantiatedWhichIsYellow.transform.position = shelves.transform.GetChild(2).transform.GetChild(0).transform.position;
        }


    }
    public void getBottles()
    {


        ConveyorBelt.GetComponent<Animator>().SetTrigger("Working");
        for (int i = 0; i < bottleCount; i++)
        {
            int rndBottle = Random.Range(0, bottles.Count);
            int rndMaterial = Random.Range(0, capMaterials.Count);
            GameObject instantiatedBottle = Instantiate(bottles[rndBottle], new Vector3(bottleCarrier.transform.position.x + offset, bottleCarrier.transform.position.y + 0.15f, bottleCarrier.transform.position.z), Quaternion.identity);
            instantiatedBottle.transform.GetChild(0).GetComponent<Renderer>().material = capMaterials[rndMaterial];
            instantiatedBottle.transform.SetParent(bottleCarrier.transform);
            offset += offsetToNextBottle;
            instantiatedBottle.tag = instantiatedBottle.transform.GetChild(0).GetComponent<Renderer>().material.name.Substring(0, instantiatedBottle.transform.GetChild(0).GetComponent<Renderer>().material.name.Length - 14) + "Pill";



            if (threeBottle)
            {
                bottles.RemoveAt(rndBottle);
                capMaterials.RemoveAt(rndMaterial);
            }
            if (sixBottle)
            {
                if (capMaterials[rndMaterial].name == "GreenMat")
                {
                    greenCapCount += 1;
                }
                if (capMaterials[rndMaterial].name == "PinkMat")
                {
                    pinkCapCount += 1;
                }
                if (capMaterials[rndMaterial].name == "YellowMat")
                {
                    yellowCapCount += 1;
                }
            }



        }
        Debug.Log(greenCapCount.ToString() + yellowCapCount.ToString() + pinkCapCount.ToString());
    }
    public bool bottlesColoringDone()
    {
        if (unColoredBottleCount == 0)
        {
            return true;
        }
        return false;
    }
    public bool bottlesFillingDone()
    {
        if (completeBottleCount == bottleCount)
        {
            return true;
        }
        return false;
    }
    public void ThreeBottleBtn()
    {
        gameObject.GetComponent<LevelManager>().threeBottle = true;
        gameObject.GetComponent<LevelManager>().sixBottle = false;
        Destroy(GameObject.FindGameObjectWithTag("StartGamePanel"));
        startGame();
        Time.timeScale = 1;
    }
    public void SixBottleBtn()
    {
        gameObject.GetComponent<LevelManager>().threeBottle = false;
        gameObject.GetComponent<LevelManager>().sixBottle = true;
        Destroy(GameObject.FindGameObjectWithTag("StartGamePanel"));
        startGame();
        Time.timeScale = 1;
    }
   
}
