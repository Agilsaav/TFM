using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRadius : MonoBehaviour
{
    [SerializeField] Vector3 initialRadius;
    [SerializeField] float maxDist = 50f;
    [SerializeField] float step = 1.0f;
    [SerializeField] float waveSeconds = 0.01f;
    [SerializeField] float alphaPercentatgeChange = 0.5f;
    [SerializeField] float alphaStep = 0.1f;


    Renderer m_ObjectRenderer;
    float distanceAlpha;

    private void Awake()
    {
       m_ObjectRenderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        transform.localScale = initialRadius;

        distanceAlpha = maxDist * alphaPercentatgeChange;

        StartCoroutine(RadarWave1());
    }

    private void Update()
    {
        if (transform.localScale.x >= distanceAlpha) ChangeAlpha();
        if (transform.localScale.x >= maxDist) Destroy(gameObject);
    }

    private IEnumerator RadarWave1()
    {
        while(transform.localScale.x <= maxDist)
        {
            transform.localScale += new Vector3(step, step, step);
            yield return new WaitForSeconds(waveSeconds);
        }        
    }

    private void ChangeAlpha()
    {
        float currenTrans = m_ObjectRenderer.material.GetFloat("_transparency");
        if ((currenTrans - alphaStep) > 0.0f)
        {
            currenTrans -= alphaStep;
            m_ObjectRenderer.material.SetFloat("_transparency", currenTrans);

        }       
        else
            m_ObjectRenderer.material.SetFloat("Vector1_B7C5EC10", 0.0f);

        //Color currentColor = m_ObjectRenderer.material.GetColor("Color_6B90766");
        //Debug.Log(currentColor.a);
        ////currentColor.a -= alphaStep;
        //currentColor.a = 0.0f;
        ////Debug.Log(currentColor.a);
        //m_ObjectRenderer.material.SetColor("Color_6B90766", currentColor);
    }
}
