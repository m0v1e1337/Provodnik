using System.Diagnostics;
namespace ConsoleApp1
{
    public delegate void Key(ConsoleKeyInfo key);
    class FileManager
    {
        public event Key KeyPress;
        FilePanel filePanel = new FilePanel();
        public FileManager()
        {
            KeyPress += filePanel.Key;
            filePanel.borders();
            Console.SetCursorPosition(1, 18);
            Console.WriteLine("F6 Создать файл, F7 Создать директорий, F8 Удаление(директории/файла), ESC");
        }
        public void Explorer()
        {
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo userKey = Console.ReadKey(true);
                    switch (userKey.Key)
                    {
                        case ConsoleKey.Enter:
                            MovingInFM();
                            break;
                        case ConsoleKey.F6:
                            CreateFile();
                            break;
                        case ConsoleKey.F7:
                            CreateDirectory();
                            break;
                        case ConsoleKey.F8:
                            Delete();
                            break;
                        case ConsoleKey.DownArrow:
                            KeyPress(userKey);
                            break;
                        case ConsoleKey.UpArrow:
                            KeyPress(userKey);
                            break;
                        case ConsoleKey.Escape:
                            return;
                    }
                }
            }
        }
        private string NewFileName(string message)
        {
            string name;
            Console.CursorVisible = true;
            do
            {
                PositionString(new String(' ', Console.WindowWidth), 0, Console.WindowHeight - 2);
                PositionString(message, 0, Console.WindowHeight - 2);
                name = Console.ReadLine();
            } while (name.Length == 0);
            Console.CursorVisible = false;
            PositionString(new String(' ', Console.WindowWidth), 0, Console.WindowHeight - 2);
            return name;
        }
        private void NameError(string error)
        {
            Console.SetCursorPosition(1, 20);
            Console.Write(error);
            Thread.Sleep(5000);
            Console.SetCursorPosition(1, 20);
            Console.Write("                                     ");
        }
        private void Delete()
        {
            if (filePanel.discs1)
            {
                NameError("Здесь ничего нельзя удалять");
                return;
            }
            FileSystemInfo fileObject = filePanel.GetActiveObject();

            if (fileObject is DirectoryInfo)
            {
                Directory.Delete(Convert.ToString(fileObject), true);
            }
            else
            {
                File.Delete(Convert.ToString(fileObject));
            }
            NewPannels();
            return;
        }
        private void CreateFile()
        {
            if (filePanel.discs1)
            {
                NameError("Здесь нельзя создать файл");
                return;
            }
            string destPath = filePanel.Path;
            string FileName = NewFileName("Укажите имя файла вместе с форматом\nВведите имя файла: ");
            string FileFullName = Path.Combine(destPath, FileName);
            if (!File.Exists(FileFullName))
            {
                File.Create(FileFullName);
            }
            else
            {
                NameError("Файл с таким именем уже сущетсвует");
            }
            NewPannels();
        }
        private void CreateDirectory()
        {
            if (filePanel.discs1)
            {
                NameError("Здесь нельзя создать директорию");
                return;
            }
            string destPath = filePanel.Path;
            string dirName = NewFileName("Введите имя директории: ");
            string dirFullName = Path.Combine(destPath, dirName);
            DirectoryInfo dir = new DirectoryInfo(dirFullName);
            if (!dir.Exists)
            {
                dir.Create();
            }
            else
            {
                NameError("Директория с таким именем уже сущетсвует");
            }
            NewPannels();
        }
        public static void PositionString(string str, int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(str);
        }
        private void NewPannels()
        {
            if (!filePanel.discs1)
            {
                filePanel.UpdateContent(true);
            }
            else if (filePanel == null)
            {
                return;
            }
        }
        private void MovingInFM()
        {
            FileSystemInfo fsInfo = filePanel.GetActiveObject();
            if (fsInfo != null)
            {
                if (fsInfo is DirectoryInfo)
                {
                    Directory.GetDirectories(fsInfo.FullName);
                    filePanel.Path = fsInfo.FullName;
                    filePanel.SetLists();
                    filePanel.Panel();
                }
                else
                {
                    Process.Start(new ProcessStartInfo(((FileInfo)fsInfo).FullName) { UseShellExecute = true });
                }
            }
            else
            {
                string currentPath = filePanel.Path;
                DirectoryInfo currentDirectory = new DirectoryInfo(currentPath);
                DirectoryInfo upLevelDirectory = currentDirectory.Parent;
                if (upLevelDirectory != null)
                {
                    filePanel.Path = upLevelDirectory.FullName;
                    filePanel.SetLists();
                    filePanel.Panel();
                }
                else
                {
                    filePanel.SetDiscs();
                    filePanel.Panel();
                }
            }
        }
    }
}