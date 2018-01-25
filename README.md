# DewXamarinLocalization
A .NET standard helper for xamarin forms localization in shared projects







# How to use
My first though was about that this localization system must be easy, and (maybe it can be still better, who knows?) I think I've done it!

## Install package
Your first step is to install the DewXamarinLocalization in your projects (all, you can use solution nuget package manager)

## Files
The files are simple json dictionary, like this:
```json
{
  "AppTitle": "DewLocalization!",
  "AboutTitle": "Dew is great!"
}
```
# IMPORTANT! --------------------------
These steps are important!
#### Naming
This is really important, you must be carefull about the file names.
Every filename __MUST__ correspond to the __CultureInfo__ Name property in lower case (en-us,it-it,it-ch,en-gb,fr-fr,etc.) and must be __json__. Obviously if your app support 3 languages, you should have 3 different culture and you must support all of them.
#### Path
The file __MUST__ be placed in a folder called __Localized__ in the shared project. 
#### Embedded
When you create the file, you __MUST__ set in the properties _"Embedded resource"_ like _Build Action_
## NOTE
If a file for a culture isn't present you'll get an exception when the app is opening __only__ with that culture.
# IMPORTANT! --------------------------
## XAML Code

This page is from the xamarin.forms base template for a new app.

```xaml
<?xml version="1.0" encoding="utf-8" ?>
<TabbedPage xmlns="http://xamarin.com/schemas/2014/forms"
            xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
            xmlns:views="clr-namespace:ExampleApp.Views"
            xmlns:loc="clr-namespace:DewCore.Xamarin.Localization;assembly=DewXamarinLocalization"
            x:Class="ExampleApp.Views.MainPage">
    <TabbedPage.Children>
        <NavigationPage Title="{loc:_ S=AppTitle}">
            
            <NavigationPage.Icon>
                <OnPlatform x:TypeArguments="FileImageSource">
                    <On Platform="iOS" Value="tab_feed.png"/>
                </OnPlatform>
            </NavigationPage.Icon>
            <x:Arguments>
                <views:ItemsPage />
            </x:Arguments>
        </NavigationPage>
        
        <NavigationPage Title="{loc:_ S=AboutTitle}">
            <NavigationPage.Icon>
                <OnPlatform x:TypeArguments="FileImageSource">
                    <On Platform="iOS" Value="tab_about.png"/>
                </OnPlatform>
            </NavigationPage.Icon>
            <x:Arguments>
                <views:AboutPage />
            </x:Arguments>
        </NavigationPage>
    </TabbedPage.Children>
</TabbedPage>
```

How you can see, your first step is to import the namespace:
```text
xmlns:loc="clr-namespace:DewCore.Xamarin.Localization;assembly=DewXamarinLocalization"
```
After this you can use it thanks the Markup extension

```text
<NavigationPage Title="{loc:_ S=AppTitle}">
```

#### Another approach
You can also set the strings in this way
```xaml
<NavigationPage >
    <NavigationPage.Title>
        <loc:_ S="AppTitle"/>
    </NavigationPage.Title>
    <NavigationPage.Icon>
        <OnPlatform x:TypeArguments="FileImageSource">
            <On Platform="iOS" Value="tab_feed.png"/>
        </OnPlatform>
    </NavigationPage.Icon>
    <x:Arguments>
        <views:ItemsPage />
    </x:Arguments>
</NavigationPage>
```
#### In code

If you need to acces to the dictionary in code behind you can this way:
```csharp
public void Test()
{
    System.Diagnostics.Debug.Write(_.GetString("AppTitle"))
}
```

## If string doesn't exists?

The localization system is based on aspnet core middleware [DewLocalizationMiddleaware](https://github.com/andreabbondanza/DewLocalizationMiddleware) and works in the same way.

Files must be present otherwise you'll get an exception, but strings doesn't need this.

If a string doesn't exists, the Localization class just return the key value.

This way if you want you can use the default word like key, so if in the translation it doesn't exists it will return the key self.

An example

```xaml
<NavigationPage Title="{loc:_ S='My App\'s name'}">
```
or
```xaml
<NavigationPage.Title>
    <loc:_ S="My App's name"/>
</NavigationPage.Title>
```

If the it-it.json file contains a voice for "My App's name", it will be translated, otherwise the app will print "My App's name" (that is the key).

## Note 
## NuGet
You can find it on nuget with the name [DewXamarinLocalization](https://www.nuget.org/packages/DewXamarinLocalization/)

## About
[Andrea Vincenzo Abbondanza](http://www.andrewdev.eu)

## Donate
[Help me to grow up, if you want](https://payPal.me/andreabbondanza)
