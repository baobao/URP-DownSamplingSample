using UnityEngine.Rendering.Universal;

public class DownSamplingRenderFeature : ScriptableRendererFeature
{
    public int downSample = 10;
    
    DownSamplingRenderPass _renderPass;

    public override void Create()
    {
        _renderPass = new DownSamplingRenderPass();
    }

    /// ここでは、レンダラーに1つまたは複数のレンダーパスを注入することができます。
    /// このメソッドは、レンダラーをカメラごとに設定する際に呼び出されます。
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        // パスにカメラのカラーを渡す
        _renderPass.SetParam(renderer.cameraColorTarget, downSample);
        renderer.EnqueuePass(_renderPass);
    }
}


