using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace MG.Pwsh.Rdp.Serialization
{
    public class RdpWriter : IDisposable
    {
        internal const char DELIMITER = (char)58;  // : (colon)
        internal const char LITTLE_I = (char)105;
        internal const char LITTLE_S = (char)115;
        //private AttributeValuator _attVal;
        private bool _disposed;
        private StreamWriter _streamWriter;
        private WriteState _writeState = WriteState.Setting;

        public WriteState WriteState => _writeState;

        public RdpWriter(string filePath)
        {
            //_attVal = new AttributeValuator();
            _streamWriter = new StreamWriter(new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite),
                RdpReader._encoding)
            {
                NewLine = Environment.NewLine
            };
        }
        public void Close()
        {
            if (this.WriteState != WriteState.Closed)
            {
                if (this.WriteState == WriteState.EOL)
                    _streamWriter.WriteLine();

                _streamWriter.Close();
            }
        }
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _streamWriter.Flush();
                this.Close();
                _streamWriter.Dispose();
                _disposed = true;
            }
        }
        private char GetValueType(object value)
        {
            if (value is int || value is Enum || value is bool)
            {
                return LITTLE_I;
            }
            else
                return LITTLE_S;
        }
        private void WriteAndMove(object argument = null)
        {
            switch (_writeState)
            {
                case WriteState.Setting:
                    _streamWriter.Write(argument);
                    _streamWriter.Write(DELIMITER);
                    _writeState = WriteState.Value;
                    break;

                case WriteState.Value:
                    char c = this.GetValueType(argument);
                    if (c.Equals(LITTLE_I))
                        argument = Convert.ToInt32(argument);

                    _streamWriter.Write(c);
                    _streamWriter.Write(DELIMITER);
                    _streamWriter.Write(argument);
                    _writeState = WriteState.EOL;
                    break;

                case WriteState.EOL:
                    _streamWriter.WriteLine();
                    _writeState = WriteState.Setting;
                    break;
            }
        }
        public void WriteSetting(object setting)
        {
            if (this.WriteState == WriteState.EOL)
            {
                this.WriteAndMove();
            }
            else if (this.WriteState != WriteState.Setting)
                throw new InvalidOperationException(
                    string.Format(
                        "Cannot write 'Setting' when the current state is '{0}'", _writeState.ToString()));

            this.WriteAndMove(setting);
        }
        public void WriteRaw(string settingAndValue)
        {
            if (this.WriteState == WriteState.EOL)
            {
                this.WriteAndMove();
            }
            else if (this.WriteState != WriteState.Setting)
            {
                throw new InvalidOperationException(
                    string.Format(
                        "Cannot write 'Raw' when the current state is '{0}'", _writeState.ToString()));
            }

            _streamWriter.WriteLine(settingAndValue);
        }
        public void WriteValue(object value)
        {
            if (this.WriteState != WriteState.Value)
            {
                throw new InvalidOperationException(
                    string.Format(
                        "Cannot write 'Value' when the current state is '{0}'", _writeState.ToString()));
            }

            this.WriteAndMove(value);
        }
    }
}
