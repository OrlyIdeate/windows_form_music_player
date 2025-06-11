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
        private IWavePlayer waveOut;
        private AudioFileReader audioFileReader;
        private Timer seekTimer;
        private bool isPlaying;
        private bool isSeeking;
        private AudioFilesInfo lastPlayedInfo;
        private bool suppressAutoPlay;
        public enum RepeatMode { None, One, All }
        private RepeatMode repeatMode = RepeatMode.None;

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
            using (OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "音声ファイル|*.wav;*.mp3|すべてのファイル|*.*",
                Title = "音声ファイルを選択してください",
                Multiselect = true
            })
            {
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
            if (IsFileAlreadyInList(filePath))
            {
                MessageBox.Show($"{Path.GetFileName(filePath)}はすでにリストに追加されています。");
                return;
            }

            var info = AudioUtils.CreateAudioFilesInfo(filePath);
            var item = new ListViewItem(new[] { info.FileName, info.Date, info.Type, info.Size })
            {
                Tag = info
            };
            listViewFiles.Items.Add(item);
        }

        private bool IsFileAlreadyInList(string filePath)
        {
            foreach (ListViewItem item in listViewFiles.Items)
            {
                var info = item.Tag as AudioFilesInfo;
                if (info != null && string.Equals(info.FilePath, filePath, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
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
            if (!isPlaying) StartPlayback(sender);
            else StopPlayback(sender);
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
            if (info == null) return;

            string path = info.FilePath;
            if (!IsValidAudioFormat(path))
            {
                MessageBox.Show("WAVまたはMP3ファイルを選択してください。");
                return;
            }

            try
            {
                SetupAudio(path, info);
                waveOut.Play();
                isPlaying = true;
                SetPlayButtonText(sender, "停止");
                InitSeekBar();
                StartSeekTimer();

                waveOut.PlaybackStopped += (s, args) => HandlePlaybackStopped(sender);
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
            SaveCurrentPosition();
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

        // --- 再生関連のヘルパー ---
        private bool IsValidAudioFormat(string path)
        {
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return (ext == ".wav" || ext == ".mp3");
        }

        private void SetupAudio(string filePath, AudioFilesInfo info)
        {
            audioFileReader = new AudioFileReader(filePath);
            totalTime.Text = audioFileReader.TotalTime.ToString(@"\/mm\:ss");
            title.Text = info.FileName;

            waveOut = new WaveOutEvent();
            waveOut.Init(audioFileReader);

            // 前回の続きから再生
            if (ShouldResumeLastPosition(info, audioFileReader))
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
        }

        private bool ShouldResumeLastPosition(AudioFilesInfo info, AudioFileReader reader)
        {
            return lastPlayedInfo == info
                && info.LastPositionSeconds > 0
                && info.LastPositionSeconds < reader.TotalTime.TotalSeconds;
        }

        private void HandlePlaybackStopped(object sender)
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

            var currentItem = listViewFiles.SelectedItems.Count > 0 ? listViewFiles.SelectedItems[0] : null;

            // 1曲リピート
            if (repeatMode == RepeatMode.One && currentItem != null)
            {
                StartPlayback(sender);
                return;
            }

            // 次の曲へ or 全曲リピート
            var nextItem = currentItem != null ? GetNextListViewItem(currentItem) : null;
            if (nextItem != null)
            {
                listViewFiles.SelectedItems.Clear();
                nextItem.Selected = true;
                listViewFiles.Select();
                StartPlayback(sender);
            }
            else if (repeatMode == RepeatMode.All && listViewFiles.Items.Count > 0)
            {
                var firstItems = listViewFiles.Items[0];
                listViewFiles.SelectedItems.Clear();
                firstItems.Selected = true;
                listViewFiles.Select();
                StartPlayback(sender);
            }
        }

        private void SaveCurrentPosition()
        {
            if (listViewFiles.SelectedItems.Count > 0 && audioFileReader != null)
            {
                var info = listViewFiles.SelectedItems[0].Tag as AudioFilesInfo;
                if (info != null) info.LastPositionSeconds = audioFileReader.CurrentTime.TotalSeconds;
            }
        }

        private void DisposeAudio()
        {
            waveOut?.Dispose();
            audioFileReader?.Dispose();
            waveOut = null;
            audioFileReader = null;
        }

        private void StartSeekTimer()
        {
            if (seekTimer == null)
            {
                seekTimer = new Timer { Interval = 500 };
                seekTimer.Tick += SeekTimer_Tick;
            }
            seekTimer.Start();
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
                var pos = (int)audioFileReader.CurrentTime.TotalSeconds;
                if (trackBarSeek.Minimum <= pos && pos <= trackBarSeek.Maximum)
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
            var idx = currentItem.Index;
            if (idx < listViewFiles.Items.Count - 1)
            {
                return listViewFiles.Items[idx + 1];
            }
            return null;
        }

        // --- リピート変更 ---
        private void radioRepeatNone_CheckedChanged(object sender, EventArgs e)
        {
            if (radioRepeatNone.Checked) repeatMode = RepeatMode.None;
        }

        private void radioRepeatOne_CheckedChanged(object sender, EventArgs e)
        {
            if (radioRepeatOne.Checked) repeatMode = RepeatMode.One;
        }

        private void radioRepeatAll_CheckedChanged(object sender, EventArgs e)
        {
            if (radioRepeatAll.Checked) repeatMode = RepeatMode.All;
        }
    }
}
