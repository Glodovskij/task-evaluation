using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace process_manager.ProcessFiles
{
    public static class ProcessCollectionExtensions
    {
        public static bool ContatinsProcess(this ObservableCollection<WrappedProcess> processes, Process process)
        {
            for (int i = processes.Count - 1; i >= 0; i--)
            {
                if (processes[i].Process.Id == process.Id)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
