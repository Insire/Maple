using DryIoc;
using MahApps.Metro.Controls;

namespace InsireBot
{
    public class IoCWindow : MetroWindow
    {
        public IContainer Container { get; set; }

        public IoCWindow(IContainer container) : base()
        {
            Container = container;
        }
    }
}
