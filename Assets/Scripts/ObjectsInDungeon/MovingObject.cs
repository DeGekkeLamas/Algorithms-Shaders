using UnityEngine;

public class MovingObject : MonoBehaviour
{
    /// <summary>
    /// Script intended to be placed on enemies, though it can be placed on other moving obstacles as well
    /// </summary>

    [Header("Movement related")]
    public float moveSpeedX;
    public float moveRangeX;
    public float moveSpeedY;
    public float moveRangeY;
    public float moveSpeedZ;
    public float moveRangeZ;
    public float offsetX;
    public float offsetY;
    public float offsetZ;

    Vector3 _oriPos;
    float _oriRot;

    public enum MovementType { Circular, PingPong, Forward};
    public MovementType currentMovement = MovementType.PingPong;

    GameManager GM;

    // Start is called before the first frame update
    void Awake()
    {
        _oriPos = this.transform.localPosition;
        _oriRot = transform.eulerAngles.y;
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
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
                        Mathf.Sin(moveSpeedX * Time.time + offsetX) * moveRangeX,
                        Mathf.Sin(moveSpeedY * Time.time + offsetY) * moveRangeY,
                        Mathf.Sin(moveSpeedZ * Time.time + offsetZ) * moveRangeZ);
    }
    void MovementCircular()
    {
        this.transform.localPosition = _oriPos + new Vector3(
                        Mathf.Sin(moveSpeedX * Time.time + offsetX) * moveRangeX,
                        Mathf.Sin(moveSpeedY * Time.time + offsetY) * moveRangeY,
                        Mathf.Cos(moveSpeedZ * Time.time + offsetZ) * moveRangeZ);
    }
    void MovementForward()
    {
        this.transform.localPosition = _oriPos + new Vector3(
                        moveSpeedX * Time.time,
                        Mathf.Sin(moveSpeedY * Time.time) * moveRangeY,
                        moveSpeedZ * Time.time);
    }
}
