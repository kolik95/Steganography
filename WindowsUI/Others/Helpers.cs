using System;
using System.Drawing;
using System.Windows;
using WindowsUI.ViewModels;
using Microsoft.Win32;

namespace WindowsUI.Others
{
	public static class Helpers
    {

		private static OpenFileDialog fileDialog { get; set; }

	    public static void Initialize()
	    {
			fileDialog = new OpenFileDialog();

		    fileDialog.Multiselect = false;

		    fileDialog.CheckFileExists = true;

			fileDialog.Title = "Otevřít obrázek";

		}

	    public static bool IsImage(string path)
	    {

		    try
		    {

			    Image.FromFile(path);

		    }
		    catch (Exception e)
		    {

			    return false;

		    }

		    return true;

	    }

	    public static bool CheckResolution(ref string path)
	    {

		    Bitmap image = (Bitmap)Image.FromFile(path);
		    if (image.Height * image.Width <= 2160 * 3840) return true;
		    return false;

	    }

	    public static string GetImage()
	    {  

			fileDialog.ShowDialog(MainWindowViewModel.GetInstance.AppWindow);

		    if (!IsImage(fileDialog.FileName)) return "";
		    return fileDialog.FileName;

	    }

	    public static string Drop(ref object sender, ref DragEventArgs e)
	    {

		    var paths = (string[])e.Data.GetData(DataFormats.FileDrop);

		    if (paths.Length == 0) return "";
		    if (!IsImage(paths[0])) return "";
		    return paths[0];

	    }
	}
}
