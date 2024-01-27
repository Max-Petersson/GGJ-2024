using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityUtils;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuManager : PersistentSingleton<MenuManager>
{
    [SerializeField] private bool activeOnStart;
    [SerializeField] private GameObject canvas;
    [SerializeField] private Button play;
    [SerializeField] private Button mainMenu;
    [SerializeField] private Button testing;
    [SerializeField] private Button end;

    [SerializeField] private GameObject EventSystemPrefab;
    private EventSystem eventSystem;
    private const string PLAY_SCENE_NAME = "Play";

    private void Start()
    {
        ConfigEventSystem();
        canvas.SetActive(activeOnStart);
    }

    void OnEnable()
    {
        play.onClick.AddListener(OnStartClick);
        mainMenu.onClick.AddListener(OnMainMenuClick);
        testing.onClick.AddListener(OnTestingClick);
        end.onClick.AddListener(OnEndClick);
        
        SceneManager.sceneLoaded += OnSceneLoaded;

    }



    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);

        if (scene.name != "Menu" || scene.name != "End")
        {
            HideMenu();
        }

        ConfigEventSystem();
    }

    private void ConfigEventSystem()
    {
        eventSystem = FindAnyObjectByType<EventSystem>();

        if (eventSystem == null)
        {
            GameObject go = Instantiate(EventSystemPrefab);
            eventSystem = go.GetComponent<EventSystem>();
            Debug.Log("no eventsystem was found: Adding event system", eventSystem);
        }

        eventSystem.firstSelectedGameObject = play.gameObject;
    }

    private void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    private void OnEndClick()
    {
        LoadScene("End");
    }

    private void OnMainMenuClick()
    {
        LoadScene("Menu");
    }

    private void OnTestingClick()
    {
        LoadScene("PhysicsTesting");
    }

    private void OnStartClick()
    {
        LoadScene(PLAY_SCENE_NAME);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            canvas.SetActive(!canvas.activeSelf);
        }
    }

    [ContextMenu("Show Menu")]
    public void ShowMenu()
    {
        canvas.SetActive(true);
    }

    [ContextMenu("Hide Menu")]
    public void HideMenu()
    {
        canvas.SetActive(false);
    }
}
