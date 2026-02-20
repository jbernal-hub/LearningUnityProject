using UnityEngine;

public class AlwaysLookAtCamera : MonoBehaviour
{
    Transform camera;

    void Start()
    {
        camera = Camera.main.transform;

        transform.forward = camera.forward;
    }

    //void Update()
    //{
    //    Vector3 currentFwd = transform.eulerAngles;
    //    currentFwd = transform.position - camera.position;
    //    transform.forward = currentFwd.normalized;
    //}
}
