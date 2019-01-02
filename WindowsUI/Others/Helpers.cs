using System;
using System.Drawing;

namespace WindowsUI
{
	public static class Helpers
    {

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

	    public static string DefaultPath => "pack://application:,,,/Resources/Images/Transparent.png";

    }
}
