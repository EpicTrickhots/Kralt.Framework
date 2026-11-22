using System;
using System.Diagnostics;
using Kralt.Framework.Graphics;
using Kralt.Framework.InputHandling;
using Kralt.Framework.Physics;
using Kralt.Framework.Runtime;
using Kralt.Mathematics;


namespace Kralt;

internal class KraltPlayerController : Transformable
{
    private bool debug { get; }

    private const float MOUSE_SENS_X = .002f;

    private const float MOUSE_SENS_Y = .002f;

    private const float MOVE_SPEED = 20f;

    private const float MOVE_SPEED_DEBUG = 10f;

    private const float BOUNDS_RADIUS = 0.5f;

    private const float BOUNDS_HEIGHT = 2f;

    private const float SLOPE_LIMIT = 45f;

    private const float GRAVITY = -9.8f;

    private const float GRAVITY_MUL = 2f;

    private const float JUMP_FORCE = 10f;

    internal bool isGrounded { get; private set; }

    internal float verticalVelocity { get; private set; }

    private AABB playerBounds
    {
        get
        {
            Vector3 min = position + new Vector3(-BOUNDS_RADIUS, 0f, -BOUNDS_RADIUS);
            Vector3 max = position + new Vector3(BOUNDS_RADIUS, BOUNDS_HEIGHT, BOUNDS_RADIUS);
            return new AABB(min, max);
        }
    }

    Quaternion playerTargetRotation;
    Quaternion cameraTargetRotation;

    private Camera camera;
    public Transformable? collisionObjects;

    public KraltPlayerController(bool debug = false)
    {
        this.debug = debug;

        camera = new Camera() { localPosition = Vector3.up * 1.8f };

        Add(camera);

        playerTargetRotation = localRotation;
        cameraTargetRotation = camera.localRotation;
    }

    public override void Update()
    {
        if (debug)
        {
            HandleDebugMovement();
            HandleMouseLook();
            return;
        }

        HandleMovement();
        HandleJump();
        HandleMouseLook();
        HandleGravity();

        if (Input.GetKeyDown(Key.E))
        {
            position = Vector3.up;
            verticalVelocity = 1f;
        }
    }

    private void HandleMovement()
    {
        Vector2 moveDelta = Input.WasdDelta;

        Vector3 movement = right * moveDelta.x + forward * moveDelta.y;

        if (movement != Vector3.zero)
            Move(movement * Time.deltaTime * MOVE_SPEED);
    }

    private void HandleDebugMovement()
    {
        Vector2 moveDelta = Input.WasdDelta;

        float u = Input.GetKey(Key.Space) ? 1f : 0f;
        float d = Input.GetKey(Key.LeftShift) ? 1f : 0f;

        Vector3 movement = right * moveDelta.x + forward * moveDelta.y + Vector3.up * (u - d);
        localPosition += movement * Time.deltaTime * MOVE_SPEED_DEBUG;
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(Key.Space) && isGrounded)
        {
            verticalVelocity = JUMP_FORCE;
            isGrounded = false;
        }
    }

    private void HandleMouseLook()
    {
        Vector2 mouseDelta = Input.MouseDelta;

        float x = mouseDelta.y * MOUSE_SENS_X;
        float y = mouseDelta.x * MOUSE_SENS_Y;

        playerTargetRotation *= Quaternion.Euler(0.0f, -y, 0.0f);
        cameraTargetRotation *= Quaternion.Euler(-x, 0.0f, 0.0f);

        cameraTargetRotation = ClampRotationAroundAxis(cameraTargetRotation);

        localRotation = playerTargetRotation;
        camera.localRotation = cameraTargetRotation;
    }

    private CollisionFlags Move(Vector3 movement)
    {
        CollisionFlags flags = CollisionFlags.None;

        flags |= MoveAlongAxis(movement.x, Axis.X);
        flags |= MoveAlongAxis(movement.y, Axis.Y);
        flags |= MoveAlongAxis(movement.z, Axis.Z);

        return flags;
    }

    private CollisionFlags MoveAlongAxis(float amt, Axis axis)
    {
        if (amt == 0f || collisionObjects == null)
            return CollisionFlags.None;

        Vector3 movement = Vector3.zero;
        movement[(int)axis] = amt;

        localPosition += movement;

        AABB bounds = playerBounds;

        foreach (Transformable t in collisionObjects)
        {
            if (t is not KraltObject kr)
                continue;

            foreach (AABB collision in kr.collisionAABB)
            {
                if (!bounds.Intersects(collision))
                    continue;

                Console.WriteLine(bounds);

                switch (axis)
                {
                    case Axis.X:
                    localPosition.x = amt > 0f
                        ? collision.min.x - BOUNDS_RADIUS
                        : collision.max.x + BOUNDS_RADIUS;
                    return CollisionFlags.Sides;

                    case Axis.Y:
                    localPosition.y = amt > 0f
                        ? collision.min.y - BOUNDS_HEIGHT
                        : collision.max.y;
                    return amt > 0f ? CollisionFlags.Above : CollisionFlags.Below;

                    case Axis.Z:
                    localPosition.z = amt > 0f
                        ? collision.min.z - BOUNDS_RADIUS
                        : collision.max.z + BOUNDS_RADIUS;
                    return CollisionFlags.Sides;
                }
            }

            bounds = playerBounds;
        }

        return CollisionFlags.None;
    }

    private void HandleGravity()
    {
        verticalVelocity += GRAVITY * Time.deltaTime * GRAVITY_MUL;

        CollisionFlags flags = Move(Vector3.up * verticalVelocity * Time.deltaTime);

        isGrounded = (flags & CollisionFlags.Below) != 0;

        if (isGrounded || (flags & CollisionFlags.Above) != 0)
            verticalVelocity = 0f;
    }

    private static Quaternion ClampRotationAroundAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
        angleX = Mathf.Clamp(angleX, -90.0f, 90.0f);
        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }
}
