using UnityEngine;
using System.Collections;

public class MakeAllSetCards : MonoBehaviour {

    public SetCard prefab;

    public void Start()
    {
        
        for (int i = 0; i <= (int) SetCard.Color.purple; i++)
        {
            for(int j = 0; j <= (int) SetCard.Fill.empty; j++)
            {
                for (int k = 0; k <= (int) SetCard.Number.three; k++)
                {
                    SetCard go = Instantiate(prefab);
                    go.color = (SetCard.Color)i;
                    go.number = (SetCard.Number)j;
                    go.fill = (SetCard.Fill)k;

                    go.transform.parent = transform;
                }
            }

        }
    }
}
