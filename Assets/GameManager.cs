using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    public CinemachineMixingCamera mixingCamera;
    public Canvas splashCanvas;
    public AudioSource audioSource;

    public bool running = false;
    public bool gameEnded = false;
    public bool canRestart = false;

    public GameUIResult trashDestroyedUi;
    public GameUIResult planetHitsUi;
    public GameUIResult trashRocketsLaunchedUi;
    public GameUIResult bombRocketsLaunchedUi;
    public GameUIResult starsDestroyedUi;
    public GameUIResult takeCare;
    public GameUIResult thankYou;

    public int trashDestroyed;
    public int planetHits;
    public int trashRocketsLaunched;
    public int bombRocketsLaunched;
    public bool sunKilled;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        mixingCamera.m_Weight0 = 1.0f;
        mixingCamera.m_Weight1 = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Cancel"))
        {
            Application.Quit();
        }

        if (gameEnded && canRestart && Input.GetButton("Fire1"))
        {
            SceneManager.LoadScene("LD50");
        }
    }

    public void OnClickStart()
    {
        StartCoroutine(BeginGame());
    }

    private IEnumerator BeginGame()
    {
        LeanTween.value(gameObject, 1f, 0f, 1f).setEaseInOutCubic().setOnUpdate((float val) =>
                { mixingCamera.m_Weight0 = val; });

        LeanTween.value(gameObject, 0f, 1f, 1f).setEaseInOutCubic().setOnUpdate((float val) =>
        { mixingCamera.m_Weight1 = val; });

        running = true;

        yield return new WaitForSeconds(1f);
        splashCanvas.enabled = false;
    }

    public void EndGame()
    {
        if (gameEnded) return;

        running = false;
        gameEnded = true;
        StartCoroutine(EndGameDetails());
    }

    private IEnumerator EndGameDetails()
    {
        yield return new WaitForSeconds(0.3f);
        audioSource.Play();
        StartCoroutine(AudioFade.FadeAudio(audioSource, 1f, 8f));

        LeanTween.value(gameObject, 1f, 0f, 1.5f).setEaseInOutCubic().setOnUpdate((float val) => mixingCamera.m_Weight1 = val);
        LeanTween.value(gameObject, 0f, 1f, 1.5f).setEaseInOutCubic().setOnUpdate((float val) => mixingCamera.m_Weight0 = val);

        yield return new WaitForSeconds(0.5f);
        trashDestroyedUi.SetValue(trashDestroyed);
        yield return new WaitForSeconds(0.5f);
        planetHitsUi.SetValue(planetHits);
        yield return new WaitForSeconds(0.5f);
        trashRocketsLaunchedUi.SetValue(trashRocketsLaunched);
        yield return new WaitForSeconds(0.5f);
        bombRocketsLaunchedUi.SetValue(bombRocketsLaunched);
        if (sunKilled)
        {
            yield return new WaitForSeconds(0.5f);
            starsDestroyedUi.SetValue(1);
        }
        yield return new WaitForSeconds(2f);
        takeCare.SetValue(0);
        yield return new WaitForSeconds(2f);
        thankYou.SetValue(0);
        canRestart = true;
    }
}
