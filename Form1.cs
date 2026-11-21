using System.Diagnostics;

namespace NemesisUI
{
    public partial class Form1 : Form
    {
        private const string LoaderPlaceholder = "put ur nemesis loader path here (.exe included)";
        private static readonly string NemesisScriptPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "nemesis", "execution.txt");

        public Form1()
        {
            InitializeComponent();
            richTextBox1_TextChanged(this, EventArgs.Empty);
            richTextBox2_TextChanged(this, EventArgs.Empty);
        }

        private void button3_Click(object sender, EventArgs e) // start roblox
        {
            if (!TryGetLoaderPath(out string loader))
            {
                return;
            }

            try
            {
                Process.Start(new ProcessStartInfo(loader)
                {
                    WorkingDirectory = Path.GetDirectoryName(loader),
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not start the loader: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e) // execute the script
        {
            if (richTextBox1.Text.Trim().Length == 0)
            {
                MessageBox.Show("Enter a script in the script box first.",
                    "Nothing to execute", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                string folder = Path.GetDirectoryName(NemesisScriptPath)!;
                Directory.CreateDirectory(folder);
                File.WriteAllText(NemesisScriptPath, richTextBox1.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not write the script: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e) // loader path is inputed here
        {
            string path = NormalizePath(richTextBox2.Text);
            richTextBox2.ForeColor = path.Length > 0 && File.Exists(path)
                ? Color.Green
                : SystemColors.WindowText;
        }

        private void button2_Click(object sender, EventArgs e) // clear script textbox
        {
            richTextBox1.Clear();
            richTextBox1.Focus();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e) // scripts are inputed here
        {
            button1.Enabled = richTextBox1.Text.Trim().Length > 0;
        }

        private bool TryGetLoaderPath(out string path)
        {
            path = NormalizePath(richTextBox2.Text);

            if (path.Length == 0)
            {
                MessageBox.Show("Enter the path to your loader .exe first.",
                    "No loader selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (!File.Exists(path))
            {
                MessageBox.Show("Loader not found:\n" + path,
                    "Invalid path", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private static string NormalizePath(string text)
        {
            string path = text.Trim();

            if (path.Length >= 2 && path[0] == '"' && path[^1] == '"')
            {
                path = path[1..^1].Trim();
            }

            return path == LoaderPlaceholder ? string.Empty : path;
        }
    }
}
