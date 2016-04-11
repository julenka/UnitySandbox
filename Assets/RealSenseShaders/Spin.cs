using UnityEngine;
using System.Collections;

namespace UnitySandbox.RealSenseShaders
{
    public class Spin : MonoBehaviour
    {
        public float degreesPerSecond = 180;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            float degToRotate = degreesPerSecond * Time.deltaTime;
            transform.Rotate(Vector3.up, degToRotate);
        }
    }
}

