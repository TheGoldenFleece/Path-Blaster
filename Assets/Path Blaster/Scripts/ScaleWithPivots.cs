using UnityEngine;

public class ScaleWithPivots : MonoBehaviour
{
    [SerializeField] private Transform startObject;
    [SerializeField] private Transform endObject;

    private Vector3 InitialScale;

    private void Start() {
        InitialScale = transform.localScale;
        UpdateTransformScale();
    }

    private void Update() {
        if (startObject.hasChanged || endObject.hasChanged) {
            //UpdateTransformScale();
        }
    }
    private void UpdateTransformScale() {
        float distance = Vector3.Distance(startObject.position, endObject.position);

        //float halfOfObject = .5f;
        float scaleMultiplier = startObject.localScale.z;

        float scaleZ = distance / scaleMultiplier;
        transform.localScale = new Vector3(InitialScale.x, InitialScale.y, scaleZ);
    }
}
