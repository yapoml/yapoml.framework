# Motivation
To address different UI representations of the same page (e.g. mobile view), we want to introduce some kind of inheritance. User will be able to define different `po.yaml` files, which extends base file, and then override base if needed.

# Example
Given that user has `HomePage.po.yaml`
```yaml
Navigation:
  by: any_locator

  Bar:
    by: any_locator

    MenuItes:
      by: any_locator
```

User declares new method in this `HomePage.cs` class
```csharp
public virtual void Navigate(string menuItemName)
{
    this.Navigation.Bar.MenuItes.First(m => m.Text == menuItemName).Click();
}
```

All users` tests use this method to navigate to some specific page via navigation bar.

Now, to make it possible execute tests on mobile, test needs to click on Hamburger button before actual navigation.

So, user defines `HomeMobilePage.po.yaml`
```yaml
extends: HomePage

HamburgerButton:
  by: .h_button

Bar:
  by: any_mobile_specific_locator

  MenuItems:
    by: any_mobile_specific_locator
```

Then in `HomeMobilePage.cs` user is able to override logic for navigation
```csharp
public override void Navigate(string menuItemName)
{
    HamburgerButton.Click();

    this.Navigation.Bar.MenuItes.First(m => m.Text == menuItemName).Click();
}
```

> Note: generator should also care about all properties (virtual/override).

## Page Object Factory
Now it's user's responsibility to create proper instance of Page Object (mobile or base) depending on his environment.