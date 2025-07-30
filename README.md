# UniversalSend for Windows 10 Mobile

A third-party LocalSend client for the UWP platform, specifically targetting Windows 10 Mobile.

This project is developed based on the LocalSend protocol, see: [localsend/protocol](https://github.com/localsend/protocol)

Requirements:

- System version: Windows 10 (15063 and above)

- System architecture: ARM

- Compatibility of each platform:
  
  | Platform name       | State             | Remark                                     |
  | ------------------- | ----------------- | ------------------------------------------ |
  | Mobile              | ✔️Fully usable   | The interface has been optimized            |

Implemented functions:

* [x] Announcement through broadcasting over UDP (new!)
* [x] Receiving files
- [x] Send files, texts
- [x] Device Favorites
- [x] As the file picker location for other UWP apps
- [x] Transfer through system sharing api

Functions to be implemented:

* [ ] Receive text
* [ ] Confirm before receiving
- [ ] Multi-language support
* [ ] Support for HTTPS -> This will NEVER be supported on Windows 10 Mobile because of OS limitations.
- [ ] Display the progress of a single file transfer
- [ ] Handle the FileType of the sent file
* [ ] Support for LocalSend protocol V2

A practice project with a wild coding style. Please don't criticize if you don't like it!

Open source project references:

| Function      | Project name and link                                                                                           |
| ------- | --------------------------------------------------------------------------------------------------------------------- |
| Send and receive protocol    | [localsend/protocol](https://github.com/localsend/protocol)                                                           |
| HTTP Server | [tomkuijsten/restup](https://github.com/tomkuijsten/restup)<br>This code has been integrated in the project |
