using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.UI.Image;

public class Chara : MonoBehaviour
{
    public enum BODY_STATUS
    {
        STAND,
        WALK,
        RUN,
        SQUAT,
        SLIDING,
    }

    [SerializeField] private GameObject feild; // ÉWÉÉÉìÉvÇÃã≠Ç≥

    private Vector3 baseScale;
    private Vector3 squatScale = Vector3.one;

    public bool isGround = false;
    public BODY_STATUS _bodyStatus = BODY_STATUS.STAND;



    void Start()
    {
        baseScale = this.transform.localScale;
    }

    void Update()
    {
        updateIsGround();
        updateBodyStatus();
    }
    private void updateBodyStatus()
    {
        switch (_bodyStatus)
        {
            case BODY_STATUS.STAND:
                stand();
                break;
            case BODY_STATUS.RUN:
                break;
            case BODY_STATUS.SQUAT:
                squat();
                break;
            case BODY_STATUS.SLIDING:
                sliding();
                break;
        }
    }
    /// <summary>ê⁄ínîªíË</summary>
    private void updateIsGround()
    {
        Vector3 originPos = this.transform.position;
        Ray ray = new Ray(originPos, Vector3.down);
        float rayLen = 25.0f;

        Debug.DrawRay(ray.origin, ray.direction * rayLen, Color.red, 5.0f);
        isGround = Physics.Raycast(ray, rayLen);
    }

    private void stand()
    {
        this.transform.localScale = baseScale;
        this.transform.rotation = Quaternion.Euler(Vector3.zero);
    }
    private void squat()
    {
        this.transform.localScale = baseScale/2;
    }
    private void sliding()
    {
        this.transform.rotation = Quaternion.Euler(Vector3.forward * 20);
    }


    public void setBodyStatus( BODY_STATUS status)
    {
        this._bodyStatus = status;
    }
    public BODY_STATUS getBodyStatus()
    {
        return this._bodyStatus;
    }
}
