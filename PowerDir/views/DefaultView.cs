using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDir.views
{
    /// <summary>
    /// 
    /// </summary>
    public class DefaultView : AbstractView
    {
        // TODO to use string when using escape codes...
        protected delegate void WriteObject(object msg);
        protected readonly WriteObject _writeObject;
        public DefaultView(Action<object> writeObject)
        {
            _writeObject = new WriteObject(writeObject);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void displayResult(GetPowerDirInfo result)
        {
            _writeObject(result);
        }
    }
}
