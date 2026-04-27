# LocalSend interoperability summary

## Current status

UniversalSend has implemented the major LocalSend-compatible HTTP/UDP flows needed for practical interoperability on Windows 10 Mobile-targeted UWP.

Completed areas include:

- discovery compatibility
- manual discovery fallback
- outbound metadata improvements
- upload session validation
- sender-side protocol status handling
- received text flow
- partial acceptance
- reverse download API and web share flow
- several transport and memory improvements

## Remaining differences compared to the Rust/LocalSend implementation

### 1. Reverse download / web share is a partial approximation

LocalSend has a richer browser/web-share lifecycle and session model.
UniversalSend currently supports:

- active web share state
- `prepare-download`
- `download`
- browser share URL creation
- basic lifecycle cleanup

But it does not yet match the richer Rust-side web session flow one-for-one.

### 2. Receive-side transport is still not true streaming

UniversalSend now reduces memory usage significantly by:

- streaming direct uploads from file
- streaming reverse downloads from file
- persisting received file uploads to temp storage instead of keeping them in memory afterward
- avoiding `List<byte>` growth in request parsing

However, the HTTP request body is still fully buffered before file persistence begins.

### 3. Binary GET behavior is simpler than the Rust implementation

UniversalSend now returns raw bytes and sets `Content-Type`, but still likely lacks more advanced HTTP file-serving behavior such as:

- `Content-Disposition`
- range support
- resumable semantics
- richer cache/download headers

### 4. Reverse download behavior still needs real-client validation

The DTOs and endpoints are now close to LocalSend, but the implementation still needs confirmation against real LocalSend clients for:

- request/response shape
- expected verbs and headers
- browser-share lifecycle
- actual client-side consumption behavior

### 5. Session and token handling is simpler than Rust

UniversalSend validates session/file/token ownership for HTTP flows, but does not implement the stronger cryptographic token validation used in some Rust-side paths.

### 6. No WebRTC path

The Rust/LocalSend codebase includes WebRTC-related behavior.
UniversalSend does not implement that path.

### 7. Browser-share UX is simpler

UniversalSend currently uses a manual browser-share workflow:

- publish share
- copy URL to clipboard
- use browser/download flow

This is usable, but not as integrated as the Rust/LocalSend implementation.

### 8. Temp-file cleanup is improved but may still need more hardening

UniversalSend now cleans temp files on common reset/cancel paths, but abrupt termination and some exceptional cases may still need additional hardening.

### 9. Partial acceptance is implemented but still needs interop validation

The feature exists in UniversalSend, but its behavior should still be verified against real LocalSend clients.

### 10. Internal modeling still differs

UniversalSend remains more string-based internally for some protocol concepts where Rust/LocalSend uses more strongly typed models and enums.

This is acceptable for compatibility, but not structurally identical.

## Recommended remaining engineering work

### Phase 9 follow-up

- improve temp-file cleanup for all exceptional paths
- consider better temp-to-final move behavior
- consider optional parallel uploads if device stability allows
- reduce remaining receive-side buffering if feasible

### Phase 10

Perform a full real-device interoperability validation pass for:

- discovery
- manual discovery
- upload
- cancel
- text receive
- partial acceptance
- reverse download API
- browser share

Then fix any remaining mismatches discovered during testing.
