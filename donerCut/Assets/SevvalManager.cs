using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SevvalManager : MonoBehaviour
{
    public static SevvalManager Instance { set; get; }

    private const float REQUIRED_SLICEFROCE = 400.0f;

    public GameObject sevvalPrefab;
    public Transform trail;

    private bool  isPaused;
    private List<Sevval> sevvales = new List<Sevval>();
    private float lastSpawn;
    private float deltaSpawn = 1.0f;   
    private Vector3 lastMousePos;
    private Collider2D[] sevvalesCols;

    //UI part of the game thingy
    private int score;
    private int highscore;
    private int lifepoint;
    public Text scoreText;
    public Text highscoreText;
    public Image[] lifepoints;
    public GameObject pauseMenu;
    public GameObject deathMenu;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        sevvalesCols = new Collider2D[0];
        NewGame();
    }

    public void NewGame()
    {
        score = 0;
        lifepoint = 3;
        pauseMenu.SetActive(false);
        scoreText.text = score.ToString();
        highscore = PlayerPrefs.GetInt("Skor");
        highscoreText.text = "Yüksek Skor : " + highscore.ToString();
        Time.timeScale = 1;
        isPaused = false;

        foreach(Image i in lifepoints)
            i.enabled = true;

        foreach(Sevval u in sevvales)
            Destroy(u.gameObject);
        sevvales.Clear();

        deathMenu.SetActive(false);
    }

    private void Update()
    {
        if(isPaused)
            return;

        if (Time.time - lastSpawn > deltaSpawn)
        {
            Sevval u = GetSevval();
            float randomX = Random.Range(-1.65f,1.65f);
            u.LaunchSevval(Random.Range(1.85f,2.75f),randomX,-randomX);
            lastSpawn = Time.time;
        }

        if (Input.GetMouseButton(0))
        {       
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = -1;
            trail.position = pos;

            Collider2D[] thisFramesSeval = Physics2D.OverlapPointAll(new Vector2 (pos.x, pos.y),LayerMask.GetMask("Sevval"));
            if ((Input.mousePosition - lastMousePos).sqrMagnitude > REQUIRED_SLICEFROCE)
            {
                foreach (Collider2D c3 in thisFramesSeval)
                {
                    for (int i = 0; i < sevvalesCols.Length; i++)
                    {
                    if(c3 == sevvalesCols[i])
                    {
                            c3.GetComponent<Sevval>().Kesildi();
                        }
                    }
                }
            }  
            lastMousePos = Input.mousePosition;
            sevvalesCols = thisFramesSeval;
        }
    }

    private Sevval GetSevval()
    {
        Sevval u = sevvales.Find(x => !x.IsActive);

        if(u == null)
        {
            u = Instantiate(sevvalPrefab).GetComponent<Sevval>();
            sevvales.Add(u);
        }

        return u;
    }

    public void IncrementScore(int scoreAmount)
    {
        score += scoreAmount * 2;
        scoreText.text = score.ToString();

        if(score > highscore)
        {
            highscore = score;
            highscoreText.text = "Yüksek Skor : " + highscore.ToString();
            PlayerPrefs.SetInt("Skor",highscore);
        }
    }

    public void LoseLP()
    {
        if(lifepoint == 0)
            return;

        lifepoint--;
        lifepoints[lifepoint].enabled = false;
        if(lifepoint == 0)
            Death();
    }

    public void Death()
    {
        isPaused = true;
        deathMenu.SetActive(true);
    }

    public void pauseGame() {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        isPaused = pauseMenu.activeSelf;
        Time.timeScale = (Time.timeScale == 0) ? 1 : 0 ;
    }

    public void ToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}