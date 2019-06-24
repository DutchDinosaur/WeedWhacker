using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class manager : MonoBehaviour
{
    Animator camFSM;
    [SerializeField]
    private car_Driving car;

    public GameObject Menu;
    public GameObject UpgradeMenu;
    public GameObject score;

    bool ESCextra = false;
    bool Eextra = false;

    public int mowed = 0;
    public int Score = 0;

    public bool MowUpgrade = false;

    public GameObject[] Mowers;
    int mow = 0;

    [SerializeField]
    private Text scor;
    [SerializeField]
    private Text mowe;
    [SerializeField]
    private Text tim;

    public int time = 0;


    bool ded = false;

    void Start()
    {
        camFSM = gameObject.GetComponent<Animator>();
        DontDestroyOnLoad(gameObject);
        Physics.gravity = new Vector3(0, -9.8f, 0);
    }

    IEnumerator timer()
    {
        while (time > 0)
        {
            time -= 1;
            yield return new WaitForSeconds(1);
        }

        Physics.gravity = -Physics.gravity;


        yield return new WaitForSeconds(3);
        camFSM.enabled = false;

        UnityEngine.SceneManagement.SceneManager.LoadScene(1);

        yield return new WaitForSeconds(3);
        GameObject.Destroy(this.gameObject, 1);
        ded = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    void Update()
    {
        if (ded)
        {
            return;
        }

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 1)
        {
            GameObject scrorre = GameObject.FindGameObjectWithTag("scor");

            scrorre.GetComponent<Text>().text = "Your Score IS: " + Score.ToString();

            return;
        }


        if (Input.GetKeyDown(KeyCode.Escape) || ESCextra)
        {
            camFSM.SetBool("ESC",true);
            ESCextra = false;
        }
        else { camFSM.SetBool("ESC", false); }

        if (Input.GetKeyDown(KeyCode.E) || Eextra)
        {
            camFSM.SetBool("E", true);
            Eextra = false;
        }
        else { camFSM.SetBool("E", false); }


        scor.text = "Score: " + Score.ToString();
        mowe.text = "Grass Mowed " + mowed.ToString();
        tim.text = time.ToString() + " Seconds Left";


    }

    public void startGame()
    {
        if (time <= 0)
        {
            Physics.gravity = new Vector3(0,-9.8f,0);


            time = 120;
            StartCoroutine(timer());
            Score = 0;
            MowUpgrade = false;
            for (int i = 0; i < Mowers.Length; i++)
            {
                Mowers[i].SetActive(false);
            }

            GameObject[] chunks = GameObject.FindGameObjectsWithTag("chunk");
            for (int i = 0; i < chunks.Length; i++)
            {
                chunks[i].GetComponent<Mowable>().refresh();
            }
        }

    }

    public void SetMenu(bool m)
    {
        Menu.SetActive(m);
    }

    public void SetUpgradeMenu(bool m)
    {
        UpgradeMenu.SetActive(m);
    }

    public void SetScore(bool s)
    {
        score.SetActive(s);
    }

    public void SetCarDrivable(bool d)
    {
        car.controlable = d;
    }


    public void SetESC()
    {
        ESCextra = true;
    }


    public void SetE()
    {
        Eextra = true;
    }

    public void quit()
    {
        Application.Quit();
    }

    public void UpgradeMowers()
    {

        if (Score < 2000)
        {
            return;
        }
        Score -= 2000;
        MowUpgrade = true;
    }

    public void installMower()
    {
        if (Score < 500)
        {
            return;
        }
        Score -= 500;
        if (mow < Mowers.Length)
        {
            Mowers[mow].SetActive(true);
            mow += 1;

            GameObject[] chunks = GameObject.FindGameObjectsWithTag("chunk");
            for (int i = 0; i < chunks.Length; i++)
            {
                chunks[i].GetComponent<Mowable>().refresh();
            }
        }
        
    }
}