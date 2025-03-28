using UnityEngine;

public class Navigator : MonoBehaviour
{
    public float currentZoomLevel = 500f;
    public LayerMask priorityLayerMask ;
    public LayerMask layerMaskForClicks;

    NavigatorUI navigatorUI;

    IClickInfo currentClickInfo = null;

    public IClickInfo CurrentClickInfo => currentClickInfo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navigatorUI = FindFirstObjectByType<NavigatorUI>();
        navigatorUI.OnResetView += ResetView;
    }

    float lastPinchDistance = 0f;
    // Update is called once per frame
    void Update()
    {

        if (MenuScene.OnPc)
        {
            float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
            currentZoomLevel *=  1 + scrollWheel;

            if (Input.GetMouseButtonDown(0))
            {
                ClickOnLocation(Input.mousePosition);
            }
        }

        else
        {
            if (Input.touchCount > 2)
            {
                Touch touch1 = Input.GetTouch(0);
                Touch touch2 = Input.GetTouch(1);
                float distance = Vector3.Distance(touch1.position / Screen.width, touch2.position / Screen.width);
                float distanceDifference = distance - lastPinchDistance;

                currentZoomLevel *= 1 + distanceDifference;

                lastPinchDistance = distance;

            }
            else
            {
                lastPinchDistance = 0f;
                if (Input.touchCount == 1)
                {
                    if (Input.touches[0].phase == TouchPhase.Ended)
                    {
                        ClickOnLocation(Input.touches[0].position);
                    }
                }
            }
        }
        currentZoomLevel = Mathf.Max(currentZoomLevel, 20);
    }

    private void LateUpdate()
    {
        transform.position = -transform.forward * currentZoomLevel;
        if (currentClickInfo != null)
        {
            transform.position += currentClickInfo.transform.position;
            navigatorUI.ShowText(currentClickInfo.InfoToShow, WorldModelInstance.Instance.WhereAmIString(currentClickInfo.transform.position));
        }
        else
        {
            navigatorUI.HideText();
        }
        
    }

    void ClickOnLocation(Vector2 positionOnScreen)
    {
        Ray ray = Camera.main.ScreenPointToRay(positionOnScreen);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 10000f, priorityLayerMask))
        {
            Debug.Log(hitInfo.collider.gameObject.ToString());
            if (hitInfo.collider.TryGetComponent(out IClickInfo component))
            {
                currentClickInfo = component;
                ShowInfo(component.InfoToShow);
                return;
            }
        }

        if (Physics.Raycast(ray, out RaycastHit hit2Info, 10000f, layerMaskForClicks))
        {
            Debug.Log(hit2Info.collider.gameObject.ToString());
            if (hit2Info.collider.TryGetComponent(out IClickInfo component))
            {
                currentClickInfo = component;
                ShowInfo(component.InfoToShow);
                return;
            }
        }


        if (Physics.SphereCast(ray, 10f, out RaycastHit hit3Info, 10000f, priorityLayerMask))
        {
            Debug.Log(hit3Info.collider.gameObject.ToString());
            if (hit3Info.collider.TryGetComponent(out IClickInfo component))
            {
                currentClickInfo = component;
                ShowInfo(component.InfoToShow);
                return;
            }
        }

        if (Physics.SphereCast(ray, 10f,  out RaycastHit hit4Info, 10000f, layerMaskForClicks)){
            Debug.Log(hit4Info.collider.gameObject.ToString());
            if (hit4Info.collider.TryGetComponent(out IClickInfo component))
            {
                currentClickInfo = component;
                ShowInfo(component.InfoToShow);
            }
        }
    }

    void ShowInfo(string info)
    {
        Debug.Log(info);
    }

    public void ResetView()
    {
        transform.position = new Vector3();
        currentZoomLevel = 750;
        currentClickInfo = null;
    }
}
