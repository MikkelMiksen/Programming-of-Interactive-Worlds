# PIW – Dieselpunk Survival Outpost

A modular Unity survival-crafting game set in a frozen dieselpunk world (1955).

# 🎮 GAME OVERVIEW 

**PIW – Dieselpunk Survival Outpost**  is a first-person survival-crafting experience set in a frozen, diesel-powered alternate 1955.
Civilization has collapsed under an unexpected global freeze, and the player awakens in a remote outpost with nothing but basic tools, a failing furnace, and the will to survive.

The gameplay focuses on **temperature management, fat metabolism, crafting, resource gathering, and maintaining outpost machinery** in a harsh, unforgiving environment. Everything is designed to be **highly modular and expandable** , supporting flexible development and future systems such as advanced crafting stations, vehicle maintenance, and procedural resource generation.

# ❄️ Core Survival Pillars

- Maintain body temperature or freeze to death

- Manage fat reserves as long-term “metabolic stamina”

- Maintain HP, hunger, and fatigue

- Gather resources and craft advanced tools

- Fuel heat sources like furnaces, campfires, and diesel refineries

- Expand and automate crafting networks

- Survive dynamic day/night cycles, seasons, and environmental hazards

# 📅 Setting

- Year: 1955

- Genre: Dieselpunk survival sandbox

- Perspective: First person

# 🔧 CORE SYSTEMS

Below is a detailed breakdown of every major system currently implemented in the project.

# 1. 🧪 RecipeSO Crafting System

Each recipe defines:
- Ingredients (ResourceType + amount)
- Output item(s)
- Crafting category (Furnace, Workbench, Campfire, Refinery…)

```csharp
[CreateAssetMenu(menuName = "Scripts/Crafting")]
public class RecipeSO : ScriptableObject
{
    public string recipeName;
    public ResourceType outputItem;
    public ItemCategory outputCategory = ItemCategory.Material;
    public int outputAmount = 1;

    public GameObject outputPrefab;
    public int toolDurability;

    [System.Serializable]
    public struct Ingredient
    {
        public ResourceType type;
        public int amount;
    }

    public List<Ingredient> ingredients = new();

    public WorkstationType requiredStation = WorkstationType.Workbench;
}
```

## 🛠️ Unified Crafting Stations

All crafting stations use the same architecture:

- Workbench – item crafting
- Furnace – heat + smelting + burning
- Campfire – basic heat + cooking
- Refinery – turns oil → diesel


Each station:

- Reads all registered RecipeSO assets
- Filters recipes by category
- Checks inventory before crafting
- Removes all required resources
- Outputs results to inventory

## 🖥️ Crafting UI (UI Toolkit)

- Modular UI based on UXML + USS
- Auto-populates recipe lists based on category
- Uses dynamic panels for ingredient display
- Fully decoupled from station scripts

# 2. 🎒 Player Inventory System (MJ_PlayerInventory)

A robust, stack-based inventory built for speed and modular expansion.

## 📦 Inventory Features

- 80 slots

- Max stack size: 128 items

- Supports all resource types:

  * Wood, Stone, Metal, Coal, Water, Diesel, etc.

- Auto-sort by resource type

- Automatic:

  * Stack merging

  * Partial stack filling

  * Empty stack removal

## 🔍 API Methods

- AddResource(ResourceType, amount)
- RemoveResource(ResourceType, amount)
- HasResource(ResourceType, amount)
- HasAllResources(List<Ingredient>)
- RemoveAllRequiredResources(List<Ingredient>)
- ClearInventory()
- AutoSort()

## 🧩 Dynamic HUD Integration

MJ_HUDManager handles:

- Hotbar slots (HUD 0–9)
- Icons mapped from ScriptableObjects
- Amount text updated via TMP
- Automatic refresh when inventory changes

# 3. 🔨 Tools HUD & Equipment System

Tools are displayed in a hotbar (1–4 or 1–9 depending on configuration).

## 🔧 Tool System Features

- Each tool has a prefab
- Selecting a tool:

  * Highlights slot

  * Spawns tool prefab into player’s hand

  * Enables the appropriate tool behavior

- Modular system allows adding new tools easily:

  * Axes

  * Pickaxes

  * Hammers

  * Repair tools

  * Advanced power tools

## 🧱 Prefab Organization
All tools use:
- **Empty parent GameObject** → clean pivot
- Actual model child underneath

# 4. 🔥 Furnace / Campfire / Refinery Systems

## **★ Shared Architecture**

All heat/crafting stations follow a unified backend pattern.

**Furnace**

- Burns **wood, coal, diesel**, and any burnable from ResourceData
- Each burnable has:
IsBurnable

  * BurnTime

  * BurnRate

  * HeatOutput

- urnace UI toggles via interaction system

- Furnace affects **local environmental temperature**

- Fuel dictionary supports stacking

**Campfire**

- Simpler, early-game heat source

- Cooks basic food

- Supports same RecipeSO pipeline

**Refinery**

- Converts raw oil or biomass into diesel

- Outputs refined fuel directly into inventory

- Uses RecipeSO category = Refinery

# 5. 🧊 Temperature & Metabolism System

## 🌡️ Body Temperature (MJ_BodyTemp)

- Starts at **37°C**

- Safe range: **28°C → 41°C**

- Temperature drifts toward environment temp

- Cold → HP loss + fat burn increase

- Heat sources slow temperature loss

- Color-coded body temp indicator in HUD

# 🥓 Fat Loss & Metabolism

- Player burns fat over time

- Rate increases with:

  * Cold exposure

  * Sprinting

  * Heavy work

- Fat acts as long-term survival buffer

- Food restores fat reserves

# 6. 🔌 Modular Project Design

The entire project emphasizes modular architecture:

  ## 🔹 Data-driven

- RecipeSO

- ResourceData

- Tool definitions

- Crafting categories

## 🔹 Modular systems

- Inventory

- Crafting

- Heat sources

- Tools

- HUD panels

- Interactables

   ## 🔹 Loose coupling

Systems reference each other minimally, making the game:

- Easy to extend

- Easy to maintain

- Friendly for future contributors

# 🧱 TECHNICAL ARCHITECTURE

## 📁 Folder Structure (Recommended Layout)

```plaintext
Assets/
│
├── Scripts/
│   ├── Player/
│   ├── Systems/
│   ├── Crafting/
│   ├── UI/
│   ├── Tools/
│   └── Environment/
│
├── ScriptableObjects/
│   ├── Recipes/
│   ├── Resources/
│   └── Tools/
│
├── Prefabs/
│   ├── Tools/
│   ├── Stations/
│   └── UI/
│
├── UI Toolkit/
│   ├── UXML/
│   └── USS/
│
├── Resources/
│
└── Art/
    ├── Models/
    ├── Textures/
    └── Icons/

```

##  🎮 CONTROLS (Introduction)

| Action           | Key        |
| ---------------- | ---------- |
| Move             | WASD       |
| Look             | Mouse      |
| Jump             | Space      |
| Sprint           | Left Shift |
| Crouch           | Left Ctrl  |
| Interact         | E          |
| Hotbar Selection | 1–9        |
| Inventory        | I          |
| Close UI         | Escape     |

# 🧩 INTERACTION SYSTEM

The player detects the nearest IInteractable using MJ_PlayerController.

### **Features**

- Shows prompt

- Highlights object

- Press E → object’s Interact() is called

- Used for:

  * Furnace

  * Workbenches

  * Resource gathering

  * Tools

  * Containers
 
## 🖥️ UI SYSTEM OVERVIEW

### **HUD Includes**

- Health bar

- Body temperature

- Time + date + season

- Hotbar (HUD 0–9)

- Resource slot icons

- Dynamic TMP number updates

### **Furnace UI**

- Separate UI panel

- Dropdown for fuel types

- Amount input

- Fuel status display

- Heat output information

## 📓 DEVELOPMENT NOTES

### 🧩 UI Structure

- HUD created with standard Canvas

- TMP used for all number displays

- Inventory icons map via Sprite-atlas in HUD Manager

- Furnace UI uses custom logic & separate controller

### 🎭 Animation System

- MJ_AnimPlayerController reads raw movement inputs

- Sets Animator parameters:

  * IsIdle

  * IsWalkingForward

  * IsWalkingLeft

  * IsWalkingRight

  * IsWalkingBackwards

- Requires assigned Animator Controller

### 🧱 Prefab Method

All prefabs use:

- Empty parent (pivot)

- Model is child

- Ensures painless positioning & scaling

### Furnace vs HUD

- Furnace UI = full interactive menu

- HUD = lightweight, always-on display

- Completely different logic and scripts

## 🚀 FUTURE FEATURES

Planned expansions:

- Base-building & outpost expansion

- NPC mechanics (traders, scavengers)

- Vehicles (snowtrucks, generators)

- Weather systems: blizzards, storms

- Procedural resource nodes

- Skill progression

- Advanced machinery

- World exploration

# 🏁 CREDITS

- **Developer:** Mikkel Emil Weber Juel, creator of *PIW - Dieselpunk Survival Ou8tpost*
- **Engine:** Unity 6
- **UI:** TextMeshPro, UI Toolkit
- **Art & Design:** MiksenDesigns :

<p align="center">
  <a href="https://instagram.com/the_dev_mikkel_juel">
    <img src="https://img.shields.io/badge/Instagram-Profile-ff0069?logo=instagram&logoColor=white" />
  </a>
</p>



<p align="center">
  <a href="https://github.com/MikkelMiksen">
    <img src="https://img.shields.io/badge/Follow%20on%20GitHub-000000?logo=github&logoColor=white" />
  </a>
</p>




<h2 align="center">🌌 My Other Project — <strong>Valendria</strong></h2>

<p align="center">
  <a href="https://github.com/MikkelMiksen/Valendria">
    <img src="https://img.shields.io/badge/Open%20Valendria%20Repo-6a5acd?style=for-the-badge&logo=github&logoColor=white" />
  </a>
</p>

<p align="center">
  A fantasy RPG universe built with deep worldbuilding, magic systems, and modular game mechanics.  
  Developed in parallel with PIW – Dieselpunk Survival Outpost.
</p>
