using UnityEngine;

public class ItemAnimation : MonoBehaviour
{

    void Update()
    {
        Vector3 itemRotation = new Vector3(0, 1, 0);
        Vector3 itemHeight = new Vector3(0, Mathf.Sin(Time.time % 360) * 0.001f, 0);

        transform.localEulerAngles += itemRotation;
        transform.position += itemHeight;

        


    }
}
