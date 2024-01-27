using System.Collections;
using System.Collections.Generic;
using UnityUtils;
using UnityEngine;
using Unity.VisualScripting;

public class LaughOMeter : MonoBehaviour
{
    // Start is called before the first frame update

    private float m_multiplyer = 1;
    private float m_currentScore = 0;



    private void OnEnable()
    {
        GameEventManager.Instance.OnGrab += Grabbed;
    }

    private void OnDisable()
    {
        GameEventManager.Instance.OnGrab -= Grabbed;
    }
    public void Grabbed()
    {

    }
}
