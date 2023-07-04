using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    [SerializeField] private int halfNum;
    [SerializeField] private float offset;
    [SerializeField] private float speed = 10f,getBackDistance=5f;
    [SerializeField] private Vector3 TargetInit = new(0.300000012f, 6.30000019f, -18f);
    [SerializeField] private Vector3 TargetInit1 = new(0.300000012f, 6.30000019f, -18f);
    [SerializeField] private GameObject flash;
    private float _stayTimer = 0.3f;
    [SerializeField] private GameObject[] lightPathImages;
    [SerializeField] private float duration = 2f;
    [SerializeField] private Material openDoorMaterial;
    private Animator _animator;
    private float _angle;
    
    private List<Transform> _childs = new();
    private bool _loadNextLevel,_wrong,_getBack, _doorAppear,_touchBallon;
    public static bool moveToVitraje,_shake,_win;
    private bool _triggerStay = true;
    private Vector3 _target;
    private Vector3 _firstPos;
    private GameObject _klyde;
   
    
     // private float _requiredStayTime = 0.3f;

    private void Start()
    {
        GetComponent<AudioSource>().Stop();
        _target = TargetInit;
        _animator = GetComponent<Animator>();
        foreach (var child in GetComponentsInChildren<Transform>())
        {
            if (child != transform)
            {
                _childs.Add(child);
               // child.GetComponent<MeshRenderer>().material.color = Color.gray;
            }
        }
        _angle = 360 /(float) LevelManager.GetNumOfHalves();
    }

    void Update()
    {
        if (_win)
        {
            moveToVitraje = true;
            CalculateLightPathPos();
            // MoveToVitrajWinCase();
        }
        else moveToVitraje = false;
        if ((RecognizeShape.GetRecognizeShape() && !_doorAppear) || LevelManager.GetLevel() == 0)
        {
            foreach (var child in _childs)
            {
                if(child.GetComponent<MeshRenderer>()!=null)
                    child.GetComponent<MeshRenderer>().material = openDoorMaterial;
            }
            _doorAppear = true;
        }
        // CalculateLightPathPos();
        float currAngle = CalculateDoorPos();

        // Calculate the adjusted angle range based on the number of halves
        float startAngle = offset + _angle * (halfNum - 1);
        float endAngle = offset + _angle * halfNum;

        // Handle wraparound from 360 to 0 degrees
        if (endAngle > 360f)
            endAngle -= 360f;

        // Check if the current angle is within the adjusted range
        if (IsAngleWithinRange(currAngle, startAngle, endAngle))
        {
            EnabledDoorChild(true);
        }
        else
        {
            EnabledDoorChild(false);
        }

        // CalculateLightPathPos();
    }


    private void EnabledDoorChild(bool enabled)
    {
        // print("enabled door child"+enabled);
        foreach (var child in _childs)
        {
            if(child.GetComponent<MeshRenderer>()!=null)
                child.GetComponent<MeshRenderer>().enabled = enabled;
            if(child.GetComponent<SpriteRenderer>()!=null)
                child.GetComponent<SpriteRenderer>().enabled = enabled;
            if(child.GetComponent<ParticleSystem>()!=null)
                child.gameObject.SetActive(enabled);
        }
        GetComponent<Collider>().enabled = enabled;
        GetComponent<CapsuleCollider>().enabled = enabled;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("klyde"))
            _animator.SetBool("moveButton",true);
        // print("_doorAppear "+_doorAppear);
        if(other.gameObject.CompareTag("klyde") && _doorAppear)
        {
            print("klyde win");
            //  GetComponent<PlayableDirector>().Play();
            _klyde = other.gameObject;
            // _klyde.GetComponent<NavMeshAgent>().SetDestination(_klyde.transform.position);
            Destroy(_klyde.GetComponent<NavMeshAgent>());
            _win = true;
            _klyde.transform.eulerAngles = new Vector3(_klyde.transform.eulerAngles.x, 180, _klyde.transform.eulerAngles.z);
            _doorAppear = false;
            _target = TargetInit;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("klyde")&& !moving.GetIsWalk() && 
            (!RecognizeShape.GetRecognizeShape() && LevelManager.GetLevel() !=0))
        {
            if (_triggerStay)
            {
                _triggerStay = false;
                StartCoroutine(ShakeCam());
            }
        } 
    }

    private void OnTriggerExit(Collider other)
    {
        _triggerStay = true;
        _animator.SetBool("moveButton",false);
    }

    private IEnumerator ShakeCam()
    {
        Vibration.Vibrate(500);
        _shake = true;
        if (!GetComponent<AudioSource>().isPlaying)
            GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(_stayTimer);
        _triggerStay = false;
    }

    private bool IsAngleWithinRange(float angle, float start, float end)
    {
        // Normalize the angles to ensure proper comparison
        angle = NormalizeAngle(angle);
        start = NormalizeAngle(start);
        end = NormalizeAngle(end);

        // Check if the angle is within the specified range
        if (start <= end)
        {
            return angle >= start && angle <= end;
        }
        else
        {
            return angle >= start || angle <= end;
        }
    }

// Helper method to normalize an angle to the range of 0-360 degrees
    private float NormalizeAngle(float angle)
    {
        if (angle < 0f)
        {
            angle %= 360f;
            angle += 360f;
        }
        else if (angle >= 360f)
        {
            angle %= 360f;
        }
        return angle;
    }


    private void MoveToVitrajWinCase()
    {
        print("MoveToVitrajWinCase");
        // _klyde.GetComponent<NavMeshAgent>().enabled = false;
        moving.SetWalkAnimationTrue();
        _klyde.transform.position = Vector3.MoveTowards(_klyde.transform.position, _target,
             Time.deltaTime*speed);
        // print(_target+" target");
        
        if (Vector3.Distance(_klyde.transform.position, _target) < 0.7f && !_loadNextLevel)
        {
            _loadNextLevel = true;
            flash.gameObject.SetActive(true);
            LevelManager.NextLevel();
            _win = false;
            _klyde.SetActive(false);
        }
    }

    public static bool GetShake()
    {
        return _shake;
    }
    public static void SetShake()
    { 
        _shake = false;
    }

    public static bool GetWin()
    {
        return _win;
    }

    private void CalculateLightPathPos()
    {
        var currAngle = CalculateDoorPos();
        var index = 0;
        var range = 0;
        var start = 0;
        switch (LevelManager.GetLevel())
        {          
            case 0:
            case 1:
            case 2:
                range = 62 / 4;
                if (currAngle > 0 && currAngle < range+5)
                    index = 0;
                else if (currAngle > range+ 5 && currAngle < (range*2))
                    index = 1;
                else if (currAngle > (range*2)-5 && currAngle < (range * 3)+5)
                    index = 2;
                else
                    index = 3;
                break;
            case 3:
                start = 165;
                range = 11;
                if (currAngle > start)
                    index = 0;
                else if (currAngle < start && currAngle > start - range)
                    index = 1;
                else
                    index = 2;
                break;
            case 4:
                range = 11;
                start = 211;
                if (currAngle > start)
                    index = 0;
                else if (currAngle < start && currAngle > start - range)
                    index = 1;
                else if (currAngle < start - range && currAngle > start - (range * 2))
                    index = 2;
                else if (currAngle < start - (range * 2) && currAngle > start - (range * 5))
                    index = 3;
                else if (currAngle < start - (range * 5) && currAngle > start - (range * 6))
                    index = 4;
                else
                    index = 5;
                break;
            case 5:
                range = 11;
                start = 222;
                if (currAngle > start)
                    index = 0;
                else if (currAngle < start && currAngle > start - range)
                    index = 1;
                else if (currAngle < start - range && currAngle > start - (range * 2))
                    index = 2;
                else if (currAngle < start - (range * 2) && currAngle > start - (range * 5))
                    index = 3;
                else if (currAngle < start - (range * 5) && currAngle > start - (range * 6))
                    index = 4;
                else if(currAngle < start - (range * 6) && currAngle > start - (range * 7))
                    index = 5;
                else
                    index = 6;
                break;
            case 6:
                range = 8;
                start = 344;
                if (currAngle > start)
                    index = 0;
                else if (currAngle < start && currAngle > start - range)
                    index = 1;
                else if (currAngle < start - range && currAngle > start - (range * 2))
                    index = 2;
                else if (currAngle < start - (range * 2) && currAngle > start - (range * 3))
                    index = 3;
                else if (currAngle < start - (range * 3) && currAngle > start - (range * 4))
                    index = 4;
                else if(currAngle < start - (range * 4) && currAngle > start - (range * 5))
                    index = 5;
                else
                    index = 6;
                break;

        }
        ShowImage(lightPathImages[index]);
    }

    private void ShowImage(GameObject image)
    {
        _klyde.GetComponentInChildren<SkinnedMeshRenderer>().material.renderQueue = 3000;
        // mat. = 4000;
        image.GetComponent<MeshRenderer>().enabled = true;
        StartCoroutine(FadeImageAlpha(image.GetComponent<MeshRenderer>().material, 0f, 1f)); 
    }
    private IEnumerator FadeImageAlpha(Material mat, float startAlpha, float endAlpha)
    {
        float startTime = Time.time;
        float endTime = startTime + duration;

        while (Time.time < endTime)
        {
            float normalizedTime = (Time.time - startTime) / duration;
            float currentAlpha = Mathf.Lerp(startAlpha, endAlpha, normalizedTime);
            mat.SetFloat("_Alpha", currentAlpha); // Updated property name
            yield return null;
        }

        mat.SetFloat("_Alpha", endAlpha); // Updated property name
        MoveToVitrajWinCase();
    }
    
    

    private float CalculateDoorPos()
    {
        var trans = transform;
        Vector3 direction = trans.position - Vector3.zero;
        // Calculate the angle between the direction vector and the forward vector
        float currAngle = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up);
        if (currAngle < 0) currAngle += 360f;
        print(currAngle+ "currAngle");
        return currAngle;
    }
}
