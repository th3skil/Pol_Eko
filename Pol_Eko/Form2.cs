using System;
using System.IO;
using System.Windows.Forms;

namespace Pol_Eko
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            InitializeFormComponents(); // Inicjalizacja komponentów formularza
        }

        private void InitializeFormComponents()
        {
            // Ustawienie właściwości dla ListBox
            listBoxFiles.AllowDrop = true;

            // Dodanie obsługi zdarzeń DragEnter i DragDrop
            listBoxFiles.DragEnter += ListBoxFiles_DragEnter;
            listBoxFiles.DragDrop += ListBoxFiles_DragDrop;

            // Dodanie przycisku do backupu
            buttonBackup.Click += ButtonBackup_Click;
        }

        private void textBox1_TextChanged_1()
        {
        }

        private void ListBoxFiles_DragEnter(object sender, DragEventArgs e)
        {
            // Obsługa przeciągnięcia plików i folderów do listy
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void ListBoxFiles_DragDrop(object sender, DragEventArgs e)
        {
            // Dodawanie plików lub folderów przeciągniętych na ListBox
            string[] items = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string item in items)
            {
                if (Directory.Exists(item))
                {
                    // Jeśli przeciągnięty element jest folderem, dodajemy jego zawartość do ListBox
                    AddFolderContentsToList(item);
                }
                else if (File.Exists(item))
                {
                    // Jeśli przeciągnięty element jest plikiem, dodajemy FileItem do ListBox
                    listBoxFiles.Items.Add(new FileItem { FileName = Path.GetFileName(item), FullPath = item });
                }
            }
        }

        private void AddFolderContentsToList(string folderPath)
        {
            // Dodawanie zawartości folderu (plików i podfolderów) do ListBox
            string[] files = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                // Dodajemy FileItem do ListBox
                listBoxFiles.Items.Add(new FileItem { FileName = Path.GetFileName(file), FullPath = file });
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Dodawanie pliku lub folderu za pomocą OpenFileDialog lub FolderBrowserDialog
            using (var dialog = new OpenFileDialog())
            {
                dialog.Multiselect = true;
                dialog.Filter = "All Files (*.*)|*.*";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (string fileName in dialog.FileNames)
                    {
                        // Dodajemy FileItem do ListBox
                        listBoxFiles.Items.Add(new FileItem { FileName = Path.GetFileName(fileName), FullPath = fileName });
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Usuwanie zaznaczonych plików z ListBox
            while (listBoxFiles.SelectedItems.Count > 0)
            {
                listBoxFiles.Items.Remove(listBoxFiles.SelectedItems[0]);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Wyczyszczenie wszystkich elementów z ListBox
            listBoxFiles.Items.Clear();
        }

        private void ButtonBackup_Click(object sender, EventArgs e)
        {
            // Utworzenie kopii zapasowej wszystkich plików i folderów
            if (listBoxFiles.Items.Count == 0)
            {
                MessageBox.Show("The list is empty. Please add files or folders to the list.", "No Files or Folders", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string backupFolder = folderDialog.SelectedPath;
                    string dateTimeString = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                    string backupPath = Path.Combine(backupFolder, $"Backup_{dateTimeString}");

                    Directory.CreateDirectory(backupPath);

                    foreach (FileItem item in listBoxFiles.Items)
                    {
                        if (File.Exists(item.FullPath))
                        {
                            string destFile = Path.Combine(backupPath, item.FileName);
                            File.Copy(item.FullPath, destFile, true);
                        }
                        else if (Directory.Exists(item.FullPath))
                        {
                            string destFolder = Path.Combine(backupPath, item.FileName);
                            CopyDirectory(item.FullPath, destFolder);
                        }
                    }

                    MessageBox.Show($"Backup created successfully at {backupPath}.", "Backup Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void CopyDirectory(string sourceDir, string destinationDir)
        {
            Directory.CreateDirectory(destinationDir);

            foreach (var file in Directory.GetFiles(sourceDir))
            {
                string destFile = Path.Combine(destinationDir, Path.GetFileName(file));
                File.Copy(file, destFile, true);
            }

            foreach (var directory in Directory.GetDirectories(sourceDir))
            {
                string destDirectory = Path.Combine(destinationDir, Path.GetFileName(directory));
                CopyDirectory(directory, destDirectory);
            }
        }
    }

    public class FileItem
    {
        public string FileName { get; set; }
        public string FullPath { get; set; }

        public override string ToString()
        {
            return FileName;
        }
    }
}
