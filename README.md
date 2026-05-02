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
- For Windows 10 Mobile: a phone that can sideload apps

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
- Paste from clipboard
  - Text paste is supported
  - Image paste is supported only when the source app exposes usable image clipboard data
- File picker integration for UWP scenarios

## Known limitations

- HTTPS / encryption will not work on Windows 10 Mobile due to OS limitations. Turn off Encryption on target devices you want to send or receive from.
- Clipboard image copy is unreliable on Windows 10 Mobile. Many apps expose only text or no usable bitmap data through the clipboard.
- Because of that platform limitation, clipboard paste should be considered text-first on phones. For images, the file picker or Share UI can be used.
- Full parity with the latest desktop LocalSend implementations is still ongoing.

## Project structure

- `UniversalSend` - UI, pages, controls, app startup, navigation
- `UniversalSend.Models` - models, managers, helpers, settings, abstractions
- `UniversalSend.Services` - HTTP, REST, UDP discovery, and transport infrastructure

## Open source project references

| Function      | Project name and link                                                                                           |
| ------- | --------------------------------------------------------------------------------------------------------------------- |
| Send and receive protocol    | [localsend/protocol](https://github.com/localsend/protocol)                                                           |
| HTTP Server | [tomkuijsten/restup](https://github.com/tomkuijsten/restup)<br>This code has been integrated in the project |

## How to build and deploy

To build and deploy the app, you need Visual Studio 2017 build **15.9.75**. It is **very important to not update beyond that version**,
because newer version have removed support for the SDKs that the phone needs. Check Google/ChatGPT on how to install version 15.9.75.
If all goes well, you should be able to deply and debug on your phone (target ARM).

First copy the certificate (Zerqle.cer) to your phone and install it. Then install all the dependencies via the Windows Device Portal.
Finally, install the application via the Windows Device Portal.

## How to use AI to developer Windows 10 Mobile apps

If you want to use AI to help develop apps for Windows 10 Mobile, you can install the latest version of VS2026 with CoPilot.
If you open the solution in both VS2017 and in VS2026, you can write your code in VS2026 with AI, then build in VS2017 and run on the phone.
