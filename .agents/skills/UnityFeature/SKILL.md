---
name: unity-feature
description: Use this skill when adding a small or medium Unity gameplay feature, system extension, UI behavior, content wiring step, or data-driven mechanic without rewriting the project architecture. Do not use it for pure bugfixes or broad codebase-wide refactors.
---

# Unity Feature Skill

## Goal

Add a new Unity feature that fits the existing project architecture and minimizes breakage.

## Workflow

1. Understand the requested feature in terms of:
   - data
   - runtime logic
   - scene/prefab integration
   - editor/config requirements
   - user-visible behavior

2. Inspect the existing architecture and attach the feature to current patterns instead of inventing a new subsystem by default.

3. Choose the lightest viable design:
   - plain C# class for core logic
   - MonoBehaviour only for Unity integration/lifecycle
   - ScriptableObject for static config/data when appropriate

4. Implement incrementally:
   - add data model changes first
   - add logic second
   - add integration points last

5. Keep serialized-file edits small and deliberate.

## Design rules

- Prefer explicit references over scene-wide searches
- Keep feature code cohesive
- Avoid mixing runtime logic and editor tooling in one place
- Do not introduce service locators, event buses, or dependency systems unless the project already uses them or the user requested them
- Preserve determinism if the existing system depends on seeded generation

## When feature touches scenes/prefabs

If scene or prefab updates are required:
- describe which object or component should be updated
- keep automated serialized changes minimal
- mention manual editor wiring if safer than mass file edits

## Output format

At the end, provide:

- implemented feature summary
- changed files
- scene/prefab/editor steps, if any
- known limitations
- suggested validation steps