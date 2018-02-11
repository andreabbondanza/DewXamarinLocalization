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
#### Tree example
```text
-Shared project
 |
 |-Localized
     |- it-it.json
     |- en-us.json
 |- Views
 |- etc.
```
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
## Change dictionary in runtime

If you want change the dictionary in runtime, you shouuld just do:
```csharp
public async Task Test()
{
    var newCulture = new System.Globalization.CultureInfo("it-IT");
    await \_.ChangeCulture(newCulture);
    System.Diagnostics.Debug.Write(_.GetString("AppTitle"));
    newCulture = new System.Globalization.CultureInfo("en-US");
    await \_.ChangeCulture(newCulture);
    System.Diagnostics.Debug.Write(_.GetString("AppTitle"));
}
```

### Scenario where you want change your language at app's start

If you want to change the language in app start, your first approach is to place the _ChangeCulture_ call into __OnStart__ event but it won't work.
This because the _ChangeCulture_ method is __asyncronous__, and the App will initialize via Xaml and this will create a conflict (with crashes sometimes).

Another approach is to change the culture in the __OnAppaering__ event of the main page, and it works, but only for the next pages, because the mainpage will be loaded with the currentculture language.

If you try to call ChangeCulture method into App constructor you'll get a NullReferenceException because DewXamarinLocalization class depends from Application class.

A solution for this particular problem is done with the static property __CultureStringOverride__.

This property is mono-use (after set it, you should call _LoadDictionary_ method, that will read it and will delete it) didn't has dependencies, so you can call into __App__ constructor without problem.

Like I've said, you __SHOULD__ call _LoadDictionary_ after set this property, but, in this particular case, will be XAML the creator of the DewXamarinLocalization class. 

__NOTE:__ If you set property and after call LoadDictionary you'll get the same result of a call of _ChangeCulture_, so you should use this property only for this scenario.

#### Example:

```csharp
public App ()
{
    InitializeComponent();
    var cul = LoadMyCultureFromSettings(); // this must be the culture name, like "it-it","en-us", etc. in lower case, see Naming paragraph for more.
    DewCore.Xamarin.Localization._.CultureStringOverride = cul;
    MainPage = new MainPage();
}

```

## Note 
## NuGet
You can find it on nuget with the name [DewXamarinLocalization](https://www.nuget.org/packages/DewXamarinLocalization/)

## About
[Andrea Vincenzo Abbondanza](http://www.andrewdev.eu)

## Donate
[Help me to grow up, if you want](https://payPal.me/andreabbondanza)
