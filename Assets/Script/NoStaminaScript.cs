using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoStaminaScript : MonoBehaviour
{
    public GameObject noStaminaText;
    public float timeToWait = 3f;
    public float m_leftTime = 0;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void ShowText()
    {
        noStaminaText.SetActive(true);
        m_leftTime = timeToWait;
    }
 
    private void Update()
    {
        if (m_leftTime > 0)
        {
            m_leftTime -= Time.deltaTime;
        }
        else
        {
            noStaminaText.SetActive(false);
        }
    }
}
