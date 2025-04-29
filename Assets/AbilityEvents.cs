using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class AbilityEvents
{
    public static event Action<AbilityItem> OnAbilityEquipped;

    public static void TriggerAbilityEquipped(AbilityItem ability, Guid id)
    {
        OnAbilityEquipped?.Invoke(ability);
    }
}