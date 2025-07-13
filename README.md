# UniversalSend

A third-party LocalSend client for the UWP platform

This project is developed based on the LocalSend protocol localsend/protocol [localsend/protocol](https://github.com/localsend/protocol)

Requirements:

-  System version: Windows 10 (15063 and above)

- System architecture: x86, x64, arm32, arm64 (arm64 needs to be compiled by yourself)

- Compatibility of each platform:
  
  | Platform name       | State        | Remark                                        |
  | ---------- | --------- | ------------------------------------------ |
  | Desktop    | ✔️Fully usable    |                                            |
  | Mobile     | ✔️Fully usable    | The interface has been optimized                                   |
  | Xbox       | ⚠️Usable, but unstable | Receive function does not work on some devices |
  | SurfaceHub | ❓Unknown       |                                            |
  | Hololens   | ✔️Fully usable    |                                            |
  | IoT        | ❓Unknown       |                                            |

Implemented functions:

* [x] Receiving files
- [x] Send files, texts
- [x] Device Favorites
- [x] As the file picker location for other UWP apps
- [x] Transfer through system sharing api

Functions to be implemented:

* [ ] Receive text
* [ ] Confirm before receiving
- [ ] Multi-language support
* [ ] Support for HTTPS
- [ ] Display the progress of a single file transfer
- [ ] Handle the FileType of the sent file
* [ ] Support for LocalSend protocol V2

A practice project with a wild coding style. Please don't criticize if you don't like it!

Open source project references:

| Function      | Project name and link                                                                                           |
| ------- | --------------------------------------------------------------------------------------------------------------------- |
| Send and receive protocol    | [localsend/protocol](https://github.com/localsend/protocol)                                                           |
| HTTP Server | [tomkuijsten/restup](https://github.com/tomkuijsten/restup)<br>有所修改，[has been modified, see the modified copy](https://github.com/Pigeon-Ming/restup) |
