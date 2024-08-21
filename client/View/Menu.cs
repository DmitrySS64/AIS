namespace client.View
{
    public class Menu
    {
        private ConsoleKey nextKey = ConsoleKey.D1;
        private string? Title = null;
        public List<MenuItem> menu { get; set; }
        public Menu(string? title = null)
        {
            Title = title;
            menu = new List<MenuItem>();
        }
        public Menu(List<MenuItem> menu, string? title = null)
        {
            Title = title;
            this.menu = menu;
            for (int i = 0; i < this.menu.Count; i++)
            {
                if (this.menu[i].ConsoleKey == null)
                {
                    this.menu[i].ConsoleKey = nextKey;
                    nextKey++;
                }
            }
        }

        public void AddItem(string item, Action action, ConsoleKey? consoleKey = null)
        {
            if (consoleKey == null)
            {
                consoleKey = nextKey;
                nextKey++;
            }
            menu.Add(new MenuItem(item, action, consoleKey));
        }
        public void UseMenu()
        {
            if (Title != null) Console.WriteLine($"=={Title}==");
            for (int i = 0; i < menu.Count; i++)
            {
                Console.WriteLine($"{menu[i].ConsoleKey} - {menu[i].Item}");
            }

            var key = Console.ReadKey(true).Key;
            var selectedItem = menu.FirstOrDefault(x => x.ConsoleKey == key);
            selectedItem?.Action?.Invoke();
        }

    }

    public record MenuItem
    {
        public string Item { get; set; }
        public ConsoleKey? ConsoleKey { get; set; }
        public Action Action { get; set; }
        public MenuItem(string item, Action action, ConsoleKey? consoleKey = null)
        {
            Item = item;
            ConsoleKey = consoleKey;
            Action = action;
        }
    }
}
