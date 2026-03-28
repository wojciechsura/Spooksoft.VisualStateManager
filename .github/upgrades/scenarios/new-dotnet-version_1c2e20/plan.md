# .NET 10.0 Upgrade Plan

## Table of Contents

- [Executive Summary](#executive-summary)
- [Migration Strategy](#migration-strategy)
- [Detailed Dependency Analysis](#detailed-dependency-analysis)
- [Project-by-Project Plans](#project-by-project-plans)
  - [Spooksoft.VisualStateManager.csproj](#spooksoftvisualstatemanagercsproj)
  - [Spooksoft.VisualStateManager.Test.csproj](#spooksoftvisualstatemanagertestcsproj)
- [Risk Management](#risk-management)
- [Testing & Validation Strategy](#testing--validation-strategy)
- [Complexity & Effort Assessment](#complexity--effort-assessment)
- [Source Control Strategy](#source-control-strategy)
- [Success Criteria](#success-criteria)

---

## Executive Summary

### Scenario Description
This plan outlines the upgrade of the Spooksoft.VisualStateManager solution from its current multi-targeting configuration to include .NET 10.0 (Long Term Support) as the primary target framework.

### Scope

**Projects Affected**: 2 projects
- `Spooksoft.VisualStateManager.csproj` (main class library)
- `Spooksoft.VisualStateManager.Test.csproj` (test project)

**Current State**:
- Main project: Multi-targeting `netstandard2.0;net48;net472;net8.0;net10.0` (already includes net10.0)
- Test project: `net48`
- Total codebase: 2,896 lines of code across 40 files
- 7 NuGet packages (all compatible)

**Target State**:
- Main project: Clean up duplicate net10.0 target (already present)
- Test project: Upgrade from `net48` to `net10.0`
- Remove `System.Data.DataSetExtensions` package (functionality now in framework)
- Address 2 behavioral changes in `System.ValueType.Equals`

### Selected Strategy

**All-At-Once Strategy** - All projects upgraded simultaneously in a single operation.

**Rationale**:
- ✅ **Small solution**: Only 2 projects with simple dependency structure
- ✅ **Low complexity**: Both projects rated 🟢 Low difficulty
- ✅ **Full compatibility**: All 7 packages compatible with .NET 10.0
- ✅ **No security issues**: Zero vulnerabilities requiring immediate attention
- ✅ **Simple dependency chain**: Test project depends only on main library
- ✅ **Minimal code impact**: Estimated 2+ LOC modifications (0.1% of codebase)
- ✅ **Main project partially upgraded**: Already has net10.0 in multi-target configuration

### Discovered Metrics

| Metric | Value | Assessment |
|--------|-------|------------|
| Total Projects | 2 | Very small |
| Dependency Depth | 1 level | Simple |
| High-Risk Projects | 0 | Low risk |
| Security Vulnerabilities | 0 | No critical issues |
| Package Compatibility | 100% | Excellent |
| LOC Impact | 0.1% | Minimal |
| API Breaking Changes | 0 binary, 0 source | None |
| Behavioral Changes | 2 | Low impact |

### Complexity Classification

**Simple Solution** ✅

This solution exhibits characteristics ideal for fast, atomic upgrade:
- Project count ≤5 (actual: 2)
- Dependency depth ≤2 (actual: 1)
- No high-risk indicators
- No security vulnerabilities
- All packages compatible

### Expected Remaining Iterations

Following the fast batch approach for simple solutions:
- **Phase 1 Complete**: Discovery & Classification (3 iterations) ✅
- **Phase 2**: Foundation (3 iterations) - In Progress
- **Phase 3**: Dynamic Detail Generation (1-2 iterations for all projects)

**Total estimated iterations**: 7-8 iterations

---

## Migration Strategy

### Approach Selection

**Selected: All-At-Once Strategy**

All projects in the solution will be upgraded simultaneously in a single atomic operation.

### Justification

This solution is an ideal candidate for All-At-Once migration:

#### Solution Characteristics Favoring All-At-Once
1. **Small Scale**: Only 2 projects (threshold: <30 projects)
2. **Modern Foundation**: Main project already multi-targets including .NET 10.0
3. **Full Compatibility**: 100% of packages compatible (7/7)
4. **Low Complexity**: Both projects rated 🟢 Low difficulty
5. **Clean Dependencies**: Simple linear dependency (no cycles)
6. **Minimal Impact**: Only 0.1% of codebase requires modification
7. **No Critical Risks**: Zero security vulnerabilities

#### All-At-Once Strategy Rationale

**Advantages for This Solution**:
- ✅ **Fastest completion time**: Single coordinated update
- ✅ **No multi-targeting complexity**: Main library already multi-targets
- ✅ **Simple testing**: One comprehensive test pass
- ✅ **Clean dependency resolution**: No intermediate compatibility states
- ✅ **Minimal coordination overhead**: Only 2 projects to synchronize

**Risk Assessment**:
- ⚠️ Short period of build instability (mitigated by working on upgrade branch)
- ⚠️ Both projects must succeed together (mitigated by low complexity and full compatibility)
- ✅ Low overall risk due to solution characteristics

### Dependency-Based Ordering

While updates happen simultaneously, the build order respects dependencies:

1. **Spooksoft.VisualStateManager.csproj** compiles first (no dependencies)
2. **Spooksoft.VisualStateManager.Test.csproj** compiles second (depends on main library)

This natural build order ensures:
- Main library changes are validated before test project builds
- Test project can reference the updated main library
- Build failures surface in dependency order

### Execution Approach

**Single Atomic Operation**:
1. Update both project files' `TargetFramework` properties simultaneously
2. Remove obsolete `System.Data.DataSetExtensions` package from main library
3. Restore dependencies across entire solution
4. Build entire solution (both projects)
5. Fix any compilation errors discovered (referencing breaking changes catalog)
6. Rebuild to verify success
7. Execute test project to validate functionality

**No Intermediate States**: 
- Solution goes from "current state" to "fully upgraded" in one operation
- No projects left in transitional framework versions
- No multi-targeting expansion required (main project already multi-targets)

---

## Detailed Dependency Analysis

### Dependency Graph Summary

The solution has a straightforward two-level dependency structure:

```
Level 0 (Foundation):
└── Spooksoft.VisualStateManager.csproj
    └── Dependencies: None
    └── Used by: Spooksoft.VisualStateManager.Test

Level 1 (Test Layer):
└── Spooksoft.VisualStateManager.Test.csproj
    └── Dependencies: Spooksoft.VisualStateManager
    └── Used by: None (top-level)
```

### Project Groupings by Migration Phase

Given the All-At-Once strategy, both projects will be upgraded in a single coordinated operation. However, the logical dependency order is:

**Phase 1: Atomic Upgrade** (all projects simultaneously)
- `Spooksoft.VisualStateManager.csproj` - Foundation library
- `Spooksoft.VisualStateManager.Test.csproj` - Test project

**Execution Notes**:
- While the test project depends on the main library, the All-At-Once approach updates both project files simultaneously
- The main library already has `net10.0` in its multi-target list, so the primary work is on the test project
- Both projects will be built together after framework updates

### Critical Path Identification

**Critical Path**: Main Library → Test Project

Since this is a simple linear dependency:
1. The main library must compile successfully first (though updates happen simultaneously)
2. The test project build depends on the main library's successful compilation
3. Test execution validates the entire upgrade

**No Blocking Issues**: 
- No circular dependencies
- No complex multi-project interdependencies
- No shared dependency version conflicts

---

## Project-by-Project Plans

### Spooksoft.VisualStateManager.csproj

#### Current State
- **Target Framework**: Multi-targeting `netstandard2.0;net48;net472;net8.0;net10.0`
- **Project Type**: Class Library (SDK-style)
- **Dependencies**: None (foundation library)
- **Dependants**: Spooksoft.VisualStateManager.Test
- **Packages**: 3 explicit packages (Microsoft.CSharp, Microsoft.DotNet.UpgradeAssistant.Extensions.Default.Analyzers, System.Data.DataSetExtensions) + 1 transitive (NETStandard.Library)
- **Lines of Code**: 1,575
- **Files**: 25 total (2 with incidents)
- **Risk Level**: 🟢 Low

#### Target State
- **Target Framework**: Clean multi-target configuration `netstandard2.0;net48;net472;net8.0;net10.0` (note: assessment shows duplicate net10.0, should be cleaned)
- **Packages**: Remove `System.Data.DataSetExtensions` (functionality now in framework)

#### Migration Steps

**1. Prerequisites**
- ✅ .NET 10.0 SDK already installed (verified during initialization)
- ✅ Project already includes net10.0 in multi-target list

**2. Project File Updates**

Update `Spooksoft.VisualStateManager.csproj`:
- Verify `<TargetFrameworks>` contains `netstandard2.0;net48;net472;net8.0;net10.0` (no duplicate net10.0)
- No framework change required if net10.0 already present

**3. Package Updates**

| Package | Current Version | Target Version | Action | Reason |
|---------|----------------|----------------|--------|--------|
| System.Data.DataSetExtensions | 4.5.0 | Remove | **Remove** | Functionality now included in .NET 10.0 framework |
| Microsoft.CSharp | 4.7.0 | 4.7.0 | **Keep** | ✅ Compatible with .NET 10.0 |
| Microsoft.DotNet.UpgradeAssistant.Extensions.Default.Analyzers | 0.3.261602 | 0.3.261602 | **Keep** | ✅ Compatible (dev-time analyzer) |
| NETStandard.Library | 2.0.3 (transitive) | 2.0.3 | **Keep** | ✅ Compatible |

**4. Expected Breaking Changes**

**Behavioral Changes** (⚠️ Requires Runtime Testing):

| API | Location | Line | Issue | Impact |
|-----|----------|------|-------|--------|
| `System.ValueType.Equals(Object)` | `Conditions\SwitchCondition.cs` | 36 | Api.0003 | Behavioral change in .NET 10.0 equality comparison |
| `System.ValueType.Equals(Object)` | `Conditions\SwitchCondition.cs` | 67 | Api.0003 | Behavioral change in .NET 10.0 equality comparison |

**Details**:
- **File**: `Spooksoft.VisualStateManager\Conditions\SwitchCondition.cs`
- **Affected Code**:
  - Line 36: `var condition = new SimpleCondition(current.Equals(value));`
  - Line 67: `pair.Value.Value = pair.Key.Equals(value);`
- **Nature**: `System.ValueType.Equals` behavior may have changed in .NET 10.0
- **Recommendation**: Validate through testing that equality comparisons behave as expected

**No Binary or Source Breaking Changes**: Assessment confirms 0 binary incompatible and 0 source incompatible changes.

**5. Code Modifications**

**Required**:
- Remove `<PackageReference Include="System.Data.DataSetExtensions" Version="4.5.0" />` from project file

**Recommended**:
- Review equality comparisons in `SwitchCondition.cs` (lines 36, 67)
- Consider adding unit tests specifically for equality comparison scenarios if not already covered

**Configuration Changes**: None required

**6. Testing Strategy**

**Unit Tests**:
- Execute all tests in dependent test project (`Spooksoft.VisualStateManager.Test`)
- Focus on tests covering `SwitchCondition` class (due to behavioral changes)

**Behavioral Validation**:
- Verify equality comparison logic in `SwitchCondition.cs` behaves correctly
- Test edge cases for value type comparisons

**Multi-Target Validation**:
- Build against all target frameworks (netstandard2.0, net48, net472, net8.0, net10.0)
- Ensure no framework-specific issues

**7. Validation Checklist**

- [ ] Project builds without errors for net10.0 target
- [ ] Project builds without warnings for net10.0 target
- [ ] Project builds successfully for all other target frameworks
- [ ] `System.Data.DataSetExtensions` package removed
- [ ] No package dependency conflicts
- [ ] Dependent test project builds successfully
- [ ] All unit tests pass
- [ ] Behavioral changes validated (equality comparisons work correctly)
- [ ] No runtime exceptions in test scenarios

---

### Spooksoft.VisualStateManager.Test.csproj

#### Current State
- **Target Framework**: `net48`
- **Project Type**: Class Library / Test Project (SDK-style)
- **Dependencies**: Spooksoft.VisualStateManager
- **Dependants**: None (top-level test project)
- **Packages**: 4 explicit packages (Microsoft.NET.Test.Sdk, MSTest.TestAdapter, MSTest.TestFramework, Microsoft.DotNet.UpgradeAssistant.Extensions.Default.Analyzers)
- **Lines of Code**: 1,321
- **Files**: 15 total (1 with incidents)
- **Risk Level**: 🟢 Low

#### Target State
- **Target Framework**: `net10.0`

#### Migration Steps

**1. Prerequisites**
- ✅ .NET 10.0 SDK already installed
- ✅ Depends on `Spooksoft.VisualStateManager` which already includes net10.0

**2. Project File Updates**

Update `Spooksoft.VisualStateManager.Test.csproj`:
- Change `<TargetFramework>net48</TargetFramework>` to `<TargetFramework>net10.0</TargetFramework>`

**3. Package Updates**

| Package | Current Version | Target Version | Action | Reason |
|---------|----------------|----------------|--------|--------|
| Microsoft.NET.Test.Sdk | 16.* | 16.* | **Keep** | ✅ Compatible with .NET 10.0 |
| MSTest.TestAdapter | 2.2.7 | 2.2.7 | **Keep** | ✅ Compatible with .NET 10.0 |
| MSTest.TestFramework | 2.2.7 | 2.2.7 | **Keep** | ✅ Compatible with .NET 10.0 |
| Microsoft.DotNet.UpgradeAssistant.Extensions.Default.Analyzers | 0.3.261602 | 0.3.261602 | **Keep** | ✅ Compatible (dev-time analyzer) |

**All packages compatible** - No package updates required.

**4. Expected Breaking Changes**

**No Breaking Changes**: Assessment shows 0 API issues for this project.

**Dependency Impact**: 
- Behavioral changes in dependent library (`Spooksoft.VisualStateManager`) may affect test outcomes
- Focus testing on `SwitchCondition` class tests (if they exist)

**5. Code Modifications**

**Required**: None (only framework property change)

**Recommended**:
- Review tests covering `SwitchCondition` class equality scenarios
- Add tests if coverage gaps exist for behavioral changes

**6. Testing Strategy**

**Unit Tests Execution**:
- Execute entire test suite against .NET 10.0 runtime
- Focus on tests covering:
  - `SwitchCondition` class (behavioral changes in main library)
  - Value type equality comparisons

**Integration Tests**:
- Validate test project correctly references and uses upgraded main library

**Test Framework Compatibility**:
- Verify MSTest framework works correctly on .NET 10.0
- Validate test discovery and execution

**7. Validation Checklist**

- [ ] Project builds without errors for net10.0
- [ ] Project builds without warnings for net10.0
- [ ] All MSTest packages load correctly
- [ ] Test discovery succeeds (all tests found)
- [ ] All tests execute successfully
- [ ] All tests pass
- [ ] No test framework compatibility issues
- [ ] Tests covering `SwitchCondition` behavioral changes pass

---

## Risk Management

### High-Level Risk Assessment

| Risk Category | Level | Description | Mitigation |
|---------------|-------|-------------|------------|
| **Package Compatibility** | 🟢 Low | All 7 packages compatible with .NET 10.0 | No action required |
| **API Breaking Changes** | 🟢 Low | 0 binary incompatible, 0 source incompatible | Monitor behavioral changes only |
| **Behavioral Changes** | 🟡 Medium | 2 instances of `System.ValueType.Equals` behavior changes | Runtime testing required |
| **Build Complexity** | 🟢 Low | Simple 2-project solution | Standard build process |
| **Test Coverage** | 🟡 Medium | Test project exists (1321 LOC) | Execute all tests post-upgrade |
| **Multi-Targeting** | 🟢 Low | Main project already multi-targets | Preserve existing targets |

### Project-Specific Risk Assessment

#### Spooksoft.VisualStateManager.csproj
- **Risk Level**: 🟢 Low
- **Factors**:
  - Already includes net10.0 in multi-target list
  - 3 issues (1 package removal, 2 behavioral changes)
  - All packages compatible
  - 1,575 LOC
- **Key Risks**:
  - Behavioral changes in `System.ValueType.Equals` (2 occurrences)
  - Removing `System.Data.DataSetExtensions` package
- **Mitigation**: Comprehensive testing to validate behavior

#### Spooksoft.VisualStateManager.Test.csproj
- **Risk Level**: 🟢 Low
- **Factors**:
  - Single framework upgrade (net48 → net10.0)
  - 1 issue (target framework change)
  - All test packages compatible
  - 1,321 LOC of test code
- **Key Risks**: Test framework compatibility
- **Mitigation**: Execute tests to validate functionality

### Security Vulnerabilities

**Status**: ✅ No security vulnerabilities detected

All packages are up-to-date and compatible with .NET 10.0.

### Contingency Plans

#### If Package Compatibility Issues Arise
- **Scenario**: Package fails to restore or has hidden incompatibility
- **Response**: Check for preview/beta versions; contact package maintainer; find alternative package
- **Rollback**: Revert to previous branch

#### If Behavioral Changes Break Functionality
- **Scenario**: `System.ValueType.Equals` behavior changes cause test failures
- **Response**: Analyze affected code paths; update comparison logic if needed; consult .NET 10.0 breaking changes documentation
- **Rollback**: Fix code to handle both behaviors; add conditional compilation if multi-targeting

#### If Build Fails After Updates
- **Scenario**: Unexpected compilation errors
- **Response**: Review error messages; check .NET 10.0 migration guide; verify SDK installation
- **Rollback**: Revert project file changes via git

### Overall Risk Summary

**Overall Solution Risk**: 🟢 **Low**

This upgrade presents minimal risk due to:
- Small codebase (2,896 LOC)
- Full package compatibility (100%)
- No binary/source breaking changes
- Existing test coverage
- Main library already partially upgraded
- Simple dependency structure

---

## Testing & Validation Strategy

### Multi-Level Testing Approach

Given the All-At-Once strategy, testing occurs in a single comprehensive phase after all projects are upgraded.

---

### Phase Testing: Atomic Upgrade Validation

**Objective**: Validate that both projects upgraded successfully and work together correctly.

#### Build Validation
1. **Clean Build**
   - Execute `dotnet clean` on solution
   - Execute `dotnet restore` on solution
   - Execute `dotnet build` on solution
   - **Success Criteria**: 0 build errors, 0 warnings

2. **Multi-Target Build** (Main Library)
   - Build `Spooksoft.VisualStateManager.csproj` for all target frameworks:
     - netstandard2.0
     - net48
     - net472
     - net8.0
     - net10.0
   - **Success Criteria**: All target frameworks compile successfully

3. **Dependency Resolution**
   - Verify test project correctly references main library
   - Verify no package version conflicts
   - **Success Criteria**: No dependency warnings or errors

#### Unit Test Execution
1. **Test Discovery**
   - Run test discovery on `Spooksoft.VisualStateManager.Test.csproj`
   - **Success Criteria**: All tests discovered (no discovery errors)

2. **Full Test Suite**
   - Execute all tests in test project
   - **Success Criteria**: All tests pass (0 failures, 0 skipped)

3. **Behavioral Change Focus**
   - Identify and execute tests covering:
     - `SwitchCondition` class
     - Equality comparison scenarios
     - Value type comparisons
   - **Success Criteria**: Tests validate expected equality behavior

#### Runtime Validation
1. **Package Loading**
   - Verify all packages load correctly at runtime
   - Verify no missing assembly errors
   - **Success Criteria**: No runtime package load failures

2. **Behavioral Change Validation**
   - Review test results for `SwitchCondition` class
   - If no existing tests, create ad-hoc validation:
     - Test equality comparisons (line 36, 67 in `SwitchCondition.cs`)
     - Verify expected behavior matches .NET 10.0 semantics
   - **Success Criteria**: Equality comparisons behave as expected

---

### Testing Checklist

#### Spooksoft.VisualStateManager.csproj
- [ ] Builds without errors for all target frameworks
- [ ] Builds without warnings for all target frameworks
- [ ] `System.Data.DataSetExtensions` package successfully removed
- [ ] No package dependency conflicts
- [ ] Multi-target build succeeds for netstandard2.0, net48, net472, net8.0, net10.0

#### Spooksoft.VisualStateManager.Test.csproj
- [ ] Builds without errors for net10.0
- [ ] Builds without warnings for net10.0
- [ ] Test discovery succeeds
- [ ] All tests execute
- [ ] All tests pass (0 failures)
- [ ] MSTest framework compatible with .NET 10.0

#### Integration & Behavioral
- [ ] Test project correctly references upgraded main library
- [ ] Tests covering `SwitchCondition` equality logic pass
- [ ] No runtime exceptions during test execution
- [ ] Behavioral changes validated (System.ValueType.Equals works as expected)

---

### Regression Testing

**Scope**: Validate no functionality was broken during upgrade

**Approach**:
1. Execute full test suite (1,321 LOC of test code)
2. Compare test results to baseline (pre-upgrade)
3. Investigate any new failures

**Success Criteria**:
- Test pass rate unchanged from baseline
- No new test failures introduced
- No performance degradation

---

### Manual Validation (Optional)

If this library has consumers or manual test scenarios:
1. Build consumer applications against upgraded library
2. Execute consumer smoke tests
3. Verify no breaking changes for consumers

**Note**: This plan focuses on automated validation via unit tests. Manual validation depends on library usage context.

---

## Complexity & Effort Assessment

### Per-Project Complexity

| Project | Complexity | Dependencies | Packages | LOC | Risk | Key Factors |
|---------|------------|--------------|----------|-----|------|-------------|
| **Spooksoft.VisualStateManager** | 🟢 Low | 0 | 3 | 1,575 | 🟢 Low | Already has net10.0; 1 package removal; 2 behavioral changes |
| **Spooksoft.VisualStateManager.Test** | 🟢 Low | 1 | 4 | 1,321 | 🟢 Low | Simple framework upgrade; all test packages compatible |

### Phase Complexity Assessment

**Phase 1: Atomic Upgrade**

All projects upgraded simultaneously in single operation.

| Aspect | Complexity | Notes |
|--------|------------|-------|
| **Project File Updates** | 🟢 Low | Main: Remove duplicate target; Test: Change single framework property |
| **Package Updates** | 🟢 Low | Only removal of `System.Data.DataSetExtensions` |
| **Dependency Resolution** | 🟢 Low | All packages compatible; no version conflicts |
| **Build Process** | 🟢 Low | Standard MSBuild; 2 projects |
| **Code Changes** | 🟢 Low | Estimated 2+ LOC (0.1% of codebase) |
| **Testing** | 🟡 Medium | Validate behavioral changes; execute test suite |

**Overall Phase Complexity**: 🟢 **Low**

### Resource Requirements

#### Skills Required
- **Framework Knowledge**: Understanding of .NET 10.0 features and behavioral changes
- **Multi-Targeting**: Awareness of multi-target framework management (main project)
- **Testing**: Ability to execute and interpret MSTest results
- **Source Control**: Git branching and merging

**Skill Level**: Intermediate .NET developer

#### Parallel Execution Capacity
- **Not Applicable**: All-At-Once strategy processes both projects in single atomic operation
- **Build Order**: Sequential (main library → test project) enforced by MSBuild automatically

### Relative Effort Distribution

```
Project File Updates:     ████░░░░░░ 20%
Package Updates:          ██░░░░░░░░ 10%
Dependency Restoration:   ██░░░░░░░░ 10%
Build & Fix Errors:       ████░░░░░░ 20%
Testing & Validation:     ████████░░ 40%
```

**Total Effort**: Low complexity upgrade

**Primary Effort Driver**: Testing and validation of behavioral changes (40% of effort)

### Complexity Rating Summary

- ✅ **Simple solution structure**: 2 projects, 1-level dependency
- ✅ **Low code volume**: <3K LOC total
- ✅ **High compatibility**: 100% package compatibility
- ✅ **No breaking changes**: Zero binary/source incompatibilities
- ⚠️ **Behavioral changes**: Require runtime validation (2 instances)
- ✅ **Existing tests**: Test project available for validation

**Overall Complexity**: 🟢 **Low** - Ideal candidate for All-At-Once upgrade strategy

---

## Source Control Strategy

### Branching Strategy

**Branch Structure**:
- **Source Branch**: `master` (starting point)
- **Upgrade Branch**: `upgrade-to-NET10` (all upgrade work)
- **Target Branch**: `master` (merge destination after validation)

**Branch Workflow**:
1. ✅ Create `upgrade-to-NET10` branch from `master` (already created during initialization)
2. Perform all upgrade work on `upgrade-to-NET10`
3. Validate and test on `upgrade-to-NET10`
4. Create pull request from `upgrade-to-NET10` → `master`
5. Review, approve, and merge

**Rationale**: Isolates upgrade work from stable `master` branch, enabling safe experimentation and easy rollback if needed.

---

### Commit Strategy

**Approach**: Single atomic commit (recommended for All-At-Once strategy)

**Rationale**:
- All-At-Once strategy updates both projects simultaneously
- Single commit ensures atomic upgrade (no intermediate states)
- Easier to revert if issues discovered
- Clear upgrade boundary in git history

**Commit Structure**:

```
Upgrade solution to .NET 10.0

Changes:
- Update Spooksoft.VisualStateManager.Test from net48 to net10.0
- Remove System.Data.DataSetExtensions package (functionality in framework)
- Validate and test all projects for .NET 10.0 compatibility

Projects affected:
- Spooksoft.VisualStateManager.csproj (verify net10.0 in multi-target)
- Spooksoft.VisualStateManager.Test.csproj (net48 → net10.0)

Validation:
- All projects build without errors/warnings
- All tests pass (0 failures)
- Behavioral changes validated (System.ValueType.Equals)

Related issues:
- Api.0003: Behavioral changes in System.ValueType.Equals (lines 36, 67 in SwitchCondition.cs)
- NuGet.0003: Removed System.Data.DataSetExtensions
- Project.0002: Framework updates completed
```

**Alternative Approach**: Multiple commits (if preferred)

If separate commits are desired:
1. **Commit 1**: Update test project framework (`net48` → `net10.0`)
2. **Commit 2**: Remove `System.Data.DataSetExtensions` package from main library
3. **Commit 3**: Build validation and test results

**Recommendation**: Use single atomic commit for simplicity and clarity.

---

### Review and Merge Process

#### Pull Request Requirements

**Title**: `Upgrade to .NET 10.0 (LTS)`

**Description Template**:
```markdown
## Overview
Upgrades Spooksoft.VisualStateManager solution to .NET 10.0 (Long Term Support).

## Changes
- **Main Library**: Verify net10.0 in multi-target configuration
- **Test Project**: Upgrade from net48 to net10.0
- **Package Removal**: System.Data.DataSetExtensions (functionality in framework)

## Testing
- ✅ All projects build without errors/warnings
- ✅ All tests pass (X tests, 0 failures)
- ✅ Behavioral changes validated (System.ValueType.Equals)

## Risk Assessment
- **Risk Level**: 🟢 Low
- **Package Compatibility**: 100% compatible (7/7 packages)
- **Breaking Changes**: 0 binary, 0 source
- **Behavioral Changes**: 2 instances (validated via tests)

## Validation Checklist
- [ ] Builds succeed for all target frameworks
- [ ] All tests pass
- [ ] No dependency conflicts
- [ ] Behavioral changes reviewed
- [ ] No warnings introduced

## Rollback Plan
Revert this PR to return to previous state.
```

#### Review Checklist

**Code Review**:
- [ ] Project file changes correct (TargetFramework properties)
- [ ] Package removal justified (System.Data.DataSetExtensions)
- [ ] No unintended changes

**Build Review**:
- [ ] CI/CD pipeline passes (if configured)
- [ ] All target frameworks build successfully
- [ ] No new warnings introduced

**Test Review**:
- [ ] All tests pass
- [ ] Test coverage maintained
- [ ] Behavioral change tests validated

#### Merge Criteria

✅ **Ready to Merge** when:
1. All validation checklist items complete
2. All tests passing
3. No build errors or warnings
4. Code review approved
5. Behavioral changes validated

**Merge Method**: Standard merge (preserves commit history)

---

### Rollback Strategy

**If Issues Discovered**:

1. **Before Merge**:
   - Revert changes on `upgrade-to-NET10` branch
   - Delete branch and start over if needed
   - Master branch remains unaffected

2. **After Merge**:
   - Revert merge commit on `master`
   - Or create new branch from pre-upgrade commit
   - Investigate issues on separate branch

**Rollback Steps**:
```bash
# Before merge (on upgrade branch)
git reset --hard origin/master

# After merge (revert merge commit)
git revert -m 1 <merge-commit-sha>
```

**Recovery**: All changes isolated to upgrade branch; master branch protected until validation complete.

---

## Success Criteria

### Technical Criteria

The upgrade is considered technically successful when all of the following are met:

#### Framework Migration
- [x] ✅ Main library (`Spooksoft.VisualStateManager`) includes `net10.0` in multi-target configuration
- [ ] Test project (`Spooksoft.VisualStateManager.Test`) upgraded from `net48` to `net10.0`

#### Package Management
- [ ] `System.Data.DataSetExtensions` package removed from main library
- [ ] All remaining packages compatible with .NET 10.0 (7 packages)
- [ ] No package version conflicts
- [ ] No security vulnerabilities present

#### Build Success
- [ ] Main library builds without errors for all target frameworks:
  - [ ] netstandard2.0
  - [ ] net48
  - [ ] net472
  - [ ] net8.0
  - [ ] net10.0
- [ ] Test project builds without errors for net10.0
- [ ] **Zero build warnings** across both projects
- [ ] No dependency resolution failures

#### Testing
- [ ] All tests discovered successfully (test discovery works)
- [ ] All tests execute successfully (no test infrastructure failures)
- [ ] **All tests pass** (0 failures, 0 skipped)
- [ ] Behavioral changes validated:
  - [ ] `System.ValueType.Equals` usage in `SwitchCondition.cs` line 36 tested
  - [ ] `System.ValueType.Equals` usage in `SwitchCondition.cs` line 67 tested

#### API Compatibility
- [x] ✅ 0 binary incompatible APIs (verified by assessment)
- [x] ✅ 0 source incompatible APIs (verified by assessment)
- [ ] 2 behavioral changes validated through testing

---

### Quality Criteria

#### Code Quality
- [ ] No new code quality issues introduced
- [ ] No new compiler warnings
- [ ] Code follows existing patterns (traditional namespaces, traditional constructors)
- [ ] No technical debt added

#### Test Coverage
- [ ] Test coverage maintained at current level
- [ ] Existing tests updated if needed for .NET 10.0
- [ ] No tests disabled or skipped to "pass"

#### Documentation
- [ ] Upgrade documented in commit message
- [ ] Breaking changes (if any) documented
- [ ] Pull request includes comprehensive description

---

### Process Criteria

#### All-At-Once Strategy Compliance
- [ ] Both projects updated in single coordinated operation
- [ ] No intermediate partially-upgraded states
- [ ] Single atomic commit (or logical commit group)
- [ ] Clear upgrade boundary in source control

#### Source Control
- [ ] All work performed on `upgrade-to-NET10` branch
- [ ] Single atomic commit with comprehensive message
- [ ] Pull request created with complete description
- [ ] Code review completed and approved

#### Validation
- [ ] All validation checklist items completed
- [ ] Manual validation performed (if applicable)
- [ ] No rollback required

---

### All-At-Once Strategy-Specific Criteria

✅ **Strategy Adherence**:
- [ ] Both projects upgraded simultaneously (not incrementally)
- [ ] Single build/test/validation cycle
- [ ] No multi-targeting expansion (main library already multi-targets)
- [ ] Clean transition from current state to fully upgraded state

---

### Definition of Done

**The .NET 10.0 upgrade is complete when**:

1. ✅ **All Technical Criteria Met**
   - Both projects target .NET 10.0
   - All packages updated or removed as specified
   - Solution builds with 0 errors, 0 warnings
   - All tests pass

2. ✅ **All Quality Criteria Met**
   - Code quality maintained
   - Test coverage maintained
   - Documentation complete

3. ✅ **All Process Criteria Met**
   - All-At-Once strategy followed
   - Source control workflow completed
   - Pull request approved and merged

4. ✅ **All Validations Pass**
   - Build validation complete
   - Test validation complete
   - Behavioral changes validated
   - No regressions detected

5. ✅ **Stakeholder Acceptance**
   - Code review approved
   - Tests reviewed and validated
   - Upgrade confirmed working in target environment

---

### Acceptance Statement

**Formal Acceptance**: The upgrade is formally accepted when the pull request is merged to `master` branch after all criteria above are satisfied and all validation checks pass.

**Post-Merge Monitoring**: Monitor for any issues in the first 24-48 hours after merge. Be prepared to quickly rollback if critical issues discovered.
