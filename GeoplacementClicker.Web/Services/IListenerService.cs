using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoplacementClicker.Web.Services
{
    public interface IListenerService
    {
        Task StartListening();

        Task StopListening();

        Task<string> GetListeningUrl();
    }
}
