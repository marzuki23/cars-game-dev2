using UnityEngine;

public class ParkingSpot : MonoBehaviour
{
    [Header("Visual Settings")]
    [SerializeField] private Color defaultColor = new Color(1f, 0.3f, 0.3f, 0.5f);
    [SerializeField] private Color occupiedColor = new Color(0.2f, 1f, 0.2f, 0.5f);
    [SerializeField] private Color glowColor = new Color(0.2f, 1f, 0.2f, 0.8f);

    [Header("References")]
    [SerializeField] private GameManager gameManager;

    private MeshRenderer meshRenderer;
    private Material materialInstance;
    private bool isOccupied;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        if (meshRenderer != null)
        {
            materialInstance = meshRenderer.material;
            materialInstance.color = defaultColor;
            materialInstance.EnableKeyword("_EMISSION");
            materialInstance.SetColor("_EmissionColor", defaultColor * 0.3f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isOccupied) return;

        if (other.CompareTag("Player") || other.GetComponent<mobil>() != null || other.GetComponentInParent<mobil>() != null)
        {
            isOccupied = true;
            OnParked();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isOccupied) return;

        if (other.CompareTag("Player") || other.GetComponent<mobil>() != null || other.GetComponentInParent<mobil>() != null)
        {
            isOccupied = true;
            OnParked();
        }
    }

    private void OnParked()
    {
        if (materialInstance != null)
        {
            materialInstance.color = occupiedColor;
            materialInstance.SetColor("_EmissionColor", glowColor);
        }

        if (gameManager != null)
        {
            gameManager.OnMissionComplete();
        }
    }
}
