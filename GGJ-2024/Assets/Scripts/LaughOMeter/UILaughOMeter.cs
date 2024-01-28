using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILaughOMeter : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject m_clockCursor;
    public float c_startingRotZ = 45;
    public float c_maxRotZ = -45;
    float m_wantedRot = -61;
    public static  float c_maxMultiplyer = 2f;
    private void OnEnable()
    {
        EventManager.Instance.OnMultiplyerChanged += UpdateMultiplyer;
    }
    private void OnDisable()
    {
        EventManager.Instance.OnMultiplyerChanged -= UpdateMultiplyer;

    }
    private void Start()
    {
        m_wantedRot = c_startingRotZ;
        m_clockCursor.transform.rotation = Quaternion.Euler(0, 0, c_startingRotZ);


    }
    private void Update()
    {
        if(m_wantedRot != m_clockCursor.transform.eulerAngles.z)
        {
            m_clockCursor.transform.rotation = Quaternion.Lerp(m_clockCursor.transform.rotation, Quaternion.Euler(new Vector3(0, 0, m_wantedRot)), Time.deltaTime);
           
        }
    }
    private void UpdateMultiplyer(float currentMultiplyer)
    {
        float percentage = (currentMultiplyer-1) / c_maxMultiplyer;
       
        percentage = Mathf.Clamp(percentage, 0,1);  // Clamp the percentage between 0 and 1
        m_wantedRot = MapPercentageToRange(percentage);
    }

    float MapPercentageToRange(float percentage)
    {
        return (1 - percentage) * c_startingRotZ + percentage * (c_maxRotZ);
    }
}
