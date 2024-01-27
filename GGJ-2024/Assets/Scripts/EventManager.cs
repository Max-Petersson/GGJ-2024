using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityUtils;
public class EventManager : PersistentSingleton<EventManager>
{

    public Action OnGrab;
    public Action OnRelease;
    public Action<int> OnScoreChanged;
    public Action<float> OnMultiplyerChanged;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
