using UnityEngine;

public class DownSampleAnimation : MonoBehaviour
{
    [SerializeField] private DownSamplingRenderFeature _feature;
    
    void Update()
    {
        var tmp = (Mathf.Sin(4f * Time.frameCount * Mathf.PI / 180f) + 1f) / 2f;
        var value = tmp * 120f;
        _feature.downSample = (int)value;
    }
}