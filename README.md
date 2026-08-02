
<div align="center">

# ByteCarve

<p>
  <b>An Avalonia desktop tool for carving fil

es from raw binary data, recovering embedded media by signature, and experimenting with low-level disassembly workflows.</b>
</p>

<p>
  <img alt="C#" src="https://img.shields.io/badge/C%23-.NET%2010-7c3aed?style=for-the-badge&logo=dotnet&logoColor=white">
  <img alt="Avalonia" src="https://img.shields.io/badge/Avalonia-Desktop%20UI-2563eb?style=for-the-badge">
  <img alt="MVVM" src="https://img.shields.io/badge/MVVM-CommunityToolkit-f97316?style=for-the-badge">
  <img alt="SQLite" src="https://img.shields.io/badge/SQLite-History-00a3cc?style=for-the-badge&logo=sqlite&logoColor=white">
  <img alt="Binary" src="https://img.shields.io/badge/Binary-File%20Carving-111827?style=for-the-badge">
</p>

<p>
  <img alt="PNG" src="https://img.shields.io/badge/PNG-Signature%20Recovery-22c55e?style=flat-square">
  <img alt="JPG" src="https://img.shields.io/badge/JPG-Segment%20Parsing-f59e0b?style=flat-square">
  <img alt="BMP" src="https://img.shields.io/badge/BMP-Header%20Recovery-ef4444?style=flat-square">
  <img alt="Inter" src="https://img.shields.io/badge/Inter-Clean%20UI-38bdf8?style=flat-square">
</p>

<p>
  <a href="#demo"><img alt="Demo" src="https://img.shields.io/badge/Demo-Reserved-00d084?style=for-the-badge"></a>
  <a href="#screenshots"><img alt="Screenshots" src="https://img.shields.io/badge/Screenshots-Table-facc15?style=for-the-badge"></a>
  <a href="#architecture"><img alt="Architecture" src="https://img.shields.io/badge/Architecture-Explained-f43f5e?style=for-the-badge"></a>
  <a href="#why-i-built-this"><img alt="Why" src="https://img.shields.io/badge/Why-I%20Built%20This-f97316?style=for-the-badge"></a>
</p>

</div>

---

## Overview

**ByteCarve** is a desktop forensic-style utility built with **C#**, **.NET 10**, and **Avalonia UI**. The app takes a raw file, lets the user choose a carving mode and output folder, scans the byte stream for recognizable file signatures/extracts recoverable content, and presents the result in a clean report view.

The current focus is media carving, with support for detecting and extracting embedded **PNG**, **JPG**, and **BMP** data. The repo also includes a low-level disassembly layer with instruction-group services for branches, loads, register operations, scalar operations, and immediate data-processing experiments.

## Demo
  <img src="https://github.com/user-attachments/assets/20dee9bd-e827-4cc0-be3f-51604503a8f8" width="800">
  
## Screenshots

| Screen | Preview |
| --- | --- |
| Source Picker | <img width="1280" height="720" alt="image" src="https://github.com/user-attachments/assets/b840fa4b-d5e3-4127-a8d8-7c76c03fdcff" />|
| Configuration | <img width="1278" height="719" alt="image" src="https://github.com/user-attachments/assets/5fc8d5c5-0322-4ccc-9566-186f8d6d3536" />|
| Carving Progress |<img width="1280" height="720" alt="image" src="https://github.com/user-attachments/assets/6050d0e4-191b-4bcc-b78f-1e2dd7ec0519" />|
| Report | <img width="1280" height="720" alt="image" src="https://github.com/user-attachments/assets/9a99f7ce-0d11-4786-a241-57df9697af00" />|
| History |<img width="1280" height="720" alt="image" src="https://github.com/user-attachments/assets/fe903b5e-ac75-45ca-aed7-4a93f58eae7c" />|

## Core Workflow

```text
Pick source file
      |
      v
Choose carve mode + output folder
      |
      v
Scan raw bytes for known signatures
      |
      v
Extract recovered files
      |
      v
Show report + save history
```

## What ByteCarve Does

| Area | Details |
| --- | --- |
| File selection | Uses Avalonia storage APIs to pick the source file from disk. |
| Carving setup | Lets the user choose a mode and output location before starting. |
| Signature scanning | Searches raw byte data for known file headers and structured endings. |
| Media recovery | Extracts PNG chunks until `IEND`, JPG scan data until `FFD9`, and BMP content by size header. |
| Progress page | Shows a dedicated carving screen while extraction runs. |
| Report view | Displays the carve name, duration, output folder, and recovered file count. |
| History | Stores past runs in a local SQLite database. |
| Disassembly lab | Includes service classes for experimenting with instruction decoding logic. |

## Architecture

| Layer | Files | Role |
| --- | --- | --- |
| App shell | `App.axaml`, `MainWindow.axaml`, `ViewLocator.cs` | Starts Avalonia, loads views from view models, and hosts page navigation. |
| View models | `PickViewModel`, `ConfigsViewModel`, `CarvingProgressViewModel`, `ReportViewModel`, `HistoryViewModel` | Own the app flow and screen state. |
| Views | `PickView`, `ConfigsView`, `CarvingProgressView`, `ReportView`, `HistoryView` | Styled Avalonia screens for each step. |
| Carving engine | `Models/carver.cs` | Reads the binary input and extracts media files by signatures and format structure. |
| Storage | `Models/database.cs`, `Assets/admin.db` | Saves and loads run history through SQLite. |
| Disassembly services | `Services/Branches.cs`, `Loads.cs`, `DP_*` | Experimental instruction decoding modules. |

## Tech Stack

| Tech | Usage |
| --- | --- |
| **C# / .NET 10** | Core app logic and binary-processing code |
| **Avalonia 12** | Cross-platform desktop UI |
| **CommunityToolkit.Mvvm** | Observable properties and relay commands |
| **Microsoft.Data.Sqlite** | Local history database |
| **Inter Font + Fluent Theme** | Clean desktop styling |

## Running Locally

```bash
cd ByteCarve
dotnet restore
dotnet run
```

Or build only:

```bash
dotnet build
```

## Why I Built This

well,reverse engineering was always something that attracted me yet i had no knowledge about it , so yea here goes an Aarch64
disassembler just for fun , but jokes aside thie repo really did help me get better at low level systems and really got a string foundation on binaries/registers/buckets etc , fun and interesting repo! (i sold my soul to finish this...)
## Developer Notes

> now for the notes:

* recorded hours of work : lost count after the 62th hour ..
* monsters/redbulls digested : 36 cans .
* current flaws : i have substracted several subbuckets , especially in the loads buckets , because god forbid i sit there for another * * month designing more subbuckets , i am not making a production level disassembler.
*frontend : helped by a web-dev friend (mostly he used generative ai ), i aint a front-end developer to spend weeks on the ui 
*total sub-buckets: 38 , price: my soul ..
* this repo is a learning project , not a serious project , further updates and commits will contribute into upgrading it into a more production ready state
*NOTE : all of the subbuckets are made by me , the only sub-bucket i used capstone for was in the data_processing_immediate section , it was an isolated and very long sub-bucket so it wasnt worth the time and effort.

---

<div align="center">
  <sub>Built with C#, Avalonia, MVVM, SQLite, byte signatures, and low-level curiosity.</sub>
</div>
