# Terraria-1.4.5.8-decomp-recomp

A **decompiled** C# game codebase (Terraria 1.4.5.8, .NET Framework 4.7.2, x86)
rebuilt into a working executable, then five defects repaired.

All five were introduced by the decompiler. All five **compile cleanly and produce
zero compiler warnings**.

<img width="1920" height="1200" alt="{63C0C58B-DB1A-4486-97ED-26ABDD4D5B54}" src="https://github.com/user-attachments/assets/97f37404-fcd1-4586-a0e0-686bfaa1b87a" />
<img width="1920" height="1200" alt="{045D5960-78F0-447A-B645-0189ABAA827D}" src="https://github.com/user-attachments/assets/1a446f0c-edf6-47e6-92c6-ef87c1e8206b" />


---

## Contents

- [The five bugs](#the-five-bugs)
- [Damage reference](#damage-reference)
- [How they were found](#how-they-were-found)
- [Build](#build)
- [Warnings](#warnings)
- [Not included](#not-included)
- [Disclaimer](#disclaimer)

---

## The five bugs

### 1 — Virtual dispatch decompiled as a self-call

Instant `StackOverflowException` on any world load.

The decompiler normalised `((Game)this).Update(gameTime)` into `this.Update(gameTime)`.
`Main` overrides `Update`, so it recurses forever. Measured at **64,526 frames** of
`Main.Update`.

```diff
- this.Update(gameTime);        + base.Update(gameTime);
- this.Update(new GameTime());  + base.Update(new GameTime());
- this.Initialize();            + base.Initialize();
- this.Draw(gameTime);          + base.Draw(gameTime);     // ×2
- this.EndDraw();               + base.EndDraw();
```

Six sites, one file (`Main.cs`). Invisible in review — `this.X()` looks normal.

### 2 — Exception handler recursing into itself → total freeze

Presents as "unresponsive, burning CPU", not a crash, so it survives normal debugging.

```csharp
AppDomain.CurrentDomain.FirstChanceException += (sender, args) =>
{
    var trace = new StackTrace(1, true).ToString();   // ← may throw
    var text  = PrintException(args.Exception, trace); // ← may throw
};
```

The handler exists to *report* exceptions. An exception thrown while reporting
fires `FirstChanceException` again, re-entering the same handler, which throws again.

Fixed with a `[ThreadStatic]` re-entrancy guard plus `try/catch/finally`.
This fix also **unmasked bug 4** — the runaway handler had been swallowing the
real exception.

### 3 — Lost PE flag → 32-bit address-space exhaustion

Large worlds only: `OutOfMemoryException` at `WorldGen.clearWorld()` line 7486,
on `Main.tile[l, m] = new Tile()`.

The rebuild dropped `IMAGE_FILE_LARGE_ADDRESS_AWARE` from the COFF characteristics:

```text
original  Characteristics=0x0122   LARGE_ADDRESS_AWARE=True    → 4 GB
rebuilt   Characteristics=0x0102   LARGE_ADDRESS_AWARE=False   → 2 GB
```

6400×1800 = **11.52 million** tiles, each individually allocated.
Too much for 2 GB, trivial under 4 GB.

The flag is not managed metadata, no compiler warns about it, and it is **lost on
every rebuild**.

### 4 — Null check decompiled as a cast → world generation dies silently

"Create New World" produces no progress bar. No crash, no dialog — just a button
that appears broken.

```diff
- if ((int)biomes == 0)     + if (biomes == null)
      biomes = new JObject();     biomes = new JObject();
```

`JToken` defines `explicit operator int`, so `(int)biomes` **compiles**, then throws
`ArgumentException: Can not convert Object to Int32` at runtime.
Generation dies on its first statement — before it can report anything, and the
progress bar is what it drives.

### 5 — 12 language files reclassified as satellite assemblies → raw UI keys

Interface text renders as `UI.Workshop` instead of `Workshop`.

```text
bin/Release/net472/en-US/Terraria.resources.dll     (138 KB)   ← the 12 files
bin/Release/net472/zh-Hans/Terraria.resources.dll   (136 KB)
```

MSBuild's culture inference saw `Content.**en-US**.json`, concluded "localised
resource", and emitted a satellite assembly. The split files (`.Game.json`,
`.Items.json`, …) have no bare culture token in their names, so they embedded
normally — which is why *some* text worked and some did not.

The game only reads these from the main assembly, so the 12 primary language
files became invisible.

```diff
- <EmbeddedResource Include="...en-US.json" LogicalName="...en-US.json" />
+ <EmbeddedResource Include="...en-US.json" LogicalName="...en-US.json" WithCulture="false" />
```

Manifest resources **88 → 100**, matching the reference binary.

---

## Damage reference

Every row compiles. None produce a warning.

| Damage pattern | Failure mode |
| ---- | ---- |
| `((Base)this).M()` → `this.M()` | infinite recursion |
| `ldnull`+`ceq` → `(int)obj == 0` | runtime `ArgumentException` |
| enum compare → `(int)enumVal == 1` | **benign**, semantics correct |
| empty case → `switch (x) {}` | **benign**, no-op |
| resource culture inference → satellite assembly | data silently relocated |
| lost COFF characteristics | address space halved → OOM |

The two benign rows matter as much: flagging everything and flagging nothing are
equally useless. Deciding which `(int)x == 0` is damage and which is an enum test
requires knowing what `x` actually is.

---

## How they were found

| Technique | Bugs found |
| --- | --- |
| ClrMD (x86 suspend-attach, dump every thread's stack) | 1, 2 |
| Windows Application event log (`0xc00000fd` = stack overflow) | nature of 1 |
| Built-in `-logerrors` / `-logfile` (first-chance exceptions) | 2, 4, 5 |
| Direct PE header + manifest resource comparison | 3, 5 |
| Metadata diff (types/fields/methods/properties/events) | 0 |
| Static scans (self-calls, illegal casts on reference types) | 0 |
| **Compiler warnings** | **0** |

The last three only narrowed the search.

---

## Build

```bash
dotnet build -c Release
```

**0 errors, 633 warnings.** `LARGEADDRESS_AWARE` must be re-applied post-build
(see bug 3); no compiler can do it.

---

## Warnings

| Code | Count | Share | Verdict |
| ---- | ---- | ---- | ---- |
| CS0618 | 1216 | 96.1% | obsolete API, intentional — do not touch |
| CS0649 | 18 | 1.4% | false positive, populated by JSON deserialisation |
| CS0169 | 12 | 0.9% | private field never read |
| CS0067 | 8 | 0.6% | event used only via framework reflection |
| CS0219 | 6 | 0.5% | assigned, never used |
| CS0414 | 2 | 0.2% | assigned, never read |
| CS1522 | 2 | 0.2% | empty `switch`, confirmed no-op |
| CS9113 | 2 | 0.2% | unused parameter |

Auditing them surfaced one real artefact: a shader saturation field that is read
but never assigned, so permanently zero (cosmetic).

None of the five serious bugs produced a single warning. In decompiled code,
warning count is a poor proxy for runtime correctness.

---

## Not included

- Game assets: no images, textures, audio, fonts
- Shipped binaries: no compiled game is redistributed
- Localisation data: language JSON absent, only the build config referencing it
- Proprietary text: no item names, dialogue or lore reproduced

---

## Disclaimer

Terraria is copyright © Re-Logic LLC. *Terraria* is a trademark of Re-Logic LLC.
This project is not affiliated with, endorsed by, or sponsored by Re-Logic LLC or
Team Terraria.

An independent, non-commercial educational study of a class of software-engineering
failure and how to detect it. Contains no game assets, no game binaries, and no
substantial copyrighted text.
