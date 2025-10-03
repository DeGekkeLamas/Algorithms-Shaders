using UnityEngine;

public class MovingObject : MonoBehaviour
{
    /// <summary>
    /// Script intended to be placed on enemies, though it can be placed on other moving obstacles as well
    /// </summary>

    [Header("Movement related")]
    public Vector3 moveSpeed;
    public Vector3 moveRange;
    public Vector3 offset;

    Vector3 _oriPos;

    public enum MovementType { Circular, PingPong, Forward};
    public MovementType currentMovement = MovementType.PingPong;


    // Start is called before the first frame update
    void Awake()
    {
        _oriPos = this.transform.localPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Moves object
        switch (currentMovement)
        {
            case MovementType.PingPong:
                MovementPingPong();
                break;
            case MovementType.Circular:
                MovementCircular();
                break;
            case MovementType.Forward:
                MovementForward();
                break;
        }
    }
    void MovementPingPong()
    {
        this.transform.localPosition = _oriPos + new Vector3(
                        Mathf.Sin(moveSpeed.x * Time.time + offset.x) * moveRange.x,
                        Mathf.Sin(moveSpeed.y * Time.time + offset.y) * moveRange.y,
                        Mathf.Sin(moveSpeed.z * Time.time + offset.z) * moveRange.z);
    }                                                             
    void MovementCircular()                                       
    {                                                             
        this.transform.localPosition = _oriPos + new Vector3(     
                        Mathf.Sin(moveSpeed.x * Time.time + offset.x) * moveRange.x,
                        Mathf.Sin(moveSpeed.y * Time.time + offset.y) * moveRange.y,
                        Mathf.Cos(moveSpeed.z * Time.time + offset.z) * moveRange.z);
    }
    void MovementForward()
    {
        this.transform.localPosition = _oriPos + new Vector3(
                        moveSpeed.x * Time.time,
                        moveSpeed.y * Time.time,
                        moveSpeed.z * Time.time);
    }
}
