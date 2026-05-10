using UnityEngine;

public class CoinAnimator : MonoBehaviour
{
    [SerializeField] float AngularSpeed = 50f;
    [SerializeField] float CoinHeight = 0.7f;
    [SerializeField] float MovementAplitude = 0.5f;
    [SerializeField] float MovementFrequency = 1f;
    [SerializeField] Transform CoinMesh = null;


    private void Update()
    {
        // Rotating the coin
        CoinMesh.Rotate(0f, AngularSpeed*Time.deltaTime, 0f);

        // Moving the coin up and down using a sine wave
        float DeltaY = MovementAplitude * Mathf.Sin(MovementFrequency * Time.time);

        CoinMesh.localPosition = new Vector3 (0f, CoinHeight + DeltaY, 0f);
    }
}
