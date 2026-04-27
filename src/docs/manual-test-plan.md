# Manual Test Plan — UniversalSend ↔ LocalSend Interoperability

## Test Devices

| ID | Device | OS | App | Role |
|----|--------|----|-----|------|
| **W** | Lumia 950XL | Windows 10 Mobile (Build 15063) | UniversalSend | Sender / Receiver |
| **A** | Android Phone | Android | LocalSend (latest) | Sender / Receiver |
| **D** | Windows Desktop | Windows 10/11 | LocalSend Desktop (latest) | Sender / Receiver |

## Prerequisites

- All three devices are connected to the same local Wi-Fi network.
- No VPN or firewall is blocking UDP port 53317 or the configured HTTP port.
- LocalSend on A and D are at default settings (port 53317, HTTPS mode disabled or v2 fallback enabled).
- UniversalSend on W is freshly launched with a recognizable device name (e.g., "Lumia950XL").

---

## 1. Discovery

### 1.1 UDP Multicast — W discovers A and D

| Step | Action | Expected Result |
|------|--------|-----------------|
| 1 | Launch UniversalSend on W | W sends UDP announcement on 224.0.0.167 |
| 2 | Launch LocalSend on A and D | A and D send UDP announcements |
| 3 | Wait 5–10 seconds | W shows A and D in device list with correct alias, device type icon, and model |
| 4 | Verify A and D also discover W | A and D show "Lumia950XL" (or configured alias) as a mobile device |

### 1.2 UDP Multicast — Late joiner

| Step | Action | Expected Result |
|------|--------|-----------------|
| 1 | W is already running with A visible | A shown in device list |
| 2 | Launch LocalSend on D | D sends announcement, W adds D to list |
| 3 | Verify D also sees W | D shows W in its device list |

### 1.3 HTTP Register fallback (manual discovery)

| Step | Action | Expected Result |
|------|--------|-----------------|
| 1 | On W, use manual/add-device with IP of A | W sends `POST /api/localsend/v2/register` to A |
| 2 | Verify A appears in W's device list | A shown with correct alias and device type |
| 3 | Verify A also discovers W via the register response | A shows W in its list |

---

## 2. Send Files (W → A, W → D)

### 2.1 Single file — W sends to A

| Step | Action | Expected Result |
|------|--------|-----------------|
| 1 | On W, select a small image file (< 1 MB) | File selected in send view |
| 2 | Tap A in device list | W sends `POST /api/localsend/v2/prepare-upload` to A |
| 3 | A shows accept/reject dialog | File name, size, and sender alias visible |
| 4 | Accept on A | A responds 200 with sessionId and file tokens |
| 5 | W uploads file via `POST /upload?sessionId=...&fileId=...&token=...` | W shows progress, then success |
| 6 | Verify file on A | File is saved, correct content, correct filename |

### 2.2 Single file — W sends to D

| Step | Action | Expected Result |
|------|--------|-----------------|
| 1 | Repeat 2.1 but target D instead of A | Same expected results |

### 2.3 Multiple files — W sends to A

| Step | Action | Expected Result |
|------|--------|-----------------|
| 1 | On W, select 3 files (mix of image and document) | Files listed in send view |
| 2 | Tap A in device list, accept on A | All 3 files transfer successfully |
| 3 | Verify all files on A | Correct names, sizes, content |

### 2.4 Large file — W sends to A

| Step | Action | Expected Result |
|------|--------|-----------------|
| 1 | On W, select a file > 50 MB | File selected |
| 2 | Send to A, accept on A | Transfer completes (may be slow) |
| 3 | Verify file on A | File is intact, correct size |

### 2.5 Text/clipboard — W sends text to A

| Step | Action | Expected Result |
|------|--------|-----------------|
| 1 | On W, share text content | Text prepared as file with `preview` field |
| 2 | Send to A, accept on A | A receives and shows the text content |

---

## 3. Receive Files (A → W, D → W)

### 3.1 Single file — A sends to W

| Step | Action | Expected Result |
|------|--------|-----------------|
| 1 | On A, select a small image and tap W | A sends `prepare-upload` to W |
| 2 | W shows accept/reject dialog | File name, size, sender alias visible |
| 3 | Accept on W | W responds 200 with sessionId/tokens |
| 4 | A uploads file | W shows progress and success |
| 5 | Verify file on W | File saved to configured folder, correct content |

### 3.2 Single file — D sends to W

| Step | Action | Expected Result |
|------|--------|-----------------|
| 1 | Repeat 3.1 but from D | Same expected results |

### 3.3 Multiple files — A sends to W

| Step | Action | Expected Result |
|------|--------|-----------------|
| 1 | On A, select 3 files, send to W | W shows accept dialog listing all 3 files |
| 2 | Accept on W | All 3 files received correctly |

### 3.4 Large file — D sends to W

| Step | Action | Expected Result |
|------|--------|-----------------|
| 1 | On D, select a file > 50 MB, send to W | Transfer completes without out-of-memory crash |
| 2 | Verify file on W | File is intact |

### 3.5 Text receive — A sends text to W

| Step | Action | Expected Result |
|------|--------|-----------------|
| 1 | On A, share clipboard text to W | W receives text, shows it (or saves as text file) |

---

## 4. Partial Acceptance

### 4.1 Accept subset — A sends 3 files, W accepts 2

| Step | Action | Expected Result |
|------|--------|-----------------|
| 1 | On A, select 3 files, send to W | W shows accept dialog with 3 files |
| 2 | On W, deselect 1 file and accept | W responds with tokens only for the 2 accepted files |
| 3 | A uploads only the 2 accepted files | A shows those 2 as successful, the deselected one as skipped |
| 4 | Verify on W | Only 2 files saved |

---

## 5. Cancel

### 5.1 Sender cancels during transfer — W cancels while sending to A

| Step | Action | Expected Result |
|------|--------|-----------------|
| 1 | W starts sending a large file to A | Transfer in progress |
| 2 | W cancels the transfer | W sends `POST /api/localsend/v2/cancel?sessionId=...` |
| 3 | A shows "Transfer cancelled" or similar | A cleans up partial file |

### 5.2 Receiver cancels during transfer — W cancels while receiving from A

| Step | Action | Expected Result |
|------|--------|-----------------|
| 1 | A sends a large file to W, W accepts | Transfer in progress |
| 2 | W cancels | W returns error on next upload chunk / cancels session |
| 3 | A shows "Transfer cancelled" | W cleans up temp files |

### 5.3 Sender cancels during acceptance — A cancels while W shows accept dialog

| Step | Action | Expected Result |
|------|--------|-----------------|
| 1 | A sends files to W | W shows accept/reject dialog |
| 2 | A cancels before W responds | A sends cancel; W dismisses dialog or shows "Cancelled" |

### 5.4 Receiver rejects — W rejects incoming from A

| Step | Action | Expected Result |
|------|--------|-----------------|
| 1 | A sends files to W | W shows accept/reject dialog |
| 2 | W taps Reject | W returns 403; A shows "Rejected" |

---

## 6. Reverse Download / Web Share

### 6.1 W shares file via browser URL — D downloads

| Step | Action | Expected Result |
|------|--------|-----------------|
| 1 | On W, select a file and start web share | W activates share, shows URL |
| 2 | On D, open a browser and navigate to URL | Browser shows download page / starts download |
| 3 | Verify downloaded file on D | Correct file, correct name |

**Known issue:** The `prepare-download` info block currently uses v3 field names (`token`, `hasWebInterface`) instead of v2 (`fingerprint`, `download`). If a LocalSend client calls `prepare-download`, it may not parse the info block correctly. This should be fixed before validating this test.

### 6.2 W shares file via browser URL — A downloads via browser

| Step | Action | Expected Result |
|------|--------|-----------------|
| 1 | On W, activate web share | URL displayed |
| 2 | On A, open browser and navigate to URL | File downloads via browser |

### 6.3 LocalSend app calls prepare-download directly (D → W)

| Step | Action | Expected Result |
|------|--------|-----------------|
| 1 | On W, activate web share | Share active |
| 2 | On D (LocalSend), initiate receive/download from W | D sends `POST /api/localsend/v2/prepare-download` to W |
| 3 | D receives file metadata response | Correct session ID, file list |
| 4 | D downloads files via `GET /download?sessionId=...&fileId=...` | Files received correctly |

---

## 7. Edge Cases and Robustness

### 7.1 Session blocked by another session

| Step | Action | Expected Result |
|------|--------|-----------------|
| 1 | A sends files to W, W accepts (transfer in progress) | Session active |
| 2 | D also tries to send to W | W returns 409 (Blocked by another session) |
| 3 | D shows appropriate error | "Busy" or "Blocked" message |

### 7.2 App backgrounded during transfer — W

| Step | Action | Expected Result |
|------|--------|-----------------|
| 1 | Start a large transfer to W | Transfer in progress |
| 2 | Switch away from UniversalSend on W briefly | Transfer should survive or gracefully fail |
| 3 | Return to UniversalSend | Transfer state is consistent (completed or cleanly failed) |

### 7.3 Network interruption

| Step | Action | Expected Result |
|------|--------|-----------------|
| 1 | Start a transfer between W and A | Transfer in progress |
| 2 | Briefly disable Wi-Fi on W | Transfer fails with timeout or error |
| 3 | Re-enable Wi-Fi | Both devices return to idle state, no hung sessions |

### 7.4 Zero-byte file

| Step | Action | Expected Result |
|------|--------|-----------------|
| 1 | On A, create and send a 0-byte file to W | W shows accept dialog |
| 2 | Accept on W | File received (0 bytes), no crash |

### 7.5 Special characters in filename

| Step | Action | Expected Result |
|------|--------|-----------------|
| 1 | On A, send a file named `tëst file (1) — copy.txt` | W shows correct filename |
| 2 | Accept on W | File saved with correct (or safely sanitized) filename |

---

## 8. Bidirectional stress

### 8.1 Simultaneous discovery + transfer

| Step | Action | Expected Result |
|------|--------|-----------------|
| 1 | W is receiving a file from A | Transfer in progress |
| 2 | D launches LocalSend | D sends UDP announcement |
| 3 | W adds D to device list | Device list updates without interrupting transfer |

### 8.2 Rapid send-cancel-send

| Step | Action | Expected Result |
|------|--------|-----------------|
| 1 | W sends a file to A, cancel immediately | Session cancelled |
| 2 | W sends another file to A | New session starts cleanly, file transfers successfully |

---

## Results Template

| Test | W→A | W→D | A→W | D→W | Notes |
|------|-----|-----|-----|-----|-------|
| 1.1 Discovery | | | | | |
| 1.2 Late joiner | | | | | |
| 1.3 HTTP Register | | | | | |
| 2.1 Single file send | | | N/A | N/A | |
| 2.2 Single file send | N/A | | N/A | N/A | |
| 2.3 Multiple files send | | | N/A | N/A | |
| 2.4 Large file send | | | N/A | N/A | |
| 2.5 Text send | | | N/A | N/A | |
| 3.1 Single file receive | N/A | N/A | | | |
| 3.2 Single file receive | N/A | N/A | N/A | | |
| 3.3 Multiple files receive | N/A | N/A | | | |
| 3.4 Large file receive | N/A | N/A | N/A | | |
| 3.5 Text receive | N/A | N/A | | | |
| 4.1 Partial acceptance | N/A | N/A | | | |
| 5.1 Sender cancel | | | N/A | N/A | |
| 5.2 Receiver cancel | N/A | N/A | | | |
| 5.3 Sender cancel during accept | N/A | N/A | | | |
| 5.4 Receiver reject | N/A | N/A | | | |
| 6.1 Web share to browser (D) | | | N/A | N/A | |
| 6.2 Web share to browser (A) | | | N/A | N/A | |
| 6.3 prepare-download (D) | | | N/A | N/A | |
| 7.1 Blocked session | N/A | N/A | | | |
| 7.2 App backgrounded | N/A | N/A | | | |
| 7.3 Network interruption | | | | | |
| 7.4 Zero-byte file | | | | | |
| 7.5 Special filename | | | | | |
| 8.1 Discovery during transfer | | | | | |
| 8.2 Rapid cancel-resend | | | | | |

Mark each cell: ✅ Pass, ❌ Fail (with bug #), ⚠️ Partial, ⏭️ Skipped.
