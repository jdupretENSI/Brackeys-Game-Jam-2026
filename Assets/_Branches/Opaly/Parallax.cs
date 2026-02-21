using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform[] layers;
    public float[] speeds;

    private Vector3 previousCamPos;

    void Start()
    {
        previousCamPos = Camera.main.transform.position;
    }

    private void LateUpdate()
    {
        Vector3 camPos = Camera.main.transform.position;
        Vector3 delta = camPos - previousCamPos;

        for(int i = 0; i < layers.Length; i++)
        {
            if (layers[i] == null) continue;

            layers[i].position += new Vector3(delta.x * speeds[i], delta.y * speeds[i], 0f);
        }
        previousCamPos = camPos;
    }
}
