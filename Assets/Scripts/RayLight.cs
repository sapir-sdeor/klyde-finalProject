#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class RayLight : MonoBehaviour
{
    public int maxReflectionCount = 5;
    public float maxStepDistance = 200;
    private void OnDrawGizmos()
    {
        #if UNITY_EDITOR
        Handles.color = Color.red;
        Handles.ArrowHandleCap(0, transform.position + transform.forward * 0.25f, 
            transform.rotation, 0.5f, EventType.Repaint);
        #endif
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.25f);
        DrawPredictedReflectionPattern(transform.position + transform.forward * 0.75f, transform.forward, maxReflectionCount);
    }

    private void DrawPredictedReflectionPattern(Vector3 position, Vector3 direction, int reflectionRemaining)
    {
        if (reflectionRemaining == 0) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(position, position + direction * maxStepDistance);
        DrawPredictedReflectionPattern(position + direction * maxStepDistance, direction, reflectionRemaining - 1);
    }
}
