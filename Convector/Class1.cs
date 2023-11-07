using System.Text;
namespace ConsoleApp1
{
    class FilePanel
    {
        List<FileSystemInfo> fs = new List<FileSystemInfo>();
        int top;
        int left;
        int height = 18;
        int width = 120;
        string path;
        int AIndex = 0;
        int FIndex = 0;
        int Amount = 16;
        public bool discs1;
        bool active;
        public string Path
        {
            get
            {
                return path;
            }
            set
            {
                DirectoryInfo di = new DirectoryInfo(value);
                if (di.Exists)
                {
                    path = value;
                }
            }
        }
        public FilePanel()
        {
            SetDiscs();
        }
        public FileSystemInfo GetActiveObject()
        {
            if (fs != null && fs.Count != 0)
            {
                return fs[AIndex];
            }
            throw new Exception();
        }
        public void Key(ConsoleKeyInfo key)
        {
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    Up();
                    break;
                case ConsoleKey.DownArrow:
                    Down();
                    break;
            }
        }
        private void Down()
        {
            if (AIndex >= FIndex + Amount - 1)
            {
                FIndex += 1;
                if (FIndex + Amount >= fs.Count)
                {
                    FIndex = fs.Count - Amount;
                }
                AIndex = FIndex + Amount - 1;
                UpdateContent(false);
            }
            else
            {
                if (AIndex >= fs.Count - 1)
                {
                    return;
                }
                DeactivateObject(AIndex);
                AIndex++;
                ActivateObject(AIndex);
            }
        }
        private void Up()
        {
            if (AIndex <= FIndex)
            {
                FIndex -= 1;
                if (FIndex < 0)
                {
                    FIndex = 0;
                }
                AIndex = FIndex;
                UpdateContent(false);
            }
            else
            {
                DeactivateObject(AIndex);
                AIndex--;
                ActivateObject(AIndex);
            }
        }
        public void SetLists()
        {
            if (fs.Count != 0)
            {
                fs.Clear();
            }
            discs1 = false;
            DirectoryInfo levelUpDirectory = null;
            fs.Add(levelUpDirectory);
            string[] directories = Directory.GetDirectories(path);
            foreach (string directory in directories)
            {
                DirectoryInfo di = new DirectoryInfo(directory);
                fs.Add(di);
            }
            string[] files = Directory.GetFiles(this.path);
            foreach (string file in files)
            {
                FileInfo fi = new FileInfo(file);
                fs.Add(fi);
            }
        }
        public void SetDiscs()
        {
            if (fs.Count != 0)
            {
                fs.Clear();
            }
            discs1 = true;
            DriveInfo[] discs = DriveInfo.GetDrives();
            foreach (DriveInfo disc in discs)
            {
                if (disc.IsReady)
                {
                    DirectoryInfo di = new DirectoryInfo(disc.Name);
                    fs.Add(di);
                }
            }
        }
        public void borders()
        {
            Clear();
            StringBuilder caption = new StringBuilder();
            FileManager.PositionString(caption.ToString(), left + width / 2 - caption.ToString().Length / 2, top);
            PrintContent();
        }
        public void Clear()
        {
            for (int i = 0; i < height; i++)
            {
                string space = new String(' ', width);
                Console.SetCursorPosition(left, top + i);
                Console.Write(space);
            }
        }
        private void PrintContent()
        {
            if (fs.Count == 0)
            {
                return;
            }
            int count = 0;
            int lastElement = FIndex + Amount;
            if (lastElement > fs.Count)
            {
                lastElement = fs.Count;
            }
            if (AIndex >= fs.Count)
            {
                AIndex = 0;
            }
            for (int i = FIndex; i < lastElement; i++)
            {
                Console.SetCursorPosition(left + 1, top + count + 1);
                if (i == AIndex && active == true)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }
                PrintObject(i);
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                count++;
            }
        }
        private void PrintObject(int index)
        {
            int currentCursorTopPosition = Console.CursorTop;
            int currentCursorLeftPosition = Console.CursorLeft;
            if (!discs1 && index == 0)
            {
                Console.Write("...");
                return;
            }
            Console.Write("{0}", fs[index].Name);
            Console.SetCursorPosition(currentCursorLeftPosition + width / 2, currentCursorTopPosition);
            if (fs[index] is DirectoryInfo)
            {
                Console.Write("{0}", ((DirectoryInfo)fs[index]).LastWriteTime);
            }
            else
            {
                Console.Write("{0}", ((FileInfo)fs[index]).Length);
            }
        }
        public void Panel()
        {
            FIndex = 0;
            AIndex = 0;
            borders();
        }
        public void UpdateContent(bool updateList)
        {
            if (updateList)
            {
                SetLists();
            }
            Clear();
            PrintContent();
        }
        private void ActivateObject(int index)
        {
            int offsetY = AIndex - FIndex;
            Console.SetCursorPosition(left + 1, top + offsetY + 1);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.White;
            PrintObject(index);
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
        }
        private void DeactivateObject(int index)
        {
            int offsetY = AIndex - FIndex;
            Console.SetCursorPosition(left + 1, top + offsetY + 1);
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
            PrintObject(index);
        }
    }
}