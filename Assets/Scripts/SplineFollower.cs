using UnityEngine;
using UnityEngine.Splines;

public class SplineFollower : MonoBehaviour, IPoolable
{
    [SerializeField] SplineAnimate splineAnimate;

    public void ResetPoolable()
    {
        //pass 
    }

    private void Awake()
    {
        splineAnimate.Completed += () => gameObject.SetActive(false);
    }

    public void SetSpline(SplineContainer spline, float speed)
    {
        splineAnimate.Container = spline;
        splineAnimate.MaxSpeed = speed;
        splineAnimate.AnimationMethod = SplineAnimate.Method.Speed;
        splineAnimate.Restart(true);
    }
}
