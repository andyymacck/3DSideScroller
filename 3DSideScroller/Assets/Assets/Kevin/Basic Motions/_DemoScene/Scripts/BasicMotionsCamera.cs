
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KevinIglesias
{
    public class BasicMotionsCamera : MonoBehaviour
    {
        [Header("[TARGET]")]
        public Transform player; // ASSIGN HERE THE PLAYER TRANSFORM ROOT
        private BasicMotionsCharacterController bMCC; // REFERENCE TO MAIN SCRIPT
        private BoxCollider playerBoxCollider; // REFERENCE TO PLAYER BOX COLLIDER

        [Header("[CAMERA PIVOT]")]
        public Transform cameraPositionsRoot; // ROOT OF ALL CAMERA POSSIBLE POSITIONS (CAMERA PIVOT)
        private List<Transform> cameraPositions; // LIST TO STORE CAMERA POSITION TRANSFORMS

        [Header("[CAMERA VERTICAL TILT]")]
        public float rotationSpeed = 100f; // SPEED FOR CAMERA TILTING UP OR DOWN
        public float maxAngle = 50f; // MAX ANGLE CAMERA WILL TILT UP
        public float minAngle = -50f; // MAX ANGLE CAMERA WILL TILT DOWN
        private float tiltInput = 0.0f; // INPUT USED FOR CALCULATING TILT ROTATION 

        [Header("[ZOOM]")]
        public int zoomLevel = 2; // CURRENT ZOOM LEVEL (ZOOM POSITION)
        public int maxZoomLevel; // MAX ZOOM LEVEL (BASED ON THE NUMBER OF CAMERA POSITIONS)
        public float zoomSpeed = 2f; // SPEED AT WHICH CAMERA WILL ZOOM IN OR OUT
        public float zoomCooldown = 0.1f; // AMOUNT OF TIME BEFORE ALLOWING TO ZOOM AGAIN
        public float zoomTimer = 0f; // TIMER TO TRACK WHEN ZOOM COOLDOWN IS COMPLETE
        private float scrollInput = 0f; // FOR TRACKING MOUSE WHEEL SCROLL INPUT

        // TO AVOID CEILING OR WALLS SUDDENLY DISAPPEAR WHEN CAMERA OBSTACLE IS DETECTED
        private float invisibleSurfaceMarginFix = 0.05f; 

        [Header("[FIXED Z POSITION]")]
        // This field enforces a constant world-space z value for the camera.
        public float fixedZ = -10f;

        /// INITIALIZE VARIABLES
        void Awake()
        {
            // INITIALIZE CAMERA POSITIONS FROM PIVOT (cameraPositionsRoot)
            cameraPositions = new List<Transform>();
            for (int i = 0; i < cameraPositionsRoot.childCount - 1; i++)
            {
                cameraPositions.Add(cameraPositionsRoot.GetChild(i));
            }

            // SET MAXIMUM ZOOM VALUE BASED ON THE NUMBER OF CAMERA POSITIONS 
            // (EXCLUDING THE LAST ONE THAT IS THE CAMERA)
            maxZoomLevel = cameraPositionsRoot.childCount - 2;

            // INITIALIZE THE ZOOM TIMER WITH THE COOLDOWN VALUE
            zoomTimer = zoomCooldown;

            // GET THE MAIN SCRIPT FROM THE PLAYER TRANSFORM
            bMCC = player.GetComponent<BasicMotionsCharacterController>();

            // GET THE PLAYER BOX COLLIDER
            playerBoxCollider = bMCC.collisionBox;
        }

        /// DETECT PLAYER INPUTS AND MOVE CAMERA
        void Update()
        {
            // GET INPUTS
            GetInputs();

            // MOVE CAMERA
            ControlCamera();
        }

        /// INPUTS ARE READ DIRECTLY FROM MOUSE (TO AVOID CONFLICTS WITH CURRENT PROJECT INPUT CONFIGURATION)
        private void GetInputs()
        {
            // INPUT FOR TILTING THE CAMERA (LOOK UP OR LOOK DOWN)
            tiltInput = 0.0f;
            if (Input.GetKey(KeyCode.R))
            {
                tiltInput = 1.0f;
            }
            if (Input.GetKey(KeyCode.F))
            {
                tiltInput = -1.0f;
            }

            // GET THE SCROLL INPUT FROM THE MOUSE
            scrollInput = -Input.mouseScrollDelta.y;
        }

        /// MOVE CAMERA BASED ON INPUTS
        private void ControlCamera()
        {
            /// TILT
            // GET THE CURRENT ROTATION OF THE CAMERA ROOT IN LOCAL EULER ANGLES (X, Y, Z)
            Vector3 currentRotation = cameraPositionsRoot.localEulerAngles;
            // ADJUST THE X ROTATION IF IT'S GREATER THAN 180 (TO KEEP THE RANGE BETWEEN -180 AND 180 DEGREES)
            if (currentRotation.x > 180)
            {
                currentRotation.x -= 360;
            }
            // CALCULATE NEW TILT ROTATION USING TILT INPUT
            float newXRotation = currentRotation.x + tiltInput * rotationSpeed * Time.deltaTime;
            // KEEP VALUE BETWEEN LIMITS TO AVOID CAMERA TILTING TOO MUCH
            newXRotation = Mathf.Clamp(newXRotation, minAngle, maxAngle);
            // APPLY TILT ROTATION TO CAMERA PIVOT (cameraPositionsRoot)
            cameraPositionsRoot.localEulerAngles = new Vector3(newXRotation, currentRotation.y, currentRotation.z);

            /// ZOOM
            if (zoomTimer >= zoomCooldown)
            {
                if (scrollInput > 0)
                {
                    zoomLevel++;
                    zoomTimer = 0f;
                }
                else if (scrollInput < 0)
                {
                    zoomLevel--;
                    zoomTimer = 0f;
                }

                // CLAMP THE zoomLevel TO ENSURE IT STAYS BETWEEN 0 AND maxZoomLevel
                zoomLevel = Mathf.Clamp(zoomLevel, 0, maxZoomLevel);

                // SET THE CAMERA'S LOCAL POSITION FROM THE PREDEFINED CAMERA POSITIONS
                transform.localPosition = cameraPositions[zoomLevel].localPosition;
            }
            else
            {
                if (zoomTimer < zoomCooldown)
                {
                    zoomTimer += Time.deltaTime;
                }
            }

            /// OBSTACLE DETECTION
            Vector3 boxColliderCenter = player.transform.position + playerBoxCollider.center;
            Vector3 cameraRayDirection = (transform.position - boxColliderCenter).normalized;
            float distance = Vector3.Distance(boxColliderCenter, transform.position);
            RaycastHit[] raycastHits = Physics.RaycastAll(boxColliderCenter, cameraRayDirection, distance);
            RaycastHit? closestHit = null; // NULLABLE RaycastHit
            foreach (RaycastHit hit in raycastHits)
            {
                // ONLY USE COLLIDERS THAT ARE CHILDREN OF collisionsRoot TRANSFORM
                if (hit.transform.parent == bMCC.collisionsRoot)
                {
                    if (closestHit == null || hit.distance < closestHit.Value.distance)
                    {
                        closestHit = hit;
                    }
                }
            }

            if (closestHit != null)
            {
                // PLACE CAMERA IN FRONT OF WALL (WITH A MARGIN TO AVOID INVISIBLE CEILING OR WALL)
                transform.position = closestHit.Value.point + (cameraRayDirection * -invisibleSurfaceMarginFix);
            }

            // ENFORCE A FIXED Z POSITION (WORLD SPACE)
            Vector3 pos = transform.position;
            pos.z = fixedZ;
            transform.position = pos;
        }
    }
}
