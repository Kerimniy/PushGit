using KHA331;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Management.Automation;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PushGit
{
    public partial class MainWindow : Window
    {
        private static string FolderPath = @"d:/testdir";
        private static string Branch = "main";
        private static string Commit = "Commit N";
        private static string gitURL = "https://github.com/Kerimniy/test2.git";
        private static short Num=0;
        private static bool tStatus=false ;
        private static bool autoPushStatus=false ;
        private static bool asyncStatus=true;
        private static int lateTime;
        FileSystemWatcher watcher;
        private bool isPushing = false;
        BitmapImage arr = new BitmapImage();
        char[] forbiddenChars = new char[] { '<', '>', '"', '{', '}', '|', '^' };

        public MainWindow()
        {
            InitializeComponent();
            textBox.IsReadOnly = true;
            outputFolderTextBox.IsReadOnly = true;
            arr.BeginInit();
            arr.UriSource = new Uri("pack://application:,,,/arr48.png", UriKind.Absolute);
            arr.EndInit();
            try
            {
                string[] saves = ReadConf();
                Num = Convert.ToInt16(saves[5]);
                gitURL = saves[3];
                FolderPath = saves[1];
                inputUrl.Text = gitURL;
                tStatus = Convert.ToBoolean(saves[7]);
                asyncStatus = Convert.ToBoolean(saves[9]);
                autoPushStatus = Convert.ToBoolean(saves[13]);
                Branch = saves[11];
                Commit = saves[15];
                if (tStatus)
                {
                    ImageBrush brush = new ImageBrush(arr);
                    brush.Stretch = Stretch.Uniform;
                    toggleButtonForce.Fill = brush;
                }
                else
                {
                    toggleButtonForce.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(31, 31, 31));

                }
                if (asyncStatus)
                {
                    ImageBrush brush = new ImageBrush(arr);
                    brush.Stretch = Stretch.Uniform;
                    toggleButtonAsync.Fill = brush;
                }
                else
                {
                    toggleButtonAsync.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(31, 31, 31));

                }


            }
            catch (Exception e)
            {

                Num = 0;
                gitURL = "";
                FolderPath = "";
                inputUrl.Text = gitURL;
                tStatus = false;
                autoPushStatus = false;
                asyncStatus = true;
                Branch = "main";
                Commit = $"Commit N{Num}";
            }
            inpuBranchName.Text = Branch;
            outputFolderTextBox.Text = FolderPath;
            inputCommit.Text = Commit;
            if (autoPushStatus)
            {
                ImageBrush brush = new ImageBrush(arr);
                brush.Stretch = Stretch.Uniform;
                toggleButtonAutoPush.Fill = brush;
            }
            else
            {
                toggleButtonAutoPush.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(31, 31, 31));

            }
            try { 
                watcher = new FileSystemWatcher(FolderPath)
                {
                    NotifyFilter = NotifyFilters.Attributes
                                     | NotifyFilters.CreationTime
                                     | NotifyFilters.DirectoryName
                                     | NotifyFilters.FileName
                                     | NotifyFilters.LastAccess
                                     | NotifyFilters.LastWrite
                                     | NotifyFilters.Security
                                     | NotifyFilters.Size,
                    IncludeSubdirectories = true,
                    InternalBufferSize = 262144
                };
                watcher.EnableRaisingEvents = true;
                watcher.Error += (s, e) => System.Windows.Forms.MessageBox.Show($"Error: {e.GetException().Message}");
                addChange(autoPushStatus);

            }
            catch (Exception ex)
            {
                if (autoPushStatus)
                {
                    System.Windows.Forms.MessageBox.Show("Folder Watcher Error, perhaps folder path is incorrect ", "Error");
                }
            }
            this.Closing += MainWindow_Closing;

            PrintHello();
 

        }

        private async void PrintHello()
        {
            try
            {
                await Task.Run(() =>
                {
                    Task.Delay(600).Wait();

                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {
                        textBox.Text += "\n======================================================================";
                    });
                    Task.Delay(300).Wait();
                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {
                        textBox.Text += "\n==============================| PushGit v0.1.0 |=============================";
                    });
                    Task.Delay(300).Wait();
                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {
                        textBox.Text += "\n======================================================================";
                        textBox.Text += "\n";
                        textBox.Text += "\n";
                    });

                });
            }
            catch (Exception ex)
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    textBox.Text += $"Fatal Execution Error: {ex.Message}";

                });
                Task.Delay(1000).Wait();
            }
        
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            CreateConf();
        }

        private void SelectFolder(object sender, MouseButtonEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.ShowNewFolderButton = true;
            dialog.Description = "Select your repository";
            dialog.AddToRecent = true;


            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FolderPath = dialog.SelectedPath;
                outputFolderTextBox.Text = FolderPath;

                try
                {
                    watcher = new FileSystemWatcher(FolderPath)
                    {
                        NotifyFilter = NotifyFilters.Attributes
                                    | NotifyFilters.CreationTime
                                    | NotifyFilters.DirectoryName
                                    | NotifyFilters.FileName
                                    | NotifyFilters.LastAccess
                                    | NotifyFilters.LastWrite
                                    | NotifyFilters.Security
                                    | NotifyFilters.Size,
                        IncludeSubdirectories = true,
                        InternalBufferSize = 262144
                    };
                    watcher.EnableRaisingEvents = true;
                    addChange(autoPushStatus);
                    watcher.Error += (s, e) => System.Windows.Forms.MessageBox.Show($"Error: {e.GetException().Message}");

                }
                catch (Exception ex)
                {
                    if (autoPushStatus)
                    {
                        System.Windows.Forms.MessageBox.Show("Folder Watcher Error, perhaps folder path is incorrect ", "Error");
                    }
                }
            }
            

        }

        private void Start()
        {
            isPushing = true;
            watcher.EnableRaisingEvents = false;
            Cursor = System.Windows.Input.Cursors.AppStarting;
            gitURL = inputUrl.Text;
            if (IsCorrectUrl(gitURL))
            {
                Num++;
                using (PowerShell powershell = PowerShell.Create())
                {
                    if (tStatus)
                    {
                        powershell.AddScript($"cd {FolderPath}");
                        powershell.AddScript(@"git init");
                        powershell.AddScript(@"git add *");
                        powershell.AddScript($@"git commit -m '{Commit}'");
                        powershell.AddScript($@"git config --global --unset url.git@github.com:.insteadOf");
                        powershell.AddScript($@"git remote rm origin");
                        powershell.AddScript($@"git remote add origin {gitURL}");
                        powershell.AddScript($@"git branch -M {Branch}");
                        powershell.AddScript($@"git push origin {Branch}  -f");
                    }
                    else
                    {
                        powershell.AddScript($"cd {FolderPath}");
                        powershell.AddScript(@"git init");
                        powershell.AddScript(@"git add *");
                        powershell.AddScript($@"git commit -m '{Commit}'");
                        powershell.AddScript($@"git config --global --unset url.git@github.com:.insteadOf");
                        powershell.AddScript($@"git remote rm origin");
                        powershell.AddScript($@"git remote add origin {gitURL}");
                        powershell.AddScript($@"git branch -M  {Branch}");
                        powershell.AddScript($@"git fetch origin {Branch}");
                        powershell.AddScript($@"git pull --rebase origin {Branch}");
                        powershell.AddScript($@"git push origin {Branch}");
                    }

                    Collection<PSObject> results = powershell.Invoke();


                    textBox.Text += "\n======================================================================";
                    textBox.Text += "\n==============================| Output (Start) |=============================";
                    textBox.Text += "\n=====================================================================";
                    textBox.Text += "\n";
                    textBox.Text += "\n";


                    if (results.Count > 0)
                    {
                        foreach (PSObject result in results)
                        {
                            textBox.Text += result?.ToString() ?? "No result";
                        }
                    }
                    else
                    {

                    }

                    List<string> logs = new List<string>();
                    List<string> errors = new List<string>();

                    if (powershell.Streams.Error.Count > 0)
                    {
                        foreach (var error in powershell.Streams.Error)
                        {
                            if (error.ToString().Contains("fatal"))
                            {
                                errors.Add($"\nError: {error.ToString()}");
                            }
                            else
                            {
                                logs.Add($"\nLog: {error.ToString()}");
                            }
                        }
                    }
                    else
                    {
                    }

                    if (logs.Count > 0)
                    {
                        textBox.Text += "\n======================================================================";
                        textBox.Text += "\n=================================| Logs |=================================";
                        textBox.Text += "\n======================================================================";
                        textBox.Text += "\n";
                        textBox.Text += "\n";


                    }
                    foreach (string log in logs)
                    {
                        textBox.Text += log;
                    }

                    if (errors.Count > 0)
                    {
                        textBox.Text += "\n======================================================================";
                        textBox.Text += "\n================================| Errors |================================";
                        textBox.Text += "\n======================================================================";
                        textBox.Text += "\n";
                        textBox.Text += "\n";

                    }
                    foreach (string err in errors)
                    {
                        textBox.Text += err;
                    }

                    textBox.Text += "\n======================================================================";
                    textBox.Text += "\n=============================| Output  (End)  |=============================";
                    textBox.Text += "\n======================================================================";
                }

            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Incorrect URL", "Message");
                gitURL = "";
            }
            Cursor = System.Windows.Input.Cursors.Arrow;
            isPushing = false;
            watcher.EnableRaisingEvents = true;
        }

        private async void Start_Async()
        {
            Cursor = System.Windows.Input.Cursors.AppStarting;
            try
            {
                isPushing = true;

                watcher.EnableRaisingEvents = false;

                await Task.Run(() =>
                {

                    string gitURL = "";
                    string _FolderPath = "";
                    short _Num=0;
                    string _Branch="";
                    Dispatcher.Invoke(() => gitURL = inputUrl.Text);
                    Dispatcher.Invoke(() => _FolderPath = FolderPath);
                    Dispatcher.Invoke(() => _Num = Num);
                    Dispatcher.Invoke(() => _Branch = Branch);
                    bool cu = (gitURL.StartsWith("https://") || gitURL.StartsWith("http://")) && (gitURL.StartsWith("http://") ? gitURL.IndexOf(".", 7) != -1 : gitURL.IndexOf(".", 8) != -1)&& (gitURL.StartsWith("http://") ? gitURL.Length > gitURL.IndexOf(".", 7) + 2 : gitURL.Length > gitURL.IndexOf(".", 8) + 2);
                    if (cu)
                    {
                        Num++;
                        using (PowerShell powershell = PowerShell.Create())
                        {
                            if (tStatus)
                            {
                                powershell.AddScript($"cd {_FolderPath}");
                                powershell.AddScript(@"git init");
                                powershell.AddScript(@"git add *");
                                powershell.AddScript($@"git commit -m '{Commit}'");
                                powershell.AddScript($@"git config --global --unset url.git@github.com:.insteadOf");
                                powershell.AddScript($@"git remote rm origin");
                                powershell.AddScript($@"git remote add origin {gitURL}");
                                powershell.AddScript($@"git branch -M {Branch}");
                                powershell.AddScript($@"git push origin {Branch}  -f");
                            }
                            else
                            {
                                powershell.AddScript($"cd {_FolderPath}");
                                powershell.AddScript(@"git init");
                                powershell.AddScript(@"git add *");
                                powershell.AddScript($@"git commit -m '{Commit}'");
                                powershell.AddScript($@"git config --global --unset url.git@github.com:.insteadOf");
                                powershell.AddScript($@"git remote rm origin");
                                powershell.AddScript($@"git remote add origin {gitURL}");
                                powershell.AddScript($@"git branch -M  {Branch}");
                                powershell.AddScript($@"git fetch origin {Branch}");
                                powershell.AddScript($@"git pull --rebase origin {Branch}");
                                powershell.AddScript($@"git push origin {Branch}");
                            }



                            Collection<PSObject> results = powershell.Invoke();

                            System.Windows.Application.Current.Dispatcher.Invoke(() =>
                            {
                                textBox.Text += "\n";
                                textBox.Text += "\n";
                                textBox.Text += "\n======================================================================";
                                textBox.Text += "\n==============================| Output (Start) |=============================";
                                textBox.Text += "\n=====================================================================";
                                textBox.Text += "\n";
                                textBox.Text += "\n";
                            });
                            Task.Delay(100).Wait();

                            

                            if (results.Count > 0)
                            {
                                foreach (PSObject result in results)
                                {
                                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                                    {
                                        textBox.Text += result?.ToString() ?? "No result";
                                    });
                                    Task.Delay(100).Wait();
                                    
                                }
                            }
                            else
                            {

                            }

                            List<string> logs = new List<string>();
                            List<string> errors = new List<string>();

                            if (powershell.Streams.Error.Count > 0)
                            {
                                foreach (var error in powershell.Streams.Error)
                                {
                                    if (error.ToString().Contains("fatal"))
                                    {
                                        errors.Add($"\nError: {error.ToString()}");
                                    }
                                    else
                                    {
                                        logs.Add($"\nLog: {error.ToString()}");
                                    }
                                }
                            }
                            else
                            {
                            }

                            if (logs.Count > 0)
                            {
                                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                                {
                                    textBox.Text += "\n";
                                    textBox.Text += "\n";
                                    textBox.Text += "\n======================================================================";
                                    textBox.Text += "\n=================================| Logs |=================================";
                                    textBox.Text += "\n======================================================================";
                                    textBox.Text += "\n";
                                    textBox.Text += "\n";
                                });
                                Task.Delay(100).Wait();
                                


                            }
                            foreach (string log in logs)
                            {
                                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                                {
                                    textBox.Text += log;

                                });
                                Task.Delay(100).Wait();
                            }

                            if (errors.Count > 0)
                            {
                                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                                {
                                    textBox.Text += "\n";
                                    textBox.Text += "\n";
                                    textBox.Text += "\n======================================================================";
                                    textBox.Text += "\n================================| Errors |================================";
                                    textBox.Text += "\n======================================================================";
                                    textBox.Text += "\n";
                                    textBox.Text += "\n";
                                });
                                Task.Delay(100).Wait();
                                

                            }
                            foreach (string err in errors)
                            {
                                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                                {
                                    textBox.Text += err;

                                });
                                Task.Delay(100).Wait();
                            }
                            System.Windows.Application.Current.Dispatcher.Invoke(() =>
                            {
                                textBox.Text += "\n";
                                textBox.Text += "\n";
                                textBox.Text += "\n======================================================================";
                                textBox.Text += "\n=============================| Output  (End)  |=============================";
                                textBox.Text += "\n======================================================================";
                                textBox.Text += "\n";
                                textBox.Text += "\n";
                            });
                            Task.Delay(100).Wait();
                            
                        }

                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Incorrect URL", "Message");
                        gitURL = "";
                    }

                });
            }
            catch (Exception ex)
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    textBox.Text += $"Fatal Execution Error: {ex.Message}";

                });
                Task.Delay(1000).Wait();
            }
            finally
            {
                Cursor = System.Windows.Input.Cursors.Arrow;
                isPushing = false;
                watcher.EnableRaisingEvents = true;
            }
        }

        private void CreateConf()
        {
            string path = System.AppDomain.CurrentDomain.BaseDirectory + "conf.ksf";
            gitURL = inputUrl.Text;
            Branch = inpuBranchName.Text;
            Commit = inputCommit.Text;

            FileInfo sFile = new FileInfo(path);
            FileStream fs = sFile.Create();
            byte[] savebytes = Cryptography.EnCrypt($"folder {FolderPath} url {gitURL} num {Num} fEnabled {tStatus} asyncSt {asyncStatus} branchName {Branch} autoSt {autoPushStatus} commit {Commit}",1709);
            byte[] data = savebytes;
            fs.Write(data, 0, data.Length);
            
            fs.Close();
            
        }

        private string[] ReadConf()
        {
            string path = System.AppDomain.CurrentDomain.BaseDirectory + "conf.ksf";
            byte[] readBytes = File.ReadAllBytes(path);
            string readStr = Cryptography.DeCrypt(readBytes, 1709);
            
            string[] values = readStr.Split();
            string ns = "";
            return values;
        }

        bool IsCorrectUrl(string url)
        {
            return (url.StartsWith("https://") || url.StartsWith("http://"))
                && (url.StartsWith("http://") ? url.IndexOf(".", 7) != -1 : url.IndexOf(".", 8) != -1)
                && (url.StartsWith("http://") ? url.Length > url.IndexOf(".", 7) + 2 : url.Length > url.IndexOf(".", 8) + 2)
                && !ContainsSymbol(forbiddenChars, url);

           
        }

        bool ContainsSymbol(char[] forbiddenChars, string url)
        {
            bool res = false;
            for (int i = 0; i < forbiddenChars.Length; i++)
            {
                res = res || url.Contains(forbiddenChars[i]);
            }
            return res;
        }

        private void tbForce_click(object sender, MouseButtonEventArgs e)
        {
            
            tStatus = !tStatus;
            if (tStatus)
            {
                ImageBrush brush = new ImageBrush(arr);
                brush.Stretch = Stretch.Uniform;
                toggleButtonForce.Fill = brush;
            }
            else
            {
                toggleButtonForce.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(31, 31, 31));
            
            }
        }

        private void tbAsync_click(object sender, MouseButtonEventArgs e)
        {

            asyncStatus = !asyncStatus;
            if (asyncStatus)
            {
                ImageBrush brush = new ImageBrush(arr);
                brush.Stretch = Stretch.Uniform;
                toggleButtonAsync.Fill = brush;
            }
            else
            {
                toggleButtonAsync.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(31, 31, 31));

            }
        }

        private void tbHoverE(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var btn = (System.Windows.Shapes.Rectangle)sender;

            string type = e.RoutedEvent.ToString();
            if (type == "Mouse.MouseEnter")
            {
                btn.Opacity = 0.75;
            }
            else
            {
                btn.Opacity = 1;
            }

        }
        private void PickFolderHoverE(object sender, System.Windows.Input.MouseEventArgs e)
        {
            string type = e.RoutedEvent.ToString();
            if (type == "Mouse.MouseEnter")
            {
                pickFolder.Opacity = 0.75;
            }
            else
            {
                pickFolder.Opacity = 1;
            }

        }

        private void Push(object sender, RoutedEventArgs e)
        {
            Commit = inputCommit.Text;
            Branch = inpuBranchName.Text;
            textBox.Text += "\n======================================================================";
            textBox.Text += "\n==============================| Trying to Push |=============================";
            textBox.Text += "\n=====================================================================";
            textBox.Text += "\n";
            textBox.Text += "\n";

            if(Commit==""|| Commit == " ")
            {
                Commit = $"Commit N{Num}";
            }

            if (Branch=="" || Branch==" ")
            {
                Branch = "main";
            }
            
            
            if (asyncStatus)
            {
                Start_Async();
            }
            else
            {
                Start();
            }
            
        }

        private void autoPush_click(object sender, MouseButtonEventArgs e)
        {
            
            autoPushStatus = !autoPushStatus;
            if (autoPushStatus)
            {
                ImageBrush brush = new ImageBrush(arr);
                brush.Stretch = Stretch.Uniform;
                toggleButtonAutoPush.Fill = brush;
            }
            else
            {
                toggleButtonAutoPush.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(31, 31, 31));

            }
            addChange(autoPushStatus);
        }
    
        private void autoPush(object sender, FileSystemEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                Commit = inputCommit.Text;
                Branch = inpuBranchName.Text;

                if (Commit == "" || Commit == " ")
                {
                    Commit = $"Commit N{Num}";
                }

                textBox.Text += "\n======================================================================";
                textBox.Text += "\n==============================| Trying to Push |=============================";
                textBox.Text += "\n=====================================================================";
                textBox.Text += "\n";
                textBox.Text += "\n";

                string dt = (DateTime.Now.ToString("hh:mm:ss"));
                dt = dt.Replace(":", "");
                int bdt = Convert.ToInt32(dt);

                if (bdt - lateTime > 0.3)
                {
                    lateTime = bdt;
                    Branch = inpuBranchName.Text;
                    if (Branch == "" || Branch == " ")
                    {
                        Branch = "main";
                    }
                    if (!isPushing)
                    {
                        if (asyncStatus)
                        {
                            Start_Async();
                        }
                        else
                        {
                            Start();
                        }
                    }
                    

                }

            });
        }
        
        private void addChange(bool st)
        {
            try
            {
                if (st)
                {

                    watcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;

                    watcher.Changed -= new FileSystemEventHandler(autoPush);
                    watcher.Created -= new FileSystemEventHandler(autoPush);
                    watcher.Deleted -= new FileSystemEventHandler(autoPush);
                    watcher.Renamed -= new RenamedEventHandler(autoPush);


                    watcher.Changed += new FileSystemEventHandler(autoPush);
                    watcher.Created += new FileSystemEventHandler(autoPush);
                    watcher.Deleted += new FileSystemEventHandler(autoPush);
                    watcher.Renamed += new RenamedEventHandler(autoPush);

                    //     watcher.Filter = "*.txt";
                    watcher.IncludeSubdirectories = true;
                    watcher.EnableRaisingEvents = true;

                }
                else
                {

                    watcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;


                    watcher.Changed -= new FileSystemEventHandler(autoPush);
                    watcher.Created -= new FileSystemEventHandler(autoPush);
                    watcher.Deleted -= new FileSystemEventHandler(autoPush);
                    watcher.Renamed -= new RenamedEventHandler(autoPush);

                    //     watcher.Filter = "*.txt";
                    watcher.IncludeSubdirectories = true;
                    watcher.EnableRaisingEvents = false;
                }
            }
            catch (Exception ex) {

                System.Windows.Forms.MessageBox.Show(ex.Message, "Error");
            }
        }

    }


}