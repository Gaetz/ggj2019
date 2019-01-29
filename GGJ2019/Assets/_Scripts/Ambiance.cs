using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ambiance : MonoBehaviour
{
    public Vector3 targetAngle = new Vector3(0f, 345f, 0f);
    public float StartDist = 0.0F; //Bord gauche du niveau
    public float EndDist = 100.0F;   //Bord droit du niveau
    float Dist = 0.0F;

    private Vector3 currentAngle;
    private Vector3 startAngle;

    Light lt;
    public Color lightColor0 = Color.red;
    public Color lightColor1 = Color.blue;
    public Color fogColor0 = Color.red;
    public Color fogColor1 = Color.blue;

    public void Start()
    {
        Dist = EndDist - StartDist;
        currentAngle = transform.eulerAngles;
        startAngle = currentAngle;
        lt = GetComponent<Light>();
        RenderSettings.fog = true;
        Camera.main.clearFlags = CameraClearFlags.SolidColor;

        Application.targetFrameRate = 60;
        Screen.SetResolution(1920, 1080, true);
    }

    public void FixedUpdate()
    {
        //Debug.Log(Mathf.LerpAngle(startAngle.x, targetAngle.x, (EndDist - Camera.main.gameObject.transform.position.x)/EndDist));
        currentAngle = new Vector3(
            Mathf.LerpAngle(startAngle.x, targetAngle.x, (EndDist - Camera.main.gameObject.transform.position.x + StartDist) / EndDist),
            Mathf.LerpAngle(startAngle.y, targetAngle.y, (EndDist - Camera.main.gameObject.transform.position.x + StartDist) / EndDist),
            Mathf.LerpAngle(startAngle.z, targetAngle.z, (EndDist - Camera.main.gameObject.transform.position.x + StartDist) / EndDist));

        transform.eulerAngles = currentAngle;

        
        lt.color = Color.Lerp(lightColor1, lightColor0, (EndDist - Camera.main.gameObject.transform.position.x + StartDist) / EndDist);
        RenderSettings.fogColor = Color.Lerp(fogColor1, fogColor0, (EndDist - Camera.main.gameObject.transform.position.x + StartDist) / EndDist);
        Camera.main.backgroundColor = RenderSettings.fogColor;
    }

   
}