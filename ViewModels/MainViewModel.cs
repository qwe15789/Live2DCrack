using HandyControl.Controls;
using Live2DCrack.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;



namespace Live2DCrack.ViewModels
{
    internal class MainViewModel : ObservableObject
    {
        private readonly SoftwareInfo softwareInfo = new();


        public string Path
        {
            get => softwareInfo.Path;
            set => SetProperty(softwareInfo.Path, value, softwareInfo, (obj, val) => obj.Path = val);
        }


        public string Version
        {
            get => softwareInfo.Version;
            set => SetProperty(softwareInfo.Version, value, softwareInfo, (obj, val) => obj.Version = val);
        }

        public ICommand WriteResourceCommand { get; }
        public ICommand OpenFolderSelectoryCommand { get; }
        public ICommand CopyDownloadUrlCommand { get; }

        public MainViewModel()
        {
            WriteResourceCommand = new RelayCommand(WriteResource);
            OpenFolderSelectoryCommand = new RelayCommand(OpenFolderSelector);
            CopyDownloadUrlCommand = new RelayCommand(CopyDownloadUrl);
            Path = Utils.GetLive2DInstallPath();
            if (string.Empty != Path)
            {
                if (Utils.GetLive2DVersion(softwareInfo.Path, out string version))
                {
                    Version = version;
                }

            }
            else
            {
                Path = "获取失败";
                Version = "未知";
                Growl.Error("没有获取到软件安装目录,如果确定安装了Live2D请尝试关闭杀毒软件或者管理员模式运行");
            }
        }

        public void WriteResource()
        {
            if (Directory.Exists(Path))
            {
                if (Utils.WriteResource(Path + @"\app\lib\rlm1221.jar"))
                {
                    Growl.Success("成功");
                }
                else
                {
                    Growl.Error("失败,请尝试关闭杀毒软件或者管理员模式运行");
                }
            }
            else
            {
                Growl.Error("没有获取到软件安装目录");
            }
        }

        private void OpenFolderSelector()
        {
            using (System.Windows.Forms.FolderBrowserDialog dialog = new())
            {
                dialog.Description = "选择Live2D安装目录";
                dialog.AutoUpgradeEnabled = true;
                var result = dialog.ShowDialog();
                if (System.Windows.Forms.DialogResult.OK == result)
                {
                    if (Directory.Exists(dialog.SelectedPath))
                    {
                        if (Utils.GetLive2DVersion(dialog.SelectedPath, out string version))
                        {
                            Path = dialog.SelectedPath;
                            Version = version;
                        }
                        else
                        {
                            Growl.Error("未获取到Live2D版本,可能是选择的目录不正确");
                        }

                    }

                }
            }

        }

        public void CopyDownloadUrl()
        {
            try
            {
                Clipboard.SetDataObject($"https://cubism.live2d.com/editor/bin/Live2D_Cubism_Setup_4.1.04_zh.exe");
                Growl.Success("复制成功,浏览器下载慢请用迅雷下载");
            }
            catch (Exception e)
            {
                Growl.Success($"复制失败{e.Message}");
            }

        }

    }
}
