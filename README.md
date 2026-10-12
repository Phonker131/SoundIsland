# SoundIsland

A Windows desktop music-player learning project using a .NET 6 Windows Forms interface and Python integration.

## Overview
The original implementation searches music by song or artist name and provides playback, pause, and volume controls. Development is currently paused.

## Screenshots
![Application menu](SoundIsland%20Images/MenuImage.png)
![Downloading tracks](SoundIsland%20Images/DuringDownloadImage.png)
![Download result](SoundIsland%20Images/ResultImage.png)

## Source and documentation
- [Streaming service/](Streaming%20service/): application source.
- [Documentation/](Documentation/): accompanying documentation.
- [SoundIsland Images/](SoundIsland%20Images/): screenshots.

## Development environment
The original project targets Windows with .NET 6, Visual Studio's Windows Forms tooling, and Python 3. Open the solution in the source directory to inspect its build configuration.

## Local configuration
The Python helper reads `ARL.txt` from the application working directory. Create this file locally with credentials for your own authorized account; it is intentionally excluded from version control. Install the helper's `pydeezer` dependency in the Python environment used by the application.

## Current limitations
- The original documentation reports issues with Cyrillic and French characters in search terms.
- Music retrieval depends on external services and local configuration.
- Building requires Windows and the Windows Media Player COM reference (`WMPLib`).
- The project file has a post-build copy step for a `Libs` folder that is absent from this checkout; the build configuration needs review.
- A clean Windows build and current external-service compatibility have not been verified.
- Credentials and session cookies must remain outside version control.

This repository documents an earlier desktop project; it is not presented as a maintained streaming service. Only use content and services you are authorized to access.

## Author
Tymur Shapoval

## License
See [LICENSE](LICENSE).
