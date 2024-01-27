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
    [SerializeField] private Button start;
    [SerializeField] private Button mainMenu;
    [SerializeField] private Button end;

    [SerializeField] private GameObject EventSystemPrefab;
    private EventSystem eventSystem;

    private void Start()
    {
        ConfigEventSystem();
        canvas.SetActive(activeOnStart);
    }

    void OnEnable()
    {
        start.onClick.AddListener(OnStartClick);
        mainMenu.onClick.AddListener(OnMainMenuClick);
        end.onClick.AddListener(OnEndClick);
        
        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);

        HideMenu();
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

        eventSystem.firstSelectedGameObject = start.gameObject;
    }

    private void OnEndClick()
    {
        SceneManager.LoadScene("End");
    }

    private void OnMainMenuClick()
    {
        SceneManager.LoadScene("Menu");
    }

    private void OnStartClick()
    {
        SceneManager.LoadScene("PhysicsTesting");
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
