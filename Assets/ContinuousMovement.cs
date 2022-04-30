using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ContinuousMovement : MonoBehaviour
{
    public float Speed = 100;
    public XRNode inputSourceLeft;
    public XRNode inputSourceRight;
    private Vector2 inputAxisLeft, inputAxisRight;
    private CharacterController character;
    public int Frame = 0;
    private Plane plane;
    private VoxelPlace voxelplace;
    float LeftTrigger, RightTrigger;
    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
        plane = GameObject.FindGameObjectWithTag("Player").GetComponent<Plane>();
        voxelplace = GameObject.FindGameObjectWithTag("Respawn").GetComponent<VoxelPlace>();
    }

    // Update is called once per frame
    void Update()
    {
        InputDevice deviceLeft = InputDevices.GetDeviceAtXRNode(inputSourceLeft);
        InputDevice deviceRight = InputDevices.GetDeviceAtXRNode(inputSourceRight);
        deviceRight.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxisRight);
        deviceLeft.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxisLeft);
        deviceLeft.TryGetFeatureValue(CommonUsages.trigger, out LeftTrigger);
        deviceRight.TryGetFeatureValue(CommonUsages.trigger, out RightTrigger);
    }

    private void FixedUpdate()
    {
        Vector3 direction = new Vector3(-inputAxisRight.x, inputAxisLeft.y, -inputAxisRight.y);
        character.Move(direction * Time.fixedDeltaTime * Speed);
        if (LeftTrigger > 0.5f && Frame > 0)
        {
            Frame--;
        }
        if (RightTrigger > 0.5f && Frame < 1279)
        {
            Frame++;
        }
        plane.Show(Frame+30);
        voxelplace.Show(Frame);
    }
}
