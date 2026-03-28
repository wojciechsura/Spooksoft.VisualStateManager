# Projects and dependencies analysis

This document provides a comprehensive overview of the projects and their dependencies in the context of upgrading to .NETCoreApp,Version=v10.0.

## Table of Contents

- [Executive Summary](#executive-Summary)
  - [Highlevel Metrics](#highlevel-metrics)
  - [Projects Compatibility](#projects-compatibility)
  - [Package Compatibility](#package-compatibility)
  - [API Compatibility](#api-compatibility)
- [Aggregate NuGet packages details](#aggregate-nuget-packages-details)
- [Top API Migration Challenges](#top-api-migration-challenges)
  - [Technologies and Features](#technologies-and-features)
  - [Most Frequent API Issues](#most-frequent-api-issues)
- [Projects Relationship Graph](#projects-relationship-graph)
- [Project Details](#project-details)

  - [Spooksoft.VisualStateManager.Test\Spooksoft.VisualStateManager.Test.csproj](#spooksoftvisualstatemanagertestspooksoftvisualstatemanagertestcsproj)
  - [Spooksoft.VisualStateManager\Spooksoft.VisualStateManager.csproj](#spooksoftvisualstatemanagerspooksoftvisualstatemanagercsproj)


## Executive Summary

### Highlevel Metrics

| Metric | Count | Status |
| :--- | :---: | :--- |
| Total Projects | 2 | All require upgrade |
| Total NuGet Packages | 7 | All compatible |
| Total Code Files | 40 |  |
| Total Code Files with Incidents | 3 |  |
| Total Lines of Code | 2896 |  |
| Total Number of Issues | 5 |  |
| Estimated LOC to modify | 2+ | at least 0,1% of codebase |

### Projects Compatibility

| Project | Target Framework | Difficulty | Package Issues | API Issues | Est. LOC Impact | Description |
| :--- | :---: | :---: | :---: | :---: | :---: | :--- |
| [Spooksoft.VisualStateManager.Test\Spooksoft.VisualStateManager.Test.csproj](#spooksoftvisualstatemanagertestspooksoftvisualstatemanagertestcsproj) | net48 | 🟢 Low | 0 | 0 |  | ClassLibrary, Sdk Style = True |
| [Spooksoft.VisualStateManager\Spooksoft.VisualStateManager.csproj](#spooksoftvisualstatemanagerspooksoftvisualstatemanagercsproj) | netstandard2.0;net48;net472;net8.0;net10.0 | 🟢 Low | 1 | 2 | 2+ | ClassLibrary, Sdk Style = True |

### Package Compatibility

| Status | Count | Percentage |
| :--- | :---: | :---: |
| ✅ Compatible | 7 | 100,0% |
| ⚠️ Incompatible | 0 | 0,0% |
| 🔄 Upgrade Recommended | 0 | 0,0% |
| ***Total NuGet Packages*** | ***7*** | ***100%*** |

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 2 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 1804 |  |
| ***Total APIs Analyzed*** | ***1806*** |  |

## Aggregate NuGet packages details

| Package | Current Version | Suggested Version | Projects | Description |
| :--- | :---: | :---: | :--- | :--- |
| Microsoft.CSharp | 4.7.0 |  | [Spooksoft.VisualStateManager.csproj](#spooksoftvisualstatemanagerspooksoftvisualstatemanagercsproj) | ✅Compatible |
| Microsoft.DotNet.UpgradeAssistant.Extensions.Default.Analyzers | 0.3.261602 |  | [Spooksoft.VisualStateManager.csproj](#spooksoftvisualstatemanagerspooksoftvisualstatemanagercsproj)<br/>[Spooksoft.VisualStateManager.Test.csproj](#spooksoftvisualstatemanagertestspooksoftvisualstatemanagertestcsproj) | ✅Compatible |
| Microsoft.NET.Test.Sdk | 16.* |  | [Spooksoft.VisualStateManager.Test.csproj](#spooksoftvisualstatemanagertestspooksoftvisualstatemanagertestcsproj) | ✅Compatible |
| MSTest.TestAdapter | 2.2.7 |  | [Spooksoft.VisualStateManager.Test.csproj](#spooksoftvisualstatemanagertestspooksoftvisualstatemanagertestcsproj) | ✅Compatible |
| MSTest.TestFramework | 2.2.7 |  | [Spooksoft.VisualStateManager.Test.csproj](#spooksoftvisualstatemanagertestspooksoftvisualstatemanagertestcsproj) | ✅Compatible |
| NETStandard.Library | 2.0.3 |  | [Spooksoft.VisualStateManager.csproj](#spooksoftvisualstatemanagerspooksoftvisualstatemanagercsproj) | ✅Compatible |
| System.Data.DataSetExtensions | 4.5.0 |  | [Spooksoft.VisualStateManager.csproj](#spooksoftvisualstatemanagerspooksoftvisualstatemanagercsproj) | Funkcja pakietu NuGet jest dołączona do dokumentacji struktury |

## Top API Migration Challenges

### Technologies and Features

| Technology | Issues | Percentage | Migration Path |
| :--- | :---: | :---: | :--- |

### Most Frequent API Issues

| API | Count | Percentage | Category |
| :--- | :---: | :---: | :--- |
| M:System.ValueType.Equals(System.Object) | 2 | 100,0% | Behavioral Change |

## Projects Relationship Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart LR
    P1["<b>📦&nbsp;Spooksoft.VisualStateManager.csproj</b><br/><small>netstandard2.0;net48;net472;net8.0;net10.0</small>"]
    P2["<b>📦&nbsp;Spooksoft.VisualStateManager.Test.csproj</b><br/><small>net48</small>"]
    P2 --> P1
    click P1 "#spooksoftvisualstatemanagerspooksoftvisualstatemanagercsproj"
    click P2 "#spooksoftvisualstatemanagertestspooksoftvisualstatemanagertestcsproj"

```

## Project Details

<a id="spooksoftvisualstatemanagertestspooksoftvisualstatemanagertestcsproj"></a>
### Spooksoft.VisualStateManager.Test\Spooksoft.VisualStateManager.Test.csproj

#### Project Info

- **Current Target Framework:** net48
- **Proposed Target Framework:** net10.0
- **SDK-style**: True
- **Project Kind:** ClassLibrary
- **Dependencies**: 1
- **Dependants**: 0
- **Number of Files**: 15
- **Number of Files with Incidents**: 1
- **Lines of Code**: 1321
- **Estimated LOC to modify**: 0+ (at least 0,0% of the project)

#### Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph current["Spooksoft.VisualStateManager.Test.csproj"]
        MAIN["<b>📦&nbsp;Spooksoft.VisualStateManager.Test.csproj</b><br/><small>net48</small>"]
        click MAIN "#spooksoftvisualstatemanagertestspooksoftvisualstatemanagertestcsproj"
    end
    subgraph downstream["Dependencies (1"]
        P1["<b>📦&nbsp;Spooksoft.VisualStateManager.csproj</b><br/><small>netstandard2.0;net48;net472;net8.0;net10.0</small>"]
        click P1 "#spooksoftvisualstatemanagerspooksoftvisualstatemanagercsproj"
    end
    MAIN --> P1

```

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 783 |  |
| ***Total APIs Analyzed*** | ***783*** |  |

<a id="spooksoftvisualstatemanagerspooksoftvisualstatemanagercsproj"></a>
### Spooksoft.VisualStateManager\Spooksoft.VisualStateManager.csproj

#### Project Info

- **Current Target Framework:** netstandard2.0;net48;net472;net8.0;net10.0
- **Proposed Target Framework:** netstandard2.0;net48;net472;net8.0;net10.0;net10.0
- **SDK-style**: True
- **Project Kind:** ClassLibrary
- **Dependencies**: 0
- **Dependants**: 1
- **Number of Files**: 25
- **Number of Files with Incidents**: 2
- **Lines of Code**: 1575
- **Estimated LOC to modify**: 2+ (at least 0,1% of the project)

#### Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph upstream["Dependants (1)"]
        P2["<b>📦&nbsp;Spooksoft.VisualStateManager.Test.csproj</b><br/><small>net48</small>"]
        click P2 "#spooksoftvisualstatemanagertestspooksoftvisualstatemanagertestcsproj"
    end
    subgraph current["Spooksoft.VisualStateManager.csproj"]
        MAIN["<b>📦&nbsp;Spooksoft.VisualStateManager.csproj</b><br/><small>netstandard2.0;net48;net472;net8.0;net10.0</small>"]
        click MAIN "#spooksoftvisualstatemanagerspooksoftvisualstatemanagercsproj"
    end
    P2 --> MAIN

```

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 2 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 1021 |  |
| ***Total APIs Analyzed*** | ***1023*** |  |

