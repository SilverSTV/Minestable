---
name: unity-bugfix
description: Use this skill for Unity bug fixing tasks: exceptions, incorrect gameplay behavior, broken references, null errors, compile issues, serialization issues, or lifecycle-related problems. Do not use it for broad refactors or entirely new features.
---

# Unity Bugfix Skill

## Goal

Diagnose and fix a Unity bug with the smallest safe code change.

## Workflow

1. Identify the symptom precisely:
   - compile error
   - runtime exception
   - wrong gameplay behavior
   - missing/broken reference
   - initialization order issue
   - serialization issue
   - editor-only issue

2. Read the nearest relevant files before changing code.

3. Prefer root-cause fixes over symptom masking.

4. Keep the fix minimal.
   Do not redesign architecture unless the bug clearly comes from architecture and the user asked for that level of change.

5. Pay special attention to Unity-specific causes:
   - `Awake` / `OnEnable` / `Start` ordering
   - null scene references
   - missing serialized fields
   - prefab overrides
   - ScriptableObject data assumptions
   - domain reload/static state assumptions
   - editor-only code leaking into runtime
   - incorrect use of `Time.deltaTime`
   - physics update vs frame update mismatch

## Fix rules

- Do not silence exceptions without reason
- Do not add broad try/catch blocks unless explicitly justified
- Do not add fallback logic that hides broken data wiring unless requested
- Preserve existing behavior outside the bug scope
- Avoid unrelated cleanup in the same change

## Output format

At the end, provide:

- probable root cause
- changed files
- exact fix made
- remaining uncertainty
- what to verify in Unity Editor or play mode