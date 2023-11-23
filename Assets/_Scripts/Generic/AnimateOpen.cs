using System;
using UnityEngine;
using UnityEngine.Events;

public class AnimateOpen : MonoBehaviour, ICreationDelay
{
    [SerializeField] bool runOnEnable = true;
    [SerializeField] float startDelay = 0f;

    [SerializeField] Vector3 startScale = Vector3.zero;
    [SerializeField] Vector3 targetScale = Vector3.one;
    [SerializeField] float animationDuration = 0.2f;
    [SerializeField] LeanTweenType tweenType = LeanTweenType.easeOutBack;

    private Action<float> animationOnUpdateCallback;
    private Action animationFinishedCallback;


    void OnEnable()
    {
        if (runOnEnable)
            Animate();
    }

    public void SetDelay(float delay) => startDelay = delay;

    public void SetDuration(float duration) => animationDuration = duration;

    public void SetTargetScale(Vector3 targetScale) => this.targetScale = targetScale;

    public void SetUpdateCallback(Action<float> onUpdateCallback) => animationOnUpdateCallback = onUpdateCallback;

    public void SetCallback(Action callback) => animationFinishedCallback = callback;

    public void Animate()
    {
        LeanTween.cancel(gameObject);
        gameObject.transform.localScale = startScale;

        LTDescr tween = LeanTween.scale(gameObject, targetScale, animationDuration)
            .setEase(tweenType)
            .setDelay(startDelay);

        if (animationOnUpdateCallback != null)
            tween.setOnUpdate((float value) => animationOnUpdateCallback?.Invoke(value));

        if (animationFinishedCallback != null)
            tween.setOnComplete(animationFinishedCallback);
    }

    void OnDisable()
    {
        if (LeanTween.isTweening(gameObject))
            LeanTween.cancel(gameObject);
    }

    public float GetCreationDelay() => animationDuration;
}
