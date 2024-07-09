using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.BZip2;
using SharpCompress.Compressors.BZip2;

namespace Pol_Eko
{
    public partial class Form1 : Form
    {
        public string selectedFolderPath; // Dodatkowa zmienna przechowuj¹ca wybran¹ œcie¿kê

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                DialogResult result = dialog.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.SelectedPath))
                {
                    selectedFolderPath = dialog.SelectedPath; // Przechowujemy wybran¹ œcie¿kê
                    GatherDirectoryInfo(selectedFolderPath);
                }
            }
        }

        private void GatherDirectoryInfo(string selectedPath)
        {
            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(selectedPath);

                if (directoryInfo.Exists)
                {
                    textBoxContent.Clear();
                    textBoxContent.AppendText($"Name: {directoryInfo.Name}{Environment.NewLine}");
                    textBoxContent.AppendText($"Date: {directoryInfo.CreationTime}{Environment.NewLine}");
                    textBoxContent.AppendText($"{Environment.NewLine}");

                    // Clear CheckedListBox
                    checkedListBox1.Items.Clear();

                    FileInfo[] files = directoryInfo.GetFiles();
                    foreach (var file in files)
                    {
                        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.Name);
                        textBoxContent.AppendText($"Type: {fileNameWithoutExtension}{Environment.NewLine}");
                        textBoxContent.AppendText($"Filename: {file.Name}{Environment.NewLine}");

                        if (Path.GetExtension(file.Name).Equals(".bz2", StringComparison.OrdinalIgnoreCase))
                        {
                            long realSize = Main(file.FullName);
                            textBoxContent.AppendText($"Size (uncompressed): {realSize} bytes{Environment.NewLine}");
                        }
                        else
                        {
                            textBoxContent.AppendText($"Size: {file.Length} bytes{Environment.NewLine}");
                        }

                        textBoxContent.AppendText($"{Environment.NewLine}");

                        // Add file names to CheckedListBox
                        checkedListBox1.Items.Add(file.Name);
                    }

                    DirectoryInfo[] directories = directoryInfo.GetDirectories();
                    foreach (var dir in directories)
                    {
                        textBoxContent.AppendText($"Folder: {dir.Name}{Environment.NewLine}");
                        textBoxContent.AppendText($"Size: (folder){Environment.NewLine}");
                        textBoxContent.AppendText($"{Environment.NewLine}");
                    }
                }
                else
                {
                    textBoxContent.Text = "Folder does not exist.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        // Nowa wersja funkcji Main, która zastêpuje funkcjê GetBZip2UncompressedSize
        private long GetBZip2UncompressedSize(string bzip2FilePath)
        {
            long totalUncompressedSize = 0;

            try
            {
                using (var fileStream = new FileStream(bzip2FilePath, FileMode.Open, FileAccess.Read))
                using (var bzip2Stream = new BZip2InputStream(fileStream))
                {
                    byte[] buffer = new byte[8192];
                    int bytesRead;
                    while ((bytesRead = bzip2Stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        totalUncompressedSize += bytesRead;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error calculating BZip2 uncompressed size: {ex.Message}");
            }

            return totalUncompressedSize;
        }
        private long Main(string bzip2FilePath)
        {
            long totalUncompressedSize = 0;

            try
            {
                var compressedDataByteArray = File.ReadAllBytes(bzip2FilePath);

                using (var mstream = new MemoryStream(compressedDataByteArray))
                using (var unzipstream = new BZip2Stream(mstream, SharpCompress.Compressors.CompressionMode.Decompress, true))
                {
                    byte[] buffer = new byte[4096];
                    int bytesRead;
                    while ((bytesRead = unzipstream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        totalUncompressedSize += bytesRead;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error calculating BZip2 uncompressed size: {ex.Message}");
            }

            return totalUncompressedSize;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(selectedFolderPath))
            {
                try
                {
                    BackupData backupData = new BackupData
                    {
                        Name = $"TES_BACKUP_{DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss")}",
                        Date = DateTime.Now,
                        Objects = new List<BackupObject>()
                    };

                    DirectoryInfo directoryInfo = new DirectoryInfo(selectedFolderPath); // Uzyskujemy dostêp do directoryInfo

                    for (int i = 0; i < checkedListBox1.Items.Count; i++)
                    {
                        string fileName = checkedListBox1.Items[i].ToString();
                        FileInfo file = new FileInfo(Path.Combine(selectedFolderPath, fileName));

                        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.Name);

                        BackupObject backupObject = new BackupObject
                        {
                            type = fileNameWithoutExtension,
                            filename = file.Name,
                            nominalSize = 0, // Placeholder for now
                            size = file.Extension.Equals(".bz2", StringComparison.OrdinalIgnoreCase)
                                ? Main(file.FullName)  // U¿ywamy nowej funkcji Main
                                : file.Length
                        };

                        backupData.Objects.Add(backupObject);
                    }

                    // Serializacja do JSON
                    string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(backupData, Newtonsoft.Json.Formatting.Indented);

                    // Zapisz JSON bezpoœrednio do wybranego folderu jako "Manifest.json"
                    string jsonFilePath = Path.Combine(selectedFolderPath, "Manifest.json");
                    File.WriteAllText(jsonFilePath, jsonData);

                    MessageBox.Show($"Data saved to JSON successfully at {jsonFilePath}.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving data to JSON: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Please select a folder first.");
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            // Utwórz nowe okno
            Form2 form2 = new Form2();

            // Ukryj bie¿¹ce okno
            this.Hide();

            // Poka¿ nowe okno
            form2.ShowDialog();

            // Po zamkniêciu nowego okna, poka¿ ponownie stare okno
            this.Show();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }
    }

    public class BackupData
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public List<BackupObject> Objects { get; set; }
    }

    public class BackupObject
    {
        public string type { get; set; }
        public string filename { get; set; }
        public long nominalSize { get; set; }
        public long size { get; set; }
    }
}
