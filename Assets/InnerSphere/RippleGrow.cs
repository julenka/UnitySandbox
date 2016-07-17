using UnityEngine;
using System.Collections;

public class RippleGrow : MonoBehaviour
{
    public Material material;
    public AnimationCurve outerRadius;
    public AnimationCurve innerRadius;
    public AnimationCurve alphaCurve;

    public GameObject ripplePrefab;
    private GameObject rippleGameObject;
    public float duration = 1;
    public float scale = 5f;
    void Start()
    {
        var rippleGameObject = Instantiate(ripplePrefab);
        material = rippleGameObject.GetComponent<MeshRenderer>().material;
        material.SetFloat("_InnerRadius", 0);
        material.SetFloat("_OuterRadius", 0);
        m_outerTint = material.GetColor("_InnerTint");
    }
    Color m_outerTint;

    private float m_startTime;
    void SpawnRipple()
    {
        m_startTime = Time.timeSinceLevelLoad;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnRipple();
        }
        if (m_startTime <= 0)
        {
            return;
        }
        var elapsed = Time.timeSinceLevelLoad - m_startTime;
        if (elapsed > duration)
        {
            return;
        }
        elapsed /= duration;
        material.SetFloat("_InnerRadius", innerRadius.Evaluate(elapsed) * scale);
        material.SetFloat("_OuterRadius", outerRadius.Evaluate(elapsed) * scale);
        var innerTint = m_outerTint;
        innerTint.a = alphaCurve.Evaluate(elapsed);
        material.SetColor("_InnerTint", innerTint);
    }
}
