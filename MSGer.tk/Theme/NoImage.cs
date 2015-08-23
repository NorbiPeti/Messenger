using System; //Copyright (c) NorbiPeti 2015 - See LICENSE file
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSGer.tk
{
    public class NoImage : IDisposable //TODO: Kiválasztható legyen az összes típus egy listából
    { //2015.07.03.
        public Image NoImg = new Bitmap(200, 200);

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    NoImg.Dispose(); //2015.08.23.
                }

                disposedValue = true;
            }
        }
        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
