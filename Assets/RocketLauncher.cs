using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : MonoBehaviour
{
    public GameObject rocketPrefab;

    public float launchChance;
    public float minLaunchDelay;
    public float maxLaunchDelay;

    private float lastLaunchTime = 0f;

    void Update()
    {
        if (!GameManager.Instance.running) return;
        if (ShouldLaunch()) LaunchRocket();
    }

    void LaunchRocket()
    {
        GameManager.Instance.trashRocketsLaunched += 1;
        lastLaunchTime = Time.time;
        var rocket = Instantiate(rocketPrefab, transform.position, Quaternion.identity);

        var angle = Random.value * 360f;
        rocket.transform.eulerAngles = new Vector3(0f, 0f, angle);
    }

    private bool ShouldLaunch()
    {
        var odds = Random.value;
        var delta = Time.time - lastLaunchTime;

        if (odds < launchChance && delta > minLaunchDelay) return true;
        if (delta > maxLaunchDelay) return true;
        return false;
    }
}
