using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;

public static class NameGenerator
{
    private static readonly Dictionary<ItemSpecific, List<string>> NameOptions = new()
    {
        {
            ItemSpecific.None, new List<string>
            {
                "Emperor's",
                "Ancient",
                "Forgotten",
                "Bloodied",
                "Wicked",
                "Holy",
                "Doomed",
                "Glorious",
                "Runed",
                "Twilight"
            }
        },
        {
            ItemSpecific.Axe, new List<string>
            {
                "Chopper",
                "Cleaver",
                "Splitter",
                "Decapitator",
                "Ravager",
                "Feller",
                "Hewer",
                "Reaper",
                "Hacksplit",
                "Goreblade"
            }
        },
        {
            ItemSpecific.Sword, new List<string>
            {
                "Slasher",
                "Fang",
                "Edge",
                "Katana",
                "Rapier",
                "Sabre",
                "Blade",
                "Longsword",
                "Wyrmfang",
                "Soulcarver"
            }
        },
        {
            ItemSpecific.Bodyarmour, new List<string>
            {
                "Carapace",
                "Plate",
                "Hauberk",
                "Harness",
                "Jerkin",
                "Tunic",
                "Breastplate",
                "Shell",
                "Vestment",
                "Hide"
            }
        },
        {
            ItemSpecific.Helmet, new List<string>
            {
                "Dome",
                "Crown",
                "Greathelm",
                "Visage",
                "Skullcap",
                "Warcap",
                "Helm",
                "Headguard",
                "Mask",
                "Casque"
            }
        },
        {
            ItemSpecific.Gloves, new List<string>
            {
                "Grip",
                "Claws",
                "Grasp",
                "Fists",
                "Gauntlets",
                "Wraps",
                "Handguards",
                "Talonwraps",
                "Fingerguards",
                "Knucklebinds"
            }
        },
        {
            ItemSpecific.Pants, new List<string>
            {
                "Trousers",
                "Legwraps",
                "Greaves",
                "Leggings",
                "Pantaloons",
                "Kneeguards",
                "Cuisses",
                "Quilted Legs",
                "Leathers",
                "Girdlewear"
            }
        },
        {
            ItemSpecific.Boots, new List<string>
            {
                "Slippers",
                "Striders",
                "Footwraps",
                "Greaves",
                "Sandals",
                "Hoofguards",
                "Treads",
                "Boots",
                "Stormshoes",
                "Steppers"
            }
        },
        {
            ItemSpecific.Elixir, new List<string>
            {
                "Concoction",
                "Tincture",
                "Vial",
                "Draught",
                "Philter",
                "Brew",
                "Potion",
                "Infusion",
                "Essence",
                "Flask"
            }
        },
        {
            ItemSpecific.Life, new List<string>
            {
                "Concoction",
                "Tincture",
                "Vial",
                "Draught",
                "Philter",
                "Brew",
                "Potion",
                "Infusion",
                "Essence",
                "Flask"
            }
        },
        {
            ItemSpecific.Mana, new List<string>
            {
                "Concoction",
                "Tincture",
                "Vial",
                "Draught",
                "Philter",
                "Brew",
                "Potion",
                "Infusion",
                "Essence",
                "Flask"
            }
        },
        {
            ItemSpecific.Ring, new List<string>
            {
                "Trinket",
                "Band",
                "Loop",
                "Seal",
                "Sigil",
                "Circle",
                "Coil",
                "Wreath",
                "Charm",
                "Ring"
            }
        },
        {
            ItemSpecific.Amulet, new List<string>
            {
                "Choker",
                "Pendant",
                "Talisman",
                "Locket",
                "Necklace",
                "Runechain",
                "Hexstring",
                "Medallion",
                "Relic",
                "Amulet"
            }
        }
    };
    public static void GenerateName(InventoryItem Item)
    {
        List<string> prefix = NameOptions[ItemSpecific.None];
        ItemSpecific tag = Item.data.ItemSpecific;
        if (tag > ItemSpecific.None && tag < ItemSpecific.Life ) tag = (ItemSpecific)Enum.Parse(typeof(ItemSpecific), Item.data.ItemType.ToString());
        if (!NameOptions.ContainsKey(tag)) return;
        List<string> suffix = NameOptions[tag];
        Item.ItemName = prefix[UnityEngine.Random.Range(0, prefix.Count)] + " " +suffix[UnityEngine.Random.Range(0, suffix.Count)];
    }
}