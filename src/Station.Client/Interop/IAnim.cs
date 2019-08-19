using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Station.Client.Interop {
	public interface IAnim {
		Task Start( IAnimCallback callback );

		Task Stop();
	}
}
