using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float normalSpeed = 5f;
    public float sprintSpeed = 10f;
    public float mouseSensitivity = 2f;
    public float jumpForce = 5f;
    public Transform cameraTransform;

    private Rigidbody rb;
    private float verticalLookRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Move();
        LookAround();
        Jump();
    }

    void Move()
    {
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : normalSpeed;
        /*Operatore ternario!:
         Se l'input dell'utente equivale allo "shift" premuto allora currentSpeed assumerà il valore dello sprint senno di quello normale*/
        float moveForward = Input.GetAxis("Vertical") * currentSpeed;
        float moveSide = Input.GetAxis("Horizontal") * currentSpeed;
        /* - Input.GetAxis("Vertical) restituisce 1 se il player si muove in avanti (preme W) e restituisce -1 se si muove indietro (schiaccia S) oppure 0 se non si schiaccia niente;
           - Il tutto ovviamente viene moltiplicato per la velocità che abbiamo impostato (Sprint o normal dipende se schiacciamo o no shift);
           - Input.GetAxis(Horizzontal) restituisce 1 se il player si muove a destra (preme D) e restituisce -1 se si muove a sinistra (schiaccia A) oppure 0 se non si schiaccia niente;
        */
        Vector3 move = transform.right * moveSide + transform.forward * moveForward;  //in questo modo creo un nuovo vettore che va a modificare il vettore di direzione (transform) del mio oggetto modificandone il rigidBody
        /*Transform.right rappresenta la direzione del valore dell'oggetto a destra
          Transform.forward rappresenta la direzione del valore dell'oggetto in avanti*/
        rb.velocity = new Vector3(move.x, rb.velocity.y, move.z);  //rappresenta la velocità del rigidBody a cui è attaccato all'oggetto
        //quindi la velocità del rigidBody sarà uguale ad un nuovo vettore con componenti x e z cambiate ma con velocità sull'asse y invariato (a 0 senza saltare e 5 quando saltiamo -- penso)
    }

    void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        verticalLookRotation -= mouseY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);
        cameraTransform.localEulerAngles = Vector3.right * verticalLookRotation;
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }
}

