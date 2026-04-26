# UniversalSend Copilot Instructions

## Project overview

- UniversalSend is a Windows UWP application for discovering nearby devices and sending content between them.
  The application is based on LocalSend, which is written in Rust and maintained at `https://github.com/localsend/localsend`.
  This archive is cloned locally at `d:\dev\Private\localsend`.
  The protocol is described at `https://github.com/localsend/protocol` and cloned locally at `d:\dev\Private\protocol\`.

  UniversalSend should implement the latest version of the LocalSend protocol fully. Aim is to make it 100% compatible.

  I have based the application on the GitHub repository at `https://github.com/Pigeon-Ming/UniversalSend`, which is cloned at
  `d:\dev\Private\UniversalSend.org\`, but I have made a significant amount of changes.

  The main goal of UniversalSend is to have it work on old Windows 10 Mobile phones like the Lumia 950XL.
  To do this, I have to target Windows 10 Creators Update (10.0; Build 15063). This constraint is very important.
  I can't upgrade to a newer version of Universal Windows.

  A limitation of this constraint is that encryption most likely won't work.

- The current solution is split into three main projects:
  - `UniversalSend`: UI, pages, controls, app startup, navigation, and view models.
  - `UniversalSend.Models`: app models, managers, helpers, settings, and shared abstractions.
  - `UniversalSend.Services`: HTTP, REST, UDP discovery, and related infrastructure.

## Architecture guidance

- Prefer keeping UI concerns in `UniversalSend`.
- Keep business logic, state management, and data managers in `UniversalSend.Models`.
- Keep protocol, transport, discovery, and server logic in `UniversalSend.Services`.
- Use the existing dependency injection setup in `App.xaml.cs`, `UniversalSend.Models/RegisterServices.cs`, and `UniversalSend.Services/RegisterServices.cs`.
- Prefer existing interfaces and managers over introducing new cross-project dependencies.

## Coding conventions

- Follow the existing C# style in the repository.
- Keep changes minimal and scoped to the requested task.
- Preserve existing naming patterns such as `IThing` interfaces, `ThingManager`, `ThingHelper`, and `ThingService` classes where applicable.
- Do not add new packages unless absolutely necessary.
- Avoid changing generated files under `obj/`.

## Implementation preferences

- Reuse existing services and managers before creating new abstractions.
- Register new services through the existing DI registration methods.
- For network and discovery behavior, prefer extending current HTTP/UDP flows instead of adding parallel mechanisms.
- For settings and persisted values, reuse the existing settings abstractions and constants.

## Testing and validation

- Validate only the files and behavior affected by the requested change.
- Do not refactor unrelated code as part of a feature or bug fix.

## Notes for Copilot

- Treat this as a mature codebase with established layering.
- Prefer small, compatible changes that fit the current architecture over broad modernization.
