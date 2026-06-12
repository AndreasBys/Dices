using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Dice_roll : MonoBehaviour
{

    Rigidbody body;

    void Start()
    {

        body = GetComponent<Rigidbody>();
    }

    public TextMeshProUGUI resultText;
    public TextMeshProUGUI latestRoll;

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            body.AddForce(Vector3.up * 20f, ForceMode.Impulse);

            body.AddTorque(Random.insideUnitSphere * Random.Range(7f, 10f), ForceMode.Impulse);

            StartCoroutine(WaitForDiceToStop());

        }

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            ResetDie();
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
        latestRoll.text = "Latest Roll: " + result.ToString();

    }



    private void ResetDie()
    {
        body.linearVelocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;
        transform.position = new Vector3(0, 1, 0);
        transform.rotation = Quaternion.identity;
        resultText.text = "";
        latestRoll.text = "";
    }

    enum DiceFace
    {
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6
    }

    public int GetDiceValue()
    {
        int topValue = 0;
        float maxDot = 0.99f;

        var faces = new (Vector3 Direction, DiceFace Face)[]
        {
        (transform.forward, DiceFace.One),
        (transform.up, DiceFace.Two),
        (-transform.right, DiceFace.Three),
        (transform.right, DiceFace.Four),
        (-transform.up, DiceFace.Five),
        (-transform.forward, DiceFace.Six)
        };


        foreach (var face in faces)
        {
            float dot = Vector3.Dot(face.Direction, Vector3.up);

            if (dot > maxDot)
            {
                maxDot = dot;
                topValue = (int)face.Face;
            }
        }

        return topValue;
    }


}
