using UnityEngine;
using System.Collections;

public class SetCard : MonoBehaviour
{
    public bool isBlackout = false;
    public enum Number { one, two, three };
    public enum Color { red, green, purple };
    public enum Fill { solid, stripe, empty };

    public Number number;
    public Color color;
    public Fill fill;
    public Material material;

    private GameObject m_cube;
    private MeshRenderer m_renderer;
    public void Start()
    {
        m_cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        m_cube.transform.parent = transform;
        m_renderer = m_cube.GetComponent<MeshRenderer>();
        m_renderer.material = material;

    }

    public void Update()
    {
        UnityEngine.Color cubeColor = UnityEngine.Color.black;
        switch (color)
        {
            case SetCard.Color.green:
                cubeColor = UnityEngine.Color.green;
                break;
            case SetCard.Color.purple:
                cubeColor = UnityEngine.Color.magenta;
                break;
            case SetCard.Color.red:
                cubeColor = UnityEngine.Color.red;
                break;

        }

        if (isBlackout)
        {
            cubeColor = UnityEngine.Color.black;
        }
        m_cube.transform.position = new Vector3((int)number, (int)color, (int)fill) * 2 + 0.5f * Vector3.one;
        m_renderer.material.color = cubeColor;
    }
}
