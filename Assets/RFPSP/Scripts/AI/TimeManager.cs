using UnityEngine;

public class TimeManager : MonoBehaviour {
    public static TimeManager instance;

    public float slowdownFactor = 0.05f;
	public float slowdownLength = 2f;
    private void Awake()
    {
        instance = this;
    }
    void Update ()
	{
		Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
		Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
	}

	public void DoSlowmotion ()
	{
		Time.timeScale = slowdownFactor;
		Time.fixedDeltaTime = Time.timeScale * .02f;
	}

}
