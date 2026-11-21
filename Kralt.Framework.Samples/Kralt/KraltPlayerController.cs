using Kralt.Framework.Graphics;
using Kralt.Framework.InputHandling;
using Kralt.Framework.Physics;
using Kralt.Framework.Runtime;
using Kralt.Mathematics;

namespace Kralt;

internal class KraltPlayerController : Transformable
{
    private const float MOUSE_SENS_X = .002f;

    private const float MOUSE_SENS_Y = .002f;

    private const float MOVE_SPEED = 10f;

    private const float BOUNDS_WIDTH = 1f;

    private const float BOUNDS_HEIGHT = 2f;

    private AABB PlayerBounds
    {
        get
        {
            Vector3 min = localPosition + new Vector3(-BOUNDS_WIDTH * 0.5f, 0f, -BOUNDS_WIDTH * 0.5f);
            Vector3 max = localPosition + new Vector3(BOUNDS_WIDTH * 0.5f, BOUNDS_HEIGHT, BOUNDS_WIDTH * 0.5f);

            return new AABB(min, max);
        }
    }

    Quaternion playerTargetRotation;
    Quaternion cameraTargetRotation;

    readonly Camera camera;
    public Transformable? collisionObjects;

    public KraltPlayerController()
    {
        camera = new Camera()
        {
            localPosition = Vector3.up * 1.8f
        };

        Add(camera);

        playerTargetRotation = localRotation;
        cameraTargetRotation = camera.localRotation;
    }

    public override void Update()
    {
        HandleMovement();
        HandleMouseLook();
    }

    private void HandleMovement()
    {
        Vector2 moveDelta = Input.WasdDelta;

        float up = Input.GetKey(Key.Space) ? 1f : 0f;
        float down = Input.GetKey(Key.LeftShift) ? 1f : 0f;

        Vector3 movement = right * moveDelta.x + forward * moveDelta.y + Vector3.up * (up - down);

        if(movement != Vector3.zero)
            Move(movement * Time.deltaTime * MOVE_SPEED);
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

    private void Move(Vector3 movement)
    {
        // localPosition += movement;

        MoveAlongAxis(movement.x, Axis.X);
        MoveAlongAxis(movement.y, Axis.Y);
        MoveAlongAxis(movement.z, Axis.Z);
    }

    private void MoveAlongAxis(float amt, Axis axis)
    {
        if (amt == 0f || collisionObjects == null)
            return;

        Vector3 movement = Vector3.zero;
        movement[(int)axis] = amt;

        localPosition += movement;

        AABB playerBounds = PlayerBounds;

        foreach(Transformable t in collisionObjects)
        {
            if (t is not KraltObject kr)
                continue;

            foreach(AABB collision in kr.collisionAABB)
            {
                if (!playerBounds.Intersects(collision))
                    continue;

                switch (axis)
                {
                    case Axis.X:
                    localPosition.x = amt > 0 ? 
                        collision.min.x - BOUNDS_WIDTH * 0.5f : 
                        collision.max.x + BOUNDS_WIDTH * 0.5f;
                    break;

                    case Axis.Y:
                    localPosition.y = amt > 0 ?
                        collision.min.y - BOUNDS_HEIGHT :
                        collision.max.y;
                    break;

                    case Axis.Z:
                    localPosition.z = amt > 0 ?
                        collision.min.z - BOUNDS_WIDTH * 0.5f :
                        collision.max.z + BOUNDS_WIDTH * 0.5f;
                    break;
                }
            }

            playerBounds = PlayerBounds;
        }
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
