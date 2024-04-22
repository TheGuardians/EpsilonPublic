using EpsilonLib.Commands;
using Stylet;
using System;
using System.Collections;
using System.Diagnostics;
using System.Windows.Input;

namespace DefinitionEditor
{
    public class NavigableSearchResults : PropertyChangedBase, IEnumerable
    {
        private IList _results = new object[0];
        private int _currentIndex;
        public ICommand NextCommand { get; }
        public ICommand PreviousCommand { get; }
        public bool HasResults => _results.Count > 0;

        public event EventHandler CurrentIndexChanged;

        public NavigableSearchResults()
        {
            PreviousCommand = new DelegateCommand(() => Navigate(-1), () => _currentIndex > 0);
            NextCommand = new DelegateCommand(() => Navigate(1), () => _currentIndex < _results.Count - 1);
        }

        public IList Results
        {
            get => _results;
            set
            {
                Debug.Assert(value != null);

                if (SetAndNotify(ref _results, value))
                {
                    CurrentIndex = 0;
                    NotifyOfPropertyChange(nameof(HasResults));
                }
            }
        }

        public int CurrentIndex
        {
            get => _currentIndex;
            set
            {
                int oldIndex = _currentIndex;
                if (SetAndNotify(ref _currentIndex, value))
                {
                    (PreviousCommand as DelegateCommand).RaiseCanExecuteChanged();
                    (NextCommand as DelegateCommand).RaiseCanExecuteChanged();

                    CurrentIndexChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private void Navigate(int direction)
        {
            var index = CurrentIndex + direction;
            if (index < 0)
                index = 0;
            if (index >= _results.Count)
                index = _results.Count - 1;

            CurrentIndex = index;
        }

        public IEnumerator GetEnumerator()
        {
            return _results.GetEnumerator();
        }

        internal void Clear()
        {
            Results.Clear();
            CurrentIndex = -1;
            NotifyOfPropertyChange(nameof(HasResults));
        }
    }
}
