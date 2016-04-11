using UnityEngine;
using UnityEngine.UI;

public class SO_32035611 : MonoBehaviour
{
    public GameObject ButtonPrototype;
    public Canvas Canvas;
    public GameObject ButtonSpawnPosition;

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            int iCopy = i;
            GameObject gameObjectCopy = Instantiate(ButtonPrototype);

            float buttonHeight = ButtonPrototype.GetComponent<RectTransform>().rect.height;

            Vector3 buttonSpawnPosition = ButtonSpawnPosition.transform.position;

            gameObjectCopy.transform.position = buttonSpawnPosition + Vector3.down * buttonHeight * (i + 1);
            Button buttonCopy = gameObjectCopy.GetComponent<Button>();
            buttonCopy.GetComponentInChildren<Text>().text = "button " + i;
            gameObjectCopy.transform.parent = Canvas.transform;

            buttonCopy.onClick.AddListener(() => _clickHandler(iCopy));
            buttonCopy.onClick.AddListener(() => _clickHandler2(buttonCopy));
        }
    }

    private void _spawnButton(int index)
    {
        GameObject gameObjectCopy = Instantiate(ButtonPrototype);

        float buttonHeight = ButtonPrototype.GetComponent<RectTransform>().rect.height;

        GameObject buttonSpawnPositionGameObject = (GameObject)GameObject.Find("ButtonSpawnPosition");
        Vector3 buttonSpawnPosition = buttonSpawnPositionGameObject.transform.position;

        gameObjectCopy.transform.position = buttonSpawnPosition + Vector3.down * buttonHeight * (index + 1);
        Button buttonCopy = gameObjectCopy.GetComponent<Button>();
        buttonCopy.GetComponentInChildren<Text>().text = "button " + index;
        gameObjectCopy.transform.parent = Canvas.transform;

        buttonCopy.onClick.AddListener(() => _clickHandler(index));
        buttonCopy.onClick.AddListener(() => _clickHandler2(buttonCopy));
    }

    private void _clickHandler(int idx)
    {
        Debug.Log("pressed button " + idx);
    }

    private void _clickHandler2(Button btn)
    {
        Debug.Log("handler2 " + btn.GetComponentInChildren<Text>().text);
    }

}
