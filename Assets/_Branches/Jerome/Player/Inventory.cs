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
    private ItemType? CurrentItem = null;

    private void Awake()
    {
        _lollipopUsableCount = _lollipopMaxUse;
        _portalUsableCount = _portalMaxUse;
        _glovesUsableCount = _glovesMaxUse;
        
        SetItem(ItemType.Lollipop);
    }
    
    public void SetItem(ItemType item)
    {
        CurrentItem = item;
    }

    public void UseItem()
    {
        // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
        switch (CurrentItem)
        {
            case ItemType.Lollipop:
                UseLollipop();
                break;
            case ItemType.Gloves:
                UseGloves();
                break;
            case ItemType.Portal:
                UsePortal();
                break;
        }
    }

    private void UseLollipop()
    {
        if (!HasLollipop || _lollipopUsableCount == 0) return;
    
        _lollipopUsableCount--;
    
        // Spawn the lollipop in front of the player
        SpawnLollipop();
    
        if (_lollipopUsableCount != 0) return;
        HasLollipop = false;
    }

    private void SpawnLollipop()
    {
        if (!_lollipop) return;
        
        // Calculate position in front of player
        Vector3 spawnPosition = transform.position;
        float direction = Mathf.Sign(transform.localScale.x);
        spawnPosition.x += direction * 1f;
        spawnPosition.y += 0.5f; // Slightly above ground
        
        Instantiate(_lollipop, spawnPosition, Quaternion.identity);
    }

    private void UsePortal()
    {
        if (!HasPortal || _portalUsableCount == 0) return;
    
        _portalUsableCount--;
    
        // Spawn the portal in front of the player
        SpawnPortal();
    
        if (_portalUsableCount != 0) return;
        HasPortal = false;
    }

    private void SpawnPortal()
    {
        // Assuming the portal prefab is assigned in the inspector
        if (!_portal) return;
        
        // Calculate position in front of player
        Vector3 spawnPosition = transform.position;
        float direction = Mathf.Sign(transform.localScale.x);
        spawnPosition.x += direction * 1.5f; // Adjust distance as needed
        
        Instantiate(_portal, spawnPosition, Quaternion.identity);
    }

    private void UseGloves()
    {
        if (!HasGloves || _glovesUsableCount == 0) return;
    
        _glovesUsableCount--;
    
        // Spawn the gloves in front of the player
        SpawnGloves();
    
        if (_glovesUsableCount != 0) return;
        HasGloves = false;
    }

    private void SpawnGloves()
    {
        if (!_gloves) return;
        
        // Calculate position in front of player
        Vector3 spawnPosition = transform.position;
        float direction = Mathf.Sign(transform.localScale.x);
        spawnPosition.x += direction * 1f;
        spawnPosition.y += 0.5f; // Slightly above ground
        
        Instantiate(_gloves, spawnPosition, Quaternion.identity);
    }

    public bool AddLollipop()
    {
        if (HasLollipop) return false;
        _lollipopUsableCount = _lollipopMaxUse;
        return true;

    }

    public bool AddPortal()
    {
        if (HasPortal) return false;
        _portalUsableCount = _portalMaxUse;
        return true;
    }

    public bool AddGloves()
    {
        if (HasGloves) return false;
        _glovesUsableCount = _glovesMaxUse;
        return true;
    }
}

public enum ItemType
{
    Lollipop,
    Portal,
    Gloves
}