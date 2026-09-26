using System;
using System.IO;
using System.Web;

public class UploadClass
{
    private HttpFileCollection files;
    public int FileCount { get; private set; }
    public int OverwriteMode { get; set; }  // 0 = Don't overwrite, 1 = Overwrite if exists

    public UploadClass()
    {
        files = HttpContext.Current.Request.Files;
        FileCount = files.Count;
        OverwriteMode = 0;
    }

    /// <summary>
    /// Reads uploaded files from the current HTTP request
    /// </summary>
    public void ReadUpload()
    {
        files = HttpContext.Current.Request.Files;
        FileCount = files.Count;
    }

    /// <summary>
    /// Returns the name of the file at the given index
    /// </summary>
    public string FileName(int index)
    {
        if (index < 0 || index >= FileCount)
            throw new IndexOutOfRangeException("Invalid file index.");

        return Path.GetFileName(files[index].FileName);
    }

    /// <summary>
    /// Returns the file size in bytes
    /// </summary>
    public long FileSize(int index)
    {
        if (index < 0 || index >= FileCount)
            throw new IndexOutOfRangeException("Invalid file index.");

        return files[index].ContentLength;
    }

    /// <summary>
    /// Saves uploaded file to the specified full path
    /// </summary>
    public void SaveFile(int index, string fullPath)
    {
        if (index < 0 || index >= FileCount)
            throw new IndexOutOfRangeException("Invalid file index.");

        if (string.IsNullOrEmpty(fullPath))
            throw new ArgumentException("Full path cannot be null or empty.");

        string dir = Path.GetDirectoryName(fullPath);
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        if (File.Exists(fullPath))
        {
            if (OverwriteMode == 1)
            {
                File.Delete(fullPath);
            }
            else
            {
                // Don't overwrite, rename
                string newName = Path.GetFileNameWithoutExtension(fullPath)
                               + "_" + DateTime.Now.ToString("HHmmssfff")
                               + Path.GetExtension(fullPath);
                fullPath = Path.Combine(dir, newName);
            }
        }

        files[index].SaveAs(fullPath);
    }
}
