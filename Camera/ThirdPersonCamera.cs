using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    #region Variables
    [SerializeField] private Camera cam;
    [SerializeField] private Transform target = null;
    [SerializeField] private Transform pivotTransform = null;

    [Header("Settings")]
    [SerializeField] private Vector3 pivotOffset = new Vector3(0, 1.55f, 0);
    [SerializeField] private Vector3 camOffset = new Vector3(0, 0, -6.5f);
    [SerializeField] private Vector2 camSensitivity = new Vector2(2f, 2f);
    [SerializeField] private Vector2 limitCamY = new Vector2(-45f, 45f);
    [SerializeField] private Vector2 limitCameraZ = new Vector2(0.4f, 10f);
    [SerializeField] private float targetCameraz = 0f;  //private만들기 현재 확인용 시리얼 지우기.
    [SerializeField] private float angleLerp = 11f;
    [SerializeField] private float angleV = 0f;
    [SerializeField] private float angleH = 0f;
    [SerializeField] private float targetAngleV = 0f;
    [SerializeField] private float targetAngleH = 0f;
    [SerializeField] private Vector2 resetOffset = new Vector2(0,0);
    private float adjustedSensitivityX;
    private float adjustedSensitivityY;
    private float sensitivityMultiplier;
    private Vector2 originPivotOffset;
    private Vector2 originCamOffset;
    private Vector2 originCamSensitivity;
    private Vector2 originLimitCamY;

    [Header("FOV")]
    [SerializeField] private float originFOV = 60f;
    [SerializeField] private float originFOVLerp = 1f;
    private float targetFOV = 0f;
    private float currentFOV = 0f;
    private float currentFOVLerp;

    [Header("Wheel")]
    [SerializeField] private float wheelLerp = 4f;
    [SerializeField] private float wheelPerValue = 2f;
    [SerializeField] private Vector2 limitWheel = new Vector2(0, 9);  //최종 Z 제한 값. 
    [SerializeField]  private float currentWheelValue = 0f;      //private만들기 현재 확인용
    private float wheelValue = 0f;         
    private float targetWheelValue = 0f;       //private만들기 현재 확인용


    [Header("Collision")]
    [Header("Detect StaticObject")]
    [SerializeField] private LayerMask objectLayer;
    [SerializeField] private float detectStaticObjRadius = 0.2f;
    [SerializeField] private float staticObjSmoothDamp = 0.1f;

    [Header("Detect Opaque Plane")]   //더블체크용. 뒤집힌 plane일같은 경우.
    [SerializeField] private LayerMask opaqueLayer;
    [SerializeField] private float opaqueLerpSpeed = 11f;
    private float targetOpaqueCheckZ = 0f;
    private float opaqueDistance = 0f;
    private bool collisionDetected = false;

    [Header("Detect Ground & Wall")]
    [SerializeField] private LayerMask groundWallLayer;
    [SerializeField] private float zoomSmoothDamp = 0.001f;
    [SerializeField] private float initLerpSpeed = 4.5f;
    [SerializeField] private float wheelZoomValue = 4f;  // wheelZoomSmoothDamp 적용할 targetWheel 값.
    [SerializeField] private float wheelZoomSmoothDamp = 0.1f;
    [SerializeField] private float currentZoom;   // 시리얼 지우기.
    private float targetZoom;
    private float zoomVelocity;
    private float currentZoomSmoothDamp;

    [Header("Camera Shake")]
    [SerializeField] private float changeRate = 0.3f;
    [SerializeField] private CameraShakeInfo info;
    private CamStrength currentStrength;
    private Vector3 shakeVector = Vector3.zero;
    private Vector3 targetShakeVector = Vector3.zero;
    private bool isCamShaking = false;
    private float shakeZoom = 0f;
    private float testTime = 0f;
    private bool testShakeCam = false;

    [Header("ResetPos")]
    [SerializeField] private Vector2 resetRotation;
    public Camera MainCam => cam;

    public float testTImer;
    public Transform PivotTr => pivotTransform;

    public Vector2 GetCamSensitivity => camSensitivity;
    public Vector3 GetCamOffset => camOffset;
    public Vector3 GetCamPivotOffset => pivotOffset;
    public Vector2 GetLimitYRot => limitCamY;
    public float GetCurrentFOV => currentFOV;
    public float GetOriginFOV => originFOV;

    public Vector2 GetOriginCamSensitivity => originCamSensitivity;
    public Vector2 GetOriginCamPivotOffset => originPivotOffset;
    public Vector2 GetOriginCamOffset => originCamOffset;
    public Vector2 GetOriginLimitCamY => originLimitCamY;

    #endregion


    public Vector2 testPos;
    public Vector2 testRot;

    private bool canUseCam = true;

    public Vector3 debugRot;
    private void Awake()
    {
        if (pivotTransform == null) pivotTransform = transform.parent;
        transform.localPosition = camOffset;
        if (target != null)
            pivotTransform.position = target.position + pivotOffset;

        targetZoom = -camOffset.z;
        currentFOV = originFOV;
        targetFOV = originFOV;
        currentFOVLerp = originFOVLerp;
        canUseCam = true;

        originPivotOffset = pivotOffset ;
        originCamOffset=camOffset;
        originCamSensitivity=camSensitivity;
        originLimitCamY=limitCamY;
    }



    private void FixedUpdate()
    {
        if (target == null) return;

        if(testShakeCam && GameManager.Instance.isWriting)
        {
            Rotate();
            targetCameraz = (currentZoom - currentWheelValue - shakeZoom);
            targetCameraz = Mathf.Clamp(targetCameraz, limitCameraZ.x, limitCameraZ.y);
            transform.localPosition = Vector3.back * targetCameraz;
            return;
        }

        if (!GameManager.Instance.canUseCamera || !canUseCam || GameManager.Instance.isWriting)
        {
            pivotTransform.position = target.position + pivotOffset;
            return;
        }

        if (isCamShaking)
            testTImer += Time.deltaTime;
        else
            testTImer = 0f;

        pivotTransform.position = target.position + pivotOffset;

        Rotate();
        CameraWheel();
        FOV();
        HandleCameraCollision();
        targetCameraz = (currentZoom - currentWheelValue - shakeZoom);
        targetCameraz = Mathf.Clamp(targetCameraz, limitCameraZ.x, limitCameraZ.y);
        transform.localPosition = Vector3.back * targetCameraz;

    }

   
 

    public void TestShake()
    {
        ShakeCamera(info);
    }

    public void SetCamSensitivity(Vector2 sensi) => camSensitivity = sensi;
    public void ResetCamSensitivity() => camSensitivity = originCamSensitivity;
    public void SetCamOffset(Vector3 camOffset)
    {
        this.camOffset = camOffset;
        targetZoom = -this.camOffset.z;
    }
    public void ResetCamOffset()
    {
        this.camOffset = originCamOffset;
        targetZoom = -this.camOffset.z;
    }
    public void SetCamPivotOffset(Vector3 sensi) => camSensitivity = sensi;
    public void ResetCamPivotOffset() => camSensitivity = originCamSensitivity;
    public void SetLimitCamYRot(Vector2 limitRot) => limitCamY = limitRot;
    public void ResetLimitCamYRot() => limitCamY = originLimitCamY;


    public void ResetRotation()
    {
        Debug.Log("Reset Cam Rotation!");
        Debug.Log($"R: {target}");
        Debug.Log($"R: {target.forward}");
        Debug.Log($"R: {pivotTransform.transform.rotation}");
        Debug.Log($"R: angleV {angleV}    targetV  {targetAngleV} ");
        Debug.Log($"R: angleV {angleH}    targetV  {targetAngleH} ");


        Vector3 dir = target.forward;
        dir.y = 0f; // y값 제거해서 수평 방향만 보게
        if (dir == Vector3.zero) dir = Vector3.forward;

        Quaternion look = Quaternion.LookRotation(dir.normalized);
        targetAngleH = look.eulerAngles.y + resetOffset.x;
        angleH = targetAngleH;

        targetAngleV = resetOffset.y;
        angleV = targetAngleV;

        float tempV = angleV;
        float tempH = angleH;


        Rotate();
        CameraWheel();
        FOV();
        HandleCameraCollision();
        targetCameraz = (currentZoom - currentWheelValue - shakeZoom);
        targetCameraz = Mathf.Clamp(targetCameraz, limitCameraZ.x, limitCameraZ.y);
        transform.localPosition = Vector3.back * targetCameraz;

        Debug.Log($"--R: {pivotTransform.transform.rotation}");
        Debug.Log($"--R: angleV {angleV}    targetV  {targetAngleV} ");
        Debug.Log($"--R: angleV {angleH}    targetV  {targetAngleH} ");


        pivotTransform.transform.rotation = Quaternion.Euler(tempV, tempH, 0.0f);
        Debug.Log($"++R: {pivotTransform.transform.rotation}");
        Debug.Log($"++R: angleV {angleV}    targetV  {targetAngleV} ");
        Debug.Log($"++R: angleV {angleH}    targetV  {targetAngleH} ");

    }

    public void ResetRotation(Transform targetT)
    {
        Vector3 dir = targetT.forward;
        dir.y = 0f; // y값 제거해서 수평 방향만 보게
        if (dir == Vector3.zero) dir = Vector3.forward;

        Quaternion look = Quaternion.LookRotation(dir.normalized);
        targetAngleH = look.eulerAngles.y + resetOffset.x;
        angleH = targetAngleH;

        targetAngleV = resetOffset.y;
        angleV = targetAngleV;
    }

    public void CamRotation(Vector2 rotVec2)
    {
        canUseCam = false;

        targetAngleH = rotVec2.y + resetOffset.x;
        angleH = rotVec2.y + resetOffset.x;
        targetAngleV = rotVec2.x + resetOffset.y;
        angleV = rotVec2.x + resetOffset.y;
        canUseCam = true;
    }

    private void FOV()
    {
        currentFOV = Mathf.Lerp(currentFOV, targetFOV, currentFOVLerp);
        cam.fieldOfView = currentFOV;
    }

    private void Rotate()
    {
        sensitivityMultiplier = Mathf.Clamp(1.0f / Time.deltaTime, 0.1f, 10.0f);
        adjustedSensitivityX = camSensitivity.x * sensitivityMultiplier;
        adjustedSensitivityY = camSensitivity.y * sensitivityMultiplier;

        targetAngleV -= Input.GetAxis("Mouse Y") * adjustedSensitivityX * Time.deltaTime;
        targetAngleH += Input.GetAxis("Mouse X") * adjustedSensitivityY * Time.deltaTime;
        targetAngleV = Mathf.Clamp(targetAngleV, limitCamY.x, limitCamY.y);

        angleV = Mathf.Lerp(angleV, targetAngleV, angleLerp * Time.deltaTime);
        angleH = Mathf.Lerp(angleH, targetAngleH, angleLerp * Time.deltaTime);

        if (isCamShaking)
            pivotTransform.transform.rotation = Quaternion.Euler(shakeVector) * Quaternion.Euler(angleV, angleH, 0.0f);
        else
            pivotTransform.transform.rotation = Quaternion.Euler(angleV, angleH, 0.0f);

    }


    private void CameraWheel()
    {
        wheelValue = Input.GetAxis("Mouse ScrollWheel") * wheelPerValue;

        if (wheelValue > 0f && targetCameraz > limitCameraZ.x)
            targetWheelValue += wheelValue;
        else if (wheelValue < 0f && targetCameraz < limitCameraZ.y)
            targetWheelValue += wheelValue;

        targetWheelValue = Mathf.Clamp(targetWheelValue, limitWheel.x, limitWheel.y);
        currentWheelValue = Mathf.Lerp(currentWheelValue, targetWheelValue, wheelLerp * Time.deltaTime);
    }

    #region Collision 

    private bool HandleCameraCollision()
    {
        if (CollisionStaticTargetToCam()) collisionDetected = true;
        else if (DetectCollisionGroundWall()) collisionDetected = true;
        else if (CollisionOpaqueCheck()) collisionDetected = true;
        else currentZoom = Mathf.Lerp(currentZoom, targetZoom, initLerpSpeed * Time.deltaTime);

        return collisionDetected;
    }

    private bool DetectCollisionGroundWall()
    {
        // + 일경우 줌 인,  -일 경우 줌 아웃.
        Ray ray = new Ray(transform.parent.position, -transform.parent.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit,  targetZoom + 0.7f, groundWallLayer))
        {
            currentZoomSmoothDamp = targetWheelValue >= wheelZoomValue ? wheelZoomSmoothDamp : zoomSmoothDamp;
            currentZoom = Mathf.SmoothDamp(currentZoom, hit.distance * 0.9f, ref zoomVelocity, currentZoomSmoothDamp);
            return true;
        }
        else
            return false;
    }

    private bool CollisionStaticTargetToCam()
    {
        Ray ray = new Ray(transform.parent.position, -transform.parent.forward);
        RaycastHit hit;
        if (Physics.SphereCast(ray, detectStaticObjRadius, out hit, targetZoom + 0.4f, objectLayer))
        {
            currentZoom = Mathf.SmoothDamp(currentZoom, hit.distance , ref zoomVelocity, staticObjSmoothDamp);
            return true;
        }
        else
            return false;
    }


    private bool CollisionOpaqueCheck()
    {
        for (targetOpaqueCheckZ = camOffset.z; targetOpaqueCheckZ < 0f; targetOpaqueCheckZ += 0.05f)
            if (!CollisionOpaqueCamToTarget(targetOpaqueCheckZ))
                break;

        if (Mathf.Abs(targetOpaqueCheckZ) < Mathf.Abs(camOffset.z))
        {
            targetOpaqueCheckZ = targetOpaqueCheckZ * 0.85f;
            currentZoom = Mathf.Lerp(currentZoom, -targetOpaqueCheckZ, opaqueLerpSpeed * Time.deltaTime);
            return true;
        }
        else
            return false;

    }


    private bool CollisionOpaqueCamToTarget(float distance)
    {
        opaqueDistance = ((target.position + pivotOffset) - transform.position).magnitude;

        if (Physics.Linecast((target.position + pivotOffset) + transform.forward * distance, transform.position + transform.forward * opaqueDistance, opaqueLayer))
            return true;
        else
            return false;
    }

    #endregion


    #region Gizmo
    private void OnDrawGizmos()
    {
        if (target == null || cam == null)
            return;
        DrawCameraPos();
    }

    private void DrawCameraPos()
    {
        if (Physics.Linecast((target.position + pivotOffset) + transform.forward * targetOpaqueCheckZ, transform.position + transform.forward * opaqueDistance, objectLayer))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine((target.position + pivotOffset) + transform.forward * targetOpaqueCheckZ, transform.position + transform.forward * opaqueDistance);
        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine((target.position + pivotOffset) + transform.forward * targetOpaqueCheckZ, transform.position + transform.forward * opaqueDistance);
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawLine((target.position + pivotOffset) + transform.forward * targetOpaqueCheckZ, transform.position + transform.forward * opaqueDistance);



        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 0.3f);

    }

    #endregion


    public void ResetCameraFOV() => targetFOV = originFOV;

    public void SetTarget(GameObject go)
    {
        target = go.transform;
    }

    public void SetCamFOV(float fov, float lerp)
    {
        currentFOVLerp = lerp <= 0 ? originFOVLerp : lerp;
        targetFOV = fov;
    }

    
    #region Camera Shake

    public void ShakeCamera(CameraShakeInfo info, bool testShakeCam = false)
    {
        if (info == null) return;
        if (currentStrength > info.camStrength && !info.isIgnoreStrength) return;

        this.testShakeCam = testShakeCam;

        switch (info.shakeType)
        {
            case CamShakeType.NONE: return;
            case CamShakeType.SMOOTH_COUNT: 
                SmoothShakeCam_Count(info.count, info.lerpSpeed, info.minShakePositon, info.maxShakePositon);
                break;
            case CamShakeType.IMMEDIATE_COUNT:
                ImmediateShakeCam_Count(info.count, info.changeRate, info.minShakePositon, info.maxShakePositon);
                break;
            case CamShakeType.SMOOTH_TIME:
                SmoothShakeCam_Time(info.duration, info.lerpSpeed, info.minShakePositon, info.maxShakePositon);
                break;
            case CamShakeType.IMMEDIATE_TIME:
                ImmediateShakeCam_Time(info.duration, info.changeRate, info.minShakePositon, info.maxShakePositon);
                break;
            case CamShakeType.SMOOTH_REDUCE_TIME:
                ReduceSmoothShakeCam(info.duration, info.lerpSpeed, info.reduceRate, info.targetShakePositon);
                break;
            case CamShakeType.IMMEDIATE_REDUCE_TIME:
                ReduceImmediateShakeCam(info.duration, info.lerpSpeed, info.reduceRate, info.targetShakePositon);
                break;
            case CamShakeType.CURVE_Z:
                CurveZ(info.duration, info.curveZ);
                break;
            case CamShakeType.CURVE_VECTOR3:
                CurveVector3(info.duration, info.curveX, info.curveY, info.curveZ , info.minShakePositon, info.maxShakePositon);
                break;
        }
    }

    private void ImmediateShakeCam_Time(float duration, float changeRate, Vector3 minShakeRange, Vector3 maxShakeRange)
    {
        StopAllCoroutines();
        StartCoroutine(ImmediateShakeTime_Co(duration, changeRate, minShakeRange, maxShakeRange));
    }
    private void SmoothShakeCam_Time(float duration, float lerpSpeed, Vector3 minShakeRange, Vector3 maxShakeRange)
    {
        StopAllCoroutines();
        StartCoroutine(SmoothShakeTime_Co(duration, lerpSpeed, minShakeRange, maxShakeRange));
    }
    private void SmoothShakeCam_Count(int count, float lerpSpeed, Vector3 minShakeRange, Vector3 maxShakeRange)
    {
        StopAllCoroutines();
        StartCoroutine(SmoothShakeCount_Co(count, lerpSpeed, minShakeRange, maxShakeRange));
    }
    private void ImmediateShakeCam_Count(int count, float changeRate, Vector3 minShakeRange, Vector3 maxShakeRange)
    {
        StopAllCoroutines();
        StartCoroutine(ImmediateShakeCount_Co(count, changeRate, minShakeRange, maxShakeRange));
    }
    private void ReduceSmoothShakeCam(float duration, float lerpSpeed, float reduceRate, Vector3 targetShakeRange)
    {
        StopAllCoroutines();
        StartCoroutine(ReduceSmoothShakeTime_Co(duration, lerpSpeed, targetShakeRange, reduceRate));
    }
    private void ReduceImmediateShakeCam(float duration, float changeRate, float reduceRate, Vector3 targetShakeRange)
    {
        StopAllCoroutines();
        StartCoroutine(ReduceImmediateShakeTime(duration, changeRate, targetShakeRange, reduceRate));
    }

    //Time
    private IEnumerator SmoothShakeTime_Co(float duration, float lerpSpeed, Vector3 minShakeRange, Vector3 maxShakeRange)
    {
        float currentTime = 0f;
        isCamShaking = true;
        shakeVector = Vector3.zero;
        testTime = 0f;   //삭제

        while (currentTime < duration)
        {
            float shakeOffsetX = Random.Range(minShakeRange.x, maxShakeRange.x);
            float shakeOffsetY = Random.Range(minShakeRange.y, maxShakeRange.y);
            float shakeOffsetZ = Random.Range(minShakeRange.z, maxShakeRange.z);
            targetShakeVector = new Vector3(shakeOffsetX, shakeOffsetY, shakeOffsetZ);

            while (Vector3.Distance(shakeVector, targetShakeVector) > 0.1f && currentTime < duration)
            {
                shakeVector = Vector3.Lerp(shakeVector, targetShakeVector, lerpSpeed * Time.deltaTime);
                currentTime += Time.deltaTime;
                yield return null;
            }
            yield return null;
        }

        while (Vector3.Distance(shakeVector, Vector3.zero) > 0f)
        {
            shakeVector = Vector3.Lerp(shakeVector, Vector3.zero, lerpSpeed * Time.deltaTime);

            if (Vector3.Distance(shakeVector, Vector3.zero) < 0.1f)
                shakeVector = Vector3.zero;

            yield return null;
        }
        currentStrength = CamStrength.NONE;
        isCamShaking = false;
        testShakeCam = false;
    }
    private IEnumerator ImmediateShakeTime_Co(float duration, float changeRate, Vector3 minShakeRange, Vector3 maxShakeRange)
    {
        float currentTime = 0f;
        isCamShaking = true;
        shakeVector = Vector3.zero;
     

        while (currentTime <= duration)
        {
            float shakeOffsetX = Random.Range(minShakeRange.x, maxShakeRange.x);
            float shakeOffsetY = Random.Range(minShakeRange.y, maxShakeRange.y);
            float shakeOffsetZ = Random.Range(minShakeRange.z, maxShakeRange.z);
            shakeVector = new Vector3(shakeOffsetX, shakeOffsetY, shakeOffsetZ);

            currentTime += Time.deltaTime + changeRate;
            yield return new WaitForSeconds(changeRate);
        }
        currentStrength = CamStrength.NONE;
        shakeVector = Vector3.zero;
        isCamShaking = false;
        testShakeCam = false;
    }

    //Count
    private IEnumerator SmoothShakeCount_Co(int count, float lerpSpeed, Vector3 minShakeRange, Vector3 maxShakeRange)
    {
        int currentCount = 0;
        shakeVector = Vector3.zero;
        targetShakeVector = Vector3.zero;
        isCamShaking = true;

        while (currentCount < count)
        {
            float shakeOffsetX = Random.Range(minShakeRange.x, maxShakeRange.x);
            float shakeOffsetY = Random.Range(minShakeRange.y, maxShakeRange.y);
            float shakeOffsetZ = Random.Range(minShakeRange.z, maxShakeRange.z);
            targetShakeVector = new Vector3(shakeOffsetX, shakeOffsetY, shakeOffsetZ);

            while (Vector3.Distance(shakeVector, targetShakeVector) > 0.1f)
            {
                shakeVector = Vector3.Lerp(shakeVector, targetShakeVector, lerpSpeed * Time.deltaTime);
                yield return null;
            }
            currentCount++;
            Debug.Log(currentCount + " / " + count);

        }
        Debug.Log(Vector3.Distance(shakeVector, Vector3.zero));

        while (Vector3.Distance(shakeVector, Vector3.zero) > 0f)
        {
            shakeVector = Vector3.Lerp(shakeVector, Vector3.zero, lerpSpeed * Time.deltaTime);

            if (Vector3.Distance(shakeVector, Vector3.zero) < 0.1f)
            {
                shakeVector = Vector3.zero;
            }
            yield return null;
        }
        currentStrength = CamStrength.NONE;

        isCamShaking = false;
        testShakeCam = false;
    }
    private IEnumerator ImmediateShakeCount_Co(int count, float changeRate, Vector3 minShakeRange, Vector3 maxShakeRange)
    {
        float currentCount = 0f;
        isCamShaking = true;
        shakeVector = Vector3.zero;

        while (currentCount < count)
        {
            float shakeOffsetX = Random.Range(minShakeRange.x, maxShakeRange.x);
            float shakeOffsetY = Random.Range(minShakeRange.y, maxShakeRange.y);
            float shakeOffsetZ = Random.Range(minShakeRange.z, maxShakeRange.z);
            shakeVector = new Vector3(shakeOffsetX, shakeOffsetY, shakeOffsetZ);

            yield return new WaitForSeconds(changeRate);
            currentCount++;
            Debug.Log(currentCount + "/" + count);

        }
        currentStrength = CamStrength.NONE;

        shakeVector = Vector3.zero;
        isCamShaking = false;
        testShakeCam = false;
    }

    //Reduce
    private IEnumerator ReduceSmoothShakeTime_Co(float duration, float lerpSpeed, Vector3 shakeRange, float reduceRate)
    {
        isCamShaking = true;
        float currentTime = 0f;
        int index = 0;
        shakeVector = Vector3.zero;
        Debug.Log(shakeRange);
        targetShakeVector = new Vector3(shakeRange.x, shakeRange.y, shakeRange.z);
        Vector2 negative = -targetShakeVector;
        Vector2 positive = targetShakeVector;

       
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            while (Mathf.Abs(Vector3.Distance(shakeVector, targetShakeVector)) > 0.1f && currentTime < duration)
            {
                currentTime += Time.deltaTime;
                shakeVector = Vector3.Lerp(shakeVector, targetShakeVector, lerpSpeed * Time.deltaTime);
                if (Mathf.Abs(Vector3.Distance(shakeVector, targetShakeVector)) < 0.05f)
                {
                    shakeVector = targetShakeVector;
                    break;
                }
                yield return null;
            }
            Debug.Log("Dis ; " + Vector3.Distance(shakeVector, targetShakeVector));

            if (index % 2 == 0)
            {
                if (negative.x < 0)
                {
                    negative.x += reduceRate;
                    if (negative.x >= 0) negative.x = 0;
                }
                if (negative.y < 0)
                {
                    negative.y += reduceRate;
                    if (negative.y >= 0) negative.y = 0;
                }
                targetShakeVector.x = negative.x;
                targetShakeVector.y = negative.y;

                negative.x += reduceRate;
                negative.y += reduceRate;
                if (negative.x >= 0) negative.x = 0;
                if (negative.y >= 0) negative.y = 0;
            }
            else
            {
                if (positive.x > 0)
                {
                    positive.x -= reduceRate;
                    if (positive.x <= 0) positive.x = 0;
                }
                if (positive.y > 0)
                {
                    positive.y -= reduceRate;
                    if (positive.y <= 0) positive.y = 0;
                }
                targetShakeVector.x = positive.x;
                targetShakeVector.y = positive.y;

                positive.x -= reduceRate;
                positive.y -= reduceRate;
                if (positive.x <= 0) positive.x = 0;
                if (positive.y <= 0) positive.y = 0;
            }

            index++;
            yield return null;

            Debug.Log("2");

        }


        while (Vector3.Distance(shakeVector, Vector3.zero) > 0f)
        {
            shakeVector = Vector3.Lerp(shakeVector, Vector3.zero, lerpSpeed * Time.deltaTime);
            Debug.Log("3");

            if (Vector3.Distance(shakeVector, Vector3.zero) < 0.1f)
                shakeVector = Vector3.zero;

            yield return null;
        }
        targetShakeVector = Vector3.zero;
        currentStrength = CamStrength.NONE;
        isCamShaking = false;
        testShakeCam = false;
        Debug.Log("4");

    }
    private IEnumerator ReduceImmediateShakeTime(float duration, float ChangeRate, Vector3 shakeRange, float reduceRate)
    {
        isCamShaking = true;
        float currentTime = 0f;
        int index = 0;
        targetShakeVector = new Vector3(shakeRange.x, shakeRange.y, shakeRange.z);
        Vector2 negative = -targetShakeVector;
        Vector2 positive = targetShakeVector;
        while (currentTime < duration)
        {
            shakeVector = targetShakeVector;
            currentTime += Time.deltaTime + changeRate;
            yield return new WaitForSeconds(ChangeRate);

            if (index % 2 == 0)
            {
                if (negative.x < 0)
                {
                    negative.x += reduceRate;
                    if (negative.x >= 0) negative.x = 0;
                }
                if (negative.y < 0)
                {
                    negative.y += reduceRate;
                    if (negative.y >= 0) negative.y = 0;
                }
                targetShakeVector.x = negative.x;
                targetShakeVector.y = negative.y;

                negative.x += reduceRate;
                negative.y += reduceRate;
                if (negative.x >= 0) negative.x = 0;
                if (negative.y >= 0) negative.y = 0;
            }
            else
            {
                if (positive.x > 0)
                {
                    positive.x -= reduceRate;
                    if (positive.x <= 0) positive.x = 0;
                }
                if (positive.y > 0)
                {
                    positive.y -= reduceRate;
                    if (positive.y <= 0) positive.y = 0;
                }
                targetShakeVector.x = positive.x;
                targetShakeVector.y = positive.y;

                positive.x -= reduceRate;
                positive.y -= reduceRate;
                if (positive.x <= 0) positive.x = 0;
                if (positive.y <= 0) positive.y = 0;
            }
            index++;
        }
        currentStrength = CamStrength.NONE;
        targetShakeVector = Vector3.zero;
        isCamShaking = false;
        testShakeCam = false;

    }

    #endregion


    /// <summary>
    ///  duration 시간동안 카메라 Z를 curve의 수치만큼 조절.
    /// </summary>
    public void CurveZ(float durationTime, AnimationCurve curve)
    {
        StopAllCoroutines();
        StartCoroutine(CurveZ_Co(durationTime, curve));
    }

    public void CurveVector3(float durationTime, AnimationCurve curveX, AnimationCurve curveY, AnimationCurve curveZ, Vector3 minVec, Vector3 maxVec)
    {
        StopAllCoroutines();
        StartCoroutine(CurveVector_Co(durationTime, curveX, curveY, curveZ,minVec, maxVec ));
    }

    private IEnumerator CurveZ_Co(float durationTIme, AnimationCurve curve)
    {
        float timer = 0f;
        float normalizedTime;

        while (timer < durationTIme)
        {
            timer += Time.deltaTime;
            normalizedTime = timer / durationTIme;
            shakeZoom = curve.Evaluate(normalizedTime);
            yield return null;
        }

        while (shakeZoom > 0)
        {
            shakeZoom = Mathf.Lerp(shakeZoom, 0, Time.deltaTime);
            yield return null;
        }

        shakeZoom = 0;
        testShakeCam = false;
    }



    private IEnumerator CurveVector_Co(float durationTIme, AnimationCurve curveX, AnimationCurve curveY, AnimationCurve curveZ, Vector3 minVec,Vector3 maxVec)
    {
        isCamShaking = true;
        float timer = 0f;
        float normalizedTime;

        Vector3 random = Vector3.zero;
        if (minVec == Vector3.zero && maxVec == Vector3.zero) random = Vector3.one;
        else
        {
            random.x = Random.Range(minVec.x, maxVec.x);
            random.y = Random.Range(minVec.y, maxVec.y);
            random.z = Random.Range(minVec.z, maxVec.z);
        }

        while (timer < durationTIme)
        {
            timer += Time.deltaTime;
            normalizedTime = timer / durationTIme; 

            shakeVector.x = curveX.Evaluate(normalizedTime) * random.x;
            shakeVector.y = curveY.Evaluate(normalizedTime) * random.y;
            shakeZoom = curveZ.Evaluate(normalizedTime) * random.z;
            //Debug.Log("진행중" + timer / durationTIme);
            yield return null;
        }

        while (Vector3.Distance(shakeVector, Vector3.zero) > 0.05f)
        {
            shakeVector = Vector3.Lerp(shakeVector, Vector3.zero, Time.deltaTime);
           // Debug.Log("zero 진행중" + timer);
            yield return null;
        }

        while (shakeZoom > 0)
        {
            shakeZoom = Mathf.Lerp(shakeZoom, 0, Time.deltaTime);
            yield return null;
        }

        shakeVector = Vector3.zero;
        isCamShaking = false;
        testShakeCam = false;
    }


}
