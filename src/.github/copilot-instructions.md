# Copilot Instructions

## Project Guidelines
- In UniversalSend, RootPage15063 is used on phone/Windows 10 Mobile and RootPage is used on newer Windows, so NavigationView contract warnings in RootPage can be ignored for the phone-targeted path.
- Preserve the QuickSave navigation branch in RootPage.xaml.cs for UniversalSend; do not simplify it away even if it currently looks unreachable, because QuickSave is planned to be used later. Continue implementing the planned work without waiting for approval between steps.

## Project Summaries
- Capture project summaries in a markdown file and reference them from this document to maintain clarity and context for Copilot usage.

## Notes for Copilot

- Treat this as a mature codebase with established layering.
- Prefer small, compatible changes that fit the current architecture over broad modernization.
- For the current LocalSend compatibility status and remaining differences versus the Rust implementation, see `docs/localsend-interop-summary.md`.