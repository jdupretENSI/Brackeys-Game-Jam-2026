using UnityEngine;

/// <summary>
/// Fixed inventory system, keeping it simple and dumb
/// </summary>
public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject _lollipop, _portal, _gloves;
    [SerializeField] private int _lollipopMaxUse, _portalMaxUse, _glovesMaxUse;
    
    // Keeps track of how many use the player has and when they reach 0 then they will reset,
    // and we remove the game object to signify that the player doesn't have anymore in their inventory.
    private int _lollipopUsableCount, _portalUsableCount, _glovesUsableCount;
    public bool HasLollipop, HasPortal, HasGloves;

    public void UseLollipop()
    {
        _lollipopUsableCount--;

        Lollipop.UseLollipop();

        if (_lollipopUsableCount != 0) return;
        
        HasLollipop = false;
        _lollipopUsableCount = _lollipopMaxUse;
    }

    public void UsePortal()
    {
        _portalUsableCount--;
        
        Portal.UsePortal();
        
        if (_portalUsableCount != 0) return;
        HasPortal = false;
        _portalUsableCount = _portalMaxUse;
    }

    public void UseGloves()
    {
        _glovesUsableCount--;
        
        Gloves.UseGloves();
        
        if (_glovesUsableCount != 0) return;
        HasGloves = false;
    }
}
