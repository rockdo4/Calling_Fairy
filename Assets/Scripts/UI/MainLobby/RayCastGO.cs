using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class RayCastGO : MonoBehaviour
{
    // Start is called before the first frame update
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

            if (hit.collider != null)
            {
                var hitGo = hit.collider.gameObject;
                if(hitGo.CompareTag(Tags.Player))
                {
                    var townCharMove = hitGo.GetComponent<TownCharMove>();
                    townCharMove.state = State.Stun;
                }

            }
        }
    }
}
