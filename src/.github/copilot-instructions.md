# Copilot Instructions

## Project Guidelines
- In UniversalSend, RootPage15063 is used on phone/Windows 10 Mobile and RootPage is used on newer Windows, so NavigationView contract warnings in RootPage can be ignored for the phone-targeted path.
- Preserve the QuickSave navigation branch in RootPage.xaml.cs for UniversalSend; do not simplify it away even if it currently looks unreachable, because QuickSave is planned to be used later. Continue implementing the planned work without waiting for approval between steps.