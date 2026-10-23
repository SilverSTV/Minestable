# AGENTS.md

## Project overview

This repository contains a Unity game project.
Primary goal: make safe, minimal, testable changes that preserve project stability and avoid unnecessary editor/asset churn.

## Source of truth

Treat the existing codebase and project structure as the source of truth for current runtime behavior and architecture.
Treat `Docs/Game-Design-Document.md` as the source of truth for agreed target gameplay design.
In the GDD, `Implemented` / `Реализовано` describes intended current behavior but must be verified against code before relying on it; `Planned` / `Планируется` is not implemented unless the code proves otherwise.
Prefer adapting to the current architecture over introducing a new one.
Do not rewrite broad subsystems unless explicitly asked.

Do not copy detailed balance values or full feature descriptions from the GDD into this file. Keep them in the GDD so gameplay intent has one maintained source.

## Repository areas

Main working areas:

- `Assets/` — gameplay code, MonoBehaviours, ScriptableObjects, editor scripts, prefabs, scenes, art-related assets
- `Packages/` — package manifests and package settings
- `ProjectSettings/` — Unity project settings
- `Docs/` — product and game design documentation
- `.agents/skills/` — repo-scoped Codex skills
- `.codex/` — project-scoped Codex configuration

## Files and folders to avoid

Do not modify these unless the user explicitly asks:

- `Library/`
- `Temp/`
- `Logs/`
- `Obj/`
- `Build/`
- `Builds/`
- generated caches
- auto-generated IDE files

Do not delete `.meta` files.
Do not rename or move assets unless explicitly requested.

## Safety rules for Unity changes

Prefer changes in this order:

1. C# scripts in `Assets/**/*.cs`
2. asmdef files when needed
3. `Packages/manifest.json` only when required
4. `ProjectSettings/*` only when required
5. scenes, prefabs, ScriptableObject assets only on explicit request

When touching serialized Unity files (`.unity`, `.prefab`, `.asset`, `.anim`, `.controller`):
- keep changes minimal
- avoid broad formatting churn
- do not mass-rewrite unrelated serialized fields

## Coding conventions

- Use clear, small, focused classes and methods
- Preserve existing naming/style unless explicitly asked to standardize
- Avoid overengineering
- Prefer composition over deep inheritance
- Avoid hidden side effects
- Keep runtime code and editor-only code separated
- Put editor-only code under `Editor/` folders when relevant
- Avoid allocations in hot paths where possible
- Avoid `FindObjectOfType` / `FindObjectsOfType` in frequently executed gameplay code unless justified
- Avoid repeated `GetComponent` calls inside update loops unless cached
- Avoid LINQ in hot gameplay loops unless performance impact is irrelevant

## Architecture preferences

Unless asked otherwise:

- Keep gameplay logic decoupled from view code
- Do not move core logic into MonoBehaviours if it can stay in plain C# classes
- MonoBehaviours should mainly compose scene references, lifecycle hooks, and integration points
- ScriptableObjects are acceptable for static data/config
- Prefer deterministic logic when the system already relies on seeds or reproducible generation

## How to work on tasks

For each task:

1. Read the relevant files first
2. For gameplay work, read the relevant GDD section and any connected rules before designing the change
3. Identify the smallest safe change
4. Implement only what is needed
5. Preserve backward compatibility unless asked otherwise
6. Summarize what changed and what still may need verification

When a gameplay task touches one system, inspect its state transitions and adjacent systems when relevant. Typical transitions are returning to base, ordinary death, global collapse, starting a new session, and entering or resolving a boss fight.

Do not invent final behavior for items listed under `Open decisions` / `Открытые решения` in the GDD. If an unresolved decision is required to implement the task, ask the user or clearly mark a temporary prototype assumption.

## Before editing

Before large edits:
- inspect related scripts
- check for asmdef boundaries
- check for namespace consistency
- check whether the target code is runtime, editor, or test code

## Testing and verification

When possible, verify with the least invasive method available.

Preferred checks:
- compile-level consistency
- obvious namespace/reference correctness
- Unity-specific file placement correctness
- note any steps that should be verified in the Unity Editor

If you cannot run Unity or editor-specific validation, say so explicitly.
Do not claim the scene/prefab/editor behavior was verified unless it actually was.

## Git hygiene

Prefer small diffs.
Do not modify unrelated files.
If a change is risky, say so clearly.
If a file likely has high merge-conflict risk (scene/prefab/package manifest), mention it in the summary.

## Response expectations

When finishing a task, report:
- changed files
- what was changed
- possible risks
- what should be checked in Unity Editor

## Project mental model

This is a 2D side-view mining game with a grid-based world and incremental progression.
The central loop is mining, safely returning resources, buying upgrades, and making later expeditions measurably more efficient.

Core systems:
- MineGrid: pure C# grid model of the mine
- CellState: value struct for a single cell
- BlockDatabase: ScriptableObject with block configs
- MineGenerator: creates initial mine content from seed
- CaveGenerator: generates empty spaces
- VeinGenerator: generates ore veins
- CollapseSystem: scaffold for planned cave-ins and support checks; verify implementation before relying on it
- MineInteractionService: applies block damage, destruction, drops, and visual change records
- MineView: renders grid cells to scene objects / tilemaps
- GameManager: connects scene objects to runtime services

Rules:
- Keep gameplay rules in plain C# services when possible
- MonoBehaviours should mainly connect Unity scene references to gameplay services
- Do not move deterministic generation logic into MonoBehaviours

## Gameplay design invariants

Use `Docs/Game-Design-Document.md` for complete rules and current open questions. Preserve these high-level distinctions when implementing or reviewing gameplay:

- A session lasts from the first mined block until global collapse or a boss-fight outcome. It can contain multiple expeditions.
- An expedition lasts from entering the mine until returning to base or dying. Ordinary death ends the expedition, not the session.
- Delivered resources move to persistent base storage and are protected from later expedition deaths and collapse.
- Carried loot, dropped world loot, equipped items, consumables, and base storage are distinct states and must not share lifecycle assumptions implicitly.
- Persistent progression and base storage survive sessions. Generated mine geometry, routes, dropped ordinary ore, repaired outposts, and other session-scoped state reset on collapse unless the GDD explicitly defines an exception.
- Lost equipment is an explicit exception: it remains recoverable across regenerated mines until collected. The free replacement starter tool must not accidentally create exploitable permanent duplicates.
- Inventory capacity is slot- and stack-based, without weight. Loot slots, consumable slots, and equipment slots are separate concepts.
- Supports have diminishing returns and cannot stop global instability completely. Re-entering the mine or delivering more supports must not reset the diminishing-return calculation for the current session.
- Fast travel consumes session time and requires a traversable route. It ignores local interactions in its initial design but does not protect the player from missing the global-collapse deadline.
- Entering a boss room pauses global instability and locks retreat. Defeat causes a lethal collapse; victory ends the session through a safe transition to the next biome.

The intended emotional payoff is visible efficiency growth. When reviewing progression changes, verify that an upgrade creates an understandable improvement in a later expedition rather than only changing an internal number.

## Directory map

- `Assets/Game/Scripts/Gameplay/` — gameplay logic plus current character and view integrations; inspect each file rather than assuming it is pure C#
- `Assets/Game/Scripts/Gameplay/Mine/` — mine model, generation, interaction, state changes, and collapse work
- `Assets/Game/Scripts/Gameplay/Resources/` — resource storage and services
- `Assets/Game/Scripts/Gameplay/View/` — scene-facing MonoBehaviours and rendering glue
- `Assets/Game/Scripts/Infrastructure/` — composition and Unity integration, including `GameManager`
- `Assets/Game/Scripts/Root/Input/` — input collection and mode-specific interpretation
- `Assets/Game/Scripts/Configs/` — ScriptableObject config types and databases
- `Assets/Game/Data/Blocks/` — ScriptableObjects with block definitions
- `Assets/Game/Art/Blocks/` — block sprites
- `Assets/Game/Art/UI/` — UI sprites
- `Assets/Game/Prefabs/World/` — world prefabs
- `Assets/Game/Prefabs/UI/` — UI prefabs
- `Assets/Game/Scenes/` — scenes

## Core game concepts

Important domain enums:

- `BlockType` describes the type of block stored in a world cell.
  Examples: Air, Dirt, Stone, Ore, etc.
  It answers the question: "What block is currently placed in this cell?"

- `ResourceType` describes a resource stored, spent, or granted by the current resource systems.
  It answers the question: "What resource amount does the player receive or spend?"

Equipment and general inventory item identity are separate concepts from both enums. Do not reuse `BlockType` or `ResourceType` as a universal item identifier when those systems are implemented.

## Relationship between BlockType and ResourceType

`BlockType` and `ResourceType` are not the same concept.

- `BlockType` is world-state data.
- `ResourceType` is resource economy data.

A block may grant a resource when mined, but block type and granted resource type must not be assumed to be identical.

Examples:
- `BlockType.IronOre` may grant `ResourceType.Iron`
- `BlockType.Coal` may grant `ResourceType.Coal`
- `BlockType.Air` should not produce a drop

When implementing mining, drops, inventory, loot tables, or block destruction, preserve this separation.

## Design rule

Do not use `ResourceType` as a replacement for `BlockType`.
Do not use `BlockType` as an inventory identifier unless explicitly required by an existing system.

If a conversion is needed between block and drop, use an explicit mapping layer such as:
- block settings
- loot table
- drop config
- ScriptableObject data

## Review context

When reviewing recent changes, treat entries in `Current project decisions` as source-of-truth project intent unless the code clearly contradicts them at runtime.
Do not flag these decisions as architecture issues during MVP review.
Focus review findings on compile errors, runtime errors, behavioral regressions, missing serialized references, Unity build risks, and data loss/churn risks.
For gameplay changes, also report contradictions with agreed GDD rules that are in the task's scope. Do not report every unimplemented planned feature as a defect.

When persistence or lifecycle behavior is touched, verify the relevant GDD acceptance scenarios. At minimum consider:

- full loot inventory and dropped overflow
- returning loot to base, then dying on a later expedition
- recovering lost equipment before and after one or more collapses
- delivering supports over multiple expeditions in one session
- fast travel that reaches or misses the collapse deadline
- boss victory and defeat
- tutorial outcomes with safe return versus first-expedition death

## Current project decisions

- Input System callbacks may be used as a low-level state collector independently of the currently selected gameplay input service.
- `PlayerInputAdapter` is responsible only for collecting raw input state.
- `IInputService` implementations decide how to interpret that state for the current mode.
- Switching between debug/gameplay input modes currently does not require switching Unity Input System action maps.
- Resource spending must be atomic: batch spending should validate the full aggregated cost before mutating storage.
- Resource change notifications should be emitted once per changed resource, plus one aggregate `ResourcesChanged` event.
