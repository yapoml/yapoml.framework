Given `SearchPage.po.yaml`

```yaml
ya:
  SearchInput:
    by: ./input
```

It should generate

SearchPage.cs
```csharp
class SearchPage
{
    public SearchInput SearchInput {get;}
}
```

SearchInput.cs
```csharp
class SearchInput
{
    // implements/wraps? IWebElement from Selenium

    // yet another elements
}
```


And then user will use it
```csharp
_driver.Ya().SearchPage.SearchInput.SendKeys("ya");
```