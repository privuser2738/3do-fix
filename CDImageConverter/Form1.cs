using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CDImageConverter
{
    public partial class Form1 : Form
    {
        private string _chdmanPath = @"C:\Users\rob\Games\3DO\MAME\chdman.exe";
        private string _outputFolder = @"C:\Users\rob\Games\3DO";
        private string _configFile = "converter.config";

        public Form1()
        {
            InitializeComponent();
            LoadConfig();
            UpdateUI();
        }

        private void LoadConfig()
        {
            if (File.Exists(_configFile))
            {
                var lines = File.ReadAllLines(_configFile);
                if (lines.Length > 0) _outputFolder = lines[0];
                if (lines.Length > 1) _chdmanPath = lines[1];
            }
        }

        private void SaveConfig()
        {
            File.WriteAllLines(_configFile, new[] { _outputFolder, _chdmanPath });
        }

        private void UpdateUI()
        {
            lblOutputFolder.Text = $"Output: {_outputFolder}";
            lblChdmanPath.Text = $"chdman: {_chdmanPath}";
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "CD Images (*.chd;*.iso;*.cue)|*.chd;*.iso;*.cue|All Files (*.*)|*.*";
                ofd.Title = "Select CD Image File";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtInputFile.Text = ofd.FileName;
                }
            }
        }

        private void btnSelectOutput_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Select Output Folder";
                fbd.SelectedPath = _outputFolder;

                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    _outputFolder = fbd.SelectedPath;
                    SaveConfig();
                    UpdateUI();
                }
            }
        }

        private void btnSelectChdman_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "chdman.exe|chdman.exe|All Files (*.*)|*.*";
                ofd.Title = "Select chdman.exe";
                ofd.FileName = "chdman.exe";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _chdmanPath = ofd.FileName;
                    SaveConfig();
                    UpdateUI();
                }
            }
        }

        private async void btnConvert_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtInputFile.Text))
            {
                MessageBox.Show("Please select an input file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!File.Exists(txtInputFile.Text))
            {
                MessageBox.Show("Input file does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!File.Exists(_chdmanPath))
            {
                MessageBox.Show($"chdman.exe not found at:\n{_chdmanPath}\n\nPlease configure the correct path.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string inputFile = txtInputFile.Text;
            string ext = Path.GetExtension(inputFile).ToLower();

            if (ext != ".chd" && ext != ".iso")
            {
                MessageBox.Show("Only .chd and .iso files are supported for conversion.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Generate output filenames
            string baseName = Path.GetFileNameWithoutExtension(inputFile);
            string outputCue = Path.Combine(_outputFolder, baseName + ".cue");
            string outputBin = Path.Combine(_outputFolder, baseName + ".bin");

            // Check if files already exist
            if (File.Exists(outputCue) || File.Exists(outputBin))
            {
                var result = MessageBox.Show(
                    $"Output files already exist:\n{outputCue}\n{outputBin}\n\nOverwrite?",
                    "Confirm Overwrite",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.No)
                    return;
            }

            // Disable UI during conversion
            btnConvert.Enabled = false;
            btnSelectFile.Enabled = false;
            progressBar.Style = ProgressBarStyle.Marquee;
            txtLog.AppendText($"Starting conversion: {Path.GetFileName(inputFile)}\r\n");

            try
            {
                if (ext == ".chd")
                {
                    await ConvertCHD(inputFile, outputCue, outputBin);
                }
                else if (ext == ".iso")
                {
                    await ConvertISO(inputFile, outputBin);
                    CreateCueFile(outputCue, outputBin);
                }

                txtLog.AppendText("Conversion complete!\r\n");
                txtLog.AppendText($"Created: {outputCue}\r\n");
                txtLog.AppendText($"Created: {outputBin}\r\n\r\n");

                MessageBox.Show($"Conversion successful!\n\nOutput files:\n{outputCue}\n{outputBin}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                txtLog.AppendText($"ERROR: {ex.Message}\r\n\r\n");
                MessageBox.Show($"Conversion failed:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnConvert.Enabled = true;
                btnSelectFile.Enabled = true;
                progressBar.Style = ProgressBarStyle.Blocks;
            }
        }

        private async Task ConvertCHD(string inputFile, string outputCue, string outputBin)
        {
            txtLog.AppendText($"Extracting CHD using chdman...\r\n");

            var psi = new ProcessStartInfo
            {
                FileName = _chdmanPath,
                Arguments = $"extractcd -i \"{inputFile}\" -o \"{outputCue}\" -ob \"{outputBin}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using (var process = Process.Start(psi))
            {
                if (process == null)
                    throw new Exception("Failed to start chdman.exe");

                string output = await process.StandardOutput.ReadToEndAsync();
                string error = await process.StandardError.ReadToEndAsync();

                await process.WaitForExitAsync();

                if (process.ExitCode != 0)
                {
                    throw new Exception($"chdman failed with exit code {process.ExitCode}\n{error}");
                }

                txtLog.AppendText(output);
            }
        }

        private async Task ConvertISO(string inputFile, string outputBin)
        {
            txtLog.AppendText($"Converting ISO to BIN...\r\n");

            // ISO files are already in BIN format, just copy
            await Task.Run(() => File.Copy(inputFile, outputBin, true));

            txtLog.AppendText($"ISO copied to BIN format.\r\n");
        }

        private void CreateCueFile(string cueFile, string binFile)
        {
            string binFileName = Path.GetFileName(binFile);
            string cueContent = $"FILE \"{binFileName}\" BINARY\r\n  TRACK 01 MODE1/2352\r\n    INDEX 01 00:00:00\r\n";

            File.WriteAllText(cueFile, cueContent);
            txtLog.AppendText($"CUE file created.\r\n");
        }
    }
}
