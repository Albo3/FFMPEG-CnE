using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FmpegLists
{
    public partial class Form1 : Form
    {
        private readonly List<CommandEntry> _commands = new();
        private readonly List<string> _lastDroppedPaths = new();
        private string _storePath = "";

        // Default extension used by {output} when the template doesn't override it.
        private const string DefaultOutputExtension = "";

        // If ffmpeg isn't on PATH, set the full path here (e.g. @"C:\Tools\ffmpeg\bin\ffmpeg.exe")
        private const string FfmpegPath = "ffmpeg";

        private static readonly string NullDevice =
            (OperatingSystem.IsWindows() ? "NUL" : "/dev/null");

        public Form1()
        {
            InitializeComponent();

            _storePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "FmpegLists",
                "commands.json");

            WireUpEvents();
            LoadCommands();
            RefreshUI();
        }

        private void WireUpEvents()
        {
            addButton.Click += AddButton_Click;
            runButton.Click += RunButton_Click;
            commandListBox.SelectedIndexChanged += CommandListBox_SelectedIndexChanged;
            commandListBox.KeyDown += CommandListBox_KeyDown;

            // Remove button
            rmButton.Click += RmButton_Click;

            // Drag & drop on both the form and the drop panel
            this.DragEnter += OnDragEnter;
            this.DragDrop += OnDragDrop;
            panel1.AllowDrop = true;
            panel1.DragEnter += OnDragEnter;
            panel1.DragDrop += OnDragDrop;

            // Keep remove button state in sync
            commandSelector.SelectedIndexChanged += (_, __) => UpdateRemoveButtonState();
        }

        #region Persistence

        private void LoadCommands()
        {
            try
            {
                if (File.Exists(_storePath))
                {
                    var json = File.ReadAllText(_storePath);
                    var loaded = JsonSerializer.Deserialize<List<CommandEntry>>(json) ?? new List<CommandEntry>();
                    _commands.Clear();
                    _commands.Add(new CommandEntry
                    {
                        Name = "H.264 MP4 (CRF 23)",
                        Template = "-y -i {input} -c:v libx264 -preset veryfast -crf 23 -c:a aac -b:a 192k {output}"
                    });

                    _commands.Add(new CommandEntry
                    {
                        Name = "VP9 2-pass (deint+IVTC, CRF 28)",
                        Template = @"
-y -i {input} -c:v libvpx-vp9 -pix_fmt yuv420p -sn -vf ""fieldmatch=combmatch=full,bwdif=mode=0:deint=1,decimate=dupthresh=1.7,scale=trunc(iw/2)*2:trunc(ih/2)*2"" -b:v 0 -crf 28 -pass {pass} -passlogfile {passlog} -an -f null {null}
---
-y -i {input} -c:v libvpx-vp9 -pix_fmt yuv420p -c:a libopus -b:a 128k -sn -vf ""fieldmatch=combmatch=full,bwdif=mode=0:deint=1,decimate=dupthresh=1.7,scale=trunc(iw/2)*2:trunc(ih/2)*2"" -b:v 0 -crf 28 -pass {pass} -passlogfile {passlog} {output:webm}
".Trim()
                    });

                    SaveCommands();
                }
            }
            catch (Exception ex)
            {
                AppendOutput($"Failed to load commands: {ex.Message}");
            }
        }

        private void SaveCommands()
        {
            try
            {
                var dir = Path.GetDirectoryName(_storePath)!;
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                var json = JsonSerializer.Serialize(_commands, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_storePath, json);
            }
            catch (Exception ex)
            {
                AppendOutput($"Failed to save commands: {ex.Message}");
            }
        }

        #endregion

        #region UI helpers

        private void RefreshUI()
        {
            commandListBox.BeginUpdate();
            commandListBox.Items.Clear();
            foreach (var c in _commands)
                commandListBox.Items.Add($"{c.Name}  —  {c.Template}");
            commandListBox.EndUpdate();

            commandSelector.BeginUpdate();
            commandSelector.Items.Clear();
            foreach (var c in _commands)
                commandSelector.Items.Add(c.Name);
            commandSelector.EndUpdate();

            if (commandSelector.Items.Count > 0 && commandSelector.SelectedIndex < 0)
                commandSelector.SelectedIndex = 0;

            UpdateRemoveButtonState();
        }

        private void CommandListBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            var idx = commandListBox.SelectedIndex;
            if (idx >= 0 && idx < _commands.Count)
            {
                commandNameTextBox.Text = _commands[idx].Name;
                commandTextBox.Text = _commands[idx].Template;
                UpdateRemoveButtonState();
            }
        }

        private void CommandListBox_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                DoRemoveSelected();
                e.Handled = true;
            }
        }

        private void UpdateRemoveButtonState()
        {
            rmButton.Enabled = _commands.Count > 0 &&
                               (commandListBox.SelectedIndex >= 0 || commandSelector.SelectedIndex >= 0);
        }

        private void AppendOutput(string text)
        {
            if (outputTextBox.InvokeRequired)
            {
                outputTextBox.BeginInvoke(new Action(() =>
                {
                    outputTextBox.AppendText(text + Environment.NewLine);
                }));
            }
            else
            {
                outputTextBox.AppendText(text + Environment.NewLine);
            }
        }

        #endregion

        #region Add / Remove commands

        private void AddButton_Click(object? sender, EventArgs e)
        {
            var name = (commandNameTextBox.Text ?? "").Trim();
            var template = (commandTextBox.Text ?? "").Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Give the command a name.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(template))
            {
                MessageBox.Show("Enter a command template.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var existing = _commands.FirstOrDefault(c => string.Equals(c.Name, name, StringComparison.OrdinalIgnoreCase));
            if (existing != null)
            {
                existing.Template = template;
            }
            else
            {
                _commands.Add(new CommandEntry { Name = name, Template = template });
            }

            SaveCommands();
            RefreshUI();
        }

        private void RmButton_Click(object? sender, EventArgs e) => DoRemoveSelected();

        private void DoRemoveSelected()
        {
            int idx = commandListBox.SelectedIndex >= 0 ? commandListBox.SelectedIndex : commandSelector.SelectedIndex;

            if (idx < 0 || idx >= _commands.Count)
            {
                MessageBox.Show("Select a command in the list or dropdown to remove.", "Nothing selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var toRemove = _commands[idx];
            var confirm = MessageBox.Show($"Remove \"{toRemove.Name}\"?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            _commands.RemoveAt(idx);
            SaveCommands();
            RefreshUI();

            if (string.Equals(commandNameTextBox.Text.Trim(), toRemove.Name, StringComparison.OrdinalIgnoreCase))
            {
                commandNameTextBox.Clear();
                commandTextBox.Clear();
            }

            AppendOutput($"Removed: {toRemove.Name}");
        }

        #endregion

        #region Drag & drop

        private void OnDragEnter(object? sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void OnDragDrop(object? sender, DragEventArgs e)
        {
            try
            {
                _lastDroppedPaths.Clear();
                if (e.Data == null) return;

                var obj = e.Data.GetData(DataFormats.FileDrop);
                if (obj is string[] paths && paths.Length > 0)
                {
                    foreach (var p in paths)
                    {
                        if (Directory.Exists(p))
                        {
                            var files = Directory.EnumerateFiles(p, "*.*", SearchOption.TopDirectoryOnly)
                                .Where(IsLikelyMedia)
                                .ToList();
                            _lastDroppedPaths.AddRange(files);
                        }
                        else if (File.Exists(p))
                        {
                            _lastDroppedPaths.Add(p);
                        }
                    }

                    label1.Text = _lastDroppedPaths.Count > 0
                        ? $"Ready: {_lastDroppedPaths.Count} item(s)"
                        : "No media files found";
                    AppendOutput($"Dropped {_lastDroppedPaths.Count} item(s).");
                }
            }
            catch (Exception ex)
            {
                AppendOutput($"Drop failed: {ex.Message}");
            }
        }

        private static bool IsLikelyMedia(string path)
        {
            string[] exts = { ".mp4", ".mkv", ".mov", ".avi", ".webm", ".mpg", ".mpeg", ".wmv", ".mp3", ".wav", ".flac", ".m4a", ".aac" };
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return exts.Contains(ext);
        }

        #endregion

        #region Run + templating (including 2-pass)

        // Split a multi-step template by a line that contains only --- (trimmed)
        private static List<string> SplitTemplateIntoSteps(string template)
        {
            var parts = new List<string>();
            var lines = (template ?? "").Replace("\r\n", "\n").Split('\n');
            var current = new List<string>();
            foreach (var raw in lines)
            {
                var line = raw.Trim();
                if (line == "---")
                {
                    if (current.Count > 0) { parts.Add(string.Join("\n", current)); current.Clear(); }
                }
                else
                {
                    current.Add(raw);
                }
            }
            if (current.Count > 0) parts.Add(string.Join("\n", current));
            return parts;
        }

        private async void RunButton_Click(object? sender, EventArgs e)
        {
            if (commandSelector.SelectedIndex < 0 || commandSelector.SelectedIndex >= _commands.Count)
            {
                MessageBox.Show("Pick a command to run.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (_lastDroppedPaths.Count == 0)
            {
                MessageBox.Show("Drag files or a folder into the drop area first.", "Nothing to do", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selected = _commands[commandSelector.SelectedIndex];
            runButton.Enabled = false;

            AppendOutput($"Running \"{selected.Name}\" on {_lastDroppedPaths.Count} item(s)...");

            try
            {
                foreach (var inputPath in _lastDroppedPaths)
                {
                    var steps = SplitTemplateIntoSteps(selected.Template);
                    int code;

                    if (steps.Count <= 1)
                    {
                        var args = BuildArguments(selected.Template, inputPath);
                        AppendOutput($"> ffmpeg {args}");
                        code = await RunFfmpegAsync(args);
                    }
                    else
                    {
                        AppendOutput($"(multi-step: {steps.Count} steps)");
                        code = await RunFfmpegStepsAsync(steps, inputPath);
                    }

                    if (code == 0)
                        AppendOutput($"✔ Done: {Path.GetFileName(inputPath)}");
                    else
                        AppendOutput($"✖ Failed ({code}): {Path.GetFileName(inputPath)}");
                }
            }
            finally
            {
                runButton.Enabled = true;
            }

            AppendOutput("All done.");
        }

        private async Task<int> RunFfmpegStepsAsync(List<string> steps, string inputPath)
        {
            var dir = Path.GetDirectoryName(inputPath) ?? "";
            var name = Path.GetFileNameWithoutExtension(inputPath);
            var passlogBase = Path.Combine(dir, $"{name}_2pass");

            for (int i = 0; i < steps.Count; i++)
            {
                int passNo = i + 1;
                var args = BuildArguments(steps[i], inputPath, passNo, passlogBase);
                AppendOutput($"> ffmpeg {args}");
                int code = await RunFfmpegAsync(args);
                if (code != 0) return code;
            }

            // Clean up pass logs (best effort)
            try
            {
                var baseName = Path.GetFileName(passlogBase);
                var baseDir = Path.GetDirectoryName(passlogBase) ?? dir;
                foreach (var f in Directory.EnumerateFiles(baseDir, baseName + "*"))
                    File.Delete(f);
            }
            catch { }

            return 0;
        }

        private string BuildArguments(string template, string inputPath, int? pass = null, string? passlogBase = null)
        {
            var dir = Path.GetDirectoryName(inputPath) ?? "";
            var name = Path.GetFileNameWithoutExtension(inputPath);
            var ext = Path.GetExtension(inputPath);

            // Allow {output:webm} override, else fall back to DefaultOutputExtension
            string ResolveOutputPath(string tmpl)
            {
                string extOverride = null;
                // find occurrences like {output:xxx}
                int idx = tmpl.IndexOf("{output:", StringComparison.OrdinalIgnoreCase);
                if (idx >= 0)
                {
                    int end = tmpl.IndexOf('}', idx);
                    if (end > idx)
                    {
                        var within = tmpl.Substring(idx + 8, end - (idx + 8)); // after '{output:' up to '}'
                        extOverride = within.Trim().TrimStart('.'); // tolerate leading dot
                    }
                }
                var outExt = string.IsNullOrWhiteSpace(extOverride) ? DefaultOutputExtension : "." + extOverride;
                return Path.Combine(dir, $"{name}_out{outExt}");
            }

            var outputPath = ResolveOutputPath(template);

            string Quote(string s)
            {
                if (string.IsNullOrEmpty(s)) return "\"\"";
                return "\"" + s.Replace("\"", "\\\"") + "\"";
            }

            string args = template
                .Replace("{input}", Quote(inputPath))
                .Replace("{dir}", Quote(dir))
                .Replace("{name}", name)
                .Replace("{ext}", ext)
                .Replace("{output}", Quote(outputPath))
                .Replace("{null}", NullDevice);

            // Replace any {output:ext} form as well
            args = System.Text.RegularExpressions.Regex.Replace(
                args, @"\{output:[^}]+\}", Quote(outputPath));

            if (pass.HasValue)
                args = args.Replace("{pass}", pass.Value.ToString());

            if (!string.IsNullOrWhiteSpace(passlogBase))
                args = args.Replace("{passlog}", Quote(passlogBase));

            return args;
        }

        private Task<int> RunFfmpegAsync(string arguments)
        {
            var tcs = new TaskCompletionSource<int>();

            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = FfmpegPath,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                var proc = new Process { StartInfo = psi, EnableRaisingEvents = true };

                proc.OutputDataReceived += (_, e) => { if (e.Data != null) AppendOutput(e.Data); };
                proc.ErrorDataReceived += (_, e) => { if (e.Data != null) AppendOutput(e.Data); };
                proc.Exited += (_, __) =>
                {
                    tcs.TrySetResult(proc.ExitCode);
                    proc.Dispose();
                };

                if (!proc.Start())
                {
                    AppendOutput("Failed to start ffmpeg.");
                    tcs.TrySetResult(-1);
                }
                else
                {
                    proc.BeginOutputReadLine();
                    proc.BeginErrorReadLine();
                }
            }
            catch (Exception ex)
            {
                AppendOutput($"ffmpeg error: {ex.Message}");
                tcs.TrySetResult(-1);
            }

            return tcs.Task;
        }

        #endregion

        private class CommandEntry
        {
            public string Name { get; set; } = "";
            public string Template { get; set; } = "";
        }
    }
}
