using log4net.ObjectRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Codice.Client.Common.Connection.AskCredentialsToUser;

public class EquipmentManager : MonoBehaviour
{
    private Dictionary<string, Color> itemColors = new Dictionary<string, Color>
    {
        { "Power_Core", new Color32(89,7,200,255) },
        { "Mana_Core", new Color32(0,71,219,255) },
        { "Life_Core", new Color32(255,0,0,255) }
    };
    [SerializeField] Renderer Core;
    [SerializeField] private Transform rightHandTransform;
    [SerializeField] private Transform leftHandTransform;
    [SerializeField] private Transform HeadTransform;
    [SerializeField] private Transform[] Index;
    [SerializeField] private Transform[] Mid;
    [SerializeField] private Transform[] Ring;
    [SerializeField] private Transform[] Pinky;
    [SerializeField] private Transform[] Thumb;
    private Quaternion[] defaultRingRotations;
    private Quaternion[] defaultPinkyRotations;
    private Quaternion[] defaultMidRotations;
    private Quaternion[] defaultIndexRotations;
    private Quaternion[] defaultThumbRotations;
    private GameObject currentWeaponInstance;
    private GameObject currentHelmetInstance;
    private void Start()
    {
        Core.gameObject.SetActive(false);
        EquipmentSlot.OnItemChanged += UpdateItem;
        EquipmentSlot.OnMainHandEquipmentChanged += UpdateWeapon;
        defaultRingRotations = GetRotations(Ring);
        defaultPinkyRotations = GetRotations(Pinky);
        defaultMidRotations = GetRotations(Mid);
        defaultIndexRotations = GetRotations(Index);
        defaultThumbRotations = GetRotations(Thumb);
    }
    private Quaternion[] GetRotations(Transform[] joints)
    {
        Quaternion[] rotations = new Quaternion[joints.Length];
        for (int i = 0; i < joints.Length; i++)
            rotations[i] = joints[i].localRotation;
        return rotations;
    }
    private void UpdateItem(EquipmentSlot item)
    {
        if (item == null) return;
        InventoryItem i = item.Item;
        switch (item.itemType)
        {
            case ItemType.Helmet:
                UpdateHelmet(i);
                break;
            case ItemType.Core:
                UpdateCore(i);
                break;
            default:
                break;
        }
    }
    private void UpdateHelmet(InventoryItem helmet)
    {
        if (currentHelmetInstance != null)
        {
            Destroy(currentHelmetInstance);
            currentHelmetInstance = null;
        }
        if (helmet != null && helmet.item.ItemPrefab != null)
        {
            currentHelmetInstance = Instantiate(
                helmet.item.ItemPrefab,
                HeadTransform
            );
        }
    }
    private void UpdateCore(InventoryItem core)
    {
        if (core != null)
        {
            Core.gameObject.SetActive(true);
            if (!itemColors.TryGetValue(core.item.ItemName, out Color itemColor))
            {
                Debug.LogWarning($"No color mapping found for item: {core.item.ItemName}");
                itemColor = Color.white;
            }
            if (Core != null)
            {
                float intensity = (float)Math.Pow(2, 3);
                Core.material.color = itemColor*intensity;
            }
        }
        else
        {
            Core.gameObject.SetActive(false);
        }
    }
    private void UpdateWeapon(InventoryItem weapon)
    {
        if (currentWeaponInstance != null)
        {
            Destroy(currentWeaponInstance);
            ResetFingers();
            currentWeaponInstance = null;
        }
        if (weapon != null && weapon.item.ItemPrefab != null)
        {
            if (weapon.item.ItemSpecific == ItemSpecific.Book)
            {
                currentWeaponInstance = Instantiate(
                    weapon.item.ItemPrefab,
                    leftHandTransform
                );
            }
            else
            {
                CurlFingers();
                currentWeaponInstance = Instantiate(
                    weapon.item.ItemPrefab,
                    rightHandTransform
                );
            }
        }
    }
    private void ResetFingers()
    {
        SetRotations(Ring, defaultRingRotations);
        SetRotations(Pinky, defaultPinkyRotations);
        SetRotations(Mid, defaultMidRotations);
        SetRotations(Index, defaultIndexRotations);
        SetRotations(Thumb, defaultThumbRotations);
    }

    private void SetRotations(Transform[] joints, Quaternion[] targetRotations)
    {
        for (int i = 0; i < joints.Length; i++)
            joints[i].localRotation = targetRotations[i];
    }
    private void CurlFingers()
    {
        Ring[0].localRotation = Quaternion.Euler(-59.952f, -179.27f, 161.917f);
        Ring[1].localRotation = Quaternion.Euler(-49.302f, -1.497f, 1.856f);
        Ring[2].localRotation = Quaternion.Euler(-81.057f, 175.072f, -176.776f);
        Pinky[0].localRotation = Quaternion.Euler(-15.436f, -4.52f, 13.28f);
        Pinky[1].localRotation = Quaternion.Euler(-83.995f, 143.027f, -158.216f);
        Pinky[2].localRotation = Quaternion.Euler(-83.613f, 29.97f, -30.462f);
        Pinky[3].localRotation = Quaternion.Euler(-62.192f, -3.748f, 2.081f);
        Mid[0].localRotation = Quaternion.Euler(-61.716f, -175.575f, 159.841f);
        Mid[1].localRotation = Quaternion.Euler(-66.218f, -2.217f, 2.048f);
        Mid[2].localRotation = Quaternion.Euler(-66.038f, -1.437f, 0.132f);
        Index[0].localRotation = Quaternion.Euler(-42.48f, -180.73f, 167.31f);
        Index[1].localRotation = Quaternion.Euler(-66.554f, -4.479f, 4.58f);
        Index[2].localRotation = Quaternion.Euler(-78.878f, -9.273f, 9.334f);
        Thumb[0].localRotation = Quaternion.Euler(-18.921f, -105.787f, 31.84f);
        Thumb[1].localRotation = Quaternion.Euler(12.696f, -1.343f, -119.387f);
        Thumb[2].localRotation = Quaternion.Euler(16.017f, -7.838f, -65.026f);
    }
}
