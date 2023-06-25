using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class Door : MonoBehaviour
{
    [SerializeField] private int halfNum;
    [SerializeField] private float offset;
    [SerializeField] private float speed = 10f,getBackDistance=5f;
    [SerializeField] private Vector3 TargetInit = new(0.300000012f, 6.30000019f, -18f);
    [SerializeField] private GameObject flash;
    [SerializeField] private float _stayTimer = 0.3f;
 
    private float _angle;
    private List<Transform> _childs = new();
    private bool _win, _rotateOnce,_wrong,_getBack, _doorAppear,_touchBallon;
    public static bool moveToVitraje,_shake;
    private bool _triggerStay = true;
    private Vector3 _target;
    private Vector3 _firstPos;
    private GameObject _klyde;
    
     // private float _requiredStayTime = 0.3f;

    private void Start()
    {
        GetComponent<AudioSource>().Stop();
        _target = TargetInit;
        foreach (var child in GetComponentsInChildren<Transform>())
        {
            if (child != transform)
            {
                _childs.Add(child);
                child.GetComponent<MeshRenderer>().material.color = Color.gray;
            }
        }
        _angle = 360 /(float) LevelManager.GetNumOfHalves();
    }

    void Update()
    {
        if (_win)
        {
            moveToVitraje = true;
            MoveToVitrajWinCase();
        }
        else moveToVitraje = false;
        if ((RecognizeShape.GetRecognizeShape() && !_doorAppear) || LevelManager.GetLevel() == 0)
        {
            foreach (var child in _childs)
            {
                child.GetComponent<MeshRenderer>().material.color = new Color32(200, 111, 103, 255);
            }
            _doorAppear = true;
        }

        var trans = transform;
        Vector3 direction = trans.position - Vector3.zero;
        // Calculate the angle between the direction vector and the forward vector
        float currAngle = Vector3.SignedAngle(Vector3.forward, direction, Vector3.up);
        if (currAngle < 0) currAngle += 360f;

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
    }


    private void EnabledDoorChild(bool enabled)
    {
        print("enabled door child"+enabled);
        foreach (var child in GetComponentsInChildren<MeshRenderer>())
        {
            child.GetComponent<MeshRenderer>().enabled = enabled;
        }
        GetComponent<Collider>().enabled = enabled;
    }
    //todo: understand how collider work with ballon

    private void OnTriggerEnter(Collider other)
    {
        print("_doorAppear "+_doorAppear);
        if(other.gameObject.CompareTag("klyde") && _doorAppear)
        {
             print("klyde win");
           //  GetComponent<PlayableDirector>().Play();
             _klyde = other.gameObject;
             _win = true;
             _klyde.transform.eulerAngles =
                 new Vector3(_klyde.transform.eulerAngles.x, 180, _klyde.transform.eulerAngles.z);
             moving.SetWalkAnimationFalse();
             other.GetComponent<NavMeshAgent>().enabled = false;
            // other.transform.parent = transform;
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
        print("win");
        _klyde.GetComponent<NavMeshAgent>().enabled = false;
        _klyde.GetComponent<moving>().enabled = false;
        _klyde.transform.position = Vector3.MoveTowards(_klyde.transform.position, _target,
            Time.deltaTime * speed);
        if (!_rotateOnce)
        {
            _klyde.transform.Rotate(45, 0, 0);
            _rotateOnce = true;
        }
        
        if (Vector3.Distance(_klyde.transform.position, _target) < 0.01f)
        {
            flash.gameObject.SetActive(true);
            // var rendererComponent = flash.gameObject.GetComponent<Renderer>();
        
            // Change the render queue
            // rendererComponent.material.renderQueue = newRenderQueue;
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


 

    private void MoveToVitrajWrongCase()
    {
        print("enterWrong");
        _klyde.GetComponent<NavMeshAgent>().enabled = false;
        _klyde.GetComponent<moving>().enabled = false;
        _klyde.transform.position = Vector3.MoveTowards(_klyde.transform.position, _target,
            Time.deltaTime * speed*1.5f);
        if (!_rotateOnce)
        {
            _klyde.transform.Rotate(45, 0, 0);
            _rotateOnce = true;
        }
        if (Vector3.Distance(_klyde.transform.position, _target) < getBackDistance && !_getBack)
        {
            _getBack = true;
            _target = _firstPos;
            _rotateOnce = false;
        }
        else if (Vector3.Distance(_klyde.transform.position, _target) < 0.01f)
        {
            _wrong = false;
            _klyde.transform.eulerAngles =
                new Vector3(_klyde.transform.eulerAngles.x, 0, _klyde.transform.eulerAngles.z);
            _klyde.GetComponent<NavMeshAgent>().enabled = true;
            _klyde.GetComponent<moving>().enabled = true;
            _touchBallon = true;
            _rotateOnce = false;
        }
    }
}
