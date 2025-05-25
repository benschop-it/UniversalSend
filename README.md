# UniversalSend

UWP平台的第三方LocalSend客户端

本项目基于LocalSend协议[localsend/protocol](https://github.com/localsend/protocol)开发

运行要求：

-  系统版本：Windows 10（15063及以上）

- 系统架构：x86，x64，arm32，arm64（arm64需自行编译）

- 各平台兼容情况：
  
  | 平台名称       | 状态     | 备注       |
  | ---------- | ------ | -------- |
  | Desktop    | ✔️完全可用 |          |
  | Mobile     | ✔️完全可用 | 已对界面进行优化 |
  | Xbox       | ❓未知    |          |
  | SurfaceHub | ❓未知    |          |
  | Hololens   | ✔️完全可用 |          |
  | IoT        | ❓未知    |          |

已实现的功能：

* [x] 接收文件
- [x] 发送文件、文本
- [x] 设备收藏夹
- [x] 作为其它UWP应用的文件选取器位置
- [x] 通过系统分享api传输

待实现的功能：

* [ ] 接收文本
* [ ] 接收前确认
- [ ] 多语言支持
* [ ] 对HTTPS的支持
- [ ] 显示单个文件传输的进度
- [ ] 处理发送文件的FileType
* [ ] 对LocalSend协议V2的支持

练手项目，代码风格及其狂野，不喜勿喷！

开源项目引用：

| 功能      | 项目名称及连接                                                                                                               |
| ------- | --------------------------------------------------------------------------------------------------------------------- |
| 收发协议    | [localsend/protocol](https://github.com/localsend/protocol)                                                           |
| HTTP服务器 | [tomkuijsten/restup](https://github.com/tomkuijsten/restup)<br>有所修改，[查看修改后的副本](https://github.com/Pigeon-Ming/restup) |
