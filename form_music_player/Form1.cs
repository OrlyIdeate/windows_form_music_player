using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Windows.Forms;
using NAudio.Wave;
using System.Linq;

namespace 上野迅_インターン_20250513
{
    public partial class Form1 : Form
    {
        // --- フィールド ---
        private IWavePlayer waveOut = null;
        private AudioFileReader audioFileReader = null;
        private Timer seekTimer = null;
        private bool isPlaying = false;
        private bool isSeeking = false;
        private AudioFilesInfo lastPlayedInfo = null;
        private bool suppressAutoPlay = false;

        // --- コンストラクタ ---
        public Form1()
        {
            InitializeComponent();
            InitializeDragDrop();
        }

        // --- 初期化 ---
        private void InitializeDragDrop()
        {
            panelDrop.AllowDrop = true;
            panelDrop.DragEnter += panelDrop_DragEnter;
            panelDrop.DragDrop += panelDrop_DragDrop;
        }

        // --- フォームイベント ---
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadList();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveList();
        }

        // --- ドラッグ＆ドロップ ---
        private void panelDrop_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop)
                ? DragDropEffects.Copy
                : DragDropEffects.None;
        }

        private void panelDrop_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            foreach (var filePath in files)
            {
                AddFileToListView(filePath);
            }
        }

        // --- ファイル参照 ---
        private void referance(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "音声ファイル|*.wav;*.mp3|すべてのファイル|*.*";
                ofd.Title = "音声ファイルを選択してください";
                ofd.Multiselect = true;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    foreach (var filePath in ofd.FileNames)
                    {
                        AddFileToListView(filePath);
                    }
                }
            }
        }

        // --- ListView操作 ---
        private void AddFileToListView(string filePath)
        {
            // 既存のファイルがリストにあるかチェック
            foreach (ListViewItem item in listViewFiles.Items)
            {
                var info = item.Tag as AudioFilesInfo;
                if (info != null && string.Equals(info.FilePath, filePath, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show($"{info.FileName}はすでにリストに追加されています。");
                    return;
                }
            }

            var newinfo = AudioUtils.CreateAudioFilesInfo(filePath);
            var newitem = new ListViewItem(new[] { newinfo.FileName, newinfo.Date, newinfo.Type, newinfo.Size })
            {
                Tag = newinfo
            };
            listViewFiles.Items.Add(newitem);
        }

        private void delete_Click(object sender, EventArgs e)
        {
            if (listViewFiles.SelectedItems.Count == 0)
            {
                MessageBox.Show("Listから選択してください");
                return;
            }
            var result = MessageBox.Show("本当に削除しますか？", "リストから削除", MessageBoxButtons.YesNoCancel);
            if (result != DialogResult.Yes) return;

            foreach (ListViewItem item in listViewFiles.SelectedItems)
            {
                listViewFiles.Items.Remove(item);
            }
        }

        // --- 再生・停止 ---
        private void play_Click(object sender, EventArgs e)
        {
            if (!isPlaying)
            {
                StartPlayback(sender);
            }
            else
            {
                StopPlayback(sender);
            }
        }

        private void StartPlayback(object sender)
        {
            if (listViewFiles.SelectedItems.Count == 0)
            {
                MessageBox.Show("Listから選択してください");
                return;
            }

            var item = listViewFiles.SelectedItems[0];
            var info = item.Tag as AudioFilesInfo;
            string filePath = info?.FilePath;
            string ext = Path.GetExtension(filePath).ToLower();

            if (ext != ".wav" && ext != ".mp3")
            {
                MessageBox.Show("WAVまたはMP3ファイルを選択してください。");
                return;
            }

            try
            {
                audioFileReader = new AudioFileReader(filePath);
                totalTime.Text = audioFileReader.TotalTime.ToString(@"\/mm\:ss");
                title.Text = info.FileName;
                waveOut = new WaveOutEvent();
                waveOut.Init(audioFileReader);

                // 続きから再生（同じファイルの場合のみ）
                if (lastPlayedInfo == info && info.LastPositionSeconds > 0 && info.LastPositionSeconds < audioFileReader.TotalTime.TotalSeconds)
                {
                    audioFileReader.CurrentTime = TimeSpan.FromSeconds(info.LastPositionSeconds);
                    trackBarSeek.Value = (int)info.LastPositionSeconds;
                    currentTime.Text = audioFileReader.CurrentTime.ToString(@"mm\:ss");
                }
                else
                {
                    info.LastPositionSeconds = 0;
                    trackBarSeek.Value = 0;
                    currentTime.Text = "00:00";
                }

                waveOut.Play();
                isPlaying = true;
                SetPlayButtonText(sender, "停止");
                InitSeekBar();

                if (seekTimer == null)
                {
                    seekTimer = new Timer { Interval = 500 };
                    seekTimer.Tick += SeekTimer_Tick;
                }
                seekTimer.Start();

                waveOut.PlaybackStopped += (s, args) =>
                {
                    isPlaying = false;
                    SetPlayButtonText(sender, "再生");
                    seekTimer?.Stop();
                    DisposeAudio();

                    if (suppressAutoPlay)
                    {
                        suppressAutoPlay = false;
                        return;
                    }

                    // --- 自動で次の曲を再生 ---
                    var currentItem = listViewFiles.SelectedItems.Count > 0 ? listViewFiles.SelectedItems[0] : null;
                    var nextItem = currentItem != null ? GetNextListViewItem(currentItem) : null;
                    if (nextItem != null)
                    {
                        listViewFiles.SelectedItems.Clear();
                        nextItem.Selected = true;
                        listViewFiles.Select();
                        StartPlayback(sender);
                    }
                };

                lastPlayedInfo = info;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"再生できませんでした：\n{ex.Message}");
            }
        }

        private void StopPlayback(object sender)
        {
            suppressAutoPlay = true;
            // 停止時に再生位置を記録
            if (listViewFiles.SelectedItems.Count > 0)
            {
                var item = listViewFiles.SelectedItems[0];
                var info = item.Tag as AudioFilesInfo;
                if (audioFileReader != null)
                {
                    info.LastPositionSeconds = audioFileReader.CurrentTime.TotalSeconds;
                }
            }
            waveOut?.Stop();
            isPlaying = false;
            SetPlayButtonText(sender, "再生");
            seekTimer?.Stop();
        }

        private void SetPlayButtonText(object sender, string text)
        {
            if (sender is Button btn)
            {
                if (btn.InvokeRequired)
                    btn.Invoke((Action)(() => btn.Text = text));
                else
                    btn.Text = text;
            }
        }

        private void DisposeAudio()
        {
            waveOut?.Dispose();
            audioFileReader?.Dispose();
            waveOut = null;
            audioFileReader = null;
        }

        // --- シークバー ---
        private void InitSeekBar()
        {
            trackBarSeek.Minimum = 0;
            trackBarSeek.Maximum = (int)audioFileReader.TotalTime.TotalSeconds;
        }

        private void SeekTimer_Tick(object sender, EventArgs e)
        {
            if (audioFileReader != null && !isSeeking)
            {
                int pos = (int)audioFileReader.CurrentTime.TotalSeconds;
                if (pos >= trackBarSeek.Minimum && pos <= trackBarSeek.Maximum)
                {
                    trackBarSeek.Value = pos;
                }
                currentTime.Text = audioFileReader.CurrentTime.ToString(@"mm\:ss");
            }
        }

        private void trackBarSeek_MouseDown(object sender, MouseEventArgs e)
        {
            isSeeking = true;
        }

        private void trackBarSeek_MouseUp(object sender, MouseEventArgs e)
        {
            if (audioFileReader != null)
            {
                audioFileReader.CurrentTime = TimeSpan.FromSeconds(trackBarSeek.Value);
            }
            isSeeking = false;
        }

        // --- リスト保存・読込 ---
        private void SaveList()
        {
            var list = listViewFiles.Items.Cast<ListViewItem>()
                .Select(item => item.Tag as AudioFilesInfo)
                .Where(info => info != null)
                .ToList();

            AudioUtils.SaveListToFile("audiofiles.json", list);
        }

        private void LoadList()
        {
            var list = AudioUtils.LoadListFromFile("audiofiles.json");
            listViewFiles.Items.Clear();
            foreach (var info in list)
            {
                var item = new ListViewItem(new[] { info.FileName, info.Date, info.Type, info.Size })
                {
                    Tag = info
                };
                listViewFiles.Items.Add(item);
            }
        }

        // --- 次の曲を取得 ---
        private ListViewItem GetNextListViewItem(ListViewItem currentItem)
        {
            int idx = currentItem.Index;
            if (idx < listViewFiles.Items.Count - 1)
            {
                return listViewFiles.Items[idx + 1];
            }
            return null;
        }
    }
}
