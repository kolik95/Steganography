using System;
using System.Drawing;

namespace WindowsUI.Others
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

    }
}
