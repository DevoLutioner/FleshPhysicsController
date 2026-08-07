using System;
using System.Runtime.InteropServices;

namespace ThighPhysicsController;

/// <summary>
/// Minimal Win32 file dialog wrapper (comdlg32 GetSaveFileNameW / GetOpenFileNameW).
/// The game does not ship System.Windows.Forms, so the standard dialogs are called
/// directly via P/Invoke. This lets users save/load presets to any external file.
/// </summary>
internal static class WindowsFileDialog
{
    private const int OfnPathMustExist = 0x00000800;
    private const int OfnFileMustExist = 0x00001000;
    private const int OfnOverwritePrompt = 0x00000002;
    private const int OfnHideReadOnly = 0x00000004;
    private const int OfnExplorer = 0x00080000;

    private const int MaxFileBuffer = 4096;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    private struct OpenFileName
    {
        public int lStructSize;
        public IntPtr hwndOwner;
        public IntPtr hInstance;
        public string lpstrFilter;
        public string lpstrCustomFilter;
        public int nMaxCustFilter;
        public int nFilterIndex;
        public IntPtr lpstrFile;
        public int nMaxFile;
        public string lpstrFileTitle;
        public int nMaxFileTitle;
        public string lpstrInitialDir;
        public string lpstrTitle;
        public int Flags;
        public short nFileOffset;
        public short nFileExtension;
        public string lpstrDefExt;
        public IntPtr lCustData;
        public IntPtr lpfnHook;
        public string lpTemplateName;
        public IntPtr pvReserved;
        public int dwReserved;
        public int flagsEx;
    }

    [DllImport("comdlg32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern bool GetSaveFileNameW(ref OpenFileName ofn);

    [DllImport("comdlg32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern bool GetOpenFileNameW(ref OpenFileName ofn);

    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    public static bool ShowSave(string initialDirectory, string defaultFileName, out string selectedPath)
    {
        return ShowDialog(isSave: true, initialDirectory, defaultFileName, out selectedPath);
    }

    public static bool ShowOpen(string initialDirectory, out string selectedPath)
    {
        return ShowDialog(isSave: false, initialDirectory, null, out selectedPath);
    }

    private static bool ShowDialog(bool isSave, string initialDirectory, string defaultFileName,
        out string selectedPath)
    {
        selectedPath = null;
        OpenFileName ofn = new OpenFileName();
        ofn.lStructSize = Marshal.SizeOf(typeof(OpenFileName));
        ofn.hwndOwner = GetActiveWindow();
        ofn.lpstrFilter = "Flesh physics preset (*.xml)\0*.xml\0All files (*.*)\0*.*\0";
        ofn.nFilterIndex = 1;
        ofn.lpstrInitialDir = initialDirectory;
        ofn.lpstrTitle = isSave ? "Save flesh physics preset" : "Load flesh physics preset";
        ofn.Flags = OfnExplorer | OfnHideReadOnly | OfnPathMustExist |
                    (isSave ? OfnOverwritePrompt : OfnFileMustExist);
        ofn.lpstrDefExt = isSave ? "xml" : null;

        IntPtr buffer = Marshal.AllocHGlobal(MaxFileBuffer * 2);
        try
        {
            Marshal.WriteInt16(buffer, 0, 0);
            if (!string.IsNullOrEmpty(defaultFileName))
            {
                char[] chars = defaultFileName.ToCharArray();
                Marshal.Copy(chars, 0, buffer, chars.Length);
            }
            ofn.lpstrFile = buffer;
            ofn.nMaxFile = MaxFileBuffer;

            bool ok = isSave
                ? GetSaveFileNameW(ref ofn)
                : GetOpenFileNameW(ref ofn);
            if (!ok)
            {
                return false;
            }
            string path = Marshal.PtrToStringUni(buffer);
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }
            selectedPath = path;
            return true;
        }
        finally
        {
            Marshal.FreeHGlobal(buffer);
        }
    }
}
