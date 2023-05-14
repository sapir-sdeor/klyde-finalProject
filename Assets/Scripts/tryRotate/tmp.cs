namespace tryRotate
{
    public class tmp
    {
        // Vector3 mousePos = Input.mousePosition;
        //                mousePos.z = Camera.main.nearClipPlane;
        //            
        //                // Convert the mouse position from screen space to world space
        //                Vector3 mousePositionInWorldSpace = camera.ScreenToWorldPoint(mousePos);
        //                // print(mousePositionInWorldSpace);
        //
        //                // Find the closest point on the NavMesh to the mouse position
        //                NavMesh.SamplePosition(mousePositionInWorldSpace, out NavMeshHit hit, 1000f, NavMesh.AllAreas);
        //
        //                // Set the destination of the NavMeshAgent to the closest point on the NavMesh
        //                print(hit.position);
        //
        //                if (agent.isOnNavMesh && !Rotate2D3D.GetIsRotating()) // Check if agent is on NavMesh
        //                {
        //                    agent.SetDestination(hit.position);
        //                }
        
        // void Update()
        // {
        //
        //     for (int i = 0; i < transform.childCount; i++)
        //     {
        //         Transform child = transform.GetChild(i);
        //         Material mat = child.GetComponent<Renderer>().material;
        //         if (child.position.x < limit)
        //         {
        //             FadeOut(mat,child.gameObject);
        //             
        //         }
        //         else
        //         {
        //             FadeIn(mat,child.gameObject);
        //         }
        //     }
        //  
        //     
        // } 

        // void FadeIn(Material mat, GameObject child)
        //     =>  StartCoroutine(Fade(mat, child, InitialAlpha, true));
        // void FadeOut(Material mat, GameObject child)
        //     =>  StartCoroutine(Fade(mat, child, 0, false));
        //
        // IEnumerator Fade(Material mat, GameObject child, float target, bool activeStart)
        // {
        //     var startAlpha = mat.color.a;
        //
        //     child.gameObject.SetActive(activeStart);
        //     var startT = Time.time;
        //     Color c =mat.color;
        //     while (Time.time < startT + fadeDuration)
        //     {
        //         var fraction = (Time.time - startT) / fadeDuration;
        //         var f = Mathf.Lerp(startAlpha, target, 1-fraction);
        //         c.a = f;
        //         mat.color = c;
        //         c = mat.color;
        //         yield return new WaitForEndOfFrame();
        //     }
        //     c.a = target;
        //     mat.color = c;
        //     child.gameObject.SetActive(!activeStart);
        //     print("do fade");
        // }
    }
}