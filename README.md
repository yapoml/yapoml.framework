# Yapoml.Framework

Core framework for Yapoml — a page object generation tool that parses YAML-based workspace definitions into a structured model of pages, components, and spaces.

## Workspace Parser

The workspace parser scans `.page.yaml` and `.component.yaml` files, organizing them by directory structure into a hierarchy of spaces (namespaces), pages, and components.

### Usage

```csharp
var parser = new WorkspaceParser();
var builder = new WorkspaceContextBuilder("/path/to/workspace", "MyProject", parser);

builder.AddFile("login.page.yaml", File.ReadAllText("login.page.yaml"));
builder.AddFile("shared/header.component.yaml", File.ReadAllText("shared/header.component.yaml"));

WorkspaceContext workspace = builder.Build();
```

### YAML Format

**Page** (`*.page.yaml`):
```yaml
url: /login
base: BasePage

username: css input#user
password: css input#pass
submit_button:
  by:
    xpath: //button[@type='submit']
```

**Component** (`*.component.yaml`):
```yaml
logo: css .logo
nav_items: css .nav > li
```
