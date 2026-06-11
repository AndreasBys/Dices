using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Dice_roll : MonoBehaviour
{

    Rigidbody body;

    void Start() { body = GetComponent<Rigidbody>(); }

    public TextMeshProUGUI resultText;

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            body.AddForce(Vector3.up * 5f, ForceMode.Impulse);

            body.AddTorque(Random.insideUnitSphere * 5f, ForceMode.Impulse);

            StartCoroutine(WaitForDiceToStop());

        }

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            Resetdie();
        }


    }

    IEnumerator WaitForDiceToStop()
    {
        yield return new WaitForSeconds(1.5f);

        while (body.linearVelocity.magnitude > 0.05f || body.angularVelocity.magnitude > 0.05f)
        {
            Debug.Log("Still rolling... velocity: " + body.linearVelocity.magnitude);
            yield return new WaitForSeconds(2f); 
        }

        int result = GetDiceValue();

        resultText.text = result.ToString();


    }



    private void Resetdie()
    {
        body.linearVelocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;
        transform.position = new Vector3(0, 1, 0);
        transform.rotation = Quaternion.identity;
        resultText.text = "";
    }


    int GetDiceValue()
    {
        int topValue = 0;
        float maxDot = 0.99f;


        Vector3[] dice = new Vector3[] // Ordren i arrayet er retningernes assignede værdier, så 1 er fremad osv.
        {
            transform.forward.normalized, // 1
            transform.up.normalized, // 2
            -transform.right.normalized, // 3
            transform.right.normalized, // 4
            -transform.up.normalized, // 5
            -transform.forward.normalized, // 6
            
        };


        for (int i = 0; i < dice.Length; i++)
        {
            
            float dot = Vector3.Dot(dice[i], Vector3.up);

            if (dot > maxDot)
            {
                
                maxDot = dot;
                topValue = i+1;
            }
        }

        return topValue;
    }


}
