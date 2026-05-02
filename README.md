# UniversalSend for Windows 10 Mobile

UniversalSend is a UWP LocalSend-compatible client focused on old Windows 10 Mobile devices, especially systems limited to Windows 10 Creators Update (build 15063).

The project is based on the LocalSend protocol:
- [localsend/protocol](https://github.com/localsend/protocol)

## Goals

- Keep the app usable on older Windows 10 Mobile devices such as the Lumia 950 XL.
- Stay compatible with current LocalSend protocol behavior where possible.
- Prefer small, compatible changes over platform upgrades that would break support for older phones.

## Requirements

- Windows 10 version 15063 or newer
- UWP-capable device

## Current status

| Platform | State | Notes |
| --- | --- | --- |
| Windows 10 Mobile | Supported | Primary target platform |
| UWP on newer Windows 10 | Supported | Some newer UI contract warnings can be ignored for the phone-targeted path |

## Implemented features

- UDP device discovery
- Send files
- Send folders by enumerating files inside the selected folder
- Send text messages
- Receive files
- Receive text messages
- Confirm incoming transfers before receiving
- LocalSend v2 transfer flow
- Device favorites
- Manual send by IP address or hashtag
- Web Share / browser download share
- Optional PIN for Web Share
- Share target activation from other apps
- Paste from clipboard
  - Text paste is supported
  - Image paste is supported only when the source app exposes usable image clipboard data
- File picker integration for UWP scenarios

## Known limitations

- HTTPS / encryption is not expected to work reliably on Windows 10 Mobile due to OS limitations.
- Clipboard image copy is unreliable on Windows 10 Mobile. Many apps expose only text or no usable bitmap data through the clipboard.
- Because of that platform limitation, clipboard paste should be considered text-first on phones. For images, the file picker or Share UI is usually more reliable.
- Single-file transfer progress/details can still be improved.
- Full parity with the latest desktop LocalSend implementations is still ongoing.

## Clipboard behavior

Clipboard paste has been improved to better match current app behavior:

- Repeated clipboard sends now replace older clipboard-origin items in the send queue instead of reusing stale content.
- Image-related clipboard formats are preferred over text when both are present.
- Temporary files created for pasted bitmap data use unique names to avoid collisions.

On Windows 10 Mobile, image copy support depends heavily on the source app and OS clipboard support. If an image cannot be pasted, use file picking or sharing from the source app instead.

## Project structure

- `UniversalSend` - UI, pages, controls, app startup, navigation
- `UniversalSend.Models` - models, managers, helpers, settings, abstractions
- `UniversalSend.Services` - HTTP, REST, UDP discovery, and transport infrastructure

## References

| Area | Project |
| --- | --- |
| Send and receive protocol | [localsend/protocol](https://github.com/localsend/protocol) |
| HTTP server foundation | [tomkuijsten/restup](https://github.com/tomkuijsten/restup) |
