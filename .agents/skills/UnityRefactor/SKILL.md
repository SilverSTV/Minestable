---
name: unity-refactor
description: Use this skill for safe Unity-oriented refactoring: simplifying classes, extracting logic, improving readability, reducing coupling, or preparing code for future features without intentionally changing behavior. Do not use it for feature implementation or direct bug-only fixes unless refactoring is the requested goal.
---

# Unity Refactor Skill

## Goal

Improve code structure while preserving behavior.

## Workflow

1. Identify the smallest refactor that provides clear value:
   - extract method/class
   - separate runtime logic from MonoBehaviour glue
   - reduce coupling
   - improve naming
   - isolate configuration/data
   - remove duplication

2. Preserve behavior.
   If behavior changes are unavoidable, state them explicitly before presenting the result.

3. Keep Unity-specific risks in mind:
   - serialized field names
   - inspector bindings
   - script file and class name relationships
   - MonoBehaviour attachment stability
   - ScriptableObject asset compatibility
   - asmdef boundaries
   - namespace changes

4. Avoid risky refactors unless explicitly requested:
   - mass renames across serialized assets
   - broad folder moves
   - changing public serialized API
   - replacing working patterns with speculative architecture

## Refactor principles

- Prefer extraction over rewrite
- Preserve public contracts unless explicitly allowed to change them
- Avoid mixing cleanup with feature work
- Keep diffs small and reviewable
- Add comments only when they clarify non-obvious logic

## Unity serialization precautions

Before renaming serialized members or moving MonoBehaviour classes:
- consider inspector breakage
- consider asset reference breakage
- prefer non-breaking transitions where possible

## Output format

At the end, provide:

- refactor goal
- preserved behavior assumptions
- changed files
- possible Unity serialization/editor risks
- what should be smoke-tested