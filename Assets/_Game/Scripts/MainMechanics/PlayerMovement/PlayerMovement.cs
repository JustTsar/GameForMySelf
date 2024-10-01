using System;
using UnityEngine;

namespace _Game.Scripts.MainMechanics
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour {
        
        [SerializeField] private Transform _playerCam;
        [SerializeField] private Transform _orientation;
        [SerializeField] private float _sensitivity = 50f;
        [SerializeField] private float _sensMultiplier = 1f;
        [SerializeField] private float _moveSpeed = 4500;
        [SerializeField] private float _maxSpeed = 20;
        [SerializeField] private float _maxSlopeAngle = 35f;
        [SerializeField] private float _counterMovement = 0.175f;
        [SerializeField] private float _slideForce = 400;
        [SerializeField] private float _slideCounterMovement = 0.2f;
        [SerializeField] private float _jumpForce = 550f;


        [SerializeField] private LayerMask _groundMask;
        private Rigidbody _rigidBody;
        private float _xRotation;


        private bool _isGrounded;
        private float threshold = 0.01f;
        
        private Vector3 crouchScale = new Vector3(1, 0.5f, 1);
        
        private Vector3 playerScale;
        private bool _readyToJump = true;
        
        private float _jumpCooldown = 0.25f;
        private float _axisX;
        private float _axisY;
        private bool _jumping;
        private bool _sprinting;
        private bool _crouching;
        
        private Vector3 _normalVector = Vector3.up;
        private Vector3 _wallNormalVector;

        private void Awake() 
        {
            _rigidBody = GetComponent<Rigidbody>();
        }
    
        private void Start()
        {
            playerScale =  transform.localScale;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
        private void FixedUpdate() 
        {
            Movement();
        }

        private void Update() 
        {
            MyInput();
            Look();
        }
        
        private void MyInput()
        {
            _axisX = Input.GetAxisRaw("Horizontal");
            _axisY = Input.GetAxisRaw("Vertical");
            _jumping = Input.GetButton("Jump");
            _crouching = Input.GetKey(KeyCode.LeftControl);
            
            if (Input.GetKeyDown(KeyCode.LeftControl))
                StartCrouch();
            if (Input.GetKeyUp(KeyCode.LeftControl))
                StopCrouch();
        }

        private void StartCrouch() 
        {
            transform.localScale = crouchScale;
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
            if (_rigidBody.velocity.magnitude > 0.5f) {
                if (_isGrounded) {
                    _rigidBody.AddForce(_orientation.transform.forward * _slideForce);
                }
            }
        }

        private void StopCrouch()
        {
            transform.localScale = playerScale;
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        }

        private void Movement() 
        {
            //Extra gravity
            _rigidBody.AddForce(Vector3.down * Time.deltaTime * 10);
        
            //Find actual velocity relative to where player is looking
            Vector2 mag = FindVelRelativeToLook();
            float xMag = mag.x, yMag = mag.y;

            //Counteract sliding and sloppy movement
            CounterMovement(_axisX, _axisY, mag);
        
            //If holding jump && ready to jump, then jump
            if (_readyToJump && _jumping) Jump();

            //Set max speed
            float maxSpeed = this._maxSpeed;
        
            //If sliding down a ramp, add force down so player stays grounded and also builds speed
            if (_crouching && _isGrounded && _readyToJump) {
                _rigidBody.AddForce(Vector3.down * Time.deltaTime * 3000);
                return;
            }
        
            //If speed is larger than maxspeed, cancel out the input so you don't go over max speed
            if (_axisX > 0 && xMag > maxSpeed) _axisX = 0;
            if (_axisX < 0 && xMag < -maxSpeed) _axisX = 0;
            if (_axisY > 0 && yMag > maxSpeed) _axisY = 0;
            if (_axisY < 0 && yMag < -maxSpeed) _axisY = 0;

            //Some multipliers
            float multiplier = 1f, multiplierV = 1f;
        
            // Movement in air
            if (!_isGrounded) {
                multiplier = 0.5f;
                multiplierV = 0.5f;
            }
        
            // Movement while sliding
            if (_isGrounded && _crouching) multiplierV = 0f;

            //Apply forces to move player
            _rigidBody.AddForce(_orientation.transform.forward * _axisY * _moveSpeed * Time.deltaTime * multiplier * multiplierV);
            _rigidBody.AddForce(_orientation.transform.right * _axisX * _moveSpeed * Time.deltaTime * multiplier);
        }

        private void Jump() 
        {
            if (_isGrounded && _readyToJump) {
                _readyToJump = false;

                //Add jump forces
                _rigidBody.AddForce(Vector2.up * _jumpForce * 1.5f);
                _rigidBody.AddForce(_normalVector * _jumpForce * 0.5f);
            
                //If jumping while falling, reset y velocity.
                Vector3 vel = _rigidBody.velocity;
                if (_rigidBody.velocity.y < 0.5f)
                    _rigidBody.velocity = new Vector3(vel.x, 0, vel.z);
                else if (_rigidBody.velocity.y > 0) 
                    _rigidBody.velocity = new Vector3(vel.x, vel.y / 2, vel.z);
            
                Invoke(nameof(ResetJump), _jumpCooldown);
            }
        }
    
        private void ResetJump() 
        {
            _readyToJump = true;
        }
    
        private float desiredX;
        private void Look() {
            float mouseX = Input.GetAxis("Mouse X") * _sensitivity * Time.fixedDeltaTime * _sensMultiplier;
            float mouseY = Input.GetAxis("Mouse Y") * _sensitivity * Time.fixedDeltaTime * _sensMultiplier;

            //Find current look rotation
            Vector3 rot = _playerCam.transform.localRotation.eulerAngles;
            desiredX = rot.y + mouseX;
        
            //Rotate, and also make sure we dont over- or under-rotate.
            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

            //Perform the rotations
            _playerCam.transform.localRotation = Quaternion.Euler(_xRotation, desiredX, 0);
            _orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);
        }

        private void CounterMovement(float x, float y, Vector2 mag)
        {
            if (!_isGrounded || _jumping) return;

            //Slow down sliding
            if (_crouching) {
                _rigidBody.AddForce(_moveSpeed * Time.deltaTime * -_rigidBody.velocity.normalized * _slideCounterMovement);
                return;
            }

            //Counter movement
            if (Math.Abs(mag.x) > threshold && Math.Abs(x) < 0.05f || (mag.x < -threshold && x > 0) || (mag.x > threshold && x < 0)) {
                _rigidBody.AddForce(_moveSpeed * _orientation.transform.right * Time.deltaTime * -mag.x * _counterMovement);
            }
            if (Math.Abs(mag.y) > threshold && Math.Abs(y) < 0.05f || (mag.y < -threshold && y > 0) || (mag.y > threshold && y < 0)) {
                _rigidBody.AddForce(_moveSpeed * _orientation.transform.forward * Time.deltaTime * -mag.y * _counterMovement);
            }
        
            //Limit diagonal running. This will also cause a full stop if sliding fast and un-crouching, so not optimal.
            if (Mathf.Sqrt((Mathf.Pow(_rigidBody.velocity.x, 2) + Mathf.Pow(_rigidBody.velocity.z, 2))) > _maxSpeed) {
                float fallspeed = _rigidBody.velocity.y;
                Vector3 n = _rigidBody.velocity.normalized * _maxSpeed;
                _rigidBody.velocity = new Vector3(n.x, fallspeed, n.z);
            }
        }

        /// <summary>
        /// Find the velocity relative to where the player is looking
        /// Useful for vectors calculations regarding movement and limiting movement
        /// </summary>
        /// <returns></returns>
        public Vector2 FindVelRelativeToLook() 
        {
            float lookAngle = _orientation.transform.eulerAngles.y;
            float moveAngle = Mathf.Atan2(_rigidBody.velocity.x, _rigidBody.velocity.z) * Mathf.Rad2Deg;

            float u = Mathf.DeltaAngle(lookAngle, moveAngle);
            float v = 90 - u;

            float magnitue = _rigidBody.velocity.magnitude;
            float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
            float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);
        
            return new Vector2(xMag, yMag);
        }

        private bool IsFloor(Vector3 v) 
        {
            float angle = Vector3.Angle(Vector3.up, v);
            return angle < _maxSlopeAngle;
        }

        private bool cancellingGrounded;
    
        /// <summary>
        /// Handle ground detection
        /// </summary>
        private void OnCollisionStay(Collision other) 
        {
            //Make sure we are only checking for walkable layers
            int layer = other.gameObject.layer;
            if (_groundMask != (_groundMask | (1 << layer))) return;

            //Iterate through every collision in a physics update
            for (int i = 0; i < other.contactCount; i++) {
                Vector3 normal = other.contacts[i].normal;
                //FLOOR
                if (IsFloor(normal)) {
                    _isGrounded = true;
                    cancellingGrounded = false;
                    _normalVector = normal;
                    CancelInvoke(nameof(StopGrounded));
                }
            }

            //Invoke ground/wall cancel, since we can't check normals with CollisionExit
            float delay = 3f;
            if (!cancellingGrounded) {
                cancellingGrounded = true;
                Invoke(nameof(StopGrounded), Time.deltaTime * delay);
            }
        }

        private void StopGrounded() 
        {
            _isGrounded = false;
        }
    
    }
}
