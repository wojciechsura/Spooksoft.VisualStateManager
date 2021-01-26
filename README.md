# VisualStateManager

VisualStateManager contains a set of classes - Commands and Conditions - which simplify managing commands and their dependencies in Windows Presentation Foundation application.

## The problem

The common issue is that commands, that you create have their dependencies. For instance, a Save command should not be available if document has already been saved or Delete command should not be available when no item is selected.

If you try to manage enable-state of your commands directly from the place where they might change, it may lead to a lot of messy and hard-to-maintain code, for instance:

```csharp
public void SaveDocument()
{
    // Perform action
 
    SaveDocumentAction.Enabled = false;
}

public void HandleDocumentChanged()
{
    SaveDocumentAction.Enabled = true;
}

public void SelectionChanged()
{
    CopyCommand.Enabled = selection != null;
    CutCommand.Enabled = selection != null;
    SearchInSelectionCommand.Enabled = selection != null && selection.IsRegular;
    // ...
}
```

There are numerous problems with this approach:

* If you create a new action, you have to manually find all places, when its enable-state might change and add appropriate check there;
* If it happens, that there is an additional condition for a command, which may influence its enable-state, then you have to re-visit all places, where this state is evaluated and modify them;
* Methods, which are supposed to perform actions are aware of the whole window's or control's architecture (because they know, which actions may they influence)
* It is very easy to miss setting the enable-state, which leads to hard-to-find errors.

## The solution

Spooksoft.VisualStateManager solves the problem by doing two things:

* Encapsulating a condition under which command may run into a class, and
* Reversing the logic: instead of methods setting availability of actions, actions now listen to changes of application state and change their availability automatically.

# Usage

Spooksoft.VisualStateManager provides an `AppCommand` class, which implements `ICommand` interface and a set of classes, which allows you to handle condition changes with ease.

## Simple case

This is the simplest way you can use Commands and Conditions:

```csharp

private readonly SimpleCondition documentSavedCondition;

public ICommand SaveDocumentCommand { get; }

public MyWindow()
{
    documentSavedCondition = new SimpleCondition(false);
    SaveDocumentCommand = new AppCommand(obj => DoSaveDocument, !documentSavedCondition); // Note the !
}

public void SaveDocument()
{
    // Perform save
    
    documentSavedCondition.Value = true;
}

public void HandleDocumentChanged()
{
    documentSavedCondition.Value = false;
}
```

Note, that `SaveDocument` and `HandleDocumentChanged` methods no longer worry about specific actions, which should be updated. Instead, they modify value of the condition and all actions, which depends on it will adjust accordingly.

## AppCommand

The `AppCommand` class provides a simple implementation of `ICommand` interface and provides infrastructure required for listening to `SimpleCondition` value changes. You need to provide an `Action<object>`, which will be called when the command executes and - optionally - a `SimpleCondition`, which will control its enable-state.

## SimpleCondition

The simplest condition wraps a `bool` into a class and notifies about changes. The usage is very simple.

```csharp
myCondition = new SimpleCondition(false); // Initial value
myCommand = new AppCommand(obj => SomeMethod(), myCondition);

// ...

myCondition.Value = true; // Will propagate to all commands
```

## CompositeCondition

The `CompositeCondition` allows you to combine different conditions into one with "or" and "and" boolean operators.

You may define it directly, like:

```csharp
myCondition = new CompositeCondition(CompositionKind.And, myOtherCondition1, myOtherCondition2);
myCommand = new AppCommand(obj => SomeMethod(), myCondition);
```

But since BaseCondition overloads | and & operators, it is far easier to do it like following:

```csharp
myCommand = new AppCommand(obj => SomeMethod(), myOtherCondition1 & myOtherCondition2);
```

The latter is also a far easier to read.

## NegateCondition

You can negate value of some condition. Similarly to `CompositeCondition` you may do it in two ways:

```csharp
myCondition = new NegateCondition(myOtherCondition);
myCommand = new AppCommand(obj => SomeMethod(), myCondition);
```

Or simply:

```csharp
myCommand = new AppCommand(obj => SomeMethod(), !myOtherCondition);
```

## PropertyWatchCondition

This condition allows you to automatically track a property of some object, provided that object in question implements `INotifyPropertyChanged` interface and properly informs about property value change.

Usage:

```csharp
myCondition = new PropertyWatchCondition<WatchedObject>(watchedObjectInstance, ob => ob.SomeProperty, false);
```

The last parameter defines default value in case watched object instance was null.

## SwitchCondition

`SwitchCondition` behaves somewhat as a switch statement. It exposes a series of internal conditions, which are set basing on current value. Example should explain it better:

```csharp
myCondition = new SwitchCondition<int>(1, 2, 3, 4);

myCommand1 = new AppCommand(() => SomeMethod(), myCondition.Conditions[1]);
myCommand2 = new AppCommand(() => SomeMethod(), myCondition.Conditions[2]);

myCondition.Current = 2; // myCommand1 will be disabled and myCommand2 will be enabled
```

## LambdaCondition

The most powerful condition, which allows to specify a series of expressions, which can traverse a couple of classes and define a boolean expression at the end. For example:

```csharp
myCondition = new LambdaCondition<MainViewModel, DocumentsManager, Document>(this, 
    mvm => mvm.DocumentsManager, 
    dm => dm.CurrentDocument, 
    cd => cd.Highlighting == Highlightings.Xml,
    false);
```

`LambdaExpression` has the following requirements/restrictions:

* Only single-member accesses are allowed. So instead `x => x.A.B` write `x => x.A, a => a.B`.
* Currently it supports only three-levels of nesting. If you need more, contact me, I'll add more (or look into sources how to do it yourself)
* Classes on every level must implement `INotifyPropertyChanged` interface, so that `LambdaExpression` can listen to property value changes.

The upsides of `LambdaExpression` are:

* It automatically tracks all members on its way, including multiple member accesses (as long as they are single-level): `x => x.A + x.B > 5`
* It allows you to clearly express logic behind the condition, what simplifies reading the source code a log.
* It behaves properly if value on any level is null - in such case the default value is used.