# LocalSend interoperability summary

## Protocol versions

The LocalSend protocol has two wire versions:

- **v2** — The current standard. Uses `fingerprint`, `download`, `announce` field names. Uses HTTP only. This is what UniversalSend targets.
- **v3** — Adds HTTPS with mTLS, nonce exchange (`/api/localsend/v3/nonce`), cryptographic token validation (ed25519 signatures), and renames `fingerprint` → `token`, `download` → `hasWebInterface`. Requires TLS certificate exchange.

UniversalSend targets v2 only, which is correct given the Windows 10 Mobile / Build 15063 constraint (no modern TLS client certificate support).

## Current status

UniversalSend has implemented the major LocalSend v2-compatible HTTP/UDP flows needed for practical interoperability on Windows 10 Mobile-targeted UWP.

Completed areas include:

- UDP multicast discovery (224.0.0.167, port from settings)
- HTTP register fallback (`POST /api/localsend/v2/register`)
- `GET /api/localsend/v2/info` and `v1/info` (legacy)
- upload flow: `prepare-upload` → `upload` → `cancel`
- upload session/token validation
- sender-side protocol status handling (rejected, blocked, finished, etc.)
- received text flow (text files with `preview` field)
- partial acceptance (subset of files)
- reverse download API (`prepare-download`, `download`)
- web share (publish share, browser URL, basic lifecycle)
- `FileMetadata` support (`modified`, `accessed` timestamps)
- several transport and memory improvements

## Architecture of the LocalSend reference implementation

The LocalSend codebase is primarily **Dart/Flutter**, not Rust. The Rust `core` crate provides crypto, a minimal HTTP server skeleton, and WebRTC signaling, but the vast majority of business logic lives in:

- **`common/lib/`** — Shared Dart library: DTOs, models, API route definitions, HTTP client/upload tasks, isolate management.
- **`app/lib/`** — Flutter app: providers, server controllers, UI pages.

Key Dart files relevant to interop:

| Dart file | Purpose |
|-----------|---------|
| `common/lib/api_route_builder.dart` | All v1/v2 API paths (`/api/localsend/v2/...`) |
| `common/lib/model/dto/info_dto.dart` | `InfoDto` — v2 info response (`fingerprint`, `download`) |
| `common/lib/model/dto/register_dto.dart` | `RegisterDto` — v2 register request |
| `common/lib/model/dto/multicast_dto.dart` | `MulticastDto` — UDP announcement (`announce`, `fingerprint`) |
| `common/lib/model/dto/file_dto.dart` | `FileDto` — file metadata (uses `hash` not `sha256`) |
| `common/lib/model/dto/info_register_dto.dart` | `InfoRegisterDto` — info block in `prepare-upload` |
| `common/lib/model/dto/receive_request_response_dto.dart` | `ReceiveRequestResponseDto` — `prepare-download` response |
| `common/lib/model/device.dart` | `DeviceType` enum, `Device` model |
| `app/lib/provider/network/server/controller/receive_controller.dart` | Server-side handlers: info, register, prepare-upload, upload, cancel |
| `app/lib/provider/network/server/controller/send_controller.dart` | Server-side handlers: prepare-download, download, web share |
| `app/lib/provider/network/server/controller/common.dart` | PIN checking logic |
| `app/lib/provider/network/send_provider.dart` | Client-side send session management |

## Remaining differences compared to the LocalSend implementation

### 1. All v2 DTOs used v3 field names instead of v2

**Fixed.** `InfoDataV2`, `RegisterResponseDataV2`, and `RegisterRequestDataV2` all serialized `Fingerprint` as `"token"` and `Download` as `"hasWebInterface"` (v3 naming). The v2 protocol and the Dart `InfoDto`/`RegisterDto` both use `"fingerprint"` and `"download"`. All three DTOs now serialize with v2 names and accept both v2 and v3 names on deserialization for forward-compatibility. The separate `DownloadInfoDataV2` workaround DTO was removed since `InfoDataV2` itself now has the correct wire format.

### 2. `sha256` / `hash` field name mismatch

**Fixed.** The protocol spec (section 4.1) says `"sha256"`. LocalSend Dart uses `"hash"`. `FileRequestDataV2` now serializes as `"sha256"` (per spec) and also accepts `"hash"` as a deserialize alias, so either field name will be read correctly.

### 3. `Content-Disposition` header missing on download responses

**Fixed.** The `GET /v2/download` endpoint now sets `Content-Disposition: attachment; filename="..."` with URI-encoded filename, matching the Dart `send_controller.dart` behavior. This ensures browsers show the correct filename.

### 4. Reverse download / web share is a partial approximation

LocalSend (Dart) has a richer browser/web-share lifecycle and session model.
UniversalSend currently supports:

- active web share state
- `prepare-download` (with session reuse via `?sessionId=...` for browser refresh — **Fixed**)
- `download`
- browser share URL creation
- basic lifecycle cleanup

Still missing compared to LocalSend (Dart):

- PIN support on `prepare-download` (`?pin=123456` query parameter). The Dart `send_controller.dart` calls `checkPin()` (from `common.dart`), which enforces PIN with 3-attempt lockout (429 Too Many Requests).
- Per-session IP tracking and auto-accept mode for web share.

### 5. `Content-Length` header on download responses

The Dart handler always sets `Content-Length`. UniversalSend's HTTP server does emit `Content-Length` for streamed responses via `StreamContentLength`, but this should be verified during testing.

### 6. Receive-side transport is still not true streaming

UniversalSend now reduces memory usage significantly, but the HTTP request body is still fully buffered before file persistence begins. The Dart `receive_controller.dart` streams file content directly from the HTTP request to disk.

### 7. `DeviceType` normalization

**Fixed.** Unknown device type values are now normalized to `"desktop"` per the protocol spec (section 7.1), matching both the Dart (`@MappableEnum(defaultValue: DeviceType.desktop)`) and Rust implementations. The normalization is applied in all `DeviceManager` methods that create devices from incoming data.

### 8. Session and token handling is simpler

UniversalSend validates session/file/token ownership for HTTP flows, but does not implement the cryptographic token validation from the Rust `core` crate (v3-only: ed25519 signatures, nonce exchange, timestamp-based expiry). This is expected for v2-only.

### 10. No v3 / HTTPS / mTLS path

The Rust `core` crate supports v3 with TLS, mTLS, nonce exchange, and v3 register. The Dart app has prototype mTLS code (commented out in `server_provider.dart`). UniversalSend does not implement v3. Acceptable due to the Windows 10 Mobile constraint.

### 11. No WebRTC path

The Dart app has `webrtc_receiver.dart` and `signaling_provider.dart`. The Rust `core` crate has `webrtc/` modules. UniversalSend does not implement WebRTC.

### 12. Browser-share UX is simpler

The Dart `send_controller.dart` serves a full web page (`/`, `/main.js`, `/i18n.json`) for browser-based downloads, with internationalization and JS-based UI. UniversalSend uses a manual URL-copy workflow.

### 13. Temp-file cleanup may still need more hardening

The Dart implementation cleans up via session state management. UniversalSend now cleans temp files on common reset/cancel paths, but abrupt termination may leave orphans.

### 14. Partial acceptance is implemented but still needs interop validation

The feature exists in UniversalSend, but its behavior should be verified against real LocalSend clients. The Dart `receive_controller.dart` handles partial acceptance by returning tokens only for accepted files (missing files get no token in the response map).

### 15. `cancel` endpoint log messages

**Fixed.** The `PostCancel` methods now correctly log `"POST v2/cancel called"` instead of `"GET v2/Cancel called"`.

### 16. Self-discovery rejection on HTTP endpoints

**Fixed.** The `GET /v2/info?fingerprint=...`, `GET /v1/info?fingerprint=...`, and `POST /v2/register` endpoints now compare the incoming fingerprint against the local device and return 412 (Precondition Failed) when they match, matching the Dart `_infoHandler` and `_registerHandler` behavior.

### 17. `v1` endpoint support

The Dart app supports both v1 and v2 endpoints (`prepareUpload.v1`, `upload.v1`, `cancel.v1`). UniversalSend only maps `v1/info` (with `?fingerprint`). If a legacy v1 client tries `v1/send-request` or `v1/send`, it will get a 404 from UniversalSend. This is likely acceptable since v1 devices are very rare.

### 18. Discovery smartness

The Dart `scan_facade.dart` implements a multi-phase discovery: multicast first, then favorites scan, then (after 1 second if no devices found) legacy HTTP subnet scan. UniversalSend's discovery is simpler — multicast only plus manual add.

## Recommended remaining engineering work

### Phase 9 follow-up (hardening)

- ~~Fix all v2 DTO field names (fingerprint/download)~~ — **Done**
- ~~Fix `sha256`/`hash` field alias~~ — **Done**
- ~~Add `Content-Disposition` header to download responses~~ — **Done**
- ~~Fix cancel log messages~~ — **Done**
- ~~Add self-discovery rejection on HTTP endpoints~~ — **Done**
- ~~Normalize `DeviceType` values~~ — **Done**
- ~~Add session reuse on `prepare-download`~~ — **Done**
- Improve temp-file cleanup for all exceptional paths
- Consider better temp-to-final move behavior
- Consider optional parallel uploads if device stability allows
- Reduce remaining receive-side buffering if feasible

### Phase 10 (interop validation)

Perform a full real-device interoperability validation pass for:

- discovery (UDP multicast, both directions)
- manual discovery (HTTP register)
- upload (prepare-upload → upload)
- cancel (during and after transfer)
- text receive (clipboard text as file with preview)
- partial acceptance (accept subset of offered files)
- reverse download API (prepare-download → download)
- browser share (URL, download from browser)

Then fix any remaining mismatches discovered during testing.

See `docs/manual-test-plan.md` for the detailed test procedure.
