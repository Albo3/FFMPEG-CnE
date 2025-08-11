# FmpegLists

FmpegLists is a Windows desktop application that provides a simple graphical user interface (GUI) for the powerful command-line media converter, [ffmpeg](https://ffmpeg.org/).

It allows you to create and save templates for your favorite ffmpeg commands and then easily run them on files by dragging and dropping them into the application.

![FmpegLists Screenshot](https://i.imgur.com/Abm2g3I.png)

## Features

*   **Command Templates:** Create, save, and manage your own ffmpeg command templates.
*   **Drag and Drop:** Simply drag and drop your media files or folders containing media files onto the application to queue them for processing.
*   **Multi-pass Support:** Supports multi-step ffmpeg commands, such as those used for 2-pass video encoding.
*   **Real-time Output:** View the ffmpeg command output in real-time as it processes your files.
*   **Customizable:** Easily configure the path to your ffmpeg executable.

## Getting Started

### Prerequisites

*   [.NET 8.0 Desktop Runtime](https://dotnet.microsoft.com/download/dotnet/8.0)
*   [ffmpeg](https://ffmpeg.org/download.html) installed and accessible from your system's PATH, or you can specify the full path to `ffmpeg.exe` in the application.

### Usage

1.  **Launch the application.**
2.  **Add a command template:**
    *   Give your command a descriptive name in the "Name" field.
    *   Enter the ffmpeg command in the "Template" field. Use the following placeholders:
        *   `{input}`: The full path to the input file.
        *   `{output}`: The full path to the output file. You can also specify the output extension like this: `{output:mkv}`.
        *   `{dir}`: The directory of the input file.
        *   `{name}`: The name of the input file without the extension.
        *   `{ext}`: The extension of the input file.
        *   `{pass}`: The pass number (for multi-pass commands).
        *   `{passlog}`: The path to the pass log file (for multi-pass commands).
        *   `{null}`: The null device for your operating system.
    *   Click the "Add/Update" button to save the template.
3.  **Drag and drop files:** Drag your media files or a folder containing media files onto the drop area on the left.
4.  **Run a command:**
    *   Select the desired command from the dropdown list.
    *   Click the "Run" button.

## Building from Source

1.  Clone the repository:
    ```bash
    git clone https://github.com/your-username/FmpegLists.git
    ```
2.  Open the `FmpegLists.sln` file in Visual Studio.
3.  Build the solution (Ctrl+Shift+B).

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
