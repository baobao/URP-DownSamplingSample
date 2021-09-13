using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DownSamplingRenderPass : ScriptableRenderPass
{
    private const string CommandBufferName = nameof(DownSamplingRenderPass);
    private const int RenderTextureId = 0;
    
    private RenderTargetIdentifier _currentTarget;
    
    private int _downSample = 1;

    /// ここでは、レンダリングロジックを実装することができます。
    /// 描画コマンドの発行やコマンドバッファの実行には、<c>ScriptableRenderContext</c>を使用します。
    /// https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
    /// ScriptableRenderContext.submitを呼び出す必要はありません。
    /// レンダリングパイプラインがパイプライン内の特定のポイントで呼び出します。
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        var commandBuffer = CommandBufferPool.Get(CommandBufferName);
        var cameraData = renderingData.cameraData;
        var w = cameraData.camera.scaledPixelWidth / _downSample;
        var h = cameraData.camera.scaledPixelHeight / _downSample;

        // RenderTextureを生成
        commandBuffer.GetTemporaryRT(RenderTextureId, 
            w, h, 0, FilterMode.Point, RenderTextureFormat.Default);
        
        // 現在のカメラ描画画像をRenderTextureにコピー
        commandBuffer.Blit(_currentTarget, RenderTextureId);
        
        // RenderTextureを現在のRenderTarget（カメラ）にコピー
        commandBuffer.Blit(RenderTextureId, _currentTarget);
        context.ExecuteCommandBuffer(commandBuffer);
        context.Submit();
        
        CommandBufferPool.Release(commandBuffer);
    }
    
    /// <summary>Constructor</summary>
    public DownSamplingRenderPass() => renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
    
    /// <summary>
    /// Execute実行前のパラメータを渡すメソッド
    /// </summary>
    public void SetParam(RenderTargetIdentifier renderTarget, int downSample)
    {
        _currentTarget = renderTarget;
        _downSample = downSample;
        if (_downSample <= 0) _downSample = 1;
    }
}