using Microsoft.FSharp.Collections;
using UpdateControls;

namespace Strive.Model
{
    /// <summary>
    /// Objects stored in a Recorded should be immutable to ensure they are not modified externally,
    /// so that all changes are recorded in the history.
    /// History is stored in an ordered map of version to state.
    /// </summary>
    public class Recorded<T>
    {
        private FSharpMap<int, T> _history = MapModule.Empty<int, T>();
        private T _current;
        private int _currentVersion;
        private int _maxVersion;
        private int _minVersion = 1;
        private int _maxSize = 2000;
        private readonly Independent _indHistory = new Independent();

        public Recorded(T current)
        {
            Head = current;
        }

        public int CurrentVersion
        {
            get { _indHistory.OnGet(); return _currentVersion; }
            set
            {
                if (value >= _minVersion && value <= _maxVersion)
                {
                    _indHistory.OnSet();
                    _currentVersion = value;
                    _current = _history[value];
                }
            }
        }

        public int MaxVersion { get { _indHistory.OnGet(); return _maxVersion; } }
        public T Current { get { _indHistory.OnGet(); return _current; } }
        public int MaxSize { get { return _maxSize; } set { _maxSize = value; } }
        public int MinVersion { get { return _minVersion; } }

        public T Head
        {
            get { _indHistory.OnGet(); return _history[_maxVersion]; }
            set
            {
                _indHistory.OnSet();
                _history = _history.Add(++_maxVersion, value);
                if (_currentVersion == _maxVersion - 1)
                {
                    _current = value;
                    ++_currentVersion;
                }
                if (_maxVersion - _minVersion >= _maxSize)
                {
                    _history = _history.Remove(_minVersion);
                    ++_minVersion;
                }
            }
        }

        public FSharpMap<int, T> History {
            get { _indHistory.OnGet(); return _history; }
        }
    }
}
