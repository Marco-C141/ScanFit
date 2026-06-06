using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SafeAreaFitter : MonoBehaviour
{
    private RectTransform safeAreaRect;
    private Rect lastSafeArea = new Rect(0, 0, 0, 0);
    private Vector2Int lastScreenSize = new Vector2Int(0, 0);
    private ScreenOrientation lastOrientation = ScreenOrientation.AutoRotation;

    void Awake()
    {
        safeAreaRect = GetComponent<RectTransform>();
        ApplySafeArea();
    }

    /// <summary>
    /// We check for changes in Update rather than relying solely on Awake. 
    /// This ensures the UI adapts instantly if the user rotates the device 
    /// (e.g., the notch moves from the top to the side) or if a foldable device changes state.
    /// The performance cost of these simple comparisons is negligible.
    /// </summary>
    void Update()
    {
        Refresh();
    }

    private void Refresh()
    {
        Rect currentSafeArea = Screen.safeArea;

        // Only recalculate layout if the screen dimensions, orientation, or OS safe area have actually changed.
        if (currentSafeArea != lastSafeArea || 
            Screen.width != lastScreenSize.x || 
            Screen.height != lastScreenSize.y || 
            Screen.orientation != lastOrientation)
        {
            // Update cached variables
            lastScreenSize.x = Screen.width;
            lastScreenSize.y = Screen.height;
            lastOrientation = Screen.orientation;
            lastSafeArea = currentSafeArea;

            ApplySafeArea();
        }
    }

    private void ApplySafeArea()
    {
        // 1. Get the current safe area from the device's OS (returns pixels).
        Rect safeArea = Screen.safeArea;

        // 2. Convert pixel coordinates to normalized Canvas Anchor coordinates (0.0 to 1.0).
        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        // 3. Apply the normalized coordinates to the RectTransform.
        safeAreaRect.anchorMin = anchorMin;
        safeAreaRect.anchorMax = anchorMax;

        Debug.Log($"[SafeAreaFitter] Adjusted bounds to Min: {anchorMin}, Max: {anchorMax} for Screen Size: {Screen.width}x{Screen.height}");
    }
}