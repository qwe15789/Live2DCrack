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
        public ICommand CopyDownloadUrlCommand { get; }

        public MainViewModel()
        {
            WriteResourceCommand = new RelayCommand(WriteResource);
            CopyDownloadUrlCommand = new RelayCommand(CopyDownloadUrl);
            Path = Utils.GetLive2DInstallPath();
            if (string.Empty != Path)
            {
                Version = Utils.GetLive2DVersion(softwareInfo.Path);
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
                if(Utils.WriteResource(Path + @"\app\lib\rlm1221.jar"))
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
