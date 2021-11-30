Given `Some\SearchPage.po.yaml` file

```yaml
ya:
  SearchInput:
    by: ./input
```

It should generate `SearchPage.cs`

```csharp
namespace RootNamespace.Some.Path
{
    class SearchPage
    {
        public SearchInput SearchInput { get; }

        class SearchInput
        {
            // implements/wraps? IWebElement from Selenium

            // yet another elements
        }
    }
}
```

And `SomePlace.cs`
```csharp
namespace RootNamespace
{
    class SomePlace
    {
        public SearchPage SearchPage { get; }
    }
}
```

And single `RootPlace.cs`
```csharp
namespace RootNamespace
{
    class RootPlace
    {
        public SomePlace Some { get; }
    }
}
```

And entry point for user `Ya.cs`
```csharp
namespace RootNamespace
{
    static class YaExtensions
    {
        static RootPlace Ya(this IWebDriver driver)
        {
            return new RootPlace(driver);
        }
    }
}
```

And then user will use it
```csharp
_driver.Ya().Some.SearchPage.SearchInput.SendKeys("ya");
```