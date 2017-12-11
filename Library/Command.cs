using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Library
{
    public class Command : ICommand
    {
        public event EventHandler CanExecuteChanged; 
        public Predicate<object> CanExecuteFunc { get; set; }
        public Action<object> ExecFunc { get; set; }
        public bool CanExecute(object parameter) { return true; }
        public void Execute(object parameter)
        {
            ExecFunc(parameter);
        }
    }
}
