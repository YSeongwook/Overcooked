using DG.Tweening.Core.Easing;
using System;
using UnityEngine;
using UnityEngine.XR;

public class Ingredient : GameItem
{
    public bool isCooked = false;
    public bool isOnDesk = true;

    public enum IngredientType { Fish, Shrimp, Plate, Lettuce, Tomato, Cucumber, Chicken, Potato };
    public IngredientType type; // 재료 유형: 채소, 고기 등
    
    public enum IngredientState { Raw, Cooking, Cooked }
    public IngredientState currentState = IngredientState.Raw;

    [SerializeField] private Mesh cookedIngredient;
    [SerializeField] private Material cookedFish;

    public Vector3 fishLocalPos = new Vector3(0, 0.138f, 0.08f);
    public Vector3 shrimpLocalPos = new Vector3(-0.365000874f, -0.0890001357f, -0.423000485f);
    public Vector3 lettuceLocalPos = new Vector3(0, 0.418000013f, 0);

    public override void Interact(PlayerInteractController player)
    {
        // 기본 상호작용: 재료를 주움
        //Pickup(player);
    }

    // 상태 변경 메소드
    public void ChangeState(IngredientState newState)
    {
        currentState = newState;
        Debug.Log("State changed to: " + currentState);
    }

    // Ingredient 핸들링과 관련된 로직
    public void HandleIngredient(Transform something, IngredientType handle, bool isActive)
    {
        MeshCollider collider = transform.parent.GetComponent<MeshCollider>();
        collider.isTrigger = isActive;

        Vector3 localPosition;
        Quaternion localRotation;

        switch (handle)
        {
            case IngredientType.Fish:
                localPosition = fishLocalPos;
                localRotation = Quaternion.identity;
                break;
            case IngredientType.Shrimp:
                localPosition = isCooked ? Vector3.zero : shrimpLocalPos;
                localRotation = isCooked ? Quaternion.identity : Quaternion.Euler(new Vector3(35.029995f, -1.04264745e-06f, 326.160004f));
                break;
            case IngredientType.Lettuce:
                localPosition = lettuceLocalPos;
                localRotation = Quaternion.identity;
                break;
            case IngredientType.Tomato:
            case IngredientType.Cucumber:
            case IngredientType.Chicken:
            case IngredientType.Potato:
                localPosition = Vector3.zero;
                localRotation = Quaternion.identity;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(handle), $"Unsupported handle type: {handle}");
        }

        Transform parentTransform = transform.parent;
        parentTransform.localPosition = localPosition;
        parentTransform.localRotation = localRotation;

        if (isActive)
        {
            parentTransform.parent.SetParent(something);
            parentTransform.parent.localRotation = Quaternion.identity;
            parentTransform.parent.localPosition = new Vector3(-0.409999996f, 0, 1.84000003f);
        }
    }

    // Player와 관련된 간단한 핸들링
    public void HandlePlayer(Transform something, Vector3 target, bool isOn)
    {
        Transform myTransform = transform;
        myTransform.SetParent(something);
        myTransform.localRotation = Quaternion.identity;
        myTransform.localPosition = isOn ? new Vector3(-0.4f, 0.24f, 2.23f) : target;
    }

    // Mesh 변경 로직
    public void ChangeMesh(IngredientType handType)
    {
        MeshFilter meshFilter = transform.parent.GetComponent<MeshFilter>();
        MeshCollider meshCollider = transform.parent.GetComponent<MeshCollider>();
        meshFilter.mesh = cookedIngredient;
        meshCollider.sharedMesh = cookedIngredient;

        switch (handType)
        {
            case IngredientType.Fish:
            case IngredientType.Tomato:
            case IngredientType.Shrimp:
            case IngredientType.Lettuce:
                ApplyMaterialAndAdjustPosition(handType);
                break;
            default:
                Vector3 positionAdjustment = new Vector3(0, -0.0025f, 0);
                AdjustPosition(transform.parent.parent, positionAdjustment);
                break;
        }
    }

    private void ApplyMaterialAndAdjustPosition(IngredientType handType)
    {
        MeshRenderer meshRenderer = transform.parent.GetComponent<MeshRenderer>();
        meshRenderer.material = cookedFish; // Example for applying material
        Vector3 positionAdjustment = new Vector3(0, CalculatePositionAdjustment(handType), 0);
        AdjustPosition(transform.parent.parent, positionAdjustment);
    }

    private float CalculatePositionAdjustment(IngredientType handType)
    {
        switch (handType)
        {
            case IngredientType.Fish:
                return -0.0017f;
            case IngredientType.Tomato:
                return -0.017f;
            case IngredientType.Shrimp:
                return -0.0013f;
            case IngredientType.Lettuce:
                return -0.0028f;
            default:
                return -0.0025f;
        }
    }

    private void AdjustPosition(Transform targetTransform, Vector3 adjustment)
    {
        targetTransform.localPosition += adjustment;
    }

    // 사용되지 않는 코드의 주석 처리 및 정리
    private void OnTriggerEnter(Collider other)
    {
        //if (other.CompareTag("deadZone"))
        //{
        //    HandleDeadZoneInteraction(type);
        //}
    }

    private void HandleDeadZoneInteraction(IngredientType handType)
    {
        GameObject parent = transform.parent.parent.gameObject;
        if (handType != IngredientType.Plate)
        {
            //parent.GetComponent<DestroyIngredient>().DestroySelf();
        }
        else // Plate handling
        {
            Destroy(gameObject);
            //GameManager.instance.PlateReturn();
        }
    }

}