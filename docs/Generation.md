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
    public SearchPageContext Ya() { return new ...}
}
```

SearchPageContext.cs
```csharp
class SearchPageContext
{
    public SearchInput SearchInput {get;}
}
```

SearchInput.cs
```csharp
class SearchInput
{
    // implements/wraps? IWebElement from Selenium

    public SearchInputContext Ya() {return new ...}
}
```

SearchInputContext.cs
```csharp
class SearchInputContext
{
    // yet another elements
}
```