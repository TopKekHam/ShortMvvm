# ShortMvvm
Weird mvvm lib for wpf/uwp

## Creating Locator/Container

Lets create simple Locator and register DI "IService" with "Service" implimintaion.

```c#
  class MyLocator : Locator
  {
      public MyLocator() 
      {
        Register<IService, Service>();
      }
  }
```

You can register the service as Singleton too.

```c#
  RegisterSingleton<ISerice,Service>();
```

Or if you dont have Interface you can register just the type.

```c#
  Register<Service>();
  //Or singleton
  RegisterSingleton<Service>();
```

## DI

If you want to inject service just do it in contructor like this.

```c#
  class Service : IService
  {
    public Service(IAnotherService service)
    {
    
    }
  }
```

## View Model

In this lib you got view model base with many convenient features.

### Notify property changed

Simple method.
Notify("nameofprop");

```c# 
int number;
public int Number {
get => number;
set {
  number = value;
  Notify(nameof(Number));
}
```

### Dynamic props

In any point in your ViewModel you can use dynamic props.
This props automaticly notifies on change when assignment happends.
Using this["propname"];

```c#

public MyViewModel()
{
  //Assigning props.
  this["StringProp"] = "this is string";
  this["IntProp"] = 100;
  
  //Retriving props.
  var str = (string) this["StringProp"];
  var num = (int) this["IntProp"];
}

```

Using prop in Xaml.

```xaml
  <Button Content="{binding [StringProp]}"/>
  <TextBlock Text="{binding [IntProp]}"/>
```

### Dynamic Commands

To avoid the hasel writing each time command you can declare methods add CanExecute attribute and use the methods like commands.
Notice that the method is void.

```c#
  //adding attribute to declare that this method is command
  [CommandExecute]
  void AddCount() 
  {
    ((int)this["counter"])++;
  }
```

In xaml we bind to the method name.

```xaml
  <TextBlock Text="{Binding [counter]}"/>
  <Button Content="+1" Command="{Binding [AddCount]}"/>
```

If you want to add can execute to command you just declare another method and add CanExecuteCommand attribute.
Notice that the method returns bool in this case and the name is like command method with "Can" in the begining.

```c#
  [CanExecuteCommand]
  bool CanAddCount()
  {
    return true;
  }
```
You can add parameter to the command.

```c# 
  [ExecuteCommand]
  void AddCount(object param)
  {
      ((int)this["counter"]) += int.Parse((string)param);
  }
```

```xaml
  <TextBlock Text="{Binding [counter]}"/>
  <Button Content="+1" Command="{Binding [AddCount]}" CommandParameter="3" />
```


