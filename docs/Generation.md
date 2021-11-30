Given `some/path/SearchPage.po.yaml` file

```yaml
ya:
  SearchInput:
    by: ./input
```

It should generate `SearchPage.cs`

```csharp
namespace some.path
{
    class SearchPage
    {
        public SearchInput SearchInput {get;}

        class SearchInput
        {
            // implements/wraps? IWebElement from Selenium

            // yet another elements
        }
    }
}
```

And then user will use it
```csharp
_driver.Ya().SearchPage.SearchInput.SendKeys("ya");
```