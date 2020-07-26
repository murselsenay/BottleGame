using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour
{
    bool canChangeMat = true;
    internal int pillCount;
    bool canClose = true;
    public GameObject starDust;
    
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Move>().enabled = false;
        gameObject.transform.GetChild(2).GetComponent<SphereCollider>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (pillCount==4 && canClose)
        {
            BottleCarrier.instance.canChange2 = true;
            gameObject.transform.GetChild(2).gameObject.SetActive(false);
            StartCoroutine(CloseTheCap());
            canClose = false;
        }
    }

   
    public IEnumerator CloseTheCap()
    {
        
        yield return new WaitForSeconds(1f);
        GameObject instantiatedStarDust = Instantiate(starDust, gameObject.transform.GetChild(0).transform.position, Quaternion.identity);
        Destroy(instantiatedStarDust, 2f);
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        while (gameObject.transform.GetChild(0).transform.eulerAngles.y <= 180)
        {
            gameObject.transform.GetChild(0).transform.Rotate(0f, 2f, 0f);
            yield return new WaitForSeconds(0.01f);

        }

        LevelManager.instance.completeBottleCount += 1;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ColourTag")
        {
            if (canChangeMat)
            {
                string a= gameObject.transform.GetChild(0).GetComponent<Renderer>().material.color.ToString();
                string b = other.gameObject.GetComponent<SpriteRenderer>().color.ToString();
                if (a==b)
                {
                    LevelManager.instance.score += 5;
                    LevelManager.instance.UpdateScoreText();
                }
                else
                {
                    LevelManager.instance.score -= 3;
                    LevelManager.instance.UpdateScoreText();
                }
                gameObject.transform.GetChild(1).GetComponent<Renderer>().material.color = other.gameObject.GetComponent<SpriteRenderer>().color;
                Color oldColour = gameObject.transform.GetChild(1).GetComponent<Renderer>().material.color;
                gameObject.transform.GetChild(1).GetComponent<Renderer>().material.color= new Color(oldColour.r, oldColour.g, oldColour.b, 0.5f);
                LevelManager.instance.unColoredBottleCount -= 1;
                gameObject.transform.GetChild(2).GetComponent<SphereCollider>().enabled = true;
                Destroy(other.gameObject);
                canChangeMat = false;
                Debug.Log(gameObject.transform.GetChild(0).GetComponent<Renderer>().material.color);
                Debug.Log(other.gameObject.GetComponent<SpriteRenderer>().color);
               
            }

        }
        if (LevelManager.instance.canCheckPill)
        {
            if (other.gameObject.tag == "GreenPill" || other.gameObject.tag == "PinkPill" || other.gameObject.tag == "YellowPill")
            {
                if (other.gameObject.tag == gameObject.tag)
                {
                    other.gameObject.tag = "Untagged";
                    LevelManager.instance.score += 5;
                    LevelManager.instance.UpdateScoreText();
                }
                else
                {
                    LevelManager.instance.score -= 3;
                    LevelManager.instance.UpdateScoreText();
                }

                other.gameObject.tag = "Untagged";
                pillCount += 1;
            }

        }

    }

  


}
