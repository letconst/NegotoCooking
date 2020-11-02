using UnityEngine;

/// <summary>
/// Time.timeScaleに影響されないWaitForSecondsを提供する
/// </summary>
public class WaitForSecondsRealtime : CustomYieldInstruction
{
    private readonly float _waitTime;

    public override bool keepWaiting => Time.realtimeSinceStartup < _waitTime;

    /// <summary>
    /// Time.timeScaleに影響されずに指定秒待機する
    /// </summary>
    /// <param name="time">待機時間</param>
    public WaitForSecondsRealtime(float time)
    {
        _waitTime = Time.realtimeSinceStartup + time;
    }
}
