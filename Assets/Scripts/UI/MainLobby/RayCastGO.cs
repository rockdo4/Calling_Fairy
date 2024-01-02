using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class RayCastGO : MonoBehaviour
{
    // Start is called before the first frame update
    private int randNum;
    private int MaxNum = 5;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(UIManager.Instance.CurrentUI != null)
            {
                return;
            }
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

            if (hit.collider != null)
            {
                randNum = Random.Range(0, MaxNum);
                var hitGo = hit.collider.gameObject;
                if (hitGo.CompareTag(Tags.Player))
                {
                    var townCharMove = hitGo.GetComponent<TownCharMove>();
                    switch (randNum)
                    {
                        case 0:
                            townCharMove.state = State.Stun;
                            break;
                        case 1:
                            townCharMove.state = State.AttackBow;
                            break;
                        case 2:
                            townCharMove.state = State.AttackNormal;
                            break;
                        case 3:
                            townCharMove.state = State.SkillMagic;
                            break;
                        case 4:
                            townCharMove.state = State.SkillNormal;
                            break;
                    }
                    UIManager.Instance.SESelect(2);
                }

            }
        }
    }
}
