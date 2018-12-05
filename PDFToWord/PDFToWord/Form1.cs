using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace PDFToWord
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }
        Convertors convert = new Convertors();
        string strExtention = ".pdf";

        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "当前时间：" + DateTime.Now;//实时显示当前系统时间
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            if(folder.ShowDialog()==DialogResult.OK)
            {
                listView1.Items.Clear();//清空文件列表
                textBox1.Text = folder.SelectedPath;//记录选择路径
                DirectoryInfo dir = new DirectoryInfo(textBox1.Text);
                FileSystemInfo[] files = dir.GetFiles();//获取文件夹中所有文件
                foreach (FileInfo file in files)//遍历所有文件
                {
                    if (file.Extension.ToLower() == ".doc" || file.Extension.ToLower() == ".docx")//如果是Word文件
                    {
                        listView1.Items.Add(file.FullName);//显示文件列表
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            if (folder.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = folder.SelectedPath;//记录选择路径
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count > 0 && textBox2.Text != "")
            {
                listView2.Items.Clear();//清空文件列表
                System.Threading.ThreadPool.QueueUserWorkItem(//使用线程池
                         (P_temp) =>
                         {
                             button4.Enabled = false;
                             foreach (ListViewItem item in listView1.Items)
                             {
                                 FileInfo finfo = new FileInfo(item.Text);
                                 string fileName = finfo.Name;
                                 string otherFile = textBox2.Text.TrimEnd(new char[] { '\\' }) + "\\" + fileName.Substring(0, fileName.LastIndexOf('.')) + strExtention;
                                 convert.WordConversion(item.Text, otherFile, comboBox1.SelectedItem.ToString());
                                 listView2.Items.Add(new ListViewItem(otherFile));
                             }
                             MessageBox.Show("文档格式转换完成，快去使用吧！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                             button4.Enabled = true;
                         });
            }
            else
            {
                MessageBox.Show("请确认存在要转换的Word文档列表和转换后的文件存放路径！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(comboBox1.SelectedItem.ToString())
            {
                case "PDF":
                    strExtention = ".pdf";
                    break;
                case "PNG":
                    strExtention = ".png";
                    break;
                case "RTF":
                    strExtention = ".rtf";
                    break;
                case "HTML":
                    strExtention = ".html";
                    break;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = comboBox2.SelectedIndex = 0;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            if (folder.ShowDialog() == DialogResult.OK)
            {
                listView1.Items.Clear();//清空文件列表
                textBox4.Text = folder.SelectedPath;//记录选择路径
                DirectoryInfo dir = new DirectoryInfo(textBox4.Text);
                FileSystemInfo[] files = dir.GetFiles();//获取文件夹中所有文件
                foreach (FileInfo file in files)//遍历所有文件
                {
                    if (file.Extension.ToLower() == ".pdf")//如果是PDF文件
                    {
                        listView3.Items.Add(file.FullName);//显示文件列表
                    }
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            if (folder.ShowDialog() == DialogResult.OK)
            {
                textBox3.Text = folder.SelectedPath;//记录选择路径
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox2.SelectedItem.ToString())
            {
                case "WORD":
                    strExtention = ".doc";
                    break;
                case "PNG":
                    strExtention = ".png";
                    break;
                case "HTML":
                    strExtention = ".html";
                    break;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listView3.Items.Count > 0 && textBox3.Text != "")
            {
                listView4.Items.Clear();//清空文件列表
                System.Threading.ThreadPool.QueueUserWorkItem(//使用线程池
                         (P_temp) =>
                         {
                             button3.Enabled = false;
                             foreach (ListViewItem item in listView3.Items)
                             {
                                 FileInfo finfo = new FileInfo(item.Text);
                                 string fileName = finfo.Name;
                                 string otherFile = textBox3.Text.TrimEnd(new char[] { '\\' }) + "\\" + fileName.Substring(0, fileName.LastIndexOf('.')) + strExtention;
                                 convert.PDFConversion(item.Text, otherFile, comboBox2.SelectedItem.ToString());
                                 listView4.Items.Add(new ListViewItem(otherFile));
                             }
                             MessageBox.Show("文档格式转换完成，快去使用吧！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                             button3.Enabled = true;
                         });
            }
            else
            {
                MessageBox.Show("请确认存在要转换的Word文档列表和转换后的文件存放路径！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
                strExtention = ".pdf";
            else if(tabControl1.SelectedIndex == 1)
                strExtention = ".doc";
        }

        private void listView2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView2.SelectedIndices.Count > 0)
                System.Diagnostics.Process.Start(listView2.SelectedItems[0].Text);
        }

        private void listView4_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView4.SelectedIndices.Count > 0)
                System.Diagnostics.Process.Start(listView4.SelectedItems[0].Text);
        }
    }
}
