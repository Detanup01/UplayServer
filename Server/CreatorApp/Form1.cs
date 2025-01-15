using Google.Protobuf;
using ServerCore.Extra;

namespace Creator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //manifest
        private void button3_Click(object sender, EventArgs e)
        {
            var sp = serverpath.Text;
            var manifest = manifestbox.Text;
            var prod = productbox.Text;
            if (CheckIfEmtpyOrNull(sp, "Server Path cannot be empty!"))
                return;
            if (CheckIfEmtpyOrNull(manifest, "Manifest Id cannot be empty!"))
                return;
            if (CheckIfEmtpyOrNull(prod, "Product Id cannot be empty!"))
                return;

            var savepath = $"{sp}/Download/{prod}/manifests";
            Directory.CreateDirectory(savepath);
            Creators.MakeManifest(ToManifest.Manifest, $"{savepath}/{manifest}.manifest");
        }

        // file
        private void button1_Click(object sender, EventArgs e)
        {
            var sp = serverpath.Text;
            if (compressionbox.SelectedItem == null)
                return;
            var compression = compressionbox.SelectedItem.ToString()!;
            var manifest = manifestbox.Text;
            var prod = productbox.Text;
            var slver = sliceverbox.Text;
            var maxsiz = maxsize.Text;

            if (CheckIfEmtpyOrNull(sp, "Server Path cannot be empty!"))
                return;
            if (CheckIfEmtpyOrNull(compression, "Compression cannot be empty!"))
                return;
            if (CheckIfEmtpyOrNull(manifest, "Manifest Id cannot be empty!"))
                return;
            if (CheckIfEmtpyOrNull(prod, "Product Id cannot be empty!"))
                return;
            if (CheckIfEmtpyOrNull(slver, "Slice Version cannot be empty!"))
                return;
            if (CheckIfEmtpyOrNull(sp, "Server Path cannot be empty!"))
                return;
            if (CheckIfEmtpyOrNull(maxsiz, "Max Size cannot be empty!"))
                return;

            openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var path = openFileDialog1.FileName;
                var pathy = Path.GetDirectoryName(path)!;
                if (CheckIfEmtpyOrNull(path, "File cannot be empty!"))
                    return;

                var index = chunkbox.SelectedIndex;
                if (index == -1)
                {
                    index = 0;
                }
                ToManifest.WorkFile(path, maxsiz, compression, prod, sp, slver, pathy, index);
            }
        }

        // folder
        private void button2_Click(object sender, EventArgs e)
        {
            var sp = serverpath.Text;
            if (compressionbox.SelectedItem == null)
                return;
            var compression = compressionbox.SelectedItem.ToString()!;
            var manifest = manifestbox.Text;
            var prod = productbox.Text;
            var slver = sliceverbox.Text;
            var maxsiz = maxsize.Text;

            if (CheckIfEmtpyOrNull(sp, "Server Path cannot be empty!"))
                return;
            if (CheckIfEmtpyOrNull(compression, "Compression cannot be empty!"))
                return;
            if (CheckIfEmtpyOrNull(manifest, "Manifest Id cannot be empty!"))
                return;
            if (CheckIfEmtpyOrNull(prod, "Product Id cannot be empty!"))
                return;
            if (CheckIfEmtpyOrNull(slver, "Slice Version cannot be empty!"))
                return;
            if (CheckIfEmtpyOrNull(sp, "Server Path cannot be empty!"))
                return;
            if (CheckIfEmtpyOrNull(maxsiz, "Max Size cannot be empty!"))
                return;

            folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                var path = folderBrowserDialog1.SelectedPath;

                var files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);

                if (files.Length == 0)
                {
                    MessageBox.Show("Folder path must contain files!");
                    return;
                }
                var index = chunkbox.SelectedIndex;
                if (index == -1)
                {
                    index = 0;
                }
                foreach (var file in files)
                {

                    ToManifest.WorkFile(file, maxsiz, compression, prod, sp, slver, path, index);
                }

            }
        }

        // load mbytes
        private void button4_Click(object sender, EventArgs e)
        {
            openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var path = openFileDialog1.FileName;

                if (CheckIfEmtpyOrNull(path, "File cannot be empty!"))
                    return;
                var filebytes = File.ReadAllBytes(path);
                ToManifest.Manifest.MergeFrom(filebytes);
            }
        }

        // make json
        private void button5_Click(object sender, EventArgs e)
        {
            File.WriteAllText("manifest.json", ToManifest.Manifest.ToString());
            MemoryStream ms = new();
            ToManifest.Manifest.WriteTo(ms);
            File.WriteAllBytes("manifest.mbytes", ms.ToArray());
        }


        // clean manifest
        private void button6_Click(object sender, EventArgs e)
        {
            var prod = productbox.Text;
            if (CheckIfEmtpyOrNull(prod, "Product Id cannot be empty!"))
                return;
            ToManifest.MakeNewManifest(prod);
        }

        private bool CheckIfEmtpyOrNull(string? thisstuff, string error)
        {
            if (string.IsNullOrEmpty(thisstuff))
            {
                MessageBox.Show(error);
                return true;
            }
            return false;
        }

        //add
        private void button8_Click(object sender, EventArgs e)
        {
            var prod = productbox.Text;
            if (CheckIfEmtpyOrNull(prod, "Product Id cannot be empty!"))
                return;
            var box = chunktextbox.Text;
            if (CheckIfEmtpyOrNull(box, "Chunk Text box cannot be empty"))
                return;

            chunkbox.Items.Add(box);
            ToManifest.Manifest.Chunks.Add(new Uplay.Download.Chunk()
            {
                Files = { },
                Id = (uint)(chunkbox.Items.Count - 1),
                Type = Uplay.Download.Chunk.Types.ChunkType.Optional,
                UplayId = uint.Parse(prod)
            });
        }

        //remove
        private void button7_Click(object sender, EventArgs e)
        {
            var index = chunkbox.SelectedIndex;

            if (index == -1 || index == 0)
            {
                MessageBox.Show("Cannot remove Basic");
                return;
            }
            chunkbox.Items.RemoveAt(index);
            ToManifest.Manifest.Chunks.RemoveAt(index);
        }
    }
}